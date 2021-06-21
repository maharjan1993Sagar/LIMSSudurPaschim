using LIMS.Core;
using LIMS.Domain.Common;
using LIMS.Domain.Customers;
using LIMS.Domain.Messages;
using LIMS.Services.Authentication.External;
using LIMS.Services.Common;
using LIMS.Services.Customers;
using LIMS.Services.Directory;
using LIMS.Services.Helpers;
using LIMS.Services.Localization;
using LIMS.Services.Logging;
using LIMS.Services.Media;
using LIMS.Services.Messages;
using LIMS.Services.Stores;
using LIMS.Web.Areas.Admin.Extensions;
using LIMS.Web.Areas.Admin.Interfaces;
using LIMS.Web.Areas.Admin.Models.Customers;

using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LIMS.Framework.Mvc.Models;

namespace LIMS.Web.Areas.Admin.Services
{
    public partial class CustomerViewModelService : ICustomerViewModelService
    {
        private readonly ICustomerService _customerService;
        private readonly INewsLetterSubscriptionService _newsLetterSubscriptionService;
        private readonly IGenericAttributeService _genericAttributeService;
        private readonly ICustomerRegistrationService _customerRegistrationService;
        private readonly IDateTimeHelper _dateTimeHelper;
        private readonly ILocalizationService _localizationService;
        private readonly DateTimeSettings _dateTimeSettings;
        private readonly ICountryService _countryService;
        private readonly IStateProvinceService _stateProvinceService;
        private readonly CustomerSettings _customerSettings;
        private readonly IWorkContext _workContext;
        private readonly IStoreContext _storeContext;

        private readonly ICustomerActivityService _customerActivityService;

        private readonly IQueuedEmailService _queuedEmailService;
        private readonly EmailAccountSettings _emailAccountSettings;
        private readonly IEmailAccountService _emailAccountService;

        private readonly IExternalAuthenticationService _openAuthenticationService;
        private readonly AddressSettings _addressSettings;
        private readonly CommonSettings _commonSettings;
        private readonly IStoreService _storeService;
        private readonly ICustomerAttributeParser _customerAttributeParser;
        private readonly ICustomerAttributeService _customerAttributeService;
        private readonly IAddressAttributeParser _addressAttributeParser;
        private readonly IAddressAttributeService _addressAttributeService;
        private readonly IAddressAttributeFormatter _addressAttributeFormatter;
        private readonly IContactUsService _contactUsService;
        private readonly ICustomerTagService _customerTagService;

        private readonly IDownloadService _downloadService;
        private readonly IServiceProvider _serviceProvider;

        public CustomerViewModelService(
            ICustomerService customerService,
            ICustomerProductService customerProductService,
            INewsLetterSubscriptionService newsLetterSubscriptionService,
            IGenericAttributeService genericAttributeService,
            ICustomerRegistrationService customerRegistrationService,
            IDateTimeHelper dateTimeHelper,
            ILocalizationService localizationService,
            DateTimeSettings dateTimeSettings,
            ICountryService countryService,
            IStateProvinceService stateProvinceService,
            CustomerSettings customerSettings,
            IWorkContext workContext,
            IStoreContext storeContext,
            ICustomerActivityService customerActivityService,
            IQueuedEmailService queuedEmailService,
            EmailAccountSettings emailAccountSettings,
            IEmailAccountService emailAccountService,
            IExternalAuthenticationService openAuthenticationService,
            AddressSettings addressSettings,
            CommonSettings commonSettings,
            IStoreService storeService,
            ICustomerAttributeParser customerAttributeParser,
            ICustomerAttributeService customerAttributeService,
            IAddressAttributeParser addressAttributeParser,
            IAddressAttributeService addressAttributeService,
            IAddressAttributeFormatter addressAttributeFormatter,
            IContactUsService contactUsService,
            ICustomerTagService customerTagService,
            IDownloadService downloadService,
            IServiceProvider serviceProvider)
        {
            _customerService = customerService;
            _newsLetterSubscriptionService = newsLetterSubscriptionService;
            _genericAttributeService = genericAttributeService;
            _customerRegistrationService = customerRegistrationService;
            _dateTimeHelper = dateTimeHelper;
            _localizationService = localizationService;
            _dateTimeSettings = dateTimeSettings;
            _countryService = countryService;
            _stateProvinceService = stateProvinceService;
            _customerSettings = customerSettings;
            _commonSettings = commonSettings;
            _workContext = workContext;
            _storeContext = storeContext;
            _customerActivityService = customerActivityService;
            _queuedEmailService = queuedEmailService;
            _emailAccountSettings = emailAccountSettings;
            _emailAccountService = emailAccountService;
            _openAuthenticationService = openAuthenticationService;
            _addressSettings = addressSettings;
            _storeService = storeService;
            _customerAttributeParser = customerAttributeParser;
            _customerAttributeService = customerAttributeService;
            _addressAttributeParser = addressAttributeParser;
            _addressAttributeService = addressAttributeService;
            _addressAttributeFormatter = addressAttributeFormatter;
            _contactUsService = contactUsService;
            _customerTagService = customerTagService;
            _downloadService = downloadService;
            _serviceProvider = serviceProvider;
        }

        #region Utilities

        protected virtual string[] ParseCustomerTags(string customerTags)
        {
            var result = new List<string>();
            if (!String.IsNullOrWhiteSpace(customerTags))
            {
                string[] values = customerTags.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string val1 in values)
                    if (!String.IsNullOrEmpty(val1.Trim()))
                        result.Add(val1.Trim());
            }
            return result.ToArray();
        }

