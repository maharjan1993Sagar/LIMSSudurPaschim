using LIMS.Domain.Bali;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Web.Areas.Admin.Models.Bali
{
    public class AanudanReport
    {
        public PujigatKharchaKharakram pujigatKharchaKharakram { get; set; }
        public string PujigatKharchaId { get; set; }
        public string FiscalYearId { get; set; }
        public string Type { get; set; }
        public string ProgramType { get; set; }
        public string Address { get; set; }
        public string Name { get; set; }
        public string PhoneNo { get; set; }
        public string Rakam { get; set; }
        public string OrgName { get; set; }
        public string MaleMember { get; set; }
        public string FemaleMember { get; set; }
        public string DalitMember { get; set; }
        public string JanajatiMember { get; set; }
        public string Id { get; set; }
    }
}
