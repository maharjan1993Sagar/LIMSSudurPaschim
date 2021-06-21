using LIMS.Core;
using LIMS.Domain.Customers;
using LIMS.Domain.Stores;
using LIMS.Framework.Controllers;
using LIMS.Framework.Mvc.Filters;
using LIMS.Framework.Security.Captcha;
using LIMS.Services.Authentication;
using LIMS.Services.Authentication.External;
using LIMS.Services.Common;
using LIMS.Services.Customers;
using LIMS.Services.Directory;
using LIMS.Services.ExportImport;
using LIMS.Services.Localization;
using LIMS.Services.Messages;
using LIMS.Services.Notifications.Customers;
using LIMS.Web.Commands.Models.Customers;
using LIMS.Web.Extensions;
using LIMS.Web.Features.Models.Common;
using LIMS.Web.Features.Models.Customers;
using LIMS.Web.Models.Customer;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Web.Controllers
{
    public partial class CustomerController : BasePublicController
    {
        #region Fields

        private readonly ILIMSAuthenticationService _authenticationService;
        private readonly ILocalizationService _localizationService;
        private readonly IWorkContext _workContext;
        private readonly IStoreContext _storeContext;
        private readonly ICustomerService _customerService;
        private readonly IGenericAttributeService _genericAttributeService;
        private readonly ICustomerRegistrationService _customerRegistrationService;
        private readonly ICountryService _countryService;
        private readonly IMediator _mediator;
        private readonly CustomerSettings _customerSettings;
        private readonly CaptchaSettings _captchaSettings;

        #endregion

        #region Ctor

        public CustomerController(
            ILIMSAuthenticationService authenticationService,
            ILocalizationService localizationService,
            IWorkContext workContext,
            IStoreContext storeContext,
            ICustomerService customerService,
            IGenericAttributeService genericAttributeService,
            ICustomerRegistrationService customerRegistrationService,
            ICountryService countryService,
            IMediator mediator,
            CaptchaSettings captchaSettings,
            CustomerSettings customerSettings)
        {
            _authenticationService = authenticationService;
            _localizationService = localizationService;
            _workContext = workContext;
            _storeContext = storeContext;
            _customerService = customerService;
            _genericAttributeService = genericAttributeService;
            _customerRegistrationService = customerRegistrationService;
            _customerSettings = customerSettings;
            _countryService = countryService;
            _captchaSettings = captchaSettings;
            _mediator = mediator;
        }

        #endregion

        #region Login / logout

        ////available even when navigation is not allowed
        //[CheckAccessPublicStore(true)]
        //public virtual IActionResult Login(bool? checkoutAsGuest)
        //{
        //    return RedirectToAction("", "Login", new { area = "Admin" });
        //    //var model = new LoginModel();
        //    //model.UsernamesEnabled = _customerSettings.UsernamesEnabled;
        //    //model.CheckoutAsGuest = checkoutAsGuest.GetValueOrDefault();
        //    //model.DisplayCaptcha = _captchaSettings.Enabled && _captchaSettings.ShowOnLoginPage;
        //    //return View(model);
        //}

        //[HttpPost]
        ////available even when navigation is not allowed
        //[CheckAccessPublicStore(true)]
        //[ValidateCaptcha]
        //[AutoValidateAntiforgeryToken]
        //public virtual async Task<IActionResult> Login(LoginModel model, string returnUrl, bool captchaValid,
        //               )
        //{
        //    //validate CAPTCHA
        //    if (_captchaSettings.Enabled && _captchaSettings.ShowOnLoginPage && !captchaValid)
        //    {
        //        ModelState.AddModelError("", _captchaSettings.GetWrongCaptchaMessage(_localizationService));
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        if (_customerSettings.UsernamesEnabled && model.Username != null)
        //        {
        //            model.Username = model.Username.Trim();
        //        }
        //        var loginResult = await _customerRegistrationService.ValidateCustomer(_customerSettings.UsernamesEnabled ? model.Username : model.Email, model.Password);
        //        switch (loginResult)
        //        {
        //            case CustomerLoginResults.Successful:
        //                {
        //                    var customer = _customerSettings.UsernamesEnabled ? await _customerService.GetCustomerByUsername(model.Username) : await _customerService.GetCustomerByEmail(model.Email);
        //                    //sign in
        //                    return await SignInAction(shoppingCartService, customer, returnUrl);
        //                }
        //            case CustomerLoginResults.RequiresTwoFactor:
        //                {
        //                    var userName = _customerSettings.UsernamesEnabled ? model.Username : model.Email;

        //                    HttpContext.Session.SetString("RequiresTwoFactor", userName);

        //                    return RedirectToRoute("TwoFactorAuthorization");
        //                }

        //            case CustomerLoginResults.CustomerNotExist:
        //                ModelState.AddModelError("", _localizationService.GetResource("Account.Login.WrongCredentials.CustomerNotExist"));
        //                break;
        //            case CustomerLoginResults.Deleted:
        //                ModelState.AddModelError("", _localizationService.GetResource("Account.Login.WrongCredentials.Deleted"));
        //                break;
        //            case CustomerLoginResults.NotActive:
        //                ModelState.AddModelError("", _localizationService.GetResource("Account.Login.WrongCredentials.NotActive"));
        //                break;
        //            case CustomerLoginResults.NotRegistered:
        //                ModelState.AddModelError("", _localizationService.GetResource("Account.Login.WrongCredentials.NotRegistered"));
        //                break;
        //            case CustomerLoginResults.LockedOut:
        //                ModelState.AddModelError("", _localizationService.GetResource("Account.Login.WrongCredentials.LockedOut"));
        //                break;
        //            case CustomerLoginResults.WrongPassword:
        //                ModelState.AddModelError("", _localizationService.GetResource("Account.Login.WrongCredentials"));
        //                break;
        //            default:
        //                ModelState.AddModelError("", _localizationService.GetResource("Account.Login.WrongCredentials"));
        //                break;
        //        }
        //    }

        //    //If we got this far, something failed, redisplay form
        //    model.UsernamesEnabled = _customerSettings.UsernamesEnabled;
        //    model.DisplayCaptcha = _captchaSettings.Enabled && _captchaSettings.ShowOnLoginPage;

        //    return View(model);
        //}

        //public async Task<IActionResult> TwoFactorAuthorization([FromServices] ITwoFactorAuthenticationService twoFactorAuthenticationService)
        //{
        //    if (!_customerSettings.TwoFactorAuthenticationEnabled)
        //        return RedirectToRoute("Login");

        //    var username = HttpContext.Session.GetString("RequiresTwoFactor");
        //    if (string.IsNullOrEmpty(username))
        //        return RedirectToRoute("HomePage");

        //    var customer = _customerSettings.UsernamesEnabled ? await _customerService.GetCustomerByUsername(username) : await _customerService.GetCustomerByEmail(username);
        //    if (customer == null)
        //        return RedirectToRoute("HomePage");

        //    if (!customer.GetAttributeFromEntity<bool>(SystemCustomerAttributeNames.TwoFactorEnabled))
        //        return RedirectToRoute("HomePage");

        //    if (_customerSettings.TwoFactorAuthenticationType != TwoFactorAuthenticationType.AppVerification)
        //    {
        //        await twoFactorAuthenticationService.GenerateCodeSetup("", customer, _workContext.WorkingLanguage, _customerSettings.TwoFactorAuthenticationType);
        //    }

        //    return View();
        //}

        //[HttpPost]
        //public async Task<IActionResult> TwoFactorAuthorization(string token,
        //    [FromServices] IShoppingCartService shoppingCartService,
        //    [FromServices] ITwoFactorAuthenticationService twoFactorAuthenticationService
        //    )
        //{
        //    if (!_customerSettings.TwoFactorAuthenticationEnabled)
        //        return RedirectToRoute("Login");

        //    var username = HttpContext.Session.GetString("RequiresTwoFactor");
        //    if (string.IsNullOrEmpty(username))
        //        return RedirectToRoute("HomePage");

        //    var customer = _customerSettings.UsernamesEnabled ? await _customerService.GetCustomerByUsername(username) : await _customerService.GetCustomerByEmail(username);
        //    if (customer == null)
        //        return RedirectToRoute("Login");

        //    if (string.IsNullOrEmpty(token))
        //    {
        //        ModelState.AddModelError("", _localizationService.GetResource("Account.TwoFactorAuth.SecurityCodeIsRequired"));
        //    }
        //    else
        //    {
        //        var secretKey = customer.GetAttributeFromEntity<string>(SystemCustomerAttributeNames.TwoFactorSecretKey);
        //        if (await twoFactorAuthenticationService.AuthenticateTwoFactor(secretKey, token, customer, _customerSettings.TwoFactorAuthenticationType))
        //        {
        //            //remove session
        //            HttpContext.Session.Remove("RequiresTwoFactor");

        //            //sign in
        //            return await SignInAction(shoppingCartService, customer);
        //        }
        //        ModelState.AddModelError("", _localizationService.GetResource("Account.TwoFactorAuth.WrongSecurityCode"));
        //    }

        //    return View();
        //}

        //protected async Task<IActionResult> SignInAction(IShoppingCartService shoppingCartService, Customer customer, string returnUrl = null)
        //{
        //    //migrate shopping cart
        //    await shoppingCartService.MigrateShoppingCart(_workContext.CurrentCustomer, customer, true);

        //    //sign in new customer
        //    await _authenticationService.SignIn(customer, true);

        //    //raise event       
        //    await _mediator.Publish(new CustomerLoggedInEvent(customer));

        //    if (string.IsNullOrEmpty(returnUrl) || !Url.IsLocalUrl(returnUrl))
        //        return RedirectToRoute("HomePage");

        //    return Redirect(returnUrl);
        //}

        //available even when a store is closed
        [CheckAccessClosedStore(true)]
        //available even when navigation is not allowed
        [CheckAccessPublicStore(true)]
        public virtual async Task<IActionResult> Logout([FromServices] StoreInformationSettings storeInformationSettings)
        {
            if (_workContext.OriginalCustomerIfImpersonated != null)
            {
                //logout impersonated customer
                await _genericAttributeService.SaveAttribute<int?>(_workContext.OriginalCustomerIfImpersonated,
                    SystemCustomerAttributeNames.ImpersonatedCustomerId, null);

                //redirect back to customer details page (admin area)
                return RedirectToAction("Edit", "Customer", new { id = _workContext.CurrentCustomer.Id, area = "Admin" });

            }

            //standard logout 
            await _authenticationService.SignOut();

            //raise event       
            await _mediator.Publish(new CustomerLoggedOutEvent(_workContext.CurrentCustomer));

            //EU Cookie
            if (storeInformationSettings.DisplayEuCookieLawWarning)
            {
                //the cookie law message should not pop up immediately after logout.
                //otherwise, the user will have to click it again...
                //and thus next visitor will not click it... so violation for that cookie law..
                //the only good solution in this case is to store a temporary variable
                //indicating that the EU cookie popup window should not be displayed on the next page open (after logout redirection to homepage)
                //but it'll be displayed for further page loads
                TempData["LIMS.IgnoreEuCookieLawWarning"] = true;
            }
            return RedirectToRoute("HomePage");
        }

        #endregion

        #region Password recovery

        

        #endregion

    }
}