        protected virtual async Task SaveCustomerTags(Customer customer, string[] customerTags)
        {
            if (customer == null)
                throw new ArgumentNullException("customer");

            //product tags
            var existingCustomerTags = customer.CustomerTags.ToList();
            var customerTagsToRemove = new List<CustomerTag>();
            foreach (var existingCustomerTag in existingCustomerTags)
            {
                bool found = false;
                var existingCustomerTagName = await _customerTagService.GetCustomerTagById(existingCustomerTag);
                foreach (string newCustomerTag in customerTags)
                {
                    if (existingCustomerTagName.Name.Equals(newCustomerTag, StringComparison.OrdinalIgnoreCase))
                    {
                        found = true;
                        break;
                    }
                }
                if (!found)
                {
                    customerTagsToRemove.Add(existingCustomerTagName);
                    await _customerTagService.DeleteTagFromCustomer(existingCustomerTagName.Id, customer.Id);
                }
            }

            foreach (string customerTagName in customerTags)
            {
                CustomerTag customerTag;
                var customerTag2 = await _customerTagService.GetCustomerTagByName(customerTagName);
                if (customerTag2 == null)
                {
                    customerTag = new CustomerTag {
                        Name = customerTagName,
                    };
                    await _customerTagService.InsertCustomerTag(customerTag);
                }
                else
                {
                    customerTag = customerTag2;
                }
                if (!customer.CustomerTags.Contains(customerTag.Id))
                {
                    await _customerTagService.InsertTagToCustomer(customerTag.Id, customer.Id);
                }
            }
        }

        protected virtual string GetCustomerRolesNames(IList<CustomerRole> customerRoles, string separator = ",")
        {
            var sb = new StringBuilder();
            for (int i = 0; i < customerRoles.Count; i++)
            {
                sb.Append(customerRoles[i].Name);
                if (i != customerRoles.Count - 1)
                {
                    sb.Append(separator);
                    sb.Append(" ");
                }
            }
            return sb.ToString();
        }

        protected virtual async Task<IList<CustomerModel.AssociatedExternalAuthModel>> GetAssociatedExternalAuthRecords(Customer customer)
        {
            if (customer == null)
                throw new ArgumentNullException("customer");

            var result = new List<CustomerModel.AssociatedExternalAuthModel>();
            foreach (var record in await _openAuthenticationService.GetExternalIdentifiersFor(customer))
            {
                var method = _openAuthenticationService.LoadExternalAuthenticationMethodBySystemName(record.ProviderSystemName);
                if (method == null)
                    continue;

                result.Add(new CustomerModel.AssociatedExternalAuthModel {
                    Id = record.Id,
                    Email = record.Email,
                    ExternalIdentifier = record.ExternalIdentifier,
                    AuthMethodName = method.PluginDescriptor.FriendlyName
                });
            }

            return result;
        }

        protected virtual async Task<CustomerModel> PrepareCustomerModelForList(Customer customer)
        {
            return new CustomerModel {
                Id = customer.Id,
                Email = customer.IsRegistered() ? customer.Email : _localizationService.GetResource("Admin.Customers.Guest"),
                Username = customer.Username,
                FullName = customer.GetFullName(),
                Company = await customer.GetAttribute<string>(_genericAttributeService, SystemCustomerAttributeNames.Company),
                Phone = customer.MobileNo,
                ZipPostalCode = await customer.GetAttribute<string>(_genericAttributeService, SystemCustomerAttributeNames.ZipPostalCode),
                CustomerRoleNames = GetCustomerRolesNames(customer.CustomerRoles.ToList()),
                Active = customer.Active,
               
                CreatedOn = _dateTimeHelper.ConvertToUserTime(customer.CreatedOnUtc, DateTimeKind.Utc),
                LastActivityDate = _dateTimeHelper.ConvertToUserTime(customer.LastActivityDateUtc, DateTimeKind.Utc),
            };
        }

        protected virtual async Task PrepareStoresModel(CustomerModel model)
        {
            if (model == null)
                throw new ArgumentNullException("model");

            model.AvailableStores.Add(new SelectListItem {
                Text = _localizationService.GetResource("Admin.Customers.Customers.Fields.StaffStore.None"),
                Value = ""
            });
            var stores = await _storeService.GetAllStores();
            foreach (var store in stores)
            {
                model.AvailableStores.Add(new SelectListItem {
                    Text = store.Shortcut,
                    Value = store.Id.ToString()
                });
            }
        }

        #endregion

        public virtual async Task<CustomerListModel> PrepareCustomerListModel()
        {
            var registered = await _customerService.GetCustomerRoleBySystemName(SystemCustomerRoleNames.Registered);
            var model = new CustomerListModel {
                UsernamesEnabled = _customerSettings.UsernamesEnabled,
                CompanyEnabled = _customerSettings.CompanyEnabled,
                PhoneEnabled = _customerSettings.PhoneEnabled,
                ZipPostalCodeEnabled = _customerSettings.ZipPostalCodeEnabled,
                AvailableCustomerRoles = (await _customerService.GetAllCustomerRoles(showHidden: true)).Select(cr => new SelectListItem() { Text = cr.Name, Value = cr.Id.ToString(), Selected = (cr.Id == registered.Id) }).ToList(),
                AvailableCustomerTags = (await _customerTagService.GetAllCustomerTags()).Select(ct => new SelectListItem() { Text = ct.Name, Value = ct.Id.ToString() }).ToList(),
                SearchCustomerRoleIds = new List<string> { (await _customerService.GetAllCustomerRoles(showHidden: true)).FirstOrDefault(x => x.Id == registered.Id).Id },
            };
            return model;
        }

