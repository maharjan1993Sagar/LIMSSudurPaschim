using LIMS.Core;
using LIMS.Framework.Kendoui;
using LIMS.Framework.Mvc.Filters;
using LIMS.Framework.Security.Authorization;
using LIMS.Services.Bali;
using LIMS.Services.Basic;
using LIMS.Services.Breed;
using LIMS.Services.Localization;
using LIMS.Services.Media;
using LIMS.Services.Security;
using LIMS.Web.Areas.Admin.Extensions.Mapping;
using LIMS.Web.Areas.Admin.Helper;
using LIMS.Web.Areas.Admin.Models.Bali;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Web.Areas.Admin.Controllers
{
    public class LabambitKrishakController:BaseAdminController
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

        public LabambitKrishakController(ILocalizationService localizationService,
            ILabambitKrishakService animalRegistrationService,
            ILanguageService languageService,
            ISpeciesService speciesService,
            IBreedService breedService,
            IWorkContext workContext,
            IFiscalYearService fiscalYearService,
            ITalimService talimService,
            IIncuvationCenterService incuvationCenterService,
            IPujigatKharchaKharakramService pujigatKharchaKharakramService,
            IPictureService pictureService

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
        }

        public IActionResult Index() => RedirectToAction("List");

        public IActionResult List() => View();

        [PermissionAuthorizeAction(PermissionActionName.List)]
        [HttpPost]
        public async Task<IActionResult> List(DataSourceRequest command)
        {
            var id = _workContext.CurrentCustomer.Id;
            var bali = await _animalRegistrationService.GetLabambitKrishakHaru(id, command.Page - 1, command.PageSize);
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
            var fiscalYear = new SelectList(await _fiscalYearService.GetFiscalYear(), "Id", "NepaliFiscalYear").ToList();
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
            if (type != null )
            {
                var id = _workContext.CurrentCustomer.Id;
                var krishak = await _animalRegistrationService.GetFilteredLabambitKrishak(id, fiscalYear, programType, type);
                List<LabambitReport> report = new List<LabambitReport>();
                foreach (var item in krishak)
                {
                    var labambit = new LabambitReport();
                    labambit.pujigatKharchaKharakram = item.PujigatKharchaKharakram;
                    labambit.WorkDone = item.WorkDone;
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


        public async Task<IActionResult> Create()
        {
            var createdby = _workContext.CurrentCustomer.Id;
           
            var fiscalYear = new SelectList(await _fiscalYearService.GetFiscalYear(), "Id", "NepaliFiscalYear").ToList();
            fiscalYear.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.FiscalYearId = fiscalYear;

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

            var pujigatKaryakram = new SelectList(await _pujigatKharchaKharakramService.GetPujigatKharchaKharakram(createdby), "Id", "Program").ToList();
            pujigatKaryakram.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.pujigatKaryakram = pujigatKaryakram;

            LabambitKrishakModel model = new LabambitKrishakModel();

            return View(model);
        }

        [PermissionAuthorizeAction(PermissionActionName.Edit)]
        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public async Task<IActionResult> Create(LabambitKrishakModel model, bool continueEditing)
        {
            if (ModelState.IsValid)
            {
                var animalRegistration = model.ToEntity();
                animalRegistration.CreatedBy = _workContext.CurrentCustomer.Id;
                if(!string.IsNullOrEmpty(model.PictureId))
                {
                    var picture = await _pictureService.GetPictureById(model.PictureId);
                    if(picture!=null)
                    {
                        animalRegistration.Picture = picture;
                        animalRegistration.PictureId = model.PictureId;
                    }
                }
                animalRegistration.FiscalYear = await _fiscalYearService.GetFiscalYearById(model.FiscalyearId);
                animalRegistration.PujigatKharchaKharakram= await _pujigatKharchaKharakramService.GetPujigatKharchaKharakramById(model.PujigatKharchaKaryakramId);
                animalRegistration.CreatedBy = _workContext.CurrentCustomer.Id;
                await _animalRegistrationService.InsertLabambitKrishakHaru(animalRegistration);

                SuccessNotification(_localizationService.GetResource("Admin.Create.successful"));
                return continueEditing ? RedirectToAction("Edit", new { id = animalRegistration.Id }) : RedirectToAction("Index");
            }
         
            var createdby = _workContext.CurrentCustomer.Id;
            var pujigatKaryakram = new SelectList(await _pujigatKharchaKharakramService.GetPujigatKharchaKharakram(createdby), "Id", "Program").ToList();
            pujigatKaryakram.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.pujigatKaryakram = pujigatKaryakram;

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
            var incuvationCenter = new SelectList(await _incuvationCenterService.GetincuvationCenter(createdby), "Id", "OrganizationNameEnglish").ToList();
            incuvationCenter.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.IncuvationCenter = incuvationCenter;
            var talim = new SelectList(await _talimService.Gettalim(createdby), "Id", "NameEnglish").ToList();
            talim.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.Talim = talim;

            var species = new SelectList(await _speciesService.GetSpecies(), "Id", "EnglishName").ToList();
            species.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.SpeciesId = species;
            ViewBag.AllLanguages = await _languageService.GetAllLanguages(true);
            var fiscalYear = new SelectList(await _fiscalYearService.GetFiscalYear(), "Id", "NepaliFiscalYear").ToList();
            fiscalYear.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.FiscalYearId = fiscalYear;
            return View(model);
        }

        public async Task<IActionResult> Edit(string id)
        {
            var animalRegistration = await _animalRegistrationService.GetLabambitKrishakHaruById(id);
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
            var pujigatKaryakram = new SelectList(await _pujigatKharchaKharakramService.GetPujigatKharchaKharakram(createdby), "Id", "Program").ToList();
            pujigatKaryakram.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.pujigatKaryakram = pujigatKaryakram;

            return View(model);
        }

        [PermissionAuthorizeAction(PermissionActionName.Edit)]
        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public async Task<IActionResult> Edit(LabambitKrishakModel model, bool continueEditing)
        {
            var animalRegistration = await _animalRegistrationService.GetLabambitKrishakHaruById(model.Id);
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

                if (!string.IsNullOrEmpty(model.PictureId))
                {
                    var picture = await _pictureService.GetPictureById(model.PictureId);
                    if (picture != null)
                    {
                        m.Picture = picture;
                        m.PictureId = model.PictureId;
                    }
                }
                await _animalRegistrationService.UpdateLabambitKrishakHaru(m);

                SuccessNotification(_localizationService.GetResource("Admin.Update.Successful"));
                if (continueEditing)
                {
                    //selected tab
                    await SaveSelectedTabIndex();

                    return RedirectToAction("Edit", new { id = model.Id });
                }
                return RedirectToAction("Index");
            }
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
            var karyakram = pugigatkaryakram.Where(m => m.FiscalYear.Id == fiscalyear && m.Expenses_category == "Training");
            return Json(karyakram.ToList());
        }
        public async Task<ActionResult> GetSubsidyProgram(string fiscalyear)
        {
            var createdby = _workContext.CurrentCustomer.Id;
            var pugigatkaryakram = await _pujigatKharchaKharakramService.GetPujigatKharchaKharakram(createdby);
            var karyakram = pugigatkaryakram.Where(m => m.FiscalYear.Id == fiscalyear && m.Expenses_category == "Subsidy");
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
