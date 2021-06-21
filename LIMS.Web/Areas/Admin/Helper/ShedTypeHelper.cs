using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Web.Areas.Admin.Helper
{
    public class SHedTypeHelper
    {
        public static List<SelectListItem> GetShedType()
        {
            return new List<SelectListItem>() {

                     new SelectListItem {
                        Text="Bhakaro",
                        Value="Bhakaro",
                    },
                      new SelectListItem {
                        Text="Nali",
                        Value="Nali",
                    },
                      new SelectListItem {
                          Text = "Godam dana",
                          Value = "Godam dana",
                      },
                       new SelectListItem {
                          Text = "Godam paral",
                          Value = "Godam Paral",
                      },
                       new SelectListItem {
                          Text = "Office room",
                          Value = "Office room",
                      },
                          new SelectListItem {
                          Text = "Open land (ft)",
                          Value = "Open land",
                      },
            };


        }

    }
}
