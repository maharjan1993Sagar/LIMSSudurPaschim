using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Web.Areas.Admin.Helper
{
    public class FeedTypeCategoryHelper
    {
        public static List<SelectListItem> GetFeedTypeCategory()
        {
            return new List<SelectListItem>() {
                new SelectListItem {
                    Text="Legume",
                    Value="Legume"
                },
                new SelectListItem() {
                    Text="Non-legume",
                    Value="Non-legume"
                }

            };
        }
    }
}
