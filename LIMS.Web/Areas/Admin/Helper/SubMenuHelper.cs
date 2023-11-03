using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Web.Areas.Admin.Helper
{
    public static class SubMenuHelper
    {
        public static List<SelectListItem> GetBreedType(string mainmenu)
        {
            if (mainmenu.ToLower() == "news/event")
            {
                return new List<SelectListItem>() {

                     new SelectListItem {
                        Text="News",
                        Value="/NewsEvent/Index?type=News",
                    },
                      new SelectListItem {
                        Text="Event",
                        Value="/NewsEvent/Index?type=Event",
                    },
                      
            };
            }
            else if (mainmenu.ToLower() == "download")
            {
                return new List<SelectListItem>() {

                     new SelectListItem {
                        Text="Tender",
                        Value="/NewsEvent/Index?type=Tender",
                    },
                      new SelectListItem {
                        Text="Notices",
                        Value="/NewsEvent/Index?type=Notices",
                    },
                      new SelectListItem {
                        Text="Press",
                        Value="/NewsEvent/Index?type=Press",
                    },
                       new SelectListItem {
                        Text="Letter",
                        Value="/NewsEvent/Index?type=Letter",
                    },
                       new SelectListItem {
                        Text="Reports",
                        Value="/NewsEvent/Index?type=Reports",
                    },
                        new SelectListItem {
                        Text="Other files",
                        Value="/NewsEvent/Index?type=Other+Files",
                    },
                         new SelectListItem {
                        Text="Publication",
                        Value="/NewsEvent/Index?type=Publication",
                    },

            };
            }
            else if(mainmenu.ToLower()== "acts & regulation")
            {
                return new List<SelectListItem>() {

                     new SelectListItem {
                        Text="Rules & Regulation",
                        Value="/NewsEvent/Index?type=Rules+%26+Regulation",
                    },
                      new SelectListItem {
                        Text="Directives",
                        Value="/NewsEvent/Index?type=Directives",
                    },
                      new SelectListItem {
                        Text="Acts & Policies",
                        Value="/NewsEvent/Index?type=Acts+%26+Policies",
                    },
                      


            };
            }
            else if(mainmenu.ToLower() == "about us")
            {
                return new List<SelectListItem>() {

                     new SelectListItem {
                        Text="Introduction",
                        Value="/Pagecontent/Index?pageName=Introduction",
                    },
                      new SelectListItem {
                        Text="Staff/Officials",
                        Value="/Employee/Index",
                    },
                      new SelectListItem {
                        Text="Organogram",
                        Value="/Employee/Index",
                    },



            };

            }
            else
            {
                return new List<SelectListItem>();
            }
        }
    }
}
