using LIMS.Core.ModelBinding;
using LIMS.Domain;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace LIMS.Web.Areas.Admin.Models.AInR
{
    public enum Genders { Male,Female,castrate};
    public class OwnerModel:BaseEntity
    {
        [LIMSResourceDisplayName("Admin.Owner.Type")]

        public string Type { get; set; }
        [LIMSResourceDisplayName("Admin.Owner.NameEnglish")]

        public string NameEnglish { get; set; }
        [LIMSResourceDisplayName("Admin.Owner.NameNepali")]

        public string NameNepali { get; set; }
        [LIMSResourceDisplayName("Admin.Owner.FarmId")]

        public string FarmId { get; set; }
        public FarmModel Farm { get; set; }
        [LIMSResourceDisplayName("Admin.Owner.PhoneNo")]

        public string PhoneNo { get; set; }


        [LIMSResourceDisplayName("Admin.Owner.Email")]

        public string Email { get; set; }
      

        [LIMSResourceDisplayName("Admin.Common.Provience")]

        public string Provience { get; set; }
        [LIMSResourceDisplayName("Admin.Common.District")]

        public string District { get; set; }
        [LIMSResourceDisplayName("Admin.Common.LocalLevel")]

        public string LocalLevel { get; set; }
        [LIMSResourceDisplayName("Admin.Common.Ward")]

        public string Ward { get; set; }
        [LIMSResourceDisplayName("Admin.Common.Tole")]

        public string Tole { get; set; }
        [LIMSResourceDisplayName("Admin.Common.Latitude")]

        public string Latitude { get; set; }
        [LIMSResourceDisplayName("Admin.Common.Longitude")]

        public string Longitude { get; set; }
        [LIMSResourceDisplayName("Admin.Owner.Gender")]

        public string Gender { get; set; }
        [LIMSResourceDisplayName("Admin.Owner.EthinicGroup")]

        public string EthinicGroup { get; set; }//dalit janajati aanya
        [LIMSResourceDisplayName("Admin.Owner.Education")]

        public string EducationQualification { get; set; }
        [LIMSResourceDisplayName("Admin.Owner.ForeignJobExperience")]

        public bool ForeignJobExperience { get; set; }
        
        [UIHint("Picture")]

        [LIMSResourceDisplayName("Admin.Owner.Photo")]
       public string   Photo { get; set; }
        [UIHint("Picture")]
        [LIMSResourceDisplayName("Admin.Owner.Citizenship")]
        public string CitizenShip{ get; set; }
        [UIHint("Picture")]

        [LIMSResourceDisplayName("Admin.Owner.Other")]

        public string Other{ get; set; }


    }
}
