using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Web.Areas.Admin.Helper
{
    public static class FodderHelper
    {
          public static List<SelectListItem> GetGrassType() { 
         return new List<SelectListItem>() {
                    
                     new SelectListItem {
                        Text="Forage grass",
                        Value="Forage grass",
                    },
                      new SelectListItem {
                        Text="Fodder tress",
                        Value="Fodder trees",
                    },
                     
            };
        
        
        }
   
    }
}
