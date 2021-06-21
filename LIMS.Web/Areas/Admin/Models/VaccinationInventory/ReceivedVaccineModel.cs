using LIMS.Core.ModelBinding;
using LIMS.Domain.Vaccination;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Web.Areas.Admin.Models.VaccinationInventory
{
    public class ReceivedVaccineModel
    {
        [LIMSResourceDisplayName("Admin.VaccinnationInventory.ReceivedVaccine.VaccinationType")]
        public string VaccinationTypeId { get; set; }
        [LIMSResourceDisplayName("Admin.VaccinnationInventory.ReceivedVaccine.Quantity")]
        public string Quantity { get; set; }
        [LIMSResourceDisplayName("Admin.VaccinnationInventory.ReceivedVaccine.Unit")]
        public string UnitId { get; set; }
        [LIMSResourceDisplayName("Admin.VaccinnationInventory.ReceivedVaccine.FiscalYear")]
        public string FiscalYearId { get; set; }
        [LIMSResourceDisplayName("Admin.VaccinnationInventory.ReceivedVaccine.Propose")]
        public string Propose { get; set; }
        [LIMSResourceDisplayName("Admin.VaccinnationInventory.ReceivedVaccine.ReceivedBy")]

        public string ReceivedBy { get; set; }
        [LIMSResourceDisplayName("Admin.VaccinnationInventory.ReceivedVaccine.Amount")]
        public string Amount { get; set; }
        [LIMSResourceDisplayName("Admin.VaccinnationInventory.ReceivedVaccine.Date")]
        [UIHint("date")]
        public string Date { get; set; }
        [LIMSResourceDisplayName("Admin.VaccinnationInventory.ReceivedVaccine.ReceivedFrom")]
        public string ReceivedFrom { get; set; }
        public List<VaccinationType> VaccinationType { get; set; }
    }
}
