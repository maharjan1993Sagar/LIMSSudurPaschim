using LIMS.Framework.Localization;
using LIMS.Core.ModelBinding;
using LIMS.Core.Models;
using System.Collections.Generic;

namespace LIMS.Web.Areas.Admin.Models.Messages
{
    public partial class InteractiveFormModel : BaseEntityModel, ILocalizedModel<InteractiveFormLocalizedModel>
    {
        public InteractiveFormModel()
        {
            Locales = new List<InteractiveFormLocalizedModel>();
            AvailableEmailAccounts = new List<EmailAccountModel>();
        }

        [LIMSResourceDisplayName("Admin.Promotions.InteractiveForms.Fields.Name")]
        public string Name { get; set; }

        [LIMSResourceDisplayName("Admin.Promotions.InteractiveForms.Fields.Body")]

        public string Body { get; set; }

        [LIMSResourceDisplayName("Admin.Promotions.InteractiveForms.Fields.EmailAccount")]
        public string EmailAccountId { get; set; }

        [LIMSResourceDisplayName("Admin.Promotions.InteractiveForms.Fields.AvailableTokens")]
        public string AvailableTokens { get; set; }
        public IList<EmailAccountModel> AvailableEmailAccounts { get; set; }

        public IList<InteractiveFormLocalizedModel> Locales { get; set; }

    }

    public partial class InteractiveFormLocalizedModel : ILocalizedModelLocal
    {
        public string LanguageId { get; set; }

        [LIMSResourceDisplayName("Admin.Promotions.InteractiveForms.Fields.Name")]
        public string Name { get; set; }

        [LIMSResourceDisplayName("Admin.Promotions.InteractiveForms.Fields.Body")]

        public string Body { get; set; }

    }

}