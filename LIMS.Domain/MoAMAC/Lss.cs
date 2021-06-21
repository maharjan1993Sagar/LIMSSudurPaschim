using System;
using System.Collections.Generic;
using System.Text;

namespace LIMS.Domain.MoAMAC
{
    public class Lss:BaseEntity
    {
        public Lss()
        {
            this.Vhlsec = new Vhlsec();
            this.LssId = Guid.NewGuid();
        }
        public Guid LssId { get; set; }
        public string NameNepali { get; set; }
       

        public string NameEnglish { get; set; }
        
        public string Provience { get; set; }

        
        public string District { get; set; }

        
        public string LocalLevel { get; set; }

        
        public string Ward { get; set; }

       
        public string Tole { get; set; }
        public Vhlsec Vhlsec { get; set; }
        
        public string VhlsecId { get; set; }
     
        public string Email { get; set; }
       

        public string Phone { get; set; }

        public string Website { get; set; }
        //user Details
        public string UserEmail { get; set; }
        



    }
}
