using System;
using System.Collections.Generic;
using System.Text;

namespace LIMS.Domain.Organizations
{
   public class Organization:BaseEntity
    {
        public Organization()
        {
          
            this.OrganizationId = Guid.NewGuid();
        }
        public Guid OrganizationId { get; set; }
        public string NameNepali { get; set; }

        public string NameEnglish { get; set; }

        public string Provience { get; set; }


        public string District { get; set; }
        
        public string LocalLevel { get; set; }

        public string Ward { get; set; }


        public string Tole { get; set; }
       

        public string Email { get; set; }


        public string Phone { get; set; }

        public string Website { get; set; }
        //user Details
        public string UserEmail { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }


    }
}
