using LIMS.Api.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace LIMS.Api.DTOs.GeneralCMS
{
    public class EmployeeDto: BaseApiEntityModel
    {
        public string EmployeeId { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string Designation { get; set; }
        public string Abbrebiation { get; set; }
        public string Address { get; set; }
        public string PIS { get; set; }
        public string OfficePhone { get; set; }
        public string Phone2 { get; set; }
        public string Email1 { get; set; }
        public string Email2 { get; set; }
        public string Extension { get; set; }
        public string Description { get; set; }
        public string FacebookUrl { get; set; }
        public string TweeterUrl { get; set; }
        public string WebsiteUrl { get; set; }
        public string UserId { get; set; }
        public int SerialNo { get; set; }
        public bool IsActive { get; set; }
        public string Status { get; set; }
        public NewsEventFileDto Image { get; set; }
    }
}
