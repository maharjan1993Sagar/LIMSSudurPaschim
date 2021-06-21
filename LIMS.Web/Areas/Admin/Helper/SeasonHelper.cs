using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Web.Areas.Admin.Helper
{
    public static class SeasonHelper
    {
        public static List<SelectListItem> GetSeason()
        {

            return new List<SelectListItem>() {
                    
                     new SelectListItem {
                        Text="Winter",
                        Value="Winter",
                    },
                      new SelectListItem {
                        Text="Summer",
                        Value="Summer",
                    },
                     
            };
        
        
        }
    }
}
