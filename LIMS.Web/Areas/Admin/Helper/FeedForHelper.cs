using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Web.Areas.Admin.Helper
{
    public static class FeedForHelper
    {
        public static List<SelectListItem> GetFeedFor()
        {
            return new List<SelectListItem>() {
                new SelectListItem {
                    Text="Ruminant",
                    Value="Ruminant"
                },
                new SelectListItem {
                    Text="Non-ruminant",
                    Value="Non-ruminant"
                }
            };
        }
    }
}
