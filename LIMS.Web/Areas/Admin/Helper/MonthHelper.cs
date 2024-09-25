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
                 },
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

            };



        }
        public List<MonthSerial> AllMonths = new List<MonthSerial> {
            new MonthSerial{NepaliName = "श्रावण", EnglishName ="Shrawan", SerialNo= 0 },
            new MonthSerial{NepaliName = "भाद्र", EnglishName ="Bhadra", SerialNo= 1 },
            new MonthSerial{NepaliName = "असोज", EnglishName ="Ashwin", SerialNo= 2 },
            new MonthSerial{NepaliName = "कार्तिक", EnglishName ="Kartik", SerialNo= 3 },
            new MonthSerial{NepaliName = "मंसिर", EnglishName ="Mangsir", SerialNo=4},
            new MonthSerial{NepaliName = "पुष", EnglishName ="Poush", SerialNo= 5 },
            new MonthSerial{NepaliName = "माघ", EnglishName ="Magh", SerialNo= 6 },
            new MonthSerial{NepaliName = "फागुन", EnglishName ="Falgun", SerialNo=7 },
            new MonthSerial{NepaliName = "चैत्र", EnglishName ="Chaitra", SerialNo=8 },
            new MonthSerial{NepaliName = "बैशाख", EnglishName ="Baishakh", SerialNo=9 },
            new MonthSerial{NepaliName = "जेठ", EnglishName ="Jestha", SerialNo=10 },
            new MonthSerial{NepaliName = "असार", EnglishName ="Asar", SerialNo=11 },
        };
    }
    public class MonthSerial
    {
        public string NepaliName { get; set; }
        public string EnglishName { get; set; }
        public int SerialNo { get; set; }
    }
}
