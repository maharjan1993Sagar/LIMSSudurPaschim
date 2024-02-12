using LIMS.Core;
using LIMS.Domain.SemenDistribution;
using LIMS.Services.Ainr;
using LIMS.Services.AnimalBreeding;
using LIMS.Services.AnimalHealth;
using LIMS.Services.Basic;
using LIMS.Services.Customers;
using LIMS.Services.MoAMAC;
using LIMS.Services.Semen;
using LIMS.Services.Statisticaldata;
using LIMS.Web.Areas.Admin.Helper;
using LIMS.Web.Areas.Admin.Models.BarGraph;
using LIMS.Web.Areas.Admin.Models.Dashboard;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Web.Areas.Admin.Controllers
{
    public class DashboardController : BaseAdminController
    {
        public readonly IAnimalRegistrationService _animalService;
        public readonly ICustomerService _customerService;
        public readonly IWorkContext _workContext;
        public readonly ILssService _lssService;
        public readonly IVhlsecService _vhlsecService;
        public readonly IFarmService _farmService;
        public readonly IAiService _aiService;
        public readonly IVaccinationService _vaccinationService;
        public readonly IFiscalYearService _fiscalYearService;
        public readonly IProductionionDataService _productionDataService;
        public readonly IServiceData _serviceData;
        public readonly ISemenDistributionService _semenDistribution;
        public readonly IFiscalYearForGraphService _fiscalYearForGraphService;
        private readonly IHostingEnvironment _hostingEnvironment;

        public DashboardController(IAnimalRegistrationService animalService,
             IHostingEnvironment hostingEnvironment,
            ICustomerService customerService,
            IWorkContext workContext,
            ILssService lssService,
            IFarmService farmService,
            IAiService aiService,
            IVhlsecService vhlsecService,
            IVaccinationService vaccinationService,
            IFiscalYearService fiscalYearService,
            IProductionionDataService productionDataService,
            IServiceData serviceData,
           ISemenDistributionService semenDistributionService,
            IFiscalYearForGraphService fiscalYearForGraphService

            )
        {
            _hostingEnvironment = hostingEnvironment;
            _animalService = animalService;
            _customerService = customerService;
            _workContext = workContext;
            _lssService = lssService;
            _farmService = farmService;
            _aiService = aiService;
            _vhlsecService = vhlsecService;
            _vaccinationService = vaccinationService;
            _fiscalYearService = fiscalYearService;
            _productionDataService = productionDataService;
            _serviceData = serviceData;
            _semenDistribution = semenDistributionService;
            _fiscalYearForGraphService = fiscalYearForGraphService;
        }

        public async Task<IActionResult> Index()
        {
            List<string> roles = _workContext.CurrentCustomer.CustomerRoles.Select(x => x.Name).ToList();
            var model = new DashboardModel();
            if (roles.Contains("VhlsecUser") || roles.Contains("VhlsecAdmin"))
            {
                string vhlsecid = _workContext.CurrentCustomer.EntityId;
                List<string> lssId = _lssService.GetLssByVhlsecId(vhlsecid).Result.Select(m => m.Id).ToList();
                var customers = _customerService.GetCustomerByLssId(lssId, vhlsecid);
                List<string> customerid = customers.Select(x => x.Id).ToList();
                var ai = await _animalService.GetAnimalByLss(customerid);
                var totalAi = ai.Count();
                var animals = _animalService.GetAllAnimalByLss(customerid);
                var totalAnimal = animals.Count();
                var totalGoat = animals.Where(m => m.Species != null && m.Species.EnglishName.ToLower() == "goat").Count();
                var totalMaleGoat = animals.Where(m => m.Species != null && m.Species.EnglishName.ToLower() == "goat" && m.Gender != null && m.Gender.ToLower() == "male").Count();
                var totalFemaleGoat = animals.Where(m => m.Species != null && m.Species.EnglishName.ToLower() == "goat" && m.Gender != null && m.Gender.ToLower() == "female").Count();
                var totalCow = animals.Where(m => m.Species != null && m.Species.EnglishName.ToLower() == "cow").Count();
                var milkingbuffalo = animals.Where(m => m.Species != null && m.Species.EnglishName.ToLower() == "bufallo" && m.MilkStatus.ToLower() == "milking ").Count();
                var milkingCow = animals.Where(m => m.Species != null && m.Species.EnglishName.ToLower() == "cow" && m.MilkStatus != null && m.MilkStatus.ToLower() == "milking ").Count();

                var buffalo = animals.Where(m => m.Species != null && m.Species.EnglishName.ToLower() == "buffalo").Count();
                var totalvaccination = _vaccinationService.GetVaccinationCountByCustomerIds(customerid);
                var totalFarm = _farmService.GetFarmCountByLssId(customerid);
                var farm = await _farmService.GetFarmByLssId(customerid);
                var privateFarm = farm.Where(m => m.FarmType != null && m.FarmType.ToLower() == "private").Count();
                var publicfarm = farm.Where(m => m.FarmType != null && m.FarmType.ToLower() == "public").Count();

                var currentFisaclYears = await _fiscalYearService.GetCurrentFiscalYear();
                string currentFisaclYear = null;
                if (currentFisaclYears != null)
                {
                    currentFisaclYear = currentFisaclYears.Id;
                }
                else
                {
                    currentFisaclYear = null;
                }
                var Production = await _productionDataService.GetProduction();
                var productionByFiscalYear = (dynamic)null;
                if (currentFisaclYear != null)
                {
                    productionByFiscalYear = Production.Where(m => m.FiscalYear.Id == currentFisaclYear).ToList();
                }
                else
                {
                    productionByFiscalYear = 0;

                }

                decimal y = 0;
                var totalProduction = Production.Where(m => decimal.TryParse(m.Quantity, out y)).Sum(x => y);


                model.Farm = totalFarm;
                model.Animal = totalAnimal;
                model.Cow = totalCow;
                model.Buffalo = buffalo;
                model.Ai = totalAi;
                model.Goat = totalGoat;
                model.Vaccination = totalvaccination;
                model.MilkingBuffalo = milkingbuffalo;
                model.MilkingCow = milkingCow.ToString();
                model.PrivateFarm = privateFarm.ToString();
                model.PublicFarm = publicfarm.ToString();
                model.FemaleGoat = totalFemaleGoat;
                model.MaleGoat = totalMaleGoat;
                model.Production = totalProduction;
            }
            else if (roles.Contains("DolfdUser") || roles.Contains("DolfdAdmin"))
            {
                string dolfdId = _workContext.CurrentCustomer.EntityId;
                List<string> vhlsecId = _vhlsecService.GetVhlsecByDolfdId(dolfdId).Result.Select(m => m.Id).ToList();
                var LssIds = new List<string>();
                foreach (var item in vhlsecId)
                {
                    LssIds.AddRange(_lssService.GetLssByVhlsecId(item).Result.Select(m => m.Id).ToList());
                }
                var customers = _customerService.GetCustomerByLssId(LssIds, vhlsecId, dolfdId);
                List<string> customerid = customers.Select(x => x.Id).ToList();
                var ai = await _animalService.GetAnimalByLss(customerid);
                var totalAi = ai.Count();
                var animals = _animalService.GetAllAnimalByLss(customerid);
                var totalAnimal = animals.Count();
                var totalGoat = animals.Where(m => m.Species != null && m.Species.EnglishName.ToLower() == "goat").Count();
                var totalMaleGoat = animals.Where(m => m.Species != null && m.Species.EnglishName.ToLower() == "goat" && m.Gender != null && m.Gender.ToLower() == "male").Count();
                var totalFemaleGoat = animals.Where(m => m.Species != null && m.Species.EnglishName.ToLower() == "goat" && m.Gender != null && m.Gender.ToLower() == "female").Count();
                var totalCow = animals.Where(m => m.Species != null && m.Species.EnglishName.ToLower() == "cow").Count();
                var milkingbuffalo = animals.Where(m => m.Species != null && m.Species.EnglishName.ToLower() == "bufallo" && m.MilkStatus.ToLower() == "milking ").Count();
                var milkingCow = animals.Where(m => m.Species != null && m.Species.EnglishName.ToLower() == "cow" && m.MilkStatus != null && m.MilkStatus.ToLower() == "milking ").Count();

                var buffalo = animals.Where(m => m.Species != null && m.Species.EnglishName.ToLower() == "buffalo").Count();
                var totalvaccination = _vaccinationService.GetVaccinationCountByCustomerIds(customerid);
                var totalFarm = _farmService.GetFarmCountByLssId(customerid);
                var farm = await _farmService.GetFarmByLssId(customerid);
                var privateFarm = farm.Where(m => m.FarmType != null && m.FarmType.ToLower() == "private").Count();
                var publicfarm = farm.Where(m => m.FarmType != null && m.FarmType.ToLower() == "public").Count();

                var currentFisaclYears = await _fiscalYearService.GetCurrentFiscalYear();
                string currentFisaclYear = null;
                if (currentFisaclYears != null)
                {
                    currentFisaclYear = currentFisaclYears.Id;
                }
                else
                {
                    currentFisaclYear = null;
                }
                var Production = await _productionDataService.GetProduction();
                var productionByFiscalYear = (dynamic)null;
                if (currentFisaclYear != null)
                {
                    productionByFiscalYear = Production.Where(m => m.FiscalYear.Id == currentFisaclYear).ToList();
                }
                else
                {
                    productionByFiscalYear = 0;

                }

                decimal y = 0;
                var totalProduction = Production.Where(m => decimal.TryParse(m.Quantity, out y)).Sum(x => y);


                model.Farm = totalFarm;
                model.Animal = totalAnimal;
                model.Cow = totalCow;
                model.Buffalo = buffalo;
                model.Ai = totalAi;
                model.Goat = totalGoat;
                model.Vaccination = totalvaccination;
                model.MilkingBuffalo = milkingbuffalo;
                model.MilkingCow = milkingCow.ToString();
                model.PrivateFarm = privateFarm.ToString();
                model.PublicFarm = publicfarm.ToString();
                model.FemaleGoat = totalFemaleGoat;
                model.MaleGoat = totalMaleGoat;
                model.Production = totalProduction;
            }
            else if(roles.Contains("LssAdmin") || roles.Contains("LssUser"))
            {
                var entityid = _workContext.CurrentCustomer.EntityId;
                var allCustomer = await _customerService.GetAllCustomers();
                List<string> customerid = allCustomer.Where(m => m.EntityId == entityid).Select(x => x.Id).ToList();
                var ai = await _animalService.GetAnimalByLss(customerid);
                var totalAi = ai.Count();
                var animals = _animalService.GetAllAnimalByLss(customerid);
                var totalAnimal = animals.Count();
                var totalGoat = animals.Where(m => m.Species != null && m.Species.EnglishName.ToLower() == "goat").Count();
                var totalMaleGoat = animals.Where(m => m.Species != null && m.Species.EnglishName.ToLower() == "goat" && m.Gender!=null&&m.Gender.ToLower()=="male").Count();
                var totalFemaleGoat = animals.Where(m => m.Species != null && m.Species.EnglishName.ToLower() == "goat" && m.Gender != null && m.Gender.ToLower() == "female").Count();
                var totalCow = animals.Where(m => m.Species != null && m.Species.EnglishName.ToLower() == "cow").Count();
                var milkingbuffalo = animals.Where(m => m.Species != null && m.Species.EnglishName.ToLower() == "bufallo"&& m.MilkStatus.ToLower()== "milking ").Count();
                var milkingCow = animals.Where(m => m.Species != null && m.Species.EnglishName.ToLower() == "cow" && m.MilkStatus!=null&& m.MilkStatus.ToLower() == "milking ").Count();

                var buffalo = animals.Where(m => m.Species != null && m.Species.EnglishName.ToLower() == "buffalo").Count();
                var totalvaccination = _vaccinationService.GetVaccinationCountByCustomerIds(customerid);
                var totalFarm = _farmService.GetFarmCountByLssId(customerid);
                var farm = await _farmService.GetFarmByLssId(customerid);
                var privateFarm = farm.Where(m => m.FarmType!=null&&m.FarmType.ToLower() == "private").Count();
                var publicfarm = farm.Where(m => m.FarmType != null && m.FarmType.ToLower() == "public").Count();

                var currentFisaclYears = await _fiscalYearService.GetCurrentFiscalYear();
                string currentFisaclYear = null;
                if(currentFisaclYears!=null)
                {
                    currentFisaclYear = currentFisaclYears.Id;
                }
                else
                {
                    currentFisaclYear = null;
                }
                var Production =await _productionDataService.GetProduction();
                var productionByFiscalYear = (dynamic)null;
                if (currentFisaclYear != null)
                {
                     productionByFiscalYear = Production.Where(m => m.FiscalYear.Id == currentFisaclYear).ToList();
                }
                else
                {
                     productionByFiscalYear = 0;

                }

                decimal y = 0;
                var totalProduction = Production.Where(m => decimal.TryParse(m.Quantity,out y)).Sum(x=>y);


                model.Farm = totalFarm;
                model.Animal = totalAnimal;
                model.Cow = totalCow;
                model.Buffalo = buffalo;
                model.Ai = totalAi;
                model.Goat = totalGoat;
                model.Vaccination = totalvaccination;
                model.MilkingBuffalo = milkingbuffalo;
                model.MilkingCow = milkingCow.ToString();
                model.PrivateFarm = privateFarm.ToString();
                model.PublicFarm = publicfarm.ToString();
                model.FemaleGoat = totalFemaleGoat;
                model.MaleGoat = totalMaleGoat;
                model.Production = totalProduction;
            }
            else if (roles.Contains("NlboAdmin") || roles.Contains("NlboUser"))
            {
                var entityid = _workContext.CurrentCustomer.EntityId;
                var allCustomer = await _customerService.GetAllCustomers();
                List<string> customerid = allCustomer.Where(m => m.EntityId == entityid).Select(x => x.Id).ToList();
                var totalAi =  _aiService.GetAiCountByCustomerIds(customerid);
                var animals = _animalService.GetAllAnimalByLss(customerid);
                var totalAnimal = animals.Count();
                var totalGoat = animals.Where(m => m.Species != null && m.Species.EnglishName.ToLower() == "goat").Count();
                var totalMaleGoat = animals.Where(m => m.Species != null && m.Species.EnglishName.ToLower() == "goat" && m.Gender != null && m.Gender.ToLower() == "male").Count();
                var totalFemaleGoat = animals.Where(m => m.Species != null && m.Species.EnglishName.ToLower() == "goat" && m.Gender != null && m.Gender.ToLower() == "female").Count();
                var totalCow = animals.Where(m => m.Species != null && m.Species.EnglishName.ToLower() == "cow").Count();
                var milkingbuffalo = animals.Where(m => m.Species != null && m.Species.EnglishName.ToLower() == "bufallo" && m.MilkStatus.ToLower() == "milking ").Count();
                var milkingCow = animals.Where(m => m.Species != null && m.Species.EnglishName.ToLower() == "cow" && m.MilkStatus != null && m.MilkStatus.ToLower() == "milking ").Count();

                var buffalo = animals.Where(m => m.Species != null && m.Species.EnglishName.ToLower() == "buffalo").Count();
                var totalvaccination = _vaccinationService.GetVaccinationCountByCustomerIds(customerid);
                var totalFarm = _farmService.GetFarmCountByLssId(customerid);
                var farm = await _farmService.GetFarmByLssId(customerid);
                var privateFarm = farm.Where(m => m.FarmType != null && m.FarmType.ToLower() == "private").Count();
                var publicfarm = farm.Where(m => m.FarmType != null && m.FarmType.ToLower() == "public").Count();
                var semen =await _semenDistribution.GetSemenDistribution(customerid);
                decimal y = 0;
                 decimal semenproduction = semen.Where(m => decimal.TryParse(m.Dose, out y)).Sum(x => y);


                model.Farm = totalFarm;
                model.Animal = totalAnimal;
                model.Cow = totalCow;
                model.Buffalo = buffalo;
                model.Ai = totalAi;
                model.Goat = totalGoat;
                model.Vaccination = totalvaccination;
                model.MilkingBuffalo = milkingbuffalo;
                model.MilkingCow = milkingCow.ToString();
                model.PrivateFarm = privateFarm.ToString();
                model.PublicFarm = publicfarm.ToString();
                model.FemaleGoat = totalFemaleGoat;
                model.MaleGoat = totalMaleGoat;
                model.SemenDistribution = semenproduction;
            }


            else
            {
               
            }
            return View(model);

        }


        public ActionResult Website()
        {
            return View();
        } 
         public ActionResult AVMIS()
        {
            return View();
        } 
        public IActionResult Download() {



            var filepath = Path.Combine($"{_hostingEnvironment.WebRootPath}\\{"User.pdf"}");

            var net = new System.Net.WebClient();
            var data = net.DownloadData(filepath);
            var content = new System.IO.MemoryStream(data);
            var contentType = "APPLICATION/octet-stream";
            var fileName = "User.pdf";
            return File(content, contentType, fileName);






        }
        public async Task<IActionResult> GetAiData()
        {
            List<string> roles = _workContext.CurrentCustomer.CustomerRoles.Select(x => x.Name).ToList();
            var model = new BardataModel();
            model.Buffalo = new List<int>();
            model.Cow = new List<int>();
            model.yaxis = new List<string>();
            var fiscalyear = await _fiscalYearForGraphService.GetFiscalYear();

            if (roles.Contains("VhlsecUser") || roles.Contains("VhlsecAdmin"))
            {
                string vhlsecid = _workContext.CurrentCustomer.EntityId;
                List<string> lssId = _lssService.GetLssByVhlsecId(vhlsecid).Result.Select(m => m.Id).ToList();
                var customers = _customerService.GetCustomerByLssId(lssId, vhlsecid);
                List<string> customerid = customers.Select(x => x.Id).ToList();
                var ai = await _serviceData.GetService(customerid, "ai");

                int y = 0;
               
                foreach (var item in fiscalyear.FiscalYear)
                {
                     var ais = ai.Where(m=>m.FiscalYear.Id==item).GroupBy(m => m.FiscalYear.NepaliFiscalYear).Select(
                       g => new
                       {
                           cow = g.Where(m => m.Species.EnglishName.ToLower() == "cow").Where(m => int.TryParse(m.Quantity, out y)).Sum(x => y),
                           buffalo = g.Where(m => m.Species.EnglishName.ToLower() == "buffalo").Where(m => int.TryParse(m.Quantity, out y)).Sum(x => y)
                       }
                       ).ToList();
                    int buf = (ais.FirstOrDefault()!=null)? ais.FirstOrDefault().buffalo:0;
                    model.Buffalo.Add(buf);
                    model.yaxis.Add(_fiscalYearService.GetFiscalYearById(item).Result.NepaliFiscalYear);
                    model.Cow.Add((ais.FirstOrDefault() != null) ? ais.FirstOrDefault().cow:0);

                }

                return Json(model);

            }
            else if (roles.Contains(RoleHelper.LssAdmin) || roles.Contains(RoleHelper.LssUser))
            {
                var entityid = _workContext.CurrentCustomer.EntityId;
                var allCustomer = await _customerService.GetAllCustomers();
                List<string> customerid = allCustomer.Where(m => m.EntityId == entityid).Select(x => x.Id).ToList();

                var ai = await _serviceData.GetService(customerid, "ai");
                int y = 0;
                foreach (var item in fiscalyear.FiscalYear)
                {
                    var ais = ai.Where(m => m.FiscalYear.Id == item).GroupBy(m => m.FiscalYear.NepaliFiscalYear).Select(
                      g => new
                      {
                          cow = g.Where(m => m.Species.EnglishName.ToLower() == "cow").Where(m => int.TryParse(m.Quantity, out y)).Sum(x => y),
                          buffalo = g.Where(m => m.Species.EnglishName.ToLower() == "buffalo").Where(m => int.TryParse(m.Quantity, out y)).Sum(x => y)
                      }
                      ).ToList();
                    int buf = (ais.FirstOrDefault() != null) ? ais.FirstOrDefault().buffalo : 0;
                    model.Buffalo.Add(buf);
                    model.yaxis.Add(_fiscalYearService.GetFiscalYearById(item).Result.NepaliFiscalYear);
                    model.Cow.Add((ais.FirstOrDefault() != null) ? ais.FirstOrDefault().cow : 0);

                }
                return Json(model);


            }
            else
            {
                string dolfdId = _workContext.CurrentCustomer.EntityId;
                List<string> vhlsecId = _vhlsecService.GetVhlsecByDolfdId(dolfdId).Result.Select(m => m.Id).ToList();
                var LssIds = new List<string>();
                foreach (var item in vhlsecId)
                {
                    LssIds.AddRange(_lssService.GetLssByVhlsecId(item).Result.Select(m => m.Id).ToList());
                }
                var customers = _customerService.GetCustomerByLssId(LssIds, vhlsecId, dolfdId);
                List<string> customerid = customers.Select(x => x.Id).ToList();
                var ai = await _serviceData.GetService(customerid, "ai");
                int y = 0;
                foreach (var item in fiscalyear.FiscalYear)
                {
                    var ais = ai.Where(m => m.FiscalYear.Id == item).GroupBy(m => m.FiscalYear.NepaliFiscalYear).Select(
                      g => new
                      {
                          cow = g.Where(m => m.Species.EnglishName.ToLower() == "cow").Where(m => int.TryParse(m.Quantity, out y)).Sum(x => y),
                          buffalo = g.Where(m => m.Species.EnglishName.ToLower() == "buffalo").Where(m => int.TryParse(m.Quantity, out y)).Sum(x => y)
                      }
                      ).ToList();
                    int buf = (ais.FirstOrDefault() != null) ? ais.FirstOrDefault().buffalo : 0;
                    model.Buffalo.Add(buf);
                    model.yaxis.Add(_fiscalYearService.GetFiscalYearById(item).Result.NepaliFiscalYear);
                    model.Cow.Add((ais.FirstOrDefault() != null) ? ais.FirstOrDefault().cow : 0);

                }
                
                return Json(model);
            }
        }

        public async Task<IActionResult> GetVaccinationData()
        {
            List<string> roles = _workContext.CurrentCustomer.CustomerRoles.Select(x => x.Name).ToList();
            var model = new BardataModel();
            model.Buffalo = new List<int>();
            model.Cow = new List<int>();
            model.yaxis = new List<string>();
            var fiscalyear = await _fiscalYearForGraphService.GetFiscalYear();
            if (roles.Contains("VhlsecUser") || roles.Contains("VhlsecAdmin"))
            {
                string vhlsecid = _workContext.CurrentCustomer.EntityId;
                List<string> lssId = _lssService.GetLssByVhlsecId(vhlsecid).Result.Select(m => m.Id).ToList();
                var customers = _customerService.GetCustomerByLssId(lssId, vhlsecid);
                List<string> customerid = customers.Select(x => x.Id).ToList();
                var ai = await _serviceData.GetService(customerid,"vacccination");
               
                int y = 0;
                foreach (var item in fiscalyear.FiscalYear)
                {
                    var ais = ai.Where(m => m.FiscalYear.Id == item).GroupBy(m => m.FiscalYear.NepaliFiscalYear).Select(
                   g => new
                   {
                       cow = g.Where(m => m.Species.EnglishName.ToLower() == "cow" && m.Vaccination.MedicalName.ToLower() == "fmd").Where(m => int.TryParse(m.Quantity, out y)).Sum(x => y),
                       buffalo = g.Where(m => m.Species.EnglishName.ToLower() == "buffalo" && m.Vaccination.MedicalName.ToLower() == "fmd").Where(m => int.TryParse(m.Quantity, out y)).Sum(x => y),

                   }
                   );

                    int buf = (ais.FirstOrDefault() != null) ? ais.FirstOrDefault().buffalo : 0;
                    model.Buffalo.Add(buf);
                    model.yaxis.Add(_fiscalYearService.GetFiscalYearById(item).Result.NepaliFiscalYear);
                    model.Cow.Add((ais.FirstOrDefault() != null) ? ais.FirstOrDefault().cow : 0);
                }
                return Json(model);

            }
            else if (roles.Contains(RoleHelper.LssAdmin) || roles.Contains(RoleHelper.LssUser))
            {
                var entityid = _workContext.CurrentCustomer.EntityId;
                var allCustomer = await _customerService.GetAllCustomers();
                List<string> customerid = allCustomer.Where(m => m.EntityId == entityid).Select(x => x.Id).ToList();

                var ai = await _serviceData.GetService(customerid,"vaccination");
                int y = 0;
                foreach (var item in fiscalyear.FiscalYear)
                {
                    var ais = ai.Where(m => m.FiscalYear.Id == item).GroupBy(m => m.FiscalYear.NepaliFiscalYear).Select(
                   g => new
                   {
                       cow = g.Where(m => m.Species.EnglishName.ToLower() == "cow" && m.Vaccination!=null&&m.Vaccination.MedicalName.ToLower() == "fmd").Where(m => int.TryParse(m.Quantity, out y)).Sum(x => y),
                       buffalo = g.Where(m => m.Species.EnglishName.ToLower() == "buffalo" && m.Vaccination != null && m.Vaccination.MedicalName.ToLower() == "fmd").Where(m => int.TryParse(m.Quantity, out y)).Sum(x => y),

                   }
                   );

                    int buf = (ais.FirstOrDefault() != null) ? ais.FirstOrDefault().buffalo : 0;
                    model.Buffalo.Add(buf);
                    model.yaxis.Add(_fiscalYearService.GetFiscalYearById(item).Result.NepaliFiscalYear);
                    model.Cow.Add((ais.FirstOrDefault() != null) ? ais.FirstOrDefault().cow : 0);
                }

                return Json(model);


            }
            else
            {
                string dolfdId = _workContext.CurrentCustomer.EntityId;
                List<string> vhlsecId = _vhlsecService.GetVhlsecByDolfdId(dolfdId).Result.Select(m => m.Id).ToList();
                var LssIds = new List<string>();
                foreach (var item in vhlsecId)
                {
                    LssIds.AddRange(_lssService.GetLssByVhlsecId(item).Result.Select(m => m.Id).ToList());
                }
                var customers = _customerService.GetCustomerByLssId(LssIds, vhlsecId, dolfdId);
                List<string> customerid = customers.Select(x => x.Id).ToList();
                var ai = await _serviceData.GetService(customerid,"vacccination");
                int y = 0;
                foreach (var item in fiscalyear.FiscalYear)
                {
                    var ais = ai.Where(m => m.FiscalYear.Id == item).GroupBy(m => m.FiscalYear.NepaliFiscalYear).Select(
                   g => new
                   {
                       cow = g.Where(m => m.Species.EnglishName.ToLower() == "cow" && m.Vaccination.MedicalName.ToLower() == "fmd").Where(m => int.TryParse(m.Quantity, out y)).Sum(x => y),
                       buffalo = g.Where(m => m.Species.EnglishName.ToLower() == "buffalo" && m.Vaccination.MedicalName.ToLower() == "fmd").Where(m => int.TryParse(m.Quantity, out y)).Sum(x => y),

                   }
                   );

                    int buf = (ais.FirstOrDefault() != null) ? ais.FirstOrDefault().buffalo : 0;
                    model.Buffalo.Add(buf);
                    model.yaxis.Add(_fiscalYearService.GetFiscalYearById(item).Result.NepaliFiscalYear);
                    model.Cow.Add((ais.FirstOrDefault() != null) ? ais.FirstOrDefault().cow : 0);
                }

                return Json(model);
            }

        }
        public async Task<IActionResult> GetMilkProductionData()
        {
            List<string> roles = _workContext.CurrentCustomer.CustomerRoles.Select(x => x.Name).ToList();
            var model = new ProductionData();
            model.xaxis = new List<int>();
            model.yaxis = new List<string>();
            var fiscalyear = await _fiscalYearForGraphService.GetFiscalYear();

            if (roles.Contains("VhlsecUser") || roles.Contains("VhlsecAdmin"))
            {
                string vhlsecid = _workContext.CurrentCustomer.EntityId;
                List<string> lssId = _lssService.GetLssByVhlsecId(vhlsecid).Result.Select(m => m.Id).ToList();
                var customers = _customerService.GetCustomerByLssId(lssId, vhlsecid);
                List<string> customerid = customers.Select(x => x.Id).ToList();
                var ai = await _productionDataService.GetProduction(customerid,"milk");

                int y = 0;
                foreach (var item in fiscalyear.FiscalYear)
                {
                    var ais = ai.Where(m => m.FiscalYear.Id == item).GroupBy(m => m.FiscalYear.NepaliFiscalYear).Select(
                   g => new
                   {
                       fiscalyear = g.FirstOrDefault().FiscalYear.NepaliFiscalYear,
                       milk = g.Where(m => int.TryParse(m.Quantity, out y)).Sum(x => y),
                   }
                   );
                    model.yaxis.Add(_fiscalYearService.GetFiscalYearById(item).Result.NepaliFiscalYear);
                    model.xaxis.Add((ais.FirstOrDefault() != null) ? ais.FirstOrDefault().milk : 0);

                }
                return Json(model);

            }
            else if (roles.Contains(RoleHelper.LssAdmin) || roles.Contains(RoleHelper.LssUser))
            {
                var entityid = _workContext.CurrentCustomer.EntityId;
                var allCustomer = await _customerService.GetAllCustomers();
                List<string> customerid = allCustomer.Where(m => m.EntityId == entityid).Select(x => x.Id).ToList();

                var ai = await _productionDataService.GetProduction(customerid, "milk");
                int y = 0;
                foreach (var item in fiscalyear.FiscalYear)
                {
                    var ais = ai.Where(m => m.FiscalYear.Id == item).GroupBy(m => m.FiscalYear.NepaliFiscalYear).Select(
                   g => new
                   {
                       fiscalyear = g.FirstOrDefault().FiscalYear.NepaliFiscalYear,
                       milk = g.Where(m => int.TryParse(m.Quantity, out y)).Sum(x => y),
                   }
                   );
                    model.yaxis.Add(_fiscalYearService.GetFiscalYearById(item).Result.NepaliFiscalYear);
                    model.xaxis.Add((ais.FirstOrDefault() != null) ? ais.FirstOrDefault().milk : 0);

                }
                return Json(model);


            }
            else
            {
                string dolfdId = _workContext.CurrentCustomer.EntityId;
                List<string> vhlsecId = _vhlsecService.GetVhlsecByDolfdId(dolfdId).Result.Select(m => m.Id).ToList();
                var LssIds = new List<string>();
                foreach (var item in vhlsecId)
                {
                    LssIds.AddRange(_lssService.GetLssByVhlsecId(item).Result.Select(m => m.Id).ToList());
                }
                var customers = _customerService.GetCustomerByLssId(LssIds, vhlsecId, dolfdId);
                List<string> customerid = customers.Select(x => x.Id).ToList();
                var ai = await _productionDataService.GetProduction(customerid, "milk");
                int y = 0;
                foreach (var item in fiscalyear.FiscalYear)
                {
                    var ais = ai.Where(m => m.FiscalYear.Id == item).GroupBy(m => m.FiscalYear.NepaliFiscalYear).Select(
                   g => new
                   {
                       fiscalyear = g.FirstOrDefault().FiscalYear.NepaliFiscalYear,
                       milk = g.Where(m => int.TryParse(m.Quantity, out y)).Sum(x => y),
                   }
                   );
                    model.yaxis.Add(_fiscalYearService.GetFiscalYearById(item).Result.NepaliFiscalYear);
                    model.xaxis.Add(((ais.FirstOrDefault() != null) ? ais.FirstOrDefault().milk : 0)/1000);

                }
                return Json(model);
            }

        }

        public async Task<IActionResult> GetMeatProduction()
        {
            List<string> roles = _workContext.CurrentCustomer.CustomerRoles.Select(x => x.Name).ToList();
            var model = new ProductionData();
            model.xaxis = new List<int>();
            model.yaxis = new List<string>();
            var fiscalyear = await _fiscalYearForGraphService.GetFiscalYear();
            if (roles.Contains("VhlsecUser") || roles.Contains("VhlsecAdmin"))
            {
                string vhlsecid = _workContext.CurrentCustomer.EntityId;
                List<string> lssId = _lssService.GetLssByVhlsecId(vhlsecid).Result.Select(m => m.Id).ToList();
                var customers = _customerService.GetCustomerByLssId(lssId, vhlsecid);
                List<string> customerid = customers.Select(x => x.Id).ToList();
                var ai = await _productionDataService.GetProduction(customerid, "meat");

                int y = 0;
                foreach (var item in fiscalyear.FiscalYear)
                {
                    var ais = ai.Where(m => m.FiscalYear.Id == item).GroupBy(m => m.FiscalYear.NepaliFiscalYear).Select(
                   g => new
                   {
                       fiscalyear = g.FirstOrDefault().FiscalYear.NepaliFiscalYear,
                       milk = g.Where(m => int.TryParse(m.Quantity, out y)).Sum(x => y),
                   }
                   );
                    model.yaxis.Add(_fiscalYearService.GetFiscalYearById(item).Result.NepaliFiscalYear);
                    model.xaxis.Add(((ais.FirstOrDefault() != null) ? ais.FirstOrDefault().milk : 0) / 1000);

                }
                return Json(model);

            }
            else if (roles.Contains(RoleHelper.LssAdmin) || roles.Contains(RoleHelper.LssUser))
            {
                var entityid = _workContext.CurrentCustomer.EntityId;
                var allCustomer = await _customerService.GetAllCustomers();
                List<string> customerid = allCustomer.Where(m => m.EntityId == entityid).Select(x => x.Id).ToList();

                var ai = await _productionDataService.GetProduction(customerid, "meat");
                int y = 0;
                foreach (var item in fiscalyear.FiscalYear)
                {
                    var ais = ai.Where(m => m.FiscalYear.Id == item).GroupBy(m => m.FiscalYear.NepaliFiscalYear).Select(
                   g => new
                   {
                       fiscalyear = g.FirstOrDefault().FiscalYear.NepaliFiscalYear,
                       milk = g.Where(m => int.TryParse(m.Quantity, out y)).Sum(x => y),
                   }
                   );
                    model.yaxis.Add(_fiscalYearService.GetFiscalYearById(item).Result.NepaliFiscalYear);
                    model.xaxis.Add(((ais.FirstOrDefault() != null) ? ais.FirstOrDefault().milk : 0) / 1000);

                }
                return Json(model);


            }
        
            
            else
            {
                string dolfdId = _workContext.CurrentCustomer.EntityId;
                List<string> vhlsecId = _vhlsecService.GetVhlsecByDolfdId(dolfdId).Result.Select(m => m.Id).ToList();
                var LssIds = new List<string>();
                foreach (var item in vhlsecId)
                {
                    LssIds.AddRange(_lssService.GetLssByVhlsecId(item).Result.Select(m => m.Id).ToList());
                }
                var customers = _customerService.GetCustomerByLssId(LssIds, vhlsecId, dolfdId);
                List<string> customerid = customers.Select(x => x.Id).ToList();
                var ai = await _productionDataService.GetProduction(customerid, "meat");
                int y = 0;
                foreach (var item in fiscalyear.FiscalYear)
                {
                    var ais = ai.Where(m => m.FiscalYear.Id == item).GroupBy(m => m.FiscalYear.NepaliFiscalYear).Select(
                   g => new
                   {
                       fiscalyear = g.FirstOrDefault().FiscalYear.NepaliFiscalYear,
                       milk = g.Where(m => int.TryParse(m.Quantity, out y)).Sum(x => y),
                   }
                   );
                    model.yaxis.Add(_fiscalYearService.GetFiscalYearById(item).Result.NepaliFiscalYear);
                    model.xaxis.Add(((ais.FirstOrDefault() != null) ? ais.FirstOrDefault().milk : 0) / 1000);

                }
                return Json(model);
            }

        }
        public async Task<IActionResult> GetEggProduction()
        {
            List<string> roles = _workContext.CurrentCustomer.CustomerRoles.Select(x => x.Name).ToList();
            var model = new ProductionData();
            model.xaxis = new List<int>();
            model.yaxis = new List<string>();
            var fiscalyear = await _fiscalYearForGraphService.GetFiscalYear();
            if (roles.Contains("VhlsecUser") || roles.Contains("VhlsecAdmin"))
            {
                string vhlsecid = _workContext.CurrentCustomer.EntityId;
                List<string> lssId = _lssService.GetLssByVhlsecId(vhlsecid).Result.Select(m => m.Id).ToList();
                var customers = _customerService.GetCustomerByLssId(lssId, vhlsecid);
                List<string> customerid = customers.Select(x => x.Id).ToList();
                var ai = await _productionDataService.GetProduction(customerid, "egg");

                int y = 0;
                foreach (var item in fiscalyear.FiscalYear)
                {
                    var ais = ai.Where(m => m.FiscalYear.Id == item).GroupBy(m => m.FiscalYear.NepaliFiscalYear).Select(
                   g => new
                   {
                       fiscalyear = g.FirstOrDefault().FiscalYear.NepaliFiscalYear,
                       milk = g.Where(m => int.TryParse(m.Quantity, out y)).Sum(x => y),
                   }
                   );
                    model.yaxis.Add(_fiscalYearService.GetFiscalYearById(item).Result.NepaliFiscalYear);
                    model.xaxis.Add(((ais.FirstOrDefault() != null) ? ais.FirstOrDefault().milk : 0) / 1000);

                }
                return Json(model);

            }
            else if (roles.Contains(RoleHelper.LssAdmin) || roles.Contains(RoleHelper.LssUser))
            {
                var entityid = _workContext.CurrentCustomer.EntityId;
                var allCustomer = await _customerService.GetAllCustomers();
                List<string> customerid = allCustomer.Where(m => m.EntityId == entityid).Select(x => x.Id).ToList();

                var ai = await _productionDataService.GetProduction(customerid, "egg");
                int y = 0;
                foreach (var item in fiscalyear.FiscalYear)
                {
                    var ais = ai.Where(m => m.FiscalYear.Id == item).GroupBy(m => m.FiscalYear.NepaliFiscalYear).Select(
                   g => new
                   {
                       fiscalyear = g.FirstOrDefault().FiscalYear.NepaliFiscalYear,
                       milk = g.Where(m => int.TryParse(m.Quantity, out y)).Sum(x => y),
                   }
                   );
                    model.yaxis.Add(_fiscalYearService.GetFiscalYearById(item).Result.NepaliFiscalYear);
                    model.xaxis.Add(((ais.FirstOrDefault() != null) ? ais.FirstOrDefault().milk : 0) / 1000);

                }
                return Json(model);


            }


            else
            {
                string dolfdId = _workContext.CurrentCustomer.EntityId;
                List<string> vhlsecId = _vhlsecService.GetVhlsecByDolfdId(dolfdId).Result.Select(m => m.Id).ToList();
                var LssIds = new List<string>();
                foreach (var item in vhlsecId)
                {
                    LssIds.AddRange(_lssService.GetLssByVhlsecId(item).Result.Select(m => m.Id).ToList());
                }
                var customers = _customerService.GetCustomerByLssId(LssIds, vhlsecId, dolfdId);
                List<string> customerid = customers.Select(x => x.Id).ToList();
                var ai = await _productionDataService.GetProduction(customerid, "egg");
                int y = 0;
                foreach (var item in fiscalyear.FiscalYear)
                {
                    var ais = ai.Where(m => m.FiscalYear.Id == item).GroupBy(m => m.FiscalYear.NepaliFiscalYear).Select(
                   g => new
                   {
                       fiscalyear = g.FirstOrDefault().FiscalYear.NepaliFiscalYear,
                       milk = g.Where(m => int.TryParse(m.Quantity, out y)).Sum(x => y),
                   }
                   );
                    model.yaxis.Add(_fiscalYearService.GetFiscalYearById(item).Result.NepaliFiscalYear);
                    model.xaxis.Add(((ais.FirstOrDefault() != null) ? ais.FirstOrDefault().milk : 0) / 1000);

                }
                return Json(model);
            }

        }

        public async Task<IActionResult> GetWoolProduction()
        {
            List<string> roles = _workContext.CurrentCustomer.CustomerRoles.Select(x => x.Name).ToList();
            var model = new ProductionData();
            model.xaxis = new List<int>();
            model.yaxis = new List<string>();
            var fiscalyear = await _fiscalYearForGraphService.GetFiscalYear();
            if (roles.Contains("VhlsecUser") || roles.Contains("VhlsecAdmin"))
            {
                string vhlsecid = _workContext.CurrentCustomer.EntityId;
                List<string> lssId = _lssService.GetLssByVhlsecId(vhlsecid).Result.Select(m => m.Id).ToList();
                var customers = _customerService.GetCustomerByLssId(lssId, vhlsecid);
                List<string> customerid = customers.Select(x => x.Id).ToList();
                var ai = await _productionDataService.GetProduction(customerid, "wool");

                int y = 0;
                foreach (var item in fiscalyear.FiscalYear)
                {
                    var ais = ai.Where(m => m.FiscalYear.Id == item).GroupBy(m => m.FiscalYear.NepaliFiscalYear).Select(
                   g => new
                   {
                       fiscalyear = g.FirstOrDefault().FiscalYear.NepaliFiscalYear,
                       milk = g.Where(m => int.TryParse(m.Quantity, out y)).Sum(x => y),
                   }
                   );
                    model.yaxis.Add(_fiscalYearService.GetFiscalYearById(item).Result.NepaliFiscalYear);
                    model.xaxis.Add(((ais.FirstOrDefault() != null) ? ais.FirstOrDefault().milk : 0) );

                }
                return Json(model);

            }
            else if (roles.Contains(RoleHelper.LssAdmin) || roles.Contains(RoleHelper.LssUser))
            {
                var entityid = _workContext.CurrentCustomer.EntityId;
                var allCustomer = await _customerService.GetAllCustomers();
                List<string> customerid = allCustomer.Where(m => m.EntityId == entityid).Select(x => x.Id).ToList();

                var ai = await _productionDataService.GetProduction(customerid, "wool");
                int y = 0;
                foreach (var item in fiscalyear.FiscalYear)
                {
                    var ais = ai.Where(m => m.FiscalYear.Id == item).GroupBy(m => m.FiscalYear.NepaliFiscalYear).Select(
                   g => new
                   {
                       fiscalyear = g.FirstOrDefault().FiscalYear.NepaliFiscalYear,
                       milk = g.Where(m => int.TryParse(m.Quantity, out y)).Sum(x => y),
                   }
                   );
                    model.yaxis.Add(_fiscalYearService.GetFiscalYearById(item).Result.NepaliFiscalYear);
                    model.xaxis.Add(((ais.FirstOrDefault() != null) ? ais.FirstOrDefault().milk : 0) );

                }
                return Json(model);


            }


            else
            {
                string dolfdId = _workContext.CurrentCustomer.EntityId;
                List<string> vhlsecId = _vhlsecService.GetVhlsecByDolfdId(dolfdId).Result.Select(m => m.Id).ToList();
                var LssIds = new List<string>();
                foreach (var item in vhlsecId)
                {
                    LssIds.AddRange(_lssService.GetLssByVhlsecId(item).Result.Select(m => m.Id).ToList());
                }
                var customers = _customerService.GetCustomerByLssId(LssIds, vhlsecId, dolfdId);
                List<string> customerid = customers.Select(x => x.Id).ToList();
                var ai = await _productionDataService.GetProduction(customerid, "wool");
                int y = 0;
                foreach (var item in fiscalyear.FiscalYear)
                {
                    var ais = ai.Where(m => m.FiscalYear.Id == item).GroupBy(m => m.FiscalYear.NepaliFiscalYear).Select(
                   g => new
                   {
                       fiscalyear = g.FirstOrDefault().FiscalYear.NepaliFiscalYear,
                       milk = g.Where(m => int.TryParse(m.Quantity, out y)).Sum(x => y),
                   }
                   );
                    model.yaxis.Add(_fiscalYearService.GetFiscalYearById(item).Result.NepaliFiscalYear);
                    model.xaxis.Add(((ais.FirstOrDefault() != null) ? ais.FirstOrDefault().milk : 0) );

                }
                return Json(model);
            }

        }

    }
}
