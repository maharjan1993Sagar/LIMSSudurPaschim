using LIMS.Api.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace LIMS.Api.DTOs.GeneralCMS
{
    public class ContactUsDto: BaseApiEntityModel
    {
        public Guid ContactUsId { get; set; }
        public string OfficeName { get; set; }
        public string MinistryName { get; set; }
        public string Department { get; set; }
        public string Address { get; set; }
        public string Phone1 { get; set; }
        public string Phone2 { get; set; }
        public string Fax { get; set; }
        public string Email1 { get; set; }
        public string Email2 { get; set; }
        public string Extension { get; set; }
        public string OpeningHours { get; set; }
        public string FacebookUrl { get; set; }
        public string TweeterUrl { get; set; }
        public string WebsiteUrl { get; set; }
        public string UserId { get; set; }
    }
}
