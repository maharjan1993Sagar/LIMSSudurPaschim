using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Web.Areas.Admin.Helper
{
    public static class CategoryTypeHelper
    {
        public static List<SelectListItem> GetType()
        {
            return new List<SelectListItem> {
        new SelectListItem {
                    Text="Select",
                    Value=""
                },
                new SelectListItem {
                    Text="Input Supply",
                    Value="Input Supply"
                },
                 new SelectListItem
                 {
                     Text = "Anugaman",
                     Value = "Anugaman"
                 }
            };

        }
    }
}
