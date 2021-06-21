using LIMS.Core.ModelBinding;
using LIMS.Domain;
using LIMS.Web.Areas.Admin.Models.AInR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Web.Areas.Admin.Models.Services
{
    public class PregnencyTerminationModel:BaseEntity
    {
        public AnimalRegistrationModel AnimalRegistration { get; set; }
        public ServiceProviderModel ServiceProvider { get; set; }
        [LIMSResourceDisplayName("Admin.PregnencyTermination.ServiceProviderId")]

        public string ServiceProvicerId { get; set; }
        [LIMSResourceDisplayName("Admin.PregnencyTermination.TerminationDate")]

        public string TerminationDate { get; set; }
        [LIMSResourceDisplayName("Admin.PregnencyTermination.TerminationType")]

        public string TerminationType { get; set; }
        [LIMSResourceDisplayName("Admin.PregnencyTermination.Reason")]

        public string Reason { get; set; }
        [LIMSResourceDisplayName("Admin.PregnencyTermination.TerminitedBy")]

        public string TerminitedBy { get; set; }
        [LIMSResourceDisplayName("Admin.Common.Fiscalyear")]

        public string FiscalYear { get; set; }

    }
}
