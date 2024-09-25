using LIMS.Domain.NewsEvent;
using System;
using System.Collections.Generic;
using System.Text;

namespace LIMS.Domain.GeneralCMS

{
    public class Employee : BaseEntity
    {
        public NewsEventFile _image;

        public Employee()
        {
            this.EmployeeId = Guid.NewGuid();
        }
        public Guid EmployeeId { get; set; }
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
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
        public int SerialNo { get; set; }
        public bool IsActive { get; set; }
        public string Status { get; set; }
        public bool IsInformationOfficer { get; set; }
        public bool IsOfficeChief { get; set; }
        public string DesignationLabel { get; set; }
        public int PositionOfLabel { get; set; }
        public NewsEventFile Image {
            get { return _image ?? (_image = new NewsEventFile()); }
            set { _image = value; }
        }
    }
}
