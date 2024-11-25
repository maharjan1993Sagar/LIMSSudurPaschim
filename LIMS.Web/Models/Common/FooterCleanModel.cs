﻿using LIMS.Core.Models;

namespace LIMS.Web.Models.Common
{
    public partial class FooterCleanModel : BaseModel
    {
        public string StoreName { get; set; }
        public string CompanyName { get; set; }
        public string CompanyEmail { get; set; }
        public string CompanyHours { get; set; }
        public string CompanyAddress { get; set; }
        public string CompanyPhone { get; set; }
    }
}
