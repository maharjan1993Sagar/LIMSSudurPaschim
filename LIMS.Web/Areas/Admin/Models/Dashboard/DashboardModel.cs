using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Web.Areas.Admin.Models.Dashboard
{
    public class DashboardModel
    {
        public int Farm { get; set; }
        public string PrivateFarm { get; set; }
        public string PublicFarm { get; set; }
        public int Animal { get; set; }
        public int Goat { get; set; }
        public int MaleGoat { get; set; }
        public int FemaleGoat { get; set; }
        public int Buffalo { get; set; }
        public int MilkingBuffalo { get; set; }
        public int Cow { get; set; }
        public string MilkingCow { get; set; }
        public int Ai { get; set; }
        public int Vaccination { get; set; }
        public decimal  Production { get; set; }
        public decimal SemenDistribution { get; set; }

    }
}
