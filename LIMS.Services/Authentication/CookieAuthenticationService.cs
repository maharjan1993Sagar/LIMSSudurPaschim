﻿using LIMS.Core;
using LIMS.Core.Configuration;
using LIMS.Domain.Customers;
using LIMS.Services.Common;
using LIMS.Services.Customers;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace LIMS.Services.Authentication
{
    /// <summary>
    /// Represents service using cookie middleware for the authentication
    /// </summary>
    public partial class CookieAuthenticationService : ILIMSAuthenticationService
    {
        #region Const

        private string CUSTOMER_COOKIE_NAME => $"{_LIMSConfig.CookiePrefix}Customer";

        #endregion

        #region Fields

        private readonly CustomerSettings _customerSettings;
        private readonly ICustomerService _customerService;
        private readonly IGenericAttributeService _genericAttributeService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly LIMSConfig _LIMSConfig;
        private Customer _cachedCustomer;

        #endregion

        #region Ctor

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="customerSettings">Customer settings</param>
        /// <param name="customerService">Customer service</param>
        /// <param name="genericAttributeService">Generic sttribute service</param>
        /// <param name="httpContextAccessor">HTTP context accessor</param>
        /// <param name="LIMSConfig">AppConfig</param>
        public CookieAuthenticationService(CustomerSettings customerSettings,
        ICustomerService customerService,
            IGenericAttributeService genericAttributeService,
            IHttpContextAccessor httpContextAccessor,
            LIMSConfig LIMSConfig)
        {
            _customerSettings = customerSettings;
            _customerService = customerService;
            _genericAttributeService = genericAttributeService;
            _httpContextAccessor = httpContextAccessor;
            _LIMSConfig = LIMSConfig;

        }

        #endregion

        #region Methods

        /// <summary>
        /// Sign in
        /// </summary>
        /// <param name="customer">Customer</param>
        /// <param name="isPersistent">Whether the authentication session is persisted across multiple requests</param>
        public virtual async Task SignIn(Customer customer, bool isPersistent)
        {
            if (customer == null)
                throw new ArgumentNullException(nameof(customer));

            //create claims for customer's username and email
            var claims = new List<Claim>();

            if (!string.IsNullOrEmpty(customer.Username))
                claims.Add(new Claim(ClaimTypes.Name, customer.Username, ClaimValueTypes.String, _LIMSConfig.CookieClaimsIssuer));

            if (!string.IsNullOrEmpty(customer.Email))
                claims.Add(new Claim(ClaimTypes.Email, customer.Email, ClaimValueTypes.Email, _LIMSConfig.CookieClaimsIssuer));

            //add token
            var passwordtoken = await customer.GetAttribute<string>(_genericAttributeService, SystemCustomerAttributeNames.PasswordToken);
            if (string.IsNullOrEmpty(passwordtoken))
            {
                var passwordguid = Guid.NewGuid().ToString();
                await _genericAttributeService.SaveAttribute(customer, SystemCustomerAttributeNames.PasswordToken, passwordguid);
                claims.Add(new Claim(ClaimTypes.UserData, passwordguid, ClaimValueTypes.String, _LIMSConfig.CookieClaimsIssuer));
            }
            else
                claims.Add(new Claim(ClaimTypes.UserData, passwordtoken, ClaimValueTypes.String, _LIMSConfig.CookieClaimsIssuer));

            //create principal for the current authentication scheme
            var userIdentity = new ClaimsIdentity(claims, LIMSCookieAuthenticationDefaults.AuthenticationScheme);
            var userPrincipal = new ClaimsPrincipal(userIdentity);

            //set value indicating whether session is persisted and the time at which the authentication was issued
            var authenticationProperties = new AuthenticationProperties {
                IsPersistent = isPersistent,
                IssuedUtc = DateTime.UtcNow,
                ExpiresUtc = DateTime.UtcNow.AddHours(CommonHelper.CookieAuthExpires)
            };

            //sign in
            await _httpContextAccessor.HttpContext.SignInAsync(LIMSCookieAuthenticationDefaults.AuthenticationScheme, userPrincipal, authenticationProperties);

            //cache authenticated customer
            _cachedCustomer = customer;
        }

        /// <summary>
        /// Sign out
        /// </summary>
        public virtual async Task SignOut()
        {
            //reset cached customer
            _cachedCustomer = null;

            //and sign out from the current authentication scheme
            await _httpContextAccessor.HttpContext.SignOutAsync(LIMSCookieAuthenticationDefaults.AuthenticationScheme);
        }

        /// <summary>
        /// Get authenticated customer
        /// </summary>
        /// <returns>Customer</returns>
        public virtual async Task<Customer> GetAuthenticatedCustomer()
        {
            //whether there is a cached customer
            if (_cachedCustomer != null)
                return _cachedCustomer;

            //try to get authenticated user identity
            var authenticateResult = await _httpContextAccessor.HttpContext.AuthenticateAsync(LIMSCookieAuthenticationDefaults.AuthenticationScheme);
            if (!authenticateResult.Succeeded)
                return null;

            Customer customer = null;
            if (_customerSettings.UsernamesEnabled)
            {
                //try to get customer by username
                var usernameClaim = authenticateResult.Principal.FindFirst(claim => claim.Type == ClaimTypes.Name
                    && claim.Issuer.Equals(_LIMSConfig.CookieClaimsIssuer, StringComparison.InvariantCultureIgnoreCase));
                if (usernameClaim != null)
                    customer = await _customerService.GetCustomerByUsername(usernameClaim.Value);
            }
            else
            {
                //try to get customer by email
                var emailClaim = authenticateResult.Principal.FindFirst(claim => claim.Type == ClaimTypes.Email
                    && claim.Issuer.Equals(_LIMSConfig.CookieClaimsIssuer, StringComparison.InvariantCultureIgnoreCase));
                if (emailClaim != null)
                    customer = await _customerService.GetCustomerByEmail(emailClaim.Value);
            }

            if (customer != null)
            {
                var passwordtoken = customer.GetAttributeFromEntity<string>(SystemCustomerAttributeNames.PasswordToken);
                if (!string.IsNullOrEmpty(passwordtoken))
                {
                    var tokenClaim = authenticateResult.Principal.FindFirst(claim => claim.Type == ClaimTypes.UserData
                        && claim.Issuer.Equals(_LIMSConfig.CookieClaimsIssuer, StringComparison.InvariantCultureIgnoreCase));
                    if (tokenClaim == null || tokenClaim.Value != passwordtoken)
                    {
                        customer = null;
                    }
                }
            }
            //whether the found customer is available
            if (customer == null || !customer.Active || customer.Deleted || !customer.IsRegistered())
                return null;

            //cache authenticated customer
            _cachedCustomer = customer;

            return _cachedCustomer;
        }

        /// <summary>
        /// Get customer cookie
        /// </summary>
        /// <returns>String value of cookie</returns>
        public virtual Task<string> GetCustomerGuid()
        {
            if (_httpContextAccessor.HttpContext == null || _httpContextAccessor.HttpContext.Request == null)
                return Task.FromResult<string>(null);

            return Task.FromResult(_httpContextAccessor.HttpContext.Request.Cookies[CUSTOMER_COOKIE_NAME]);
        }

        /// <summary>
        /// Set customer cookie
        /// </summary>
        /// <param name="customerGuid">Guid of the customer</param>
        public virtual Task SetCustomerGuid(Guid customerGuid)
        {
            if (_httpContextAccessor.HttpContext == null || _httpContextAccessor.HttpContext.Response == null)
                return Task.CompletedTask;

            //delete current cookie value
            _httpContextAccessor.HttpContext.Response.Cookies.Delete(CUSTOMER_COOKIE_NAME);

            //get date of cookie expiration
            var cookieExpiresDate = DateTime.UtcNow.AddHours(CommonHelper.CookieAuthExpires);

            //if passed guid is empty set cookie as expired
            if (customerGuid == Guid.Empty)
                cookieExpiresDate = DateTime.UtcNow.AddMonths(-1);

            //set new cookie value
            var options = new CookieOptions {
                HttpOnly = true,
                Expires = cookieExpiresDate
            };
            _httpContextAccessor.HttpContext.Response.Cookies.Append(CUSTOMER_COOKIE_NAME, customerGuid.ToString(), options);

            return Task.CompletedTask;
        }
        #endregion
    }
}