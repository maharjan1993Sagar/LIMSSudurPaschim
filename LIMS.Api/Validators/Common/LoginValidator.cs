using FluentValidation;
using LIMS.Core.Configuration;
using LIMS.Core.Validators;
using LIMS.Services.Customers;
using LIMS.Services.Security;
using LIMS.Api.Models.Common;
using System.Collections.Generic;

namespace LIMS.Api.Validators.Common
{
    public class LoginValidator : BaseLIMSValidator<LoginModel>
    {
        public LoginValidator(
            IEnumerable<IValidatorConsumer<LoginModel>> validators,
            ApiConfig apiConfig, ICustomerService customerService, IUserApiService userApiService, IEncryptionService encryptionService)
            : base(validators)
        {
            if (!apiConfig.Enabled)
                RuleFor(x => x).Must((x) => false).WithMessage("API is disabled");

            RuleFor(x => x.Email).NotEmpty().WithMessage("Email is required");
            RuleFor(x => x.Password).NotEmpty().WithMessage("Password is required");
            RuleFor(x => x).MustAsync(async (x, context) =>
            {
                if (!string.IsNullOrEmpty(x.Email))
                {
                    var userapi = await userApiService.GetUserByEmail(x.Email.ToLowerInvariant());
                    if (userapi != null && userapi.IsActive)
                    {
                        var base64EncodedBytes = System.Convert.FromBase64String(x.Password);
                        var password = System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
                        if (userapi.Password == encryptionService.EncryptText(password, userapi.PrivateKey))
                            return true;
                    }
                }
                return false;
            }).WithMessage(("User not exists or password is wrong"));
            RuleFor(x => x).MustAsync(async (x, context) =>
            {
                if (!string.IsNullOrEmpty(x.Email))
                {
                    var customer = await customerService.GetCustomerByEmail(x.Email.ToLowerInvariant());
                    if (customer != null && customer.Active && !customer.IsSystemAccount)
                        return true;
                }
                return false;
            }).WithMessage("Customer not exist");
        }
    }
}
