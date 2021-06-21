﻿using LIMS.Core.Models;

namespace LIMS.Web.Areas.Admin.Models.Common
{
    public partial class SystemWarningModel : BaseModel
    {
        public SystemWarningLevel Level { get; set; }

        public string Text { get; set; }
    }

    public enum SystemWarningLevel
    {
        Pass,
        Warning,
        Fail
    }
}