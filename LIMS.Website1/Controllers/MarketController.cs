using LIMS.Website1.Data;
using LIMS.Website1.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Website1.Controllers
{
    public class MarketController:Controller
    {
        private readonly IConfiguration _config;
        private readonly ILogger<MarketController> _logger;
        private readonly DataContext _db;

        public MarketController(ILogger<MarketController> logger, IConfiguration config)
        {
            _logger = logger;
            _config = config;
            _db = new DataContext(_config);
        }

        public async Task<IActionResult> Index()
        {

            var employee = await _db.GetMarket();
            var district = employee.Where(m=>!string.IsNullOrEmpty(m.District)).Select(m => m.District).Distinct();
            var type = employee.Where(m => !string.IsNullOrEmpty(m.NameOfMarket)).Select(m => m.NameOfMarket).Distinct().Select(s => new SelectListItem { Value = s, Text = s }).ToList();
            type.Insert(0, new SelectListItem { Text = "Select", Value = "" });
            ViewBag.type = new SelectList(type, "Value", "Text");
            var market = employee.Select(m => m.NameOfMarket).Distinct();
            var fiscalyear = employee.Select(m => m.FiscalYearId).Distinct();
            string currentfiscalyear = employee.Where(m => Convert.ToBoolean(m.UnitId)).Select(m => m.FiscalYearId).FirstOrDefault();
            employee = employee.Where(m => m.FiscalYearId== currentfiscalyear).ToList();
            var months =
                  new List<SelectListItem> {

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

            var dis = district.Where(m=>!string.IsNullOrEmpty(m)).Select(s => new SelectListItem { Value = s,Text=s })
                .ToList();
           dis.Insert(0,new SelectListItem { Text = "Select", Value = "" });
            ViewBag.district = new SelectList(dis,"Value","Text");
          var fiscalyea = fiscalyear.Select(s => new SelectListItem { Value = s, Text = s})
                .ToList();
            fiscalyea.Insert(0, new SelectListItem { Text = "Select", Value = "" });
            ViewBag.fiscalyear = new SelectList(fiscalyea, "Value", "Text",currentfiscalyear);
            MarketView markets = new MarketView();
            List<string> header = new List<string>();
            header.Add("Date");

            var crops = employee.Select(m => m.BreedId).Distinct();
            header.AddRange(crops);
            markets.CropName = header;
            var date = employee.Select(m => m.RecordingDate).Distinct();
            List<GetMarketData> ms = new List<GetMarketData>();
            foreach (var item in date)
            {
                GetMarketData m = new GetMarketData();
                List<string> data = new List<string>();
                data.Add(item.ToShortDateString());
                foreach (var items in crops)
                {
                    if (employee.Where(m => m.BreedId == items && m.RecordingDate == item).FirstOrDefault() != null)
                    {
                        var averageprice = (Convert.ToInt32(employee.Where(m => m.BreedId == items && m.RecordingDate == item).FirstOrDefault().MaxPrice) + Convert.ToInt32(employee.Where(m => m.BreedId == items && m.RecordingDate == item).FirstOrDefault().MinPrice)) / 2;
                       
                        data.Add(averageprice.ToString());
                    }
                    else
                    {

                        data.Add("");
                    }
                }
                m.Data = data;
                ms.Add(m);

            }
            markets.Data = ms;
            return View(markets);
        }

            [HttpPost]
            public async Task<IActionResult> Index(string districts,string fiscalYear,string type)
            {
           
            var employee = await _db.GetMarket();
            var district = employee.Select(m => m.District).Distinct();
            var market = employee.Select(m => m.NameOfMarket).Distinct();
            var fiscalyear = employee.Select(m => m.FiscalYearId).Distinct();

            if (!string.IsNullOrEmpty(districts))
            {
                employee = employee.Where(m => m.District == districts).ToList();
            }
            if (!string.IsNullOrEmpty(type))
            {
                employee = employee.Where(m => m.NameOfMarket == type).ToList();
            }
            if (!string.IsNullOrEmpty(fiscalYear))
            {
                employee = employee.Where(m => m.FiscalYearId == fiscalYear).ToList();
            }
            //if (!string.IsNullOrEmpty(marketName))
            //{
            //    employee = employee.Where(m => m.NameOfMarket == marketName).ToList();
            //}
        

                var months =
                      new List<SelectListItem> {

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

               // ViewBag.district = new SelectList(district, "district", "district");

                MarketView markets = new MarketView();
                List<string> header = new List<string>();
                header.Add("Date");
                var crops = employee.Select(m => m.BreedId).Distinct();
                header.AddRange(crops);
                markets.CropName = header;
                var date = employee.Select(m => m.RecordingDate).Distinct();
                List<GetMarketData> ms = new List<GetMarketData>();
                foreach (var item in date)
                {
                    GetMarketData m = new GetMarketData();
                    List<string> data = new List<string>();
                    data.Add(item.ToShortDateString());
                    foreach (var items in crops)
                    {
                        if (employee.Where(m => m.BreedId == items && m.RecordingDate == item).FirstOrDefault() != null)
                        {
                            var averageprice = (Convert.ToInt32(employee.Where(m => m.BreedId == items && m.RecordingDate == item).FirstOrDefault().MaxPrice) + Convert.ToInt32(employee.Where(m => m.BreedId == items && m.RecordingDate == item).FirstOrDefault().MinPrice)) / 2;
                            data.Add(averageprice.ToString());
                        }
                        else
                        {

                            data.Add("");
                        }
                    }
                    m.Data = data;
                    ms.Add(m);

                }
                markets.Data = ms;

            var dis = district.Select(s => new SelectListItem { Value = s, Text = s })
                .ToList();
            dis.Insert(0, new SelectListItem { Text = "Select", Value = "" });
            ViewBag.district = new SelectList(dis, "Value", "Text",districts);
            var fiscalyea = fiscalyear.Select(s => new SelectListItem { Value = s, Text = s })
                .ToList();
            fiscalyea.Insert(0, new SelectListItem { Text = "Select", Value = "" });
            ViewBag.fiscalyear = new SelectList(fiscalyea, "Value", "Text", fiscalYear);
            var types = employee.Where(m => !string.IsNullOrEmpty(m.NameOfMarket)).Select(m => m.NameOfMarket).Distinct().Select(s => new SelectListItem { Value = s, Text = s }).ToList();
            types.Insert(0, new SelectListItem { Text = "Select", Value = "" });
            ViewBag.type = new SelectList(type, "Value", "Text",types);
            return View(markets);

            }
        }
}
