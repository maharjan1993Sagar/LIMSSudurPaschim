using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Web.Areas.Admin.Helper
{
    public static class MainMenuLink
    {
        public static List<SelectListItem> GetBreedType()
        {
            return new List<SelectListItem>() {
                 new SelectListItem {
                        Text="Select",
                        Value="",
                    },
                     new SelectListItem {
                        Text="Home",
                        Value="/home/index",
                    },
                      new SelectListItem {
                        Text="Gallery",
                        Value="/Gallery/Index",
                    },
                      new SelectListItem {
                          Text = "Contact us",
                          Value = "/home/contactus",
                      },
                      // new SelectListItem {
                      //    Text = "PageContent",
                      //    Value = "/PageContent/Index",
                      //},
                        new SelectListItem {
                          Text = "News/Event",
                          Value = "/NewsEvent/Index",
                      },
                      //   new SelectListItem {
                      //    Text = "Files",
                      //    Value = "/Files/Index",
                      //},
                        new SelectListItem {
                          Text = "Market",
                          Value = "/market",
                      },
                new SelectListItem {
                    Text="Fish",
                    Value="/resources/index?type=Admin.Resource.Fish"
                },
                 new SelectListItem {
                    Text="Seed",
                    Value="/resources/index?type=Admin.Resource.Seed"
                },
                   new SelectListItem {
                    Text="Plant",
                    Value="/resources/index?type=Admin.Resource.Berna"
                },
                   new SelectListItem {
                    Text="Fertilizer",
                    Value="/resources/index?type=Admin.Resource.Bisadi"
                },

            };
       


        }
    }
}
