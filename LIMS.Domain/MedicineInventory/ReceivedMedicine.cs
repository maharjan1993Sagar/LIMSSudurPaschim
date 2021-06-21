using LIMS.Domain.BasicSetup;
using LIMS.Domain.BesicSetup;
using LIMS.Domain.MoAMAC;
using LIMS.Domain.Organizations;
using LIMS.Domain.Vaccination;
using System;
using System.Collections.Generic;
using System.Text;

namespace LIMS.Domain.MedicineInventory
{
    public class ReceivedMedicine:BaseEntity
    {
        public ReceivedMedicine()
        {
            this.ReceivedMedicineId = Guid.NewGuid();
        }
        public Guid ReceivedMedicineId { get; set; }
        public string MedicineId { get; set; }
        public VaccinationType VaccinationType { get; set; }
        public string Quantity { get; set; }
        public Unit Unit { get; set; }
        public Organization Organization { get; set; }
        public string OrganizationId { get; set; }
        public FiscalYear FiscalYear { get; set; }
        public string FiscalYearId { get; set; }

        public string Month { get; set; }

        public string CreatedBy { get; set; }

    }
}
