using LIMS.Domain.BasicSetup;
using LIMS.Domain.Vaccination;
using System;
using System.Collections.Generic;
using System.Text;

namespace LIMS.Domain.MedicineInventory
{
     public class MedicineDemand:BaseEntity
    {
        public MedicineDemand()
        {
            this.MedicineDemandId = Guid.NewGuid();
        }
        public Guid MedicineDemandId { get; set; }
        public string MedicineId { get; set; }
        public VaccinationType Medicine { get; set; }
        public string Quantity { get; set; }
        public Unit Unit { get; set; }
        public string Specification { get; set; }
        public string CreatdBy { get; set; }

    }
}