        public virtual async Task<(IEnumerable<CustomerModel> customerModelList, int totalCount)> PrepareCustomerList(CustomerListModel model,
            string[] searchCustomerRoleIds, string[] searchCustomerTagIds, int pageIndex, int pageSize, string createdBy = "")
        {
            var customers = await _customerService.GetAllCustomers(
                customerRoleIds: searchCustomerRoleIds,
                customerTagIds: searchCustomerTagIds,
                email: model.SearchEmail,
                username: model.SearchUsername,
                firstName: model.SearchFirstName,
                lastName: model.SearchLastName,
                company: model.SearchCompany,
                phone: model.SearchPhone,
                zipPostalCode: model.SearchZipPostalCode,
                loadOnlyWithShoppingCart: false,
                pageIndex: pageIndex - 1,
                pageSize: pageSize, createdBy: createdBy);

            var customermodellist = new List<CustomerModel>();
            foreach (var item in customers)
            {
                customermodellist.Add(await PrepareCustomerModelForList(item));
            }
            return (customermodellist, customers.TotalCount);
        }

        public virtual async Task PrepareCustomerModel(CustomerModel model, Customer customer, bool excludeProperties)
        {
            var allStores = await _storeService.GetAllStores();
            if (customer != null)
            {
                model.Id = customer.Id;
                model.ShowMessageContactForm = _commonSettings.StoreInDatabaseContactUsForm;
                if (!excludeProperties)
                {
                    model.Email = customer.Email;
                    model.Username = customer.Username;
                    model.VendorId = customer.VendorId;
                    model.StaffStoreId = customer.StaffStoreId;
                    model.AdminComment = customer.AdminComment;
                    model.IsTaxExempt = customer.IsTaxExempt;
                    model.FreeShipping = customer.FreeShipping;
                    model.IDCardNo = customer.IDCardNo;
                    model.PhoneNo = customer.PhoneNo;
                    model.District = customer.District;
                    model.State = customer.State;
                    model.LocalLevel = customer.LocalLevel;
                    model.Wardno = customer.Wardno;
                    model.MobileNo = customer.MobileNo;
                    model.Active = customer.Active;
                    model.OrgAddress = customer.OrgAddress;
                    model.OrgName = customer.OrgName;
                    model.Owner = customer.IsOwner() ? "" : (await _customerService.GetCustomerById(customer.OwnerId))?.Email;
                    var result = new StringBuilder();
                    foreach (var item in customer.CustomerTags)
                    {
                        var ct = await _customerTagService.GetCustomerTagById(item);
                        result.Append(ct.Name);
                        result.Append(", ");
                    }
                    model.CustomerTags = result.ToString();

                    model.TimeZoneId = await customer.GetAttribute<string>(_genericAttributeService, SystemCustomerAttributeNames.TimeZoneId);
                    model.VatNumber = await customer.GetAttribute<string>(_genericAttributeService, SystemCustomerAttributeNames.VatNumber);
                    model.CreatedOn = _dateTimeHelper.ConvertToUserTime(customer.CreatedOnUtc, DateTimeKind.Utc);
                    model.LastActivityDate = _dateTimeHelper.ConvertToUserTime(customer.LastActivityDateUtc, DateTimeKind.Utc);
                    if (customer.LastPurchaseDateUtc.HasValue)
                        model.LastPurchaseDate = _dateTimeHelper.ConvertToUserTime(customer.LastPurchaseDateUtc.Value, DateTimeKind.Utc);
                    model.LastIpAddress = customer.LastIpAddress;
                    model.UrlReferrer = customer.UrlReferrer;
                    model.LastVisitedPage = await customer.GetAttribute<string>(_genericAttributeService, SystemCustomerAttributeNames.LastVisitedPage);
                    model.LastUrlReferrer = await customer.GetAttribute<string>(_genericAttributeService, SystemCustomerAttributeNames.LastUrlReferrer);

                    model.SelectedCustomerRoleIds = customer.CustomerRoles.Select(cr => cr.Id).ToArray();
                    //newsletter subscriptions
                    if (!String.IsNullOrEmpty(customer.Email))
                    {
                        var newsletterSubscriptionStoreIds = new List<string>();
                        foreach (var store in allStores)
                        {
                            var newsletterSubscription = await _newsLetterSubscriptionService
                                .GetNewsLetterSubscriptionByEmailAndStoreId(customer.Email, store.Id);
                            if (newsletterSubscription != null && newsletterSubscription.Active)
                                newsletterSubscriptionStoreIds.Add(store.Id);
                        }
                        model.SelectedNewsletterSubscriptionStoreIds = newsletterSubscriptionStoreIds.ToArray();
                    }


                    //form fields
                    model.FirstName = await customer.GetAttribute<string>(_genericAttributeService, SystemCustomerAttributeNames.FirstName);
                    model.LastName = await customer.GetAttribute<string>(_genericAttributeService, SystemCustomerAttributeNames.LastName);
                    model.Gender = await customer.GetAttribute<string>(_genericAttributeService, SystemCustomerAttributeNames.Gender);
                    model.DateOfBirth = await customer.GetAttribute<DateTime?>(_genericAttributeService, SystemCustomerAttributeNames.DateOfBirth);
                    model.Company = await customer.GetAttribute<string>(_genericAttributeService, SystemCustomerAttributeNames.Company);
                    model.StreetAddress = await customer.GetAttribute<string>(_genericAttributeService, SystemCustomerAttributeNames.StreetAddress);
                    model.StreetAddress2 = await customer.GetAttribute<string>(_genericAttributeService, SystemCustomerAttributeNames.StreetAddress2);
                    model.ZipPostalCode = await customer.GetAttribute<string>(_genericAttributeService, SystemCustomerAttributeNames.ZipPostalCode);
                    model.City = await customer.GetAttribute<string>(_genericAttributeService, SystemCustomerAttributeNames.City);
                    model.CountryId = await customer.GetAttribute<string>(_genericAttributeService, SystemCustomerAttributeNames.CountryId);
                    model.StateProvinceId = await customer.GetAttribute<string>(_genericAttributeService, SystemCustomerAttributeNames.StateProvinceId);
                    model.Phone = await customer.GetAttribute<string>(_genericAttributeService, SystemCustomerAttributeNames.Phone);
                    model.Fax = await customer.GetAttribute<string>(_genericAttributeService, SystemCustomerAttributeNames.Fax);
                }
            }

            model.UsernamesEnabled = _customerSettings.UsernamesEnabled;
            model.AllowUsersToChangeUsernames = _customerSettings.AllowUsersToChangeUsernames;
            model.AllowCustomersToSetTimeZone = _dateTimeSettings.AllowCustomersToSetTimeZone;
            foreach (var tzi in _dateTimeHelper.GetSystemTimeZones())
                model.AvailableTimeZones.Add(new SelectListItem { Text = tzi.DisplayName, Value = tzi.Id, Selected = (tzi.Id == model.TimeZoneId) });
            model.DisplayVatNumber = false;

            //stores
            await PrepareStoresModel(model);

            model.GenderEnabled = _customerSettings.GenderEnabled;
            model.DateOfBirthEnabled = _customerSettings.DateOfBirthEnabled;
            model.CompanyEnabled = _customerSettings.CompanyEnabled;
            model.StreetAddressEnabled = _customerSettings.StreetAddressEnabled;
            model.StreetAddress2Enabled = _customerSettings.StreetAddress2Enabled;
            model.ZipPostalCodeEnabled = _customerSettings.ZipPostalCodeEnabled;
            model.CityEnabled = _customerSettings.CityEnabled;
            model.CountryEnabled = _customerSettings.CountryEnabled;
            model.StateProvinceEnabled = _customerSettings.StateProvinceEnabled;
            model.PhoneEnabled = _customerSettings.PhoneEnabled;
            model.FaxEnabled = _customerSettings.FaxEnabled;

            //countries and states
            if (_customerSettings.CountryEnabled)
            {
                model.AvailableCountries.Add(new SelectListItem { Text = _localizationService.GetResource("Admin.Address.SelectCountry"), Value = "" });
                foreach (var c in await _countryService.GetAllCountries(showHidden: true))
                {
                    model.AvailableCountries.Add(new SelectListItem {
                        Text = c.Name,
                        Value = c.Id.ToString(),
                        Selected = c.Id == model.CountryId
                    });
                }

                if (_customerSettings.StateProvinceEnabled)
                {
                    //states
                    var states = await _stateProvinceService.GetStateProvincesByCountryId(model.CountryId);
                    if (states.Count > 0)
                    {
                        model.AvailableStates.Add(new SelectListItem { Text = _localizationService.GetResource("Admin.Address.SelectState"), Value = "" });

                        foreach (var s in states)
                        {
                            model.AvailableStates.Add(new SelectListItem { Text = s.Name, Value = s.Id.ToString(), Selected = (s.Id == model.StateProvinceId) });
                        }
                    }
                    else
                    {
                        bool anyCountrySelected = model.AvailableCountries.Any(x => x.Selected);

                        model.AvailableStates.Add(new SelectListItem {
                            Text = _localizationService.GetResource(anyCountrySelected ? "Admin.Address.OtherNonUS" : "Admin.Address.SelectState"),
                            Value = ""
                        });
                    }
                }
            }

            //newsletter subscriptions
            model.AvailableNewsletterSubscriptionStores = allStores
                .Select(s => new StoreModel() { Id = s.Id, Name = s.Shortcut })
                .ToList();


            //customer roles
            model.AvailableCustomerRoles = (await _customerService.GetAllCustomerRoles(showHidden: true))
                .Select(cr => cr.ToModel())
                .ToList();

            if (model.SelectedCustomerRoleIds == null && customer == null && model.AvailableCustomerRoles.Count > 0)
            {
                model.SelectedCustomerRoleIds = new[] {model.AvailableCustomerRoles
                     .FirstOrDefault(c=>c.SystemName==SystemCustomerRoleNames.Registered).Id };
            }

            if (customer != null)
            {
                //reward points history
                // model.DisplayRewardPointsHistory = _rewardPointsSettings.Enabled;
                model.AddRewardPointsValue = 0;
                model.AddRewardPointsMessage = "Some comment here...";

                //stores
                foreach (var store in allStores)
                {
                    model.RewardPointsAvailableStores.Add(new SelectListItem {
                        Text = store.Shortcut,
                        Value = store.Id.ToString(),
                        Selected = (store.Id == _storeContext.CurrentStore.Id)
                    });
                }

                //external authentication records
                model.AssociatedExternalAuthRecords = await GetAssociatedExternalAuthRecords(customer);

            }
            else
            {
                model.DisplayRewardPointsHistory = false;
            }

            //sending of the welcome message:
            //1. "admin approval" registration method
            //2. already created customer
            //3. registered
            model.AllowSendingOfWelcomeMessage = _customerSettings.UserRegistrationType == UserRegistrationType.AdminApproval &&
                customer != null &&
                customer.IsRegistered();
            //sending of the activation message
            //1. "email validation" registration method
            //2. already created customer
            //3. registered
            //4. not active
            model.AllowReSendingOfActivationMessage = _customerSettings.UserRegistrationType == UserRegistrationType.EmailValidation &&
                customer != null &&
                customer.IsRegistered() &&
                !customer.Active;
        }

