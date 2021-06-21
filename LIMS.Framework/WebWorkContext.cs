using LIMS.Core;
using LIMS.Core.Configuration;
using LIMS.Domain.Customers;
using LIMS.Domain.Directory;
using LIMS.Domain.Localization;
using LIMS.Services.Authentication;
using LIMS.Services.Common;
using LIMS.Services.Customers;
using LIMS.Services.Directory;
using LIMS.Services.Localization;
using LIMS.Services.Stores;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.Net.Http.Headers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Framework
{
    /// <summary>
    /// Represents work context for web application
    /// </summary>
    public partial class WebWorkContext : IWorkContext
    {
        #region Fields

        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILIMSAuthenticationService _authenticationService;
        private readonly IApiAuthenticationService _apiauthenticationService;
        private readonly ICurrencyService _currencyService;
        private readonly ICustomerService _customerService;
        private readonly IGenericAttributeService _genericAttributeService;
        private readonly ILanguageService _languageService;
        private readonly IStoreContext _storeContext;
        private readonly IStoreMappingService _storeMappingService;

        private readonly LocalizationSettings _localizationSettings;
        private readonly LIMSConfig _config;

        private Customer _cachedCustomer;
        private Customer _originalCustomerIfImpersonated;
        private Language _cachedLanguage;
        private Currency _cachedCurrency;

        #endregion

        #region Ctor

        public WebWorkContext(IHttpContextAccessor httpContextAccessor,
            ILIMSAuthenticationService authenticationService,
            IApiAuthenticationService apiauthenticationService,
            ICurrencyService currencyService,
            ICustomerService customerService,
            IGenericAttributeService genericAttributeService,
            ILanguageService languageService,
            IStoreContext storeContext,
            IStoreMappingService storeMappingService,
            LocalizationSettings localizationSettings,
            LIMSConfig config)
        {
            _httpContextAccessor = httpContextAccessor;
            _authenticationService = authenticationService;
            _apiauthenticationService = apiauthenticationService;
            _currencyService = currencyService;
            _customerService = customerService;
            _genericAttributeService = genericAttributeService;
            _languageService = languageService;
            _storeContext = storeContext;
            _storeMappingService = storeMappingService;
            _localizationSettings = localizationSettings;
            _config = config;
        }

        #endregion

        #region Utilities

        /// <summary>
        /// Get language from the requested page URL
        /// </summary>
        /// <returns>The found language</returns>
        protected virtual async Task<Language> GetLanguageFromUrl(IList<Language> languages)
        {
            if (_httpContextAccessor.HttpContext == null || _httpContextAccessor.HttpContext.Request == null)
                return await Task.FromResult<Language>(null);

            //whether the requsted URL is localized
            var path = _httpContextAccessor.HttpContext.Request.Path.Value;
            if (string.IsNullOrEmpty(path))
                return await Task.FromResult<Language>(null);

            //get first segment of passed URL
            var firstSegment = path.Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries).FirstOrDefault() ?? string.Empty;
            if (string.IsNullOrEmpty(firstSegment))
                return await Task.FromResult<Language>(null);

            //suppose that the first segment is the language code and try to get language
            var language = languages.FirstOrDefault(urlLanguage => urlLanguage.UniqueSeoCode.Equals(firstSegment, StringComparison.OrdinalIgnoreCase));

            if (language == null || !language.Published || !_storeMappingService.Authorize(language))
                return await Task.FromResult<Language>(null);

            return language;
        }


        /// <summary>
        /// Get language from the request
        /// </summary>
        /// <returns>The found language</returns>
        protected virtual async Task<Language> GetLanguageFromRequest(IList<Language> languages)
        {
            if (_httpContextAccessor.HttpContext == null || _httpContextAccessor.HttpContext.Request == null)
                return await Task.FromResult<Language>(null);

            //get request culture
            var requestCulture = _httpContextAccessor.HttpContext.Features.Get<IRequestCultureFeature>()?.RequestCulture;
            if (requestCulture == null)
                return await Task.FromResult<Language>(null);

            //try to get language by culture name
            var requestLanguage = languages.FirstOrDefault(language =>
                language.LanguageCulture.Equals(requestCulture.Culture.Name, StringComparison.OrdinalIgnoreCase));

            //check language availability
            if (requestLanguage == null || !requestLanguage.Published || !_storeMappingService.Authorize(requestLanguage))
                return await Task.FromResult<Language>(null);

            return requestLanguage;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the current customer
        /// </summary>
        public virtual Customer CurrentCustomer {
            get {
                return _cachedCustomer;
            }
        }

        /// <summary>
        /// Set the current customer by Middleware
        /// </summary>
        /// <returns></returns>
        public virtual async Task<Customer> SetCurrentCustomer()
        {
            Customer customer = null;
            //check whether request is made by a background (schedule) task
            if (_httpContextAccessor.HttpContext == null)
            {
                //in this case return built-in customer record for background task
                customer = await _customerService.GetCustomerBySystemName(SystemCustomerNames.BackgroundTask);
            }

            //set customer as a background task if method setted as AllowAnonymous
            var endpoint = _httpContextAccessor.HttpContext?.GetEndpoint();
            if (endpoint?.Metadata?.GetMetadata<IAllowAnonymous>() is object)
            {
                customer = await _customerService.GetCustomerBySystemName(SystemCustomerNames.BackgroundTask);
            }

            if (customer == null || customer.Deleted || !customer.Active)
            {
                //try to get registered user
                customer = await _authenticationService.GetAuthenticatedCustomer();
            }

            if (customer == null)
            {
                //try to get api user
                customer = await _apiauthenticationService.GetAuthenticatedCustomer();
                //if customer comes from api, doesn't need to create cookies
                if (customer != null)
                {
                    //cache the found customer
                    _cachedCustomer = customer;
                    return customer;
                }
            }

            if (customer != null && !customer.Deleted && customer.Active)
            {
                //get impersonate user if required
                var impersonatedCustomerId = customer.GetAttributeFromEntity<string>(SystemCustomerAttributeNames.ImpersonatedCustomerId);
                if (!string.IsNullOrEmpty(impersonatedCustomerId))
                {
                    var impersonatedCustomer = await _customerService.GetCustomerById(impersonatedCustomerId);
                    if (impersonatedCustomer != null && !impersonatedCustomer.Deleted && impersonatedCustomer.Active)
                    {
                        //set impersonated customer
                        _originalCustomerIfImpersonated = customer;
                        customer = impersonatedCustomer;
                    }
                }
            }

            if (customer == null || customer.Deleted || !customer.Active)
            {
                //get guest customer
                var customerguid = await _authenticationService.GetCustomerGuid();
                if (!string.IsNullOrEmpty(customerguid))
                {
                    if (Guid.TryParse(customerguid, out Guid customerGuid))
                    {
                        //get customer from guid (should not be registered)
                        var customerByguid = await _customerService.GetCustomerByGuid(customerGuid);
                        if (customerByguid != null && !customerByguid.IsRegistered())
                            customer = customerByguid;
                    }
                }
            }

            if (customer == null || customer.Deleted || !customer.Active)
            {
                var crawler = _httpContextAccessor.HttpContext.Request?.Crawler();
                //check whether request is made by a search engine, in this case return built-in customer record for search engines
                if (crawler != null)
                    customer = await _customerService.GetCustomerBySystemName(SystemCustomerNames.SearchEngine);
            }

            if (customer == null || customer.Deleted || !customer.Active)
            {
                //create guest if not exists
                string referrer = _httpContextAccessor?.HttpContext?.Request?.Headers[HeaderNames.Referer];
                customer = await _customerService.InsertGuestCustomer(_storeContext.CurrentStore, referrer);
            }

            if (!customer.Deleted && customer.Active)
            {
                //set customer cookie
                await _authenticationService.SetCustomerGuid(customer.CustomerGuid);
            }
            //cache the found customer
            return _cachedCustomer = customer ?? throw new Exception("No customer could be loaded");
        }


        /// <summary>
        /// Gets the original customer (in case the current one is impersonated)
        /// </summary>
        public virtual Customer OriginalCustomerIfImpersonated => _originalCustomerIfImpersonated;

        /// <summary>
        /// Gets or sets current user working language
        /// </summary>
        public virtual Language WorkingLanguage => _cachedLanguage;

        /// <summary>
        /// Set current user working language 
        /// </summary>
        public virtual async Task<Language> SetWorkingLanguage(Language language)
        {
            if (language != null)
                await _genericAttributeService.SaveAttribute(this.CurrentCustomer, SystemCustomerAttributeNames.LanguageId, language.Id, _storeContext.CurrentStore.Id);

            //then reset the cache value
            _cachedLanguage = null;

            return language;
        }

        /// <summary>
        /// Set current user working language by Middleware
        /// </summary>
        /// <param name="customer"></param>
        /// <returns></returns>
        public virtual async Task<Language> SetWorkingLanguage(Customer customer)
        {
            Language detectedLanguage = null;
            var allStoreLanguages = await _languageService.GetAllLanguages();

            //localized URLs are enabled, so try to get language from the requested page URL
            if (_config.SeoFriendlyUrlsForLanguagesEnabled)
                detectedLanguage = await GetLanguageFromUrl(allStoreLanguages);

            //whether we should detect the language from the request
            if (detectedLanguage == null && _localizationSettings.AutomaticallyDetectLanguage)
            {
                //whether language already detected by this way
                var alreadyDetected = customer.GetAttributeFromEntity<bool>(SystemCustomerAttributeNames.LanguageAutomaticallyDetected, _storeContext.CurrentStore.Id);

                //if not, try to get language from the request
                if (!alreadyDetected)
                {
                    detectedLanguage = await GetLanguageFromRequest(allStoreLanguages);
                    if (detectedLanguage != null)
                    {
                        //language already detected
                        await _genericAttributeService.SaveAttribute(this.CurrentCustomer,
                            SystemCustomerAttributeNames.LanguageAutomaticallyDetected, true, _storeContext.CurrentStore.Id);
                    }
                }
            }

            //if the language is detected we need to save it
            if (detectedLanguage != null)
            {
                //get current saved language identifier
                var currentLanguageId = customer.GetAttributeFromEntity<string>(SystemCustomerAttributeNames.LanguageId, _storeContext.CurrentStore.Id);

                //save the detected language identifier if it differs from the current one
                if (detectedLanguage.Id != currentLanguageId)
                {
                    await _genericAttributeService.SaveAttribute(customer,
                        SystemCustomerAttributeNames.LanguageId, detectedLanguage.Id, _storeContext.CurrentStore.Id);
                }
            }

            //get current customer language identifier
            var customerLanguageId = customer.GetAttributeFromEntity<string>(SystemCustomerAttributeNames.LanguageId, _storeContext.CurrentStore.Id);

            //check customer language availability
            var customerLanguage = allStoreLanguages.FirstOrDefault(language => language.Id == customerLanguageId);
            if (customerLanguage == null)
            {
                //it not found, then try to get the default language for the current store (if specified)
                customerLanguage = allStoreLanguages.FirstOrDefault(language => language.Id == _storeContext.CurrentStore.DefaultLanguageId);
            }

            //if the default language for the current store not found, then try to get the first one
            if (customerLanguage == null)
                customerLanguage = allStoreLanguages.FirstOrDefault();

            //cache the found language
            _cachedLanguage = customerLanguage;

            return _cachedLanguage ?? throw new Exception("No language could be loaded");
        }

        /// <summary>
        /// Get current user working currency
        /// </summary>
        public virtual Currency WorkingCurrency => _cachedCurrency;

        /// <summary>
        /// Set current user working currency by Middleware
        /// </summary>
        public virtual async Task<Currency> SetWorkingCurrency(Customer customer)
        {
            //return primary store currency when we're you are in admin panel
            var adminAreaUrl = _httpContextAccessor.HttpContext.Request.Path.StartsWithSegments(new PathString("/Admin"));
            if (adminAreaUrl)
            {
                var primaryStoreCurrency = await _currencyService.GetPrimaryStoreCurrency();
                if (primaryStoreCurrency != null)
                {
                    _cachedCurrency = primaryStoreCurrency;
                    return primaryStoreCurrency;
                }
            }

            //find a currency previously selected by a customer
            var customerCurrencyId = customer.GetAttributeFromEntity<string>(SystemCustomerAttributeNames.CurrencyId, _storeContext.CurrentStore.Id);

            var allStoreCurrencies = await _currencyService.GetAllCurrencies();

            //check customer currency availability
            var customerCurrency = allStoreCurrencies.FirstOrDefault(currency => currency.Id == customerCurrencyId);
            if (customerCurrency == null)
            {
                //it not found, then try to get the default currency for the current language (if specified)
                customerCurrency = allStoreCurrencies.FirstOrDefault(currency => currency.Id == this.WorkingLanguage.DefaultCurrencyId);
            }

            //if the default currency for the current store not found, then try to get the first one
            if (customerCurrency == null)
                customerCurrency = allStoreCurrencies.FirstOrDefault();

            //cache the found currency
            _cachedCurrency = customerCurrency;
            return _cachedCurrency ?? throw new Exception("No currency could be loaded");
        }

        /// <summary>
        /// Set user working currency
        /// </summary>
        public virtual async Task<Currency> SetWorkingCurrency(Currency currency)
        {
            //and save it
            await _genericAttributeService.SaveAttribute(this.CurrentCustomer,
                SystemCustomerAttributeNames.CurrencyId, currency.Id, _storeContext.CurrentStore.Id);

            //then reset the cache value
            _cachedCurrency = null;

            return currency;
        }
        #endregion
    }
}
