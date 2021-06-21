using System;
using System.Collections.Generic;
using System.Text;

namespace LIMS.Domain.Users
{
    public class Technicians: BaseEntity
    {
        public Technicians()
        {
            this.TechnicianId = Guid.NewGuid();

        }
        public Guid TechnicianId { get; set; }
        public string UserNameNepali { get; set; }
        public string UserNameEnglish { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string IDCardNo { get; set; }
        public string Province { get; set; }
        public string District { get; set; }
        public string LocalLevel { get; set; }
        public string WardNo { get; set; }
        public string Tole { get; set; }
        public string PhoneNo { get; set; }
        public string PositionId { get; set; }//from para-profession
        public string AccadamicQualification { get; set; }
        public string PanNo { get; set; }
        public string PhotoId { get; set; }
        public string CitizenshipId { get; set; }
       




    }
}
