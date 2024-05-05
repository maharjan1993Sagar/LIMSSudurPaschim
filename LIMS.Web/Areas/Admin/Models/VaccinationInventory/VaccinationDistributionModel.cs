using LIMS.Core.ModelBinding;
using LIMS.Domain.Vaccination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Web.Areas.Admin.Models.VaccinationInventory
{
    public class VaccinationDistributionModel
    {
        [LIMSResourceDisplayName("Admin.MedicalInventory.ReceivedMedicine.Medicine")]

        public string MedicineId { get; set; }
        [LIMSResourceDisplayName("Admin.MedicalInventory.ReceivedMedicine.Quantity")]

        public string Quantity { get; set; }

        [LIMSResourceDisplayName("Admin.MedicalInventory.ReceivedMedicine.Unit")]

        public string UnitId { get; set; }
        [LIMSResourceDisplayName("Admin.MedicalInventory.ReceivedMedicine.OrganizationName")]

        public string OrganizationId { get; set; }
        [LIMSResourceDisplayName("Admin.MedicalInventory.ReceivedMedicine.FiscalYear")]

        public string FiscalYearId { get; set; }
        [LIMSResourceDisplayName("Admin.MedicalInventory.ReceivedMedicine.Month")]

        public string Month { get; set; }
        [LIMSResourceDisplayName("Admin.MedicalInventory.ReceivedMedicine.Propose")]

        public string Propose { get; set; }
        public List<VaccinationType> VaccinationType { get; set; }


        public string CreatedBy { get; set; }
        public string Date { get; set; }

    }
}
