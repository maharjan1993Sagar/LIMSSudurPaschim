using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Web.Areas.Admin.Helper
{
    public class OrganizationTypeHelper
    {
        public static List<SelectListItem> GetOrganizationType()
        {
            return new List<SelectListItem>() {

                     new SelectListItem {
                        Text="Hatchery",
                        Value="Hatchery",
                    },
                      new SelectListItem {
                        Text="Feed industry",
                        Value="Feed industry",
                    },
                      new SelectListItem {
                          Text = "Milk co-operative",
                          Value = "Milk co-operative",
                      },
                      new SelectListItem {
                          Text = "Animal market",
                          Value = "Animal market",
                      },
                      new SelectListItem {
                          Text = "Diary industry",
                          Value = "Diary industry",
                      },
                       new SelectListItem {
                          Text = "Chilling center",
                          Value = "Chilling center",
                      },
                       new SelectListItem {
                          Text = "Agrovet",
                          Value = "Agrovet",
                      },
                       new SelectListItem {
                          Text = "Animal collection",
                          Value = "Animal collection",
                      },
                       new SelectListItem {
                          Text = "Diary shop",
                          Value = "Diary shop",
                      },
                       new SelectListItem {
                          Text = "Meat processing industry",
                          Value = "Meat processing industry",
                      },
                       new SelectListItem {
                          Text = "Meat shop",
                          Value = "Meat shop",
                      },
                        new SelectListItem {
                          Text ="Feed shop",
                          Value = "Feed shop",
                      },
                         new SelectListItem {
                          Text ="Fish srot",
                          Value = "Fish srot",
                      },

                         new SelectListItem {
                             Text="Livestock Resource Center",
                             Value="Livestock Resource Center"

                         },
                         new SelectListItem {
                             Text="Feed/fooder Resource Center",
                             Value="Feed/fooder Resource Center"

                         },
                          new SelectListItem {
                             Text="NGO",
                             Value="NGO"

                         },
                          new SelectListItem {
                             Text="Manpower",
                             Value="Manpower"

                         },
                          new SelectListItem {
                             Text="Tech school",
                             Value="Tech school"

                         },
                           new SelectListItem {
                             Text="Canel clube",
                             Value="Canel clube"

                         },
                           new SelectListItem {
                             Text="Vet clinic",
                             Value="Vet clinic"

                         },

        };


        }
    }
}