        public virtual string ValidateCustomerRoles(IList<CustomerRole> customerRoles)
        {
            if (customerRoles == null)
                throw new ArgumentNullException("customerRoles");

            //ensure a customer is not added to both 'Guests' and 'Registered' customer roles
            //ensure that a customer is in at least one required role ('Guests' and 'Registered')
            bool isInGuestsRole = customerRoles.FirstOrDefault(cr => cr.SystemName == SystemCustomerRoleNames.Guests) != null;
            bool isInRegisteredRole = customerRoles.FirstOrDefault(cr => cr.SystemName == SystemCustomerRoleNames.Registered) != null;
            if (isInGuestsRole && isInRegisteredRole)
                return "The customer cannot be in both 'Guests' and 'Registered' customer roles";
            if (!isInGuestsRole && !isInRegisteredRole)
                return "Add the customer to 'Guests' or 'Registered' customer role";

            //no errors
            return "";
        }
        public virtual async Task<Customer> InsertCustomerModel(CustomerModel model)
        {
            var ownerId = string.Empty;
            var customer = new Customer {
                CustomerGuid = Guid.NewGuid(),
                Email = model.Email,
                Username = model.Username,
                VendorId = model.VendorId,
                StaffStoreId = model.StaffStoreId,
                AdminComment = model.AdminComment,
                IsTaxExempt = model.IsTaxExempt,
                FreeShipping = model.FreeShipping,
                Active = model.Active,
                StoreId = _storeContext.CurrentStore.Id,
                OwnerId = ownerId,
                IDCardNo = model.IDCardNo,
                District = model.District,
                State = model.State,
                LocalLevel = model.LocalLevel,
                Wardno = model.Wardno,
                PhoneNo = model.PhoneNo,
                MobileNo = model.MobileNo,
                Province = model.Province,
                Ward = model.Ward,
                Tole = model.Tole,
                NameNepali = model.NameNepali,
                NameEnglish = model.NameEnglish,
                Position = model.Position,
                CreatedBy = model.CreatedBy,
                CreatedOnUtc = DateTime.UtcNow,
                LastActivityDateUtc = DateTime.UtcNow,
                OrgAddress = model.OrgAddress,
                OrgName = model.OrgName,
                EntityId = model.EntityId

            };
            await _customerService.InsertCustomer(customer);

            //form fields
            if (_dateTimeSettings.AllowCustomersToSetTimeZone)
                await _genericAttributeService.SaveAttribute(customer, SystemCustomerAttributeNames.TimeZoneId, model.TimeZoneId);
            if (_customerSettings.GenderEnabled)
                await _genericAttributeService.SaveAttribute(customer, SystemCustomerAttributeNames.Gender, model.Gender);
            await _genericAttributeService.SaveAttribute(customer, SystemCustomerAttributeNames.FirstName, model.FirstName);
            await _genericAttributeService.SaveAttribute(customer, SystemCustomerAttributeNames.LastName, model.LastName);
            if (_customerSettings.DateOfBirthEnabled)
                await _genericAttributeService.SaveAttribute(customer, SystemCustomerAttributeNames.DateOfBirth, model.DateOfBirth);
            if (_customerSettings.CompanyEnabled)
                await _genericAttributeService.SaveAttribute(customer, SystemCustomerAttributeNames.Company, model.Company);
            if (_customerSettings.StreetAddressEnabled)
                await _genericAttributeService.SaveAttribute(customer, SystemCustomerAttributeNames.StreetAddress, model.StreetAddress);
            if (_customerSettings.StreetAddress2Enabled)
                await _genericAttributeService.SaveAttribute(customer, SystemCustomerAttributeNames.StreetAddress2, model.StreetAddress2);
            if (_customerSettings.ZipPostalCodeEnabled)
                await _genericAttributeService.SaveAttribute(customer, SystemCustomerAttributeNames.ZipPostalCode, model.ZipPostalCode);
            if (_customerSettings.CityEnabled)
                await _genericAttributeService.SaveAttribute(customer, SystemCustomerAttributeNames.City, model.City);
            if (_customerSettings.CountryEnabled)
                await _genericAttributeService.SaveAttribute(customer, SystemCustomerAttributeNames.CountryId, model.CountryId);
            if (_customerSettings.CountryEnabled && _customerSettings.StateProvinceEnabled)
                await _genericAttributeService.SaveAttribute(customer, SystemCustomerAttributeNames.StateProvinceId, model.StateProvinceId);
            if (_customerSettings.PhoneEnabled)
                await _genericAttributeService.SaveAttribute(customer, SystemCustomerAttributeNames.Phone, model.Phone);
            if (_customerSettings.FaxEnabled)
                await _genericAttributeService.SaveAttribute(customer, SystemCustomerAttributeNames.Fax, model.Fax);

            //custom customer attributes
            await _genericAttributeService.SaveAttribute(customer, SystemCustomerAttributeNames.CustomCustomerAttributes, model.CustomAttributes);

            //newsletter subscriptions
            if (!String.IsNullOrEmpty(customer.Email))
            {
                var allStores = await _storeService.GetAllStores();
                foreach (var store in allStores)
                {
                    var newsletterSubscription = await _newsLetterSubscriptionService
                        .GetNewsLetterSubscriptionByEmailAndStoreId(customer.Email, store.Id);
                    if (model.SelectedNewsletterSubscriptionStoreIds != null &&
                        model.SelectedNewsletterSubscriptionStoreIds.Contains(store.Id))
                    {
                        //subscribed
                        if (newsletterSubscription == null)
                        {
                            await _newsLetterSubscriptionService.InsertNewsLetterSubscription(new NewsLetterSubscription {
                                NewsLetterSubscriptionGuid = Guid.NewGuid(),
                                CustomerId = customer.Id,
                                Email = customer.Email,
                                Active = true,
                                StoreId = store.Id,
                                CreatedOnUtc = DateTime.UtcNow
                            });
                        }
                    }
                    else
                    {
                        //not subscribed
                        if (newsletterSubscription != null)
                        {
                            await _newsLetterSubscriptionService.DeleteNewsLetterSubscription(newsletterSubscription);
                        }
                    }
                }
            }

            var allCustomerRoles = await _customerService.GetAllCustomerRoles(showHidden: true);
            var newCustomerRoles = new List<CustomerRole>();
            foreach (var customerRole in allCustomerRoles)
                if (model.SelectedCustomerRoleIds != null && model.SelectedCustomerRoleIds.Contains(customerRole.Id))
                    newCustomerRoles.Add(customerRole);

            //customer roles
            foreach (var customerRole in newCustomerRoles)
            {
                //ensure that the current customer cannot add to "Administrators" system role if he's not an admin himself
                if (customerRole.SystemName == SystemCustomerRoleNames.Administrators &&
                    !_workContext.CurrentCustomer.IsAdmin())
                    continue;

                customer.CustomerRoles.Add(customerRole);
                customerRole.CustomerId = customer.Id;
                await _customerService.InsertCustomerRoleInCustomer(customerRole);
            }


            //ensure that a customer with a vendor associated is not in "Administrators" role
            //otherwise, he won't be have access to the other functionality in admin area
            if (customer.IsAdmin() && !String.IsNullOrEmpty(customer.VendorId))
            {
                customer.VendorId = "";
                await _customerService.UpdateCustomerVendor(customer);
            }

            //ensure that a customer in the Vendors role has a vendor account associated.
            //otherwise, he will have access to ALL products
            if (customer.IsVendor() && string.IsNullOrEmpty(customer.VendorId))
            {
                var vendorRole = customer
                    .CustomerRoles
                    .FirstOrDefault(x => x.SystemName == SystemCustomerRoleNames.Vendors);
                customer.CustomerRoles.Remove(vendorRole);
                vendorRole.CustomerId = customer.Id;
                await _customerService.DeleteCustomerRoleInCustomer(vendorRole);
            }
            //ensure that a customer in the Staff role has a staff account associated.
            //otherwise, he will have access to ALL products
            if (customer.IsStaff() && string.IsNullOrEmpty(customer.StaffStoreId))
            {
                var staffRole = customer
                    .CustomerRoles
                    .FirstOrDefault(x => x.SystemName == SystemCustomerRoleNames.Staff);
                customer.CustomerRoles.Remove(staffRole);
                staffRole.CustomerId = customer.Id;
                await _customerService.DeleteCustomerRoleInCustomer(staffRole);
            }
            //tags
            await SaveCustomerTags(customer, ParseCustomerTags(model.CustomerTags));

            //activity log
            await _customerActivityService.InsertActivity("AddNewCustomer", customer.Id, _localizationService.GetResource("ActivityLog.AddNewCustomer"), customer.Id);

            return customer;
        }

