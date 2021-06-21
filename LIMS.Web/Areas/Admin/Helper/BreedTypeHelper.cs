using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Web.Areas.Admin.Helper
{
    public static class BreedTypeHelper
    {
        public static List<SelectListItem> GetBreedType() { 
         return new List<SelectListItem>() {
                    
                     new SelectListItem {
                        Text="Native",
                        Value="Native",
                    },
                      new SelectListItem {
                        Text="Crossbred",
                        Value="Crossbred",
                    },
                      new SelectListItem {
                          Text = "Pure exotic breed",
                          Value = "Pure exotic breed",
                      },
            };
        
        
        }
    }
}
