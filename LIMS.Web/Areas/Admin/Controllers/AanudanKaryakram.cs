using LIMS.Core;
using LIMS.Domain.Bali;
using LIMS.Domain.MoAMAC;
using LIMS.Framework.Kendoui;
using LIMS.Framework.Mvc.Filters;
using LIMS.Framework.Security.Authorization;
using LIMS.Services.Bali;
using LIMS.Services.Basic;
using LIMS.Services.Breed;
using LIMS.Services.Customers;
using LIMS.Services.Localization;
using LIMS.Services.Media;
using LIMS.Services.MoAMAC;
using LIMS.Services.Security;
using LIMS.Web.Areas.Admin.Extensions.Mapping;
using LIMS.Web.Areas.Admin.Helper;
using LIMS.Web.Areas.Admin.Models.Bali;
using LIMS.Web.Areas.Admin.Models.Bali.Aanudan;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Web.Areas.Admin.Controllers
{
    public class AanudanKaryakramController : BaseAdminController
    {
        private readonly ILabambitKrishakService _animalRegistrationService;
        private readonly IPujigatKharchaKharakramService _pujigatKharchaKharakramService;

        private readonly ISpeciesService _speciesService;
        private readonly IBreedService _breedService;
        private readonly ILocalizationService _localizationService;
        private readonly ILanguageService _languageService;
        private readonly IWorkContext _workContext;
        private readonly IFiscalYearService _fiscalYearService;
        private readonly ITalimService _talimService;
        private readonly IIncuvationCenterService _incuvationCenterService;
        private readonly IPictureService _pictureService;
        private readonly IAnudanService _anudanService;
        public readonly IDolfdService _dolfdService;
        public readonly IVhlsecService _vhlsecService;
        public readonly ICustomerService _customerService;

        public AanudanKaryakramController(ILocalizationService localizationService,
            ILabambitKrishakService animalRegistrationService,
            ILanguageService languageService,
            ISpeciesService speciesService,
            IBreedService breedService,
            IWorkContext workContext,
            IFiscalYearService fiscalYearService,
            ITalimService talimService,
            IIncuvationCenterService incuvationCenterService,
            IPujigatKharchaKharakramService pujigatKharchaKharakramService,
            IPictureService pictureService,
            IAnudanService anudanService,
            IDolfdService dolfdService,
            IVhlsecService vhlsecService,
             ICustomerService customerService


            )
        {
            _localizationService = localizationService;
            _animalRegistrationService = animalRegistrationService;
            _languageService = languageService;
            _speciesService = speciesService;
            _breedService = breedService;
            _workContext = workContext;
            _fiscalYearService = fiscalYearService;
            _talimService = talimService;
            _incuvationCenterService = incuvationCenterService;
            _pujigatKharchaKharakramService = pujigatKharchaKharakramService;
            _pictureService = pictureService;
            _anudanService = anudanService;
            _dolfdService = dolfdService;
            _vhlsecService = vhlsecService;
            _customerService = customerService;
        }

        public IActionResult Index() => RedirectToAction("List");

        public async Task<IActionResult> TabView(int index=0)
        {
            await SaveSelectedTabIndex(index);
            return View("TabView");
        }

        public IActionResult List() => View();

        [PermissionAuthorizeAction(PermissionActionName.List)]
        [HttpPost]
        public async Task<IActionResult> List(DataSourceRequest command)
        {
            var id = _workContext.CurrentCustomer.Id;
            var bali = await _anudanService.GetbaliRegister(id, command.Page - 1, command.PageSize);
            var gridModel = new DataSourceResult {
                Data = bali,
                Total = bali.TotalCount
            };
            return Json(gridModel);
        }

        public async Task<IActionResult> Report()
        {
            var id = _workContext.CurrentCustomer.Id;
            var role = _workContext.CurrentCustomer.CustomerRoles.Select(m => m.Name).ToList();
            var species = new SelectList(await _speciesService.GetSpecies(), "Id", "EnglishName").ToList();
            species.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.SpeciesId = species;

            var fiscalyear = await _fiscalYearService.GetCurrentFiscalYear();
            
            var fiscalYear = new SelectList(await _fiscalYearService.GetFiscalYear(), "Id", "NepaliFiscalYear", fiscalyear.Id).ToList();
            fiscalYear.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.FiscalYearId = fiscalYear;
            var type = PujigatType();
            type.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.Type = type;
            var programType = ProgramType();
            programType.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.ProgramType = programType;
            var month = new MonthHelper();
            var months = month.GetMonths();

            months.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.Month = months;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Report(DataSourceRequest command, string type, string programType, string fiscalYear)
        {
            if (type != null || fiscalYear != null||programType!=null)
            {
                var id = _workContext.CurrentCustomer.Id;
                var krishak = await _anudanService.GetFilteredLabambitKrishak(id, fiscalYear, programType, type);
                List<AanudanReport> report = new List<AanudanReport>();
                foreach (var item in krishak)
                {
                    var labambit = new AanudanReport();
                    labambit.pujigatKharchaKharakram = item.PujigatKharchaKharakram;
                    labambit.Rakam = Convert.ToString(item.AanudanRakam);
                    labambit.PhoneNo = item.PhoneNo;
                    labambit.Name = item.KrishakKoName;
                    labambit.Address = item.District + " " + item.LocalLevel;
                    labambit.MaleMember = item.MaleMember.ToString();
                    labambit.FemaleMember = item.FemaleMember.ToString();
                    labambit.DalitMember = item.DalitMember.ToString();
                    labambit.JanajatiMember = item.JanajatiMember.ToString();
                    labambit.Id = item.Id;

                    report.Add(labambit);
                    //labambit.Male=.

                }
                var gridModel = new DataSourceResult {
                    Data = report,
                    Total = report.Count()
                };
                return Json(gridModel);
            }

            else
            {
                List<MonthlyProgressReport> report = new List<MonthlyProgressReport>();

                var gridModel = new DataSourceResult {
                    Data = report,
                    Total = report.Count
                };
                return Json(gridModel);

            }
        }

        public async Task<IActionResult> OrgReport()
        {
            var id = _workContext.CurrentCustomer.Id;
            var role = _workContext.CurrentCustomer.CustomerRoles.Select(m => m.Name).ToList();
            var species = new SelectList(await _speciesService.GetSpecies(), "Id", "EnglishName").ToList();
            species.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.SpeciesId = species;

            var fiscalyear = await _fiscalYearService.GetCurrentFiscalYear();

            var fiscalYear = new SelectList(await _fiscalYearService.GetFiscalYear(), "Id", "NepaliFiscalYear", fiscalyear.Id).ToList();
            fiscalYear.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.FiscalYearId = fiscalYear;
            var type = PujigatType();
            type.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.Type = type;
            var programType = ProgramType();
            programType.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.ProgramType = programType;
            var month = new MonthHelper();
            var months = month.GetMonths();

            months.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.Month = months;
            if (role.Contains("MolmacAdmin") || role.Contains("MolmacUser"))
            {
                string entityId = _workContext.CurrentCustomer.EntityId;
                List<Dolfd> dolfdid = _dolfdService.GetDolfdByMolmacId(entityId).Result.ToList();

                var lss = new SelectList(dolfdid, "Id", "NameEnglish").ToList();
                lss.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
                ViewBag.dolfd = lss;
            }
            else if (role.Contains("DolfdAdmin") || role.Contains("DolfdUser") || role.Contains("AddAdmin") || role.Contains("AddUser"))
            {
                string entityId = _workContext.CurrentCustomer.EntityId;
                List<Vhlsec> dolfdid = _vhlsecService.GetVhlsecByDolfdId(entityId).Result.ToList();

                var lss = new SelectList(dolfdid, "Id", "NameEnglish").ToList();
                lss.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
                ViewBag.vhlsec = lss;
            }

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> OrgReport(DataSourceRequest command, string type, string programType, string fiscalYear, string vhlsecid, string dolfdid)
        {
            if (!string.IsNullOrEmpty(dolfdid) && string.IsNullOrEmpty(vhlsecid))
            {

                List<string> entities = _vhlsecService.GetVhlsecByDolfdId(dolfdid).Result.Select(m => m.Id).ToList();
                var customers = _customerService.GetCustomerByLssId(entities, dolfdid);
                List<string> customerid = customers.Select(x => x.Id).ToList();

                var id = _workContext.CurrentCustomer.Id;
                var krishak = await _anudanService.GetFilteredLabambitKrishak(customerid, fiscalYear, programType, type);
                List<AanudanReport> report = new List<AanudanReport>();
                foreach (var item in krishak)
                {
                    var labambit = new AanudanReport();
                    labambit.Id = item.Id;
                    labambit.pujigatKharchaKharakram = item.PujigatKharchaKharakram;
                    labambit.Rakam = Convert.ToString(item.AanudanRakam);
                    labambit.PhoneNo = item.PhoneNo;
                    labambit.Name = item.KrishakKoName;
                    labambit.Address = item.District + " " + item.LocalLevel;
                    labambit.OrgName = _customerService.GetCustomerById(item.CreatedBy).Result.OrgName;
                    report.Add(labambit);
                    //labambit.Male=.

                }
                var gridModel = new DataSourceResult {
                    Data = report,
                    Total = report.Count()
                };
                return Json(gridModel);
            }
            else if (!string.IsNullOrEmpty(vhlsecid))
            {
                string entity = vhlsecid;
                var customers = _customerService.GetCustomerByLssId(null, entity);
                List<string> customerid = customers.Select(x => x.Id).ToList();
                var krishak = await _anudanService.GetFilteredLabambitKrishak(customerid, fiscalYear, programType, type);
                List<AanudanReport> report = new List<AanudanReport>();
                foreach (var item in krishak)
                {
                    var labambit = new AanudanReport();
                    labambit.Id = item.Id;
                    labambit.pujigatKharchaKharakram = item.PujigatKharchaKharakram;
                    labambit.Rakam = Convert.ToString(item.AanudanRakam);
                    labambit.PhoneNo = item.PhoneNo;
                    labambit.Name = item.KrishakKoName;
                    labambit.Address = item.District + " " + item.LocalLevel;
                    labambit.OrgName = _customerService.GetCustomerById(item.CreatedBy).Result.OrgName;

                    report.Add(labambit);
                    //labambit.Male=.

                }
                var gridModel = new DataSourceResult {
                    Data = report,
                    Total = report.Count()
                };
                return Json(gridModel);
            }
            else
            {
                var id = _workContext.CurrentCustomer.Id;
                var krishak = await _anudanService.GetFilteredLabambitKrishak(id, fiscalYear, programType, type);
                List<AanudanReport> report = new List<AanudanReport>();
                foreach (var item in krishak)
                {
                    var labambit = new AanudanReport();
                    labambit.pujigatKharchaKharakram = item.PujigatKharchaKharakram;
                    labambit.Rakam = Convert.ToString(item.AanudanRakam);
                    labambit.PhoneNo = item.PhoneNo;
                    labambit.Name = item.KrishakKoName;
                    labambit.Address = item.District + " " + item.LocalLevel;
                    labambit.OrgName = _customerService.GetCustomerById(item.CreatedBy).Result.OrgName;

                    report.Add(labambit);
                    //labambit.Male=.

                }
                var gridModel = new DataSourceResult {
                    Data = report,
                    Total = report.Count()
                };
                return Json(gridModel);

            }
            
        }


        public async Task<IActionResult> Create()
        {
            var createdby = _workContext.CurrentCustomer.Id;
            var fiscal = await _fiscalYearService.GetCurrentFiscalYear();
            var fiscalYear = new SelectList(await _fiscalYearService.GetFiscalYear(), "Id", "NepaliFiscalYear",fiscal.Id).ToList();
            fiscalYear.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.FiscalYearId = fiscalYear;
            var pujigatKaryakram = new SelectList(await _pujigatKharchaKharakramService.GetPujigatKharchaKharakram(createdby), "Id", "Program").ToList();
            pujigatKaryakram.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.pujigatKaryakram = pujigatKaryakram;

            var sex = new List<SelectListItem>() {
                new SelectListItem {
                    Text="Male",
                    Value="Male"
                },
                  new SelectListItem {
                    Text="Female",
                    Value="Female"
                },
            };

            sex.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.Sex = sex;
            var EthnicGroup = new List<SelectListItem>() {
                new SelectListItem {
                    Text="Dalit",
                    Value="Dalit"
                },
                  new SelectListItem {
                    Text="Janajati",
                    Value="Janajati"
                },
                  new SelectListItem {
                    Text="Aanya",
                    Value="Aanya"
                },
            };
            EthnicGroup.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.EthnicGroup = EthnicGroup;

            AanudanModel model = new AanudanModel();
            model.District = _workContext.CurrentCustomer.OrgAddress;
            return View(model);
        }

        [PermissionAuthorizeAction(PermissionActionName.Edit)]
        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public async Task<IActionResult> Create(AanudanModel model, IFormCollection col)
        {
            
                var animalRegistration = model.ToEntity();
                animalRegistration.CreatedBy = _workContext.CurrentCustomer.Id;
               
                animalRegistration.FiscalYear = await _fiscalYearService.GetFiscalYearById(model.FiscalyearId);
                animalRegistration.PujigatKharchaKharakram = await _pujigatKharchaKharakramService.GetPujigatKharchaKharakramById(model.PujigatKharchaKaryakramId);
                animalRegistration.CreatedBy = _workContext.CurrentCustomer.Id;


            var Male = col["MaleMember"].ToList();
            var Female = col["FemaleMember"].ToList();
            var Dalit = col["DalitMember"].ToList();
            var Janajati = col["JanajatiMember"].ToList();
            var Others = col["Others"].ToList();

           
            var Name = col["KrishakKoName"].ToList();
            var Address = col["LocalLevel"].ToList();
            var Phone = col["PhoneNo"].ToList();
            // var WardNo = col["WardNo"].ToList();
            var Ward = col["Ward"].ToList();
            var AnudanReceiverType = col["AnudanReceiverType"].ToList();
           // var Area = col["Area"].ToList();
            var category = col["AanudanKokisim"];
            var subsidycategory = col["SubsidyCategory"];
            var AanudanRakam = col["AanudanRakam"];
            var FarmerContribution = col["FarmerContribution"];
            var Remarks = col["Remarks"].ToList();
            var ExpectedOutput = col["ExpectedOutput"].ToList();

            var LivestockDataId = col["LivestockDataId"].ToList();
            List<AanudanKokaryakram> update = new List<AanudanKokaryakram>();
            List<AanudanKokaryakram> insert = new List<AanudanKokaryakram>();
            for (int i = 0; i < Name.Count(); i++)
            {
                if (string.IsNullOrEmpty(Name[i]))
                    continue;
                AanudanKokaryakram farm = new AanudanKokaryakram();

                farm.PujigatKharchaKharakram = animalRegistration.PujigatKharchaKharakram;
                farm.PujigatKharchaKaryakramId = animalRegistration.PujigatKharchaKaryakramId;
                farm.FiscalYear = animalRegistration.FiscalYear;
                farm.FiscalyearId = animalRegistration.FiscalyearId;
                farm.CreatedBy = animalRegistration.CreatedBy;
                farm.District = animalRegistration.District;
                farm.KrishakKoName = Name[i];
                farm.PhoneNo = Phone[i];
                farm.LocalLevel = Address[i];
                farm.Ward = Ward[i];
                farm.MaleMember =Convert.ToInt32(Male[i]);
                farm.FemaleMember = Convert.ToInt32(Female[i]);
                farm.DalitMember = Convert.ToInt32(Dalit[i]);
                farm.JanajatiMember = Convert.ToInt32(Janajati[i]);
                farm.Others = Convert.ToInt32(Others[i]);
                farm.SubsidyCategory = subsidycategory[i];
                farm.Remaks = Remarks[i];
                farm.ExpectedOutput = ExpectedOutput[i];
                farm.AanudanKokisim = category[i];
              
                farm.AnudanReceiverType = AnudanReceiverType[i];
                farm.AanudanRakam = Convert.ToDecimal(AanudanRakam[i]);
                farm.FarmerContribution = Convert.ToDecimal(FarmerContribution[i]);
                if (!string.IsNullOrEmpty(LivestockDataId[i]))
                {
                    farm.Id = LivestockDataId[i];
                    await _anudanService.UpdatebaliRegister(farm);
                }
                else
                {
                  
                    await _anudanService.InsertbaliRegister(farm);
                }


            }
            

            //  await _anudanService.InsertbaliRegister(animalRegistration);

            SuccessNotification(_localizationService.GetResource("Admin.Create.successful"));
               // return continueEditing ? RedirectToAction("Edit", new { id = animalRegistration.Id }) : RedirectToAction("TabView");
            
            var createdby = _workContext.CurrentCustomer.Id;
            var incuvationCenter = new SelectList(await _incuvationCenterService.GetincuvationCenter(createdby), "Id", "OrganizationNameEnglish").ToList();
            incuvationCenter.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.IncuvationCenter = incuvationCenter;
            var talim = new SelectList(await _talimService.Gettalim(createdby), "Id", "NameEnglish").ToList();
            talim.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.Talim = talim;
            var pujigatKaryakram = new SelectList(await _pujigatKharchaKharakramService.GetPujigatKharchaKharakram(createdby), "Id", "Program").ToList();
            pujigatKaryakram.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.pujigatKaryakram = pujigatKaryakram;

            var species = new SelectList(await _speciesService.GetSpecies(), "Id", "EnglishName").ToList();
            species.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.SpeciesId = species;
            ViewBag.AllLanguages = await _languageService.GetAllLanguages(true);
            var fiscalYear = new SelectList(await _fiscalYearService.GetFiscalYear(), "Id", "NepaliFiscalYear").ToList();
            fiscalYear.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.FiscalYearId = fiscalYear;
            var EthnicGroup = new List<SelectListItem>() {
                new SelectListItem {
                    Text="Dalit",
                    Value="Dalit"
                },
                  new SelectListItem {
                    Text="Janajati",
                    Value="Janajati"
                },
                  new SelectListItem {
                    Text="Aanya",
                    Value="Aanya"
                },
            };
            EthnicGroup.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.EthnicGroup = EthnicGroup;
            return View(model);
        }

        public async Task<IActionResult> Edit(string id)
        {
            var animalRegistration = await _anudanService.GetbaliRegisterById(id);
            if (animalRegistration == null)
                return RedirectToAction("List");
            var model = animalRegistration.ToModel();
            var species = new SelectList(await _speciesService.GetSpecies(), "Id", "EnglishName").ToList();
            species.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.SpeciesId = species;
            ViewBag.AllLanguages = await _languageService.GetAllLanguages(true);
            var fiscalYear = new SelectList(await _fiscalYearService.GetFiscalYear(), "Id", "NepaliFiscalYear").ToList();
            fiscalYear.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.FiscalYearId = fiscalYear;
            var createdby = _workContext.CurrentCustomer.Id;
            var incuvationCenter = new SelectList(await _incuvationCenterService.GetincuvationCenter(createdby), "Id", "OrganizationNameEnglish").ToList();
            incuvationCenter.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.IncuvationCenter = incuvationCenter;
            var talim = new SelectList(await _talimService.Gettalim(createdby), "Id", "NameEnglish").ToList();
            talim.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.Talim = talim;
            var sex = new List<SelectListItem>() {
                new SelectListItem {
                    Text="Male",
                    Value="Male"
                },
                  new SelectListItem {
                    Text="Female",
                    Value="Female"
                },
            };
            sex.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.Sex = sex;
            var pujigatKaryakram = new SelectList(await _pujigatKharchaKharakramService.GetPujigatKharchaKharakram(createdby), "Id", "Program").ToList();
            pujigatKaryakram.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            var EthnicGroup = new List<SelectListItem>() {
                new SelectListItem {
                    Text="Dalit",
                    Value="Dalit"
                },
                  new SelectListItem {
                    Text="Janajati",
                    Value="Janajati"
                },
                  new SelectListItem {
                    Text="Aanya",
                    Value="Aanya"
                },
            };
            EthnicGroup.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.EthnicGroup = EthnicGroup;
            ViewBag.pujigatKaryakram = pujigatKaryakram;


            return View(model);
        }

        [PermissionAuthorizeAction(PermissionActionName.Edit)]
        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public async Task<IActionResult> Edit(AanudanModel model, bool continueEditing)
        {
            var animalRegistration = await _anudanService.GetbaliRegisterById(model.Id);
            if (animalRegistration == null)
                //No blog post found with the specified id
                return RedirectToAction("List");

            if (ModelState.IsValid)
            {
                var m = model.ToEntity(animalRegistration);
                animalRegistration.FiscalYear = await _fiscalYearService.GetFiscalYearById(model.FiscalyearId);
                animalRegistration.PujigatKharchaKharakram = await _pujigatKharchaKharakramService.GetPujigatKharchaKharakramById(model.PujigatKharchaKaryakramId);



                //  var farmPicture =await _pictureService.GetPictureById(farm.PictureId);
                //if (farmPicture == null)
                //    throw new ArgumentException("No farm picture found with the specified id");

                
                await _anudanService.UpdatebaliRegister(m);

                SuccessNotification(_localizationService.GetResource("Admin.Update.Successful"));
                if (continueEditing)
                {
                    //selected tab
                    await SaveSelectedTabIndex();

                    return RedirectToAction("Edit", new { id = model.Id });
                }
                return RedirectToAction("TabView");
            }
            var EthnicGroup = new List<SelectListItem>() {
                new SelectListItem {
                    Text="Dalit",
                    Value="Dalit"
                },
                  new SelectListItem {
                    Text="Janajati",
                    Value="Janajati"
                },
                  new SelectListItem {
                    Text="Aanya",
                    Value="Aanya"
                },
            };
            EthnicGroup.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.EthnicGroup = EthnicGroup;
            var sex = new List<SelectListItem>() {
                new SelectListItem {
                    Text="Male",
                    Value="Male"
                },
                  new SelectListItem {
                    Text="Female",
                    Value="Female"
                },
            };
            sex.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.Sex = sex;


            var fiscalYear = new SelectList(await _fiscalYearService.GetFiscalYear(), "Id", "NepaliFiscalYear").ToList();
            fiscalYear.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.FiscalYearId = fiscalYear;
            var species = new SelectList(await _speciesService.GetSpecies(), "Id", "EnglishName").ToList();
            species.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.SpeciesId = species;

            var createdby = _workContext.CurrentCustomer.Id;
            var incuvationCenter = new SelectList(await _incuvationCenterService.GetincuvationCenter(createdby), "Id", "OrganizationNameEnglish").ToList();
            incuvationCenter.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.IncuvationCenter = incuvationCenter;
            var talim = new SelectList(await _talimService.Gettalim(createdby), "Id", "NameEnglish").ToList();
            talim.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.Talim = talim;

            var pujigatKaryakram = new SelectList(await _pujigatKharchaKharakramService.GetPujigatKharchaKharakram(createdby), "Id", "Program").ToList();
            pujigatKaryakram.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
                        ViewBag.pujigatKaryakram = pujigatKaryakram;

            //If we got this far, something failed, redisplay form
            ViewBag.AllLanguages = await _languageService.GetAllLanguages(true);

            return RedirectToAction("Create");
        }


        public async Task<ActionResult> GetBreed(string species)
        {
            var breed = await _breedService.GetBreedBySpeciesId(species);

            return Json(breed.ToList());
        }
        public async Task<ActionResult> GetProgram(string fiscalyear)
        {
            var createdby = _workContext.CurrentCustomer.Id;
            var pugigatkaryakram = await _pujigatKharchaKharakramService.GetPujigatKharchaKharakram(createdby);
            var karyakram = pugigatkaryakram.Where(m => m.FiscalYear.Id == fiscalyear);
            return Json(karyakram.ToList());
        }
        public async Task<ActionResult> GetAnudan(string fiscalyear,string program,string district)
        {
            var createdby = _workContext.CurrentCustomer.Id;
            var pugigatkaryakram = await _anudanService.GetFilteredSubsidy(createdby,fiscalyear,district,program);
            var karyakram = pugigatkaryakram.Where(m => m.FiscalYear.Id == fiscalyear);
            return Json(karyakram.ToList());
        }
        public List<SelectListItem> PujigatType()
        {

            return new List<SelectListItem>() {
                new SelectListItem {
                    Text="chalu",
                    Value="chalu"

                },
                 new SelectListItem {
                    Text="pujigat",
                    Value="pujigat"

                },

            };

        }

        public List<SelectListItem> ProgramType()
        {

            return new List<SelectListItem>() {
                new SelectListItem {
                    Text=_localizationService.GetResource("Lims.PujigatKharcha.SanghKoSasarthaAnudanAntargat"),
                    Value="Lims.PujigatKharcha.SanghKoSasarthaAnudanAntargat",

                },
                 new SelectListItem {
                    Text=_localizationService.GetResource("Lims.PujigatKharcha.PardeshKoBajetAntargat"),
                    Value="Lims.PujigatKharcha.PardeshKoBajetAntargat",


                },

            };

        }

    }
}
