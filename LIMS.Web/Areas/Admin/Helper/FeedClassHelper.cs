using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Web.Areas.Admin.Helper
{
    public static class FeedClassHelper
    {
        public static  List<SelectListItem> GetFeedLibrary()
        {
            return new List<SelectListItem>() {
                new SelectListItem {
                Text="Concentrate",
                Value="Concentrate",
            },
                new SelectListItem {
                Text="Roughage",
                Value="Roughage",
            }

          };
        }
    }
}
