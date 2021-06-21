using System;
using System.Collections.Generic;
using System.Text;

namespace LIMS.Domain.Users
{
    public class NlboUser:BaseEntity
    {
        public NlboUser()
        {
            this.NlboUserId = Guid.NewGuid();

        }
        public Guid NlboUserId { get; set; }
        public string NameNepali { get; set; }
        public string NameEnglish { get; set; }
        public string Email { get; set; }
        public string IDCardNo { get; set; }
        public string Province { get; set; }
        public string District { get; set; }
        public string LocalLevel { get; set; }
        public string WardNo { get; set; }
        public string Tole { get; set; }
        public string PhoneNo { get; set; }
        public string PanNo { get; set; }
       
    }
}
