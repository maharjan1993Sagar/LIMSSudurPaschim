using LIMS.Domain;
using LIMS.Domain.BasicSetup;
using LIMS.Domain.Vaccination;
using LIMS.Web.Areas.Admin.Models.Vaccination;
using System;
using System.Collections.Generic;
using System.Text;

namespace LIMS.Web.Areas.Admin.Models.MedicineInventory
{
     public class MedicineDemandmodel:BaseEntity
    {
        public string MedicineId { get; set; }
        public string Quantity { get; set; }
        public Unit Unit { get; set; }
        public string Specification { get; set; }
        public string CreatdBy { get; set; }
        public string Month { get; set; }
    }
}
