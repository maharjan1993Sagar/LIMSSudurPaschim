using LIMS.Core;
using LIMS.Domain.Bali;
using LIMS.Framework.Kendoui;
using LIMS.Framework.Mvc.Filters;
using LIMS.Framework.Security.Authorization;
using LIMS.Services.Bali;
using LIMS.Services.Basic;
using LIMS.Services.Breed;
using LIMS.Services.Localization;
using LIMS.Services.Security;
using LIMS.Web.Areas.Admin.Helper;
using LIMS.Web.Areas.Admin.Models.Bali;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Web.Areas.Admin.Controllers
{
    public class MonthlyProgressController : BaseAdminController
    {
        private readonly IMonthlyPragatiService _animalRegistrationService;
        private readonly IPujigatKharchaKharakramService _pujigatKharchaKharakramService;
        private readonly ISpeciesService _speciesService;
        private readonly IBreedService _breedService;
        private readonly ILocalizationService _localizationService;
        private readonly ILanguageService _languageService;
        private readonly IWorkContext _workContext;
        private readonly IFiscalYearService _fiscalYearService;

        public MonthlyProgressController(ILocalizationService localizationService,
            IMonthlyPragatiService animalRegistrationService,
            ILanguageService languageService,
            ISpeciesService speciesService,
            IBreedService breedService,
            IWorkContext workContext,
            IFiscalYearService fiscalYearService,
            IPujigatKharchaKharakramService pujigatKharchaKharakramService
            )
        {
            _localizationService = localizationService;
            _animalRegistrationService = animalRegistrationService;
            _languageService = languageService;
            _speciesService = speciesService;
            _breedService = breedService;
            _workContext = workContext;
            _fiscalYearService = fiscalYearService;
            _pujigatKharchaKharakramService = pujigatKharchaKharakramService;
        }

        public IActionResult Index() => RedirectToAction("List");

        public IActionResult List() => View();

        [PermissionAuthorizeAction(PermissionActionName.List)]
        [HttpPost]
        public async Task<IActionResult> List(DataSourceRequest command)
        {
            var id = _workContext.CurrentCustomer.Id;
            var MonthlyPragati = await _animalRegistrationService.GetMonthlyPragati(id, command.Page - 1, command.PageSize);
            var gridModel = new DataSourceResult {
                Data = MonthlyPragati,
                Total = MonthlyPragati.TotalCount
            };
            return Json(gridModel);
        }


        public async Task<IActionResult> Create()
        {
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
            MonthlyProgressModel model = new MonthlyProgressModel();

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(MonthlyProgressModel model,IFormCollection form)
        {
            var bhautikpragati = form["BhautikPragati"].ToList();
            var bitiyaPragati = form["BitiyaPragati"].ToList();
            var pujigatKharchaId = form["PujigatKharchaId"].ToList();
            var progressDataId = form["ProgressDataId"].ToList();
            var updateLivestocks = new List<MonthlyPragati>();
            var addLivestocks = new List<MonthlyPragati>();
            string createdby = null;
           
                createdby = _workContext.CurrentCustomer.Id;
           
            for (int i = 0; i < pujigatKharchaId.Count(); i++)
            {
                if (string.IsNullOrEmpty(bitiyaPragati[i])|| string.IsNullOrEmpty(bhautikpragati[i]))
                    continue;

                var livestock = new MonthlyPragati {
                    pujigatKharchaKharakram = await _pujigatKharchaKharakramService.GetPujigatKharchaKharakramById(pujigatKharchaId[i]),
                    BitiyaPragati = bitiyaPragati[i],
                    VautikPragati = bhautikpragati[i],
                    PujigatKharchaId=pujigatKharchaId[i],
                    FiscalYearId = model.FiscalYearId,
                    FiscalYear=await _fiscalYearService.GetFiscalYearById(model.FiscalYearId),
                  
                    CreatedBy = createdby,
                    Month = model.Month

                };
                if (!string.IsNullOrEmpty(progressDataId[i]))
                {
                    livestock.Id = progressDataId[i];
                    updateLivestocks.Add(livestock);
                }
                else
                {
                    addLivestocks.Add(livestock);
                }

            }
            if (updateLivestocks.Count > 0)
                await _animalRegistrationService.UpdateMonthlyPragatiList(updateLivestocks);
            if (addLivestocks.Count > 0)
                await _animalRegistrationService.InsertMonthlyPragatiList(addLivestocks);

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


            return View(model);
        }



        public async Task<ActionResult> GetPujigatKharcha(string type, string programType, string fiscalYear)
        {
            var createdby = _workContext.CurrentCustomer.Id;
            var pujigatKaryakram = await _pujigatKharchaKharakramService.GetPujigatKharchaKharakram(createdby, programType, type, fiscalYear);
             return Json(pujigatKaryakram);
        }
        public async Task<ActionResult> GetProgress(string type, string programType, string fiscalYear,string month)
        {
            var createdby = _workContext.CurrentCustomer.Id;
            var pujigatKaryakram = await _animalRegistrationService.GetFilteredMonthlyPragati(createdby, fiscalYear, programType, type,month);
            return Json(pujigatKaryakram);
        }

        public async Task<ActionResult> GetBreed(string species)
        {
            var breed = await _breedService.GetBreedBySpeciesId(species);

            return Json(breed.ToList());
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
