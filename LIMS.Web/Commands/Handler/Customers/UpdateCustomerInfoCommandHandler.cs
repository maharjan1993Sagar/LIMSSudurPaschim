using LIMS.Domain.Customers;
using LIMS.Domain.Localization;
using LIMS.Domain.Messages;
using LIMS.Services.Authentication;
using LIMS.Services.Common;
using LIMS.Services.Customers;
using LIMS.Services.Helpers;
using LIMS.Services.Messages;
using LIMS.Web.Commands.Models.Customers;
using MediatR;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace LIMS.Web.Commands.Handler.Customers
{
    public class UpdateCustomerInfoCommandHandler : IRequestHandler<UpdateCustomerInfoCommand, bool>
    {
        private readonly ICustomerRegistrationService _customerRegistrationService;
        private readonly ILIMSAuthenticationService _authenticationService;
        private readonly IGenericAttributeService _genericAttributeService;
        private readonly IWorkflowMessageService _workflowMessageService;
        private readonly INewsLetterSubscriptionService _newsLetterSubscriptionService;

        private readonly DateTimeSettings _dateTimeSettings;
        private readonly CustomerSettings _customerSettings;
        private readonly LocalizationSettings _localizationSettings;

        public UpdateCustomerInfoCommandHandler(
            ICustomerRegistrationService customerRegistrationService,
            ILIMSAuthenticationService authenticationService,
            IGenericAttributeService genericAttributeService,
            IWorkflowMessageService workflowMessageService,
            INewsLetterSubscriptionService newsLetterSubscriptionService,
            DateTimeSettings dateTimeSettings,
            CustomerSettings customerSettings,
            LocalizationSettings localizationSettings)
        {
            _customerRegistrationService = customerRegistrationService;
            _authenticationService = authenticationService;
            _genericAttributeService = genericAttributeService;
            _workflowMessageService = workflowMessageService;
            _newsLetterSubscriptionService = newsLetterSubscriptionService;
            _dateTimeSettings = dateTimeSettings;
            _customerSettings = customerSettings;
            _localizationSettings = localizationSettings;
        }

        public async Task<bool> Handle(UpdateCustomerInfoCommand request, CancellationToken cancellationToken)
        {
            //username 
            if (_customerSettings.UsernamesEnabled && _customerSettings.AllowUsersToChangeUsernames)
            {
                if (!request.Customer.Username.Equals(request.Model.Username.Trim(), StringComparison.OrdinalIgnoreCase))
                {
                    //change username
                    await _customerRegistrationService.SetUsername(request.Customer, request.Model.Username.Trim());
                    //re-authenticate
                    if (request.OriginalCustomerIfImpersonated == null)
                        await _authenticationService.SignIn(request.Customer, true);
                }
            }
            //email
            if (!request.Customer.Email.Equals(request.Model.Email.Trim(), StringComparison.OrdinalIgnoreCase) && _customerSettings.AllowUsersToChangeEmail)
            {
                //change email
                await _customerRegistrationService.SetEmail(request.Customer, request.Model.Email.Trim());
                //re-authenticate (if usernames are disabled)
                //do not authenticate users in impersonation mode
                if (request.OriginalCustomerIfImpersonated == null)
                {
                    //re-authenticate (if usernames are disabled)
                    if (!_customerSettings.UsernamesEnabled)
                        await _authenticationService.SignIn(request.Customer, true);
                }
            }

            //form fields
            await UpdateFormFields(request);

            //newsletter
            if (_customerSettings.NewsletterEnabled)
            {
                await UpdateNewsletter(request);
            }

            //save customer attributes
            await _genericAttributeService.SaveAttribute(request.Customer, SystemCustomerAttributeNames.CustomCustomerAttributes, request.CustomerAttributesXml);

            return true;

        }

        private async Task UpdateTax(UpdateCustomerInfoCommand request)
        {
            var prevVatNumber = await request.Customer.GetAttribute<string>(_genericAttributeService, SystemCustomerAttributeNames.VatNumber);

            await _genericAttributeService.SaveAttribute(request.Customer, SystemCustomerAttributeNames.VatNumber, request.Model.VatNumber);
        }

        private async Task UpdateFormFields(UpdateCustomerInfoCommand request)
        {
            //properties
            if (_dateTimeSettings.AllowCustomersToSetTimeZone)
            {
                await _genericAttributeService.SaveAttribute(request.Customer, SystemCustomerAttributeNames.TimeZoneId, request.Model.TimeZoneId);
            }

            if (_customerSettings.GenderEnabled)
                await _genericAttributeService.SaveAttribute(request.Customer, SystemCustomerAttributeNames.Gender, request.Model.Gender);

            await _genericAttributeService.SaveAttribute(request.Customer, SystemCustomerAttributeNames.FirstName, request.Model.FirstName);
            await _genericAttributeService.SaveAttribute(request.Customer, SystemCustomerAttributeNames.LastName, request.Model.LastName);
            if (_customerSettings.DateOfBirthEnabled)
            {
                DateTime? dateOfBirth = request.Model.ParseDateOfBirth();
                await _genericAttributeService.SaveAttribute(request.Customer, SystemCustomerAttributeNames.DateOfBirth, dateOfBirth);
            }
            if (_customerSettings.CompanyEnabled)
                await _genericAttributeService.SaveAttribute(request.Customer, SystemCustomerAttributeNames.Company, request.Model.Company);
            if (_customerSettings.StreetAddressEnabled)
                await _genericAttributeService.SaveAttribute(request.Customer, SystemCustomerAttributeNames.StreetAddress, request.Model.StreetAddress);
            if (_customerSettings.StreetAddress2Enabled)
                await _genericAttributeService.SaveAttribute(request.Customer, SystemCustomerAttributeNames.StreetAddress2, request.Model.StreetAddress2);
            if (_customerSettings.ZipPostalCodeEnabled)
                await _genericAttributeService.SaveAttribute(request.Customer, SystemCustomerAttributeNames.ZipPostalCode, request.Model.ZipPostalCode);
            if (_customerSettings.CityEnabled)
                await _genericAttributeService.SaveAttribute(request.Customer, SystemCustomerAttributeNames.City, request.Model.City);
            if (_customerSettings.CountryEnabled)
                await _genericAttributeService.SaveAttribute(request.Customer, SystemCustomerAttributeNames.CountryId, request.Model.CountryId);
            if (_customerSettings.CountryEnabled && _customerSettings.StateProvinceEnabled)
                await _genericAttributeService.SaveAttribute(request.Customer, SystemCustomerAttributeNames.StateProvinceId, request.Model.StateProvinceId);
            if (_customerSettings.PhoneEnabled)
                await _genericAttributeService.SaveAttribute(request.Customer, SystemCustomerAttributeNames.Phone, request.Model.Phone);
            if (_customerSettings.FaxEnabled)
                await _genericAttributeService.SaveAttribute(request.Customer, SystemCustomerAttributeNames.Fax, request.Model.Fax);
        }

        private async Task UpdateNewsletter(UpdateCustomerInfoCommand request)
        {
            var categories = new List<string>();
            foreach (string formKey in request.Form.Keys)
            {
                if (formKey.Contains("customernewsletterCategory_"))
                {
                    try
                    {
                        var category = formKey.Split('_')[1];
                        categories.Add(category);
                    }
                    catch { }
                }
            }
            //save newsletter value
            var newsletter = await _newsLetterSubscriptionService.GetNewsLetterSubscriptionByEmailAndStoreId(request.Customer.Email, request.Store.Id);

            if (newsletter != null)
            {
                newsletter.Categories.Clear();
                categories.ForEach(x => newsletter.Categories.Add(x));

                if (request.Model.Newsletter)
                {
                    newsletter.Active = true;
                    await _newsLetterSubscriptionService.UpdateNewsLetterSubscription(newsletter);
                }
                else
                {
                    newsletter.Active = false;
                    await _newsLetterSubscriptionService.UpdateNewsLetterSubscription(newsletter);
                }
            }
            else
            {
                if (request.Model.Newsletter)
                {
                    var newsLetterSubscription = new NewsLetterSubscription {
                        NewsLetterSubscriptionGuid = Guid.NewGuid(),
                        Email = request.Customer.Email,
                        CustomerId = request.Customer.Id,
                        Active = true,
                        StoreId = request.Store.Id,
                        CreatedOnUtc = DateTime.UtcNow
                    };
                    categories.ForEach(x => newsLetterSubscription.Categories.Add(x));
                    await _newsLetterSubscriptionService.InsertNewsLetterSubscription(newsLetterSubscription);
                }
            }
        }
    }
}
