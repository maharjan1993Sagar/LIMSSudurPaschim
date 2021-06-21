using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Web.Areas.Admin.Helper
{
    public static class RepeatAiHelper
    {
        public static List<SelectListItem> RepeatAI()
        {
            return new List<SelectListItem> {
                new SelectListItem {
                    Text="1",
                    Value="1"
                },
                new SelectListItem {
                    Text="2",
                    Value="2"

                },
                new SelectListItem {
                    Text="3",
                    Value="3"
                 },
                 new SelectListItem {
                    Text="4",
                    Value="4"
                 }
            };
        }        
    }
}
