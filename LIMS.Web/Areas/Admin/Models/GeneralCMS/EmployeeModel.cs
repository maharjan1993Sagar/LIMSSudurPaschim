using LIMS.Core.ModelBinding;
using LIMS.Domain;
using LIMS.Domain.NewsEvent;
using LIMS.Web.Areas.Admin.Models.NewsEvent;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace LIMS.Web.Areas.Admin.Models.GeneralCMS

{
   public class EmployeeModel:BaseEntity
    {
       [LIMSResourceDisplayName("Admin.Employee.Name")]
        public string Name { get; set; }
        [LIMSResourceDisplayName("Admin.Employee.Designation")]

        public string Designation { get; set; }
        [LIMSResourceDisplayName("Admin.Employee.Salutation")]

        public string Salutation { get; set; }
        [LIMSResourceDisplayName("Admin.Employee.Address")]

        public string Address { get; set; }
        [LIMSResourceDisplayName("Admin.Employee.PIS")]

        public string  PIS { get; set; }
        [LIMSResourceDisplayName("Admin.Employee.OfficePhone")]

        public string  OfficePhone { get; set; }
        [LIMSResourceDisplayName("Admin.Employee.Phone2")]

        public string  Phone2 { get; set; }
        [LIMSResourceDisplayName("Admin.Employee.Email1")]

        public string  Email1 { get; set; }
        [LIMSResourceDisplayName("Admin.Employee.Email2")]

        public string  Email2 { get; set; }
        [LIMSResourceDisplayName("Admin.Employee.Extension")]

        public string  Extension { get; set; }
        [LIMSResourceDisplayName("Admin.Employee.Description")]
        
        public string Description{ get; set; }
        [LIMSResourceDisplayName("Admin.Employee.FacebookUrl")]

        public string FacebookUrl { get; set; }
        [LIMSResourceDisplayName("Admin.Employee.TweeterUrl")]

        public string TweeterUrl { get; set; }
        [LIMSResourceDisplayName("Admin.Employee.WebsiteUrl")]

        public string WebsiteUrl { get; set; }
        [LIMSResourceDisplayName("Admin.Employee.Type")]

        public string Type { get; set; }
        [LIMSResourceDisplayName("Admin.Employee.SerialNo")]

        public int SerialNo { get; set; }
        [LIMSResourceDisplayName("Admin.Employee.IsActive")]

        public bool IsActive { get; set; } = true;
        [LIMSResourceDisplayName("Admin.Employee.Status")]

        public string Status { get; set; }
        [LIMSResourceDisplayName("Admin.Employee.IsInformationOfficer")]

        public bool IsInformationOfficer { get; set; }
        [LIMSResourceDisplayName("Admin.Employee.IsOfficeChief/Director")]

        public bool IsOfficeChief { get; set; }
        [LIMSResourceDisplayName("Admin.Employee.DesignationLabel")]

        public string DesignationLabel { get; set; }
        [LIMSResourceDisplayName("Admin.Employee.PositionOfLabel")]

        public int PositionOfLabel { get; set; }

        public NewsEventFileModel ImageModel { get; set; }
    }
}