        public virtual async Task<Customer> UpdateCustomerModel(Customer customer, CustomerModel model)
        {
            customer.AdminComment = model.AdminComment;
            customer.IsTaxExempt = model.IsTaxExempt;
            customer.FreeShipping = model.FreeShipping;
            customer.IDCardNo = model.IDCardNo;
            customer.District = model.District;
            customer.State = model.State;
            customer.LocalLevel = model.LocalLevel;
            customer.Wardno = model.Wardno;
            customer.PhoneNo = model.PhoneNo;
            customer.MobileNo = model.MobileNo;
            customer.Active = model.Active;
            customer.District = model.District;
            customer.LocalLevel = model.LocalLevel;
            customer.Wardno = model.Wardno;
            customer.MobileNo = model.MobileNo;
            customer.Province = model.Province;
            customer.Ward = model.Ward;
            customer.Tole = model.Tole;
            customer.NameNepali = model.NameNepali;
            customer.NameEnglish = model.NameEnglish;
            customer.Position = model.Position;
            customer.OrgAddress = model.OrgAddress;
            customer.OrgName = model.OrgName;
            customer.EntityId = model.EntityId;
            //email
            if (!String.IsNullOrWhiteSpace(model.Email))
            {
                await _customerRegistrationService.SetEmail(customer, model.Email);
            }
            else
            {
                customer.Email = model.Email;
            }

            //username
            if (_customerSettings.UsernamesEnabled && _customerSettings.AllowUsersToChangeUsernames)
            {
                if (!String.IsNullOrWhiteSpace(model.Username))
                {
                    await _customerRegistrationService.SetUsername(customer, model.Username);
                }
                else
                {
                    customer.Username = model.Username;
                }
            }

            if (!string.IsNullOrEmpty(model.Owner))
            {
                var customerOwner = await _customerService.GetCustomerByEmail(model.Owner);
                if (customerOwner != null)
                {
                    customer.OwnerId = customerOwner.Id;
                }
            }
            else
                customer.OwnerId = string.Empty;

            //vendor
            customer.VendorId = model.VendorId;

            //staff store
            customer.StaffStoreId = model.StaffStoreId;

            //form fields
            if (_dateTimeSettings.AllowCustomersToSetTimeZone)
                await _genericAttributeService.SaveAttribute(customer, SystemCustomerAttributeNames.TimeZoneId, model.TimeZoneId);
            if (_customerSettings.GenderEnabled)
                await _genericAttributeService.SaveAttribute(customer, SystemCustomerAttributeNames.Gender, model.Gender);
            await _genericAttributeService.SaveAttribute(customer, SystemCustomerAttributeNames.FirstName, model.FirstName);
            await _genericAttributeService.SaveAttribute(customer, SystemCustomerAttributeNames.LastName, model.LastName);
            if (_customerSettings.DateOfBirthEnabled)
                await _genericAttributeService.SaveAttribute(customer, SystemCustomerAttributeNames.DateOfBirth, model.DateOfBirth);
            if (_customerSettings.CompanyEnabled)
                await _genericAttributeService.SaveAttribute(customer, SystemCustomerAttributeNames.Company, model.Company);
            if (_customerSettings.StreetAddressEnabled)
                await _genericAttributeService.SaveAttribute(customer, SystemCustomerAttributeNames.StreetAddress, model.StreetAddress);
            if (_customerSettings.StreetAddress2Enabled)
                await _genericAttributeService.SaveAttribute(customer, SystemCustomerAttributeNames.StreetAddress2, model.StreetAddress2);
            if (_customerSettings.ZipPostalCodeEnabled)
                await _genericAttributeService.SaveAttribute(customer, SystemCustomerAttributeNames.ZipPostalCode, model.ZipPostalCode);
            if (_customerSettings.CityEnabled)
                await _genericAttributeService.SaveAttribute(customer, SystemCustomerAttributeNames.City, model.City);
            if (_customerSettings.CountryEnabled)
                await _genericAttributeService.SaveAttribute(customer, SystemCustomerAttributeNames.CountryId, model.CountryId);
            if (_customerSettings.CountryEnabled && _customerSettings.StateProvinceEnabled)
                await _genericAttributeService.SaveAttribute(customer, SystemCustomerAttributeNames.StateProvinceId, model.StateProvinceId);
            if (_customerSettings.PhoneEnabled)
                await _genericAttributeService.SaveAttribute(customer, SystemCustomerAttributeNames.Phone, model.Phone);
            if (_customerSettings.FaxEnabled)
                await _genericAttributeService.SaveAttribute(customer, SystemCustomerAttributeNames.Fax, model.Fax);

            //custom customer attributes
            await _genericAttributeService.SaveAttribute(customer, SystemCustomerAttributeNames.CustomCustomerAttributes, model.CustomAttributes);

            //newsletter subscriptions
            if (!String.IsNullOrEmpty(customer.Email))
            {
                var allStores = await _storeService.GetAllStores();
                foreach (var store in allStores)
                {
                    var newsletterSubscription = await _newsLetterSubscriptionService
                        .GetNewsLetterSubscriptionByEmailAndStoreId(customer.Email, store.Id);
                    if (model.SelectedNewsletterSubscriptionStoreIds != null &&
                        model.SelectedNewsletterSubscriptionStoreIds.Contains(store.Id))
                    {
                        //subscribed
                        if (newsletterSubscription == null)
                        {
                            await _newsLetterSubscriptionService.InsertNewsLetterSubscription(new NewsLetterSubscription {
                                NewsLetterSubscriptionGuid = Guid.NewGuid(),
                                CustomerId = customer.Id,
                                Email = customer.Email,
                                Active = true,
                                StoreId = store.Id,
                                CreatedOnUtc = DateTime.UtcNow
                            });
                        }
                    }
                    else
                    {
                        //not subscribed
                        if (newsletterSubscription != null)
                        {
                            await _newsLetterSubscriptionService.DeleteNewsLetterSubscription(newsletterSubscription);
                        }
                    }
                }
            }
            var allCustomerRoles = await _customerService.GetAllCustomerRoles(showHidden: true);
            var newCustomerRoles = new List<CustomerRole>();
            foreach (var customerRole in allCustomerRoles)
                if (model.SelectedCustomerRoleIds != null && model.SelectedCustomerRoleIds.Contains(customerRole.Id))
                    newCustomerRoles.Add(customerRole);
            //customer roles
            foreach (var customerRole in allCustomerRoles)
            {
                //ensure that the current customer cannot add/remove to/from "Administrators" system role
                //if he's not an admin himself
                if (customerRole.SystemName == SystemCustomerRoleNames.Administrators &&
                    !_workContext.CurrentCustomer.IsAdmin())
                    continue;

                if (model.SelectedCustomerRoleIds != null &&
                    model.SelectedCustomerRoleIds.Contains(customerRole.Id))
                {
                    //new role
                    if (customer.CustomerRoles.Count(cr => cr.Id == customerRole.Id) == 0)
                    {
                        customer.CustomerRoles.Add(customerRole);
                    }
                }
                else
                {
                    //remove role
                    if (customer.CustomerRoles.Count(cr => cr.Id == customerRole.Id) > 0)
                        customer.CustomerRoles.Remove(customer.CustomerRoles.First(x => x.Id == customerRole.Id));
                }
            }
            await _customerService.UpdateCustomerinAdminPanel(customer);


            //ensure that a customer with a vendor associated is not in "Administrators" role
            //otherwise, he won't have access to the other functionality in admin area
            if (customer.IsAdmin() && !String.IsNullOrEmpty(customer.VendorId))
            {
                customer.VendorId = "";
                await _customerService.UpdateCustomerinAdminPanel(customer);
            }

            //ensure that a customer with a staff associated is not in "Administrators" role
            //otherwise, he won't have access to the other functionality in admin area
            if (customer.IsAdmin() && !String.IsNullOrEmpty(customer.StaffStoreId))
            {
                customer.StaffStoreId = "";
                await _customerService.UpdateCustomerinAdminPanel(customer);
            }

            //ensure that a customer in the Vendors role has a vendor account associated.
            //otherwise, he will have access to ALL products
            if (customer.IsVendor() && String.IsNullOrEmpty(customer.VendorId))
            {
                var vendorRole = customer
                    .CustomerRoles
                    .FirstOrDefault(x => x.SystemName == SystemCustomerRoleNames.Vendors);
                customer.CustomerRoles.Remove(vendorRole);
                vendorRole.CustomerId = customer.Id;
                await _customerService.DeleteCustomerRoleInCustomer(vendorRole);
            }

            //tags
            await SaveCustomerTags(customer, ParseCustomerTags(model.CustomerTags));

            //activity log
            await _customerActivityService.InsertActivity("EditCustomer", customer.Id, _localizationService.GetResource("ActivityLog.EditCustomer"), customer.Id);
            return customer;
        }

