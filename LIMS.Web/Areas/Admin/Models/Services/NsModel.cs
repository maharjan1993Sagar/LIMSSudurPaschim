using LIMS.Core.ModelBinding;
using LIMS.Web.Areas.Admin.Models.AInR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Web.Areas.Admin.Models.Services
{
    public class NsModel
    {
        public AnimalRegistrationModel AnimalRegistration { get; set; }
       
        [LIMSResourceDisplayName("Admin.Ns.Date")]

        public DateTime Date { get; set; }

        [LIMSResourceDisplayName("Admin.Ns.BullId")]

        public string BullId { get; set; }




    }
}
