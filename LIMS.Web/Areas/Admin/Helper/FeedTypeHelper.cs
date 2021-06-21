using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Web.Areas.Admin.Helper
{
    public static class FeedTypeHelper
    {
        public static List<SelectListItem> GetFeedType()
        {
            return new List<SelectListItem>() {
               new SelectListItem{
                   Text="Dry",
                   Value="Dry"
                },
                new SelectListItem{
                   Text="Green",
                   Value="Green"
                },

            };
        }
    }
}
