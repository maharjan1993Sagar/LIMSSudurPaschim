using LIMS.Core.ModelBinding;
using LIMS.Domain;
using LIMS.Domain.BasicSetup;
using LIMS.Domain.BesicSetup;
using LIMS.Domain.MoAMAC;
using LIMS.Domain.Vaccination;
using System;
using System.Collections.Generic;
using System.Text;

namespace LIMS.Web.Areas.Admin.Models.MedicineInventory
{

    //is for distribution of medicine . Received medicine is in medicine progress.
    public class ReceivedMedicineModel:BaseEntity
    {
        [LIMSResourceDisplayName("Admin.MedicalInventory.ReceivedMedicine.Medicine")]

        public string   MedicineId { get; set; }
        [LIMSResourceDisplayName("Admin.MedicalInventory.ReceivedMedicine.Quantity")]

        public string Quantity { get; set; }

        public Unit Unit { get; set; }
        [LIMSResourceDisplayName("Admin.MedicalInventory.ReceivedMedicine.Unit")]

        public string UnitId { get; set; }
        [LIMSResourceDisplayName("Admin.MedicalInventory.ReceivedMedicine.OrganizationName")]

        public string OrganizationId { get; set; }
        public FiscalYear Fiscalyear { get; set; }
        [LIMSResourceDisplayName("Admin.MedicalInventory.ReceivedMedicine.FiscalYear")]

        public string FiscalYearId { get; set; }
        [LIMSResourceDisplayName("Admin.MedicalInventory.ReceivedMedicine.Month")]

        public string Month { get; set; }
        public List<VaccinationType> VaccinationType { get; set; }


        public string CreatedBy { get; set; }

    }
}
