using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Web.Areas.Admin.Helper
{
    public static class QuaterHelper
    {
        public static List<SelectListItem> GetQuater()
        {
          return  new List<SelectListItem>() {
                new SelectListItem{
                    Text="First",
                    Value="First"
                },
                new SelectListItem{
                    Text="Second",
                    Value="Second"
                },
                new SelectListItem{
                    Text="Third",
                    Value="Third"
                },
                  
            };
        }
    }
}
