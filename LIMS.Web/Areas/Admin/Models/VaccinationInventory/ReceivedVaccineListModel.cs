using LIMS.Core.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Web.Areas.Admin.Models.VaccinationInventory
{
    public class ReceivedVaccineListModel
    {
        public string Id { get; set; }
        [LIMSResourceDisplayName("Admin.VaccinnationInventory.ReceivedVaccine.VaccineName")]
        public string VaccineName { get; set; }
        [LIMSResourceDisplayName("Admin.VaccinnationInventory.ReceivedVaccine.Quantity")]
        public string Quantity { get; set; }
        [LIMSResourceDisplayName("Admin.VaccinnationInventory.ReceivedVaccine.Unit")]
        public string Unit { get; set; }
        [LIMSResourceDisplayName("Admin.VaccinnationInventory.ReceivedVaccine.FiscalYear")]
        public string Fiscalyear { get; set; }
        [LIMSResourceDisplayName("Admin.VaccinnationInventory.ReceivedVaccine.Propose")]
        public string Propose { get; set; }
        [LIMSResourceDisplayName("Admin.VaccinnationInventory.ReceivedVaccine.ReceivedBy")]

        public string ReceivedBy { get; set; }
        [LIMSResourceDisplayName("Admin.VaccinnationInventory.ReceivedVaccine.Amount")]
        public string Amount { get; set; }
        [LIMSResourceDisplayName("Admin.VaccinnationInventory.ReceivedVaccine.Date")]
        public string Date { get; set; }
        public string UnitId { get; set; }
        public string FiscalYearId { get; set; }
    }
}
