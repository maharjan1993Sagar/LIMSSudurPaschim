using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Web.Areas.Admin.Models.MedicineInventory
{
    public class ReceivedMedicineListModel
    {
        public string Id { get; set; }
        public string MedicineName { get; set; }
        public string FiscalYearId { get; set; }
        public string Fiscalyear { get; set; }
        public string Quantity { get; set; }
        public string Month { get; set; }
        public string UnitId { get; set; }
        public string Unit { get; set; }
    }
}