        public virtual async Task DeleteCustomer(Customer customer)
        {
            await _customerService.DeleteCustomer(customer);

            //remove newsletter subscription (if exists)
            foreach (var store in await _storeService.GetAllStores())
            {
                var subscription = await _newsLetterSubscriptionService.GetNewsLetterSubscriptionByEmailAndStoreId(customer.Email, store.Id);
                if (subscription != null)
                    await _newsLetterSubscriptionService.DeleteNewsLetterSubscription(subscription);
            }

            //activity log
            await _customerActivityService.InsertActivity("DeleteCustomer", customer.Id, _localizationService.GetResource("ActivityLog.DeleteCustomer"), customer.Id);
        }

        public virtual async Task DeleteSelected(IList<string> selectedIds)
        {
            var customers = new List<Customer>();
            customers.AddRange(await _customerService.GetCustomersByIds(selectedIds.ToArray()));
            for (var i = 0; i < customers.Count; i++)
            {
                var customer = customers[i];
                if (customer.Id != _workContext.CurrentCustomer.Id)
                {
                    await _customerService.DeleteCustomer(customer);
                }
                //activity log
                await _customerActivityService.InsertActivity("DeleteCustomer", customer.Id, _localizationService.GetResource("ActivityLog.DeleteCustomer"), customer.Id);
            }
        }

