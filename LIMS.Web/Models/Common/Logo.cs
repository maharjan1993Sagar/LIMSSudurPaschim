﻿using LIMS.Core.Models;

namespace LIMS.Web.Models.Common
{
    public partial class LogoModel : BaseModel
    {
        public string StoreName { get; set; }

        public string LogoPath { get; set; }
    }
}