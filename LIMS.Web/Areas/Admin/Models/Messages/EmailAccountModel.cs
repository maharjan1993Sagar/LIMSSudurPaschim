using LIMS.Core.ModelBinding;
using LIMS.Core.Models;

namespace LIMS.Web.Areas.Admin.Models.Messages
{
    public partial class EmailAccountModel : BaseEntityModel
    {
        [LIMSResourceDisplayName("Admin.Configuration.EmailAccounts.Fields.Email")]
        public string Email { get; set; }

        [LIMSResourceDisplayName("Admin.Configuration.EmailAccounts.Fields.DisplayName")]
        public string DisplayName { get; set; }

        [LIMSResourceDisplayName("Admin.Configuration.EmailAccounts.Fields.Host")]
        public string Host { get; set; }

        [LIMSResourceDisplayName("Admin.Configuration.EmailAccounts.Fields.Port")]
        public int Port { get; set; }

        [LIMSResourceDisplayName("Admin.Configuration.EmailAccounts.Fields.Username")]
        public string Username { get; set; }

        [LIMSResourceDisplayName("Admin.Configuration.EmailAccounts.Fields.Password")]
        public string Password { get; set; }

        [LIMSResourceDisplayName("Admin.Configuration.EmailAccounts.Fields.UseServerCertificateValidation")]
        public bool UseServerCertificateValidation { get; set; }

        [LIMSResourceDisplayName("Admin.Configuration.EmailAccounts.Fields.SecureSocketOptions")]
        public int SecureSocketOptionsId { get; set; }

        [LIMSResourceDisplayName("Admin.Configuration.EmailAccounts.Fields.IsDefaultEmailAccount")]
        public bool IsDefaultEmailAccount { get; set; }

        [LIMSResourceDisplayName("Admin.Configuration.EmailAccounts.Fields.SendTestEmailTo")]
        public string SendTestEmailTo { get; set; }

    }
}