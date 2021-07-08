using LIMS.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Web.Areas.Admin.Models.Bali
{
    public class LabambitKrishakModel:BaseEntity
    {
        public string FiscalyearId { get; set; }
        public string PujigatKharchaKaryakramId { get; set; }
        public string LabambitKrishakKoNam { get; set; }
        public string PhoneNo { get; set; }
        public string Sex { get; set; }
        public string EthinicGroup { get; set; }
        public string Province { get; set; }
        public string District { get; set; }
        public string LocalLevel { get; set; }
        public string WardNo { get; set; }
        public string Tole { get; set; }
        [UIHint("Picture")]
        public string PictureId { get; set; }
        public string WorkDone { get; set; }
        public string Remarks { get; set; }
       
    }
}
