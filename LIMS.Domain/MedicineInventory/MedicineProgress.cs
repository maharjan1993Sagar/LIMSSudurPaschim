using LIMS.Domain.BasicSetup;
using LIMS.Domain.BesicSetup;
using LIMS.Domain.Vaccination;
using System;
using System.Collections.Generic;
using System.Text;

namespace LIMS.Domain.MedicineInventory
{
    public class MedicineProgress:BaseEntity
    {
        public MedicineProgress()
        {
            this.MedicineProgressId = Guid.NewGuid();
        }
        public Guid MedicineProgressId { get; set; }
        public string MedicineId { get; set; }
        public VaccinationType Vaccination { get; set; }
        public string Quantity { get; set; }
        //  public string DeleiveredMedicine { get; set; }
        public string Month { get; set; }
        public string FiscalYearId { get; set; }
        public FiscalYear FiscalYear { get; set; }
        public string UnitId { get; set; }
        public Unit Unit { get; set; }
        public DateTime Date { get; set; }
        public string CreatedBy { get; set; }
    }
}
