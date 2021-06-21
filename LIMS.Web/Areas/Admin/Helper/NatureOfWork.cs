using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Web.Areas.Admin.Helper
{
    public static class NatureOfWork
    {
        public static List<SelectListItem> GetNatureOfWork()
        {
            return new List<SelectListItem>() {

                     new SelectListItem {
                        Text="Cow palan",
                        Value="Cow palan",
                    },
                      new SelectListItem {
                        Text="Vaisi palan",
                        Value="vaisi palan",
                    }
                      
            };
        }
        
    }
}
