using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Web.Areas.Admin.Helper
{
    public class MonthHelper
    {
        public List<SelectListItem> GetMonths() {
            return new List<SelectListItem> {
                new SelectListItem {
                    Text="Baishakh",
                    Value="Baishakh"
                },
                new SelectListItem {
                    Text="Jestha",
                    Value="Jestha"

                },
                new SelectListItem {
                    Text="Asar",
                    Value="Asar"
                 },
                 new SelectListItem {
                    Text="Shrawan",
                    Value="Shrawan"
                 },
                   new SelectListItem {
                    Text="Bhadra",
                    Value="Bhadra"
                 },
                    new SelectListItem {
                    Text="Ashwin",
                    Value="Ashwin"
                 },
                  new SelectListItem {
                    Text="Kartik",
                    Value="Kartik"
                 },
                   new SelectListItem {
                    Text="Mangsir",
                    Value="Mangsir"
                 },
                   new SelectListItem {
                    Text="Poush",
                    Value="Poush"
                 },
                   new SelectListItem {
                    Text="Magh",
                    Value="Magh"
                 },
                    new SelectListItem {
                    Text="Falgun",
                    Value="Falgun"
                 },
               new SelectListItem {
                    Text="Chaitra",
                    Value="Chaitra"
                 }
                        

            };



        }
    }
}
