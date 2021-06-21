using LIMS.Core.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Web.Areas.Admin.Models.AInR
{
    public class EarTagChange
    {
        [LIMSResourceDisplayName("Admin.EarTag.EarTagChange")]
        public string NewEartag { get; set; }
        [LIMSResourceDisplayName("Admin.EarTag.TagChangeDate")]
        public string TagChangeDate { get; set; }
        public AnimalRegistrationModel AnimalRegistration { get; set; }
    }
}
