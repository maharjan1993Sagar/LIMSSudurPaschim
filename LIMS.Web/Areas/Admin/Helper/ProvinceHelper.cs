using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Web.Areas.Admin.Helper
{
    public static class ProvinceHelper
    {
        public static List<SelectListItem> GetProvince()
        {

           return new List<SelectListItem> {
                new SelectListItem { Text = "Bagamati Province", Value = "Province 3" },
            };
        }
    }
}
