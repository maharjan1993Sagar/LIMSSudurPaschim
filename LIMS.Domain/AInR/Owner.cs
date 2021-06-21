using System;
using System.Collections.Generic;
using System.Text;

namespace LIMS.Domain.AInR
{
    public class Owner:BaseEntity
    {
        public Owner()
        {
            this.OwnerId = Guid.NewGuid();

        }
        public Guid OwnerId { get; set; }
        public string Type { get; set; }
        public string NameEnglish { get; set; }
        public string NameNepali { get; set; }
        public string FarmId { get; set; }
        public Farm Farm { get; set; }
        public string PhoneNo { get; set; }
        public string Email { get; set; }
        public string Provience { get; set; }
        public string District { get; set; }
        public string LocalLevel { get; set; }
        public string Ward { get; set; }
        public string Tole { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string Gender { get; set; }
        public string EthinicGroup { get; set; }//dalit janajati aanya
        public string   PhotoId { get; set; }
        public string CitizenShipId { get; set; }
        public string OtherId { get; set; }
        public string Source { get; set; }
        public string EducationQualification { get; set; }
        public bool ForeignJobExperience { get; set; }



    }
}