        public async Task SendEmail(Customer customer, CustomerModel.SendEmailModel model)
        {
            var emailAccount = await _emailAccountService.GetEmailAccountById(_emailAccountSettings.DefaultEmailAccountId);
            if (emailAccount == null)
                emailAccount = (await _emailAccountService.GetAllEmailAccounts()).FirstOrDefault();
            if (emailAccount == null)
                throw new LIMSException("Email account can't be loaded");

            var email = new QueuedEmail {
                Priority = QueuedEmailPriority.High,
                EmailAccountId = emailAccount.Id,
                FromName = emailAccount.DisplayName,
                From = emailAccount.Email,
                ToName = customer.GetFullName(),
                To = customer.Email,
                Subject = model.Subject,
                Body = model.Body,
                CreatedOnUtc = DateTime.UtcNow,
                DontSendBeforeDateUtc = (model.SendImmediately || !model.DontSendBeforeDate.HasValue) ?
                        null : (DateTime?)_dateTimeHelper.ConvertToUtcTime(model.DontSendBeforeDate.Value)
            };
            await _queuedEmailService.InsertQueuedEmail(email);
            await _customerActivityService.InsertActivity("CustomerAdmin.SendEmail", "", _localizationService.GetResource("ActivityLog.SendEmailfromAdminPanel"), customer, model.Subject);
        }

        public virtual async Task<(IEnumerable<CustomerModel.ActivityLogModel> activityLogModels, int totalCount)> PrepareActivityLogModel(string customerId, int pageIndex, int pageSize)
        {
            var activityLog = await _customerActivityService.GetAllActivities(null, null, null, customerId, "", null, pageIndex - 1, pageSize);
            var items = new List<CustomerModel.ActivityLogModel>();
            foreach (var x in activityLog)
            {
                var m = new CustomerModel.ActivityLogModel {
                    Id = x.Id,
                    ActivityLogTypeName = (await _customerActivityService.GetActivityTypeById(x.ActivityLogTypeId))?.Name,
                    Comment = x.Comment,
                    CreatedOn = _dateTimeHelper.ConvertToUserTime(x.CreatedOnUtc, DateTimeKind.Utc),
                    IpAddress = x.IpAddress
                };
                items.Add(m);
            }
            return (items, activityLog.TotalCount);
        }

    }
}
