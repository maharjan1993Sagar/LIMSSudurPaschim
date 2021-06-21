using LIMS.Core.ModelBinding;
using LIMS.Core.Models;
using System.ComponentModel.DataAnnotations;

namespace LIMS.Web.Models.Common
{
    public partial class ContactVendorModel : BaseModel
    {
        public string VendorId { get; set; }
        public string VendorName { get; set; }

        [DataType(DataType.EmailAddress)]
        [LIMSResourceDisplayName("ContactVendor.Email")]
        public string Email { get; set; }

        [LIMSResourceDisplayName("ContactVendor.Subject")]
        public string Subject { get; set; }
        public bool SubjectEnabled { get; set; }

        [LIMSResourceDisplayName("ContactVendor.Enquiry")]
        public string Enquiry { get; set; }

        [LIMSResourceDisplayName("ContactVendor.FullName")]
        public string FullName { get; set; }

        public bool SuccessfullySent { get; set; }
        public string Result { get; set; }

        public bool DisplayCaptcha { get; set; }
    }
}