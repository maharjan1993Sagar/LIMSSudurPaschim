using System;
using System.Collections.Generic;
using System.Text;

namespace LIMS.Domain.Organizations
{
    public  class OtherOrganization:BaseEntity
    {
        public OtherOrganization()
        {

            this.OtherOrganizationId = Guid.NewGuid();
        }
        public Guid OtherOrganizationId { get; set; }
        public string  Type { get; set; }
        public string NameNepali { get; set; }
        public string NameEnglish { get; set; }

        public string Provience { get; set; }
        public string District { get; set; }

        public string LocalLevel { get; set; }

        public string Ward { get; set; }

        public string Tole { get; set; }

        public string Email { get; set; }
        public string Phone { get; set; }
        public string ContactPersonName { get; set; }
        public string MobileNo { get; set; }
        public string Proprietor { get; set; }
        public string ContactNo { get; set; }
        public string Website { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
    }
}
