using LIMS.Core;
using LIMS.Domain.BasicSetup;
using LIMS.Domain.Organizations;
using LIMS.Framework.Kendoui;
using LIMS.Framework.Mvc.Filters;
using LIMS.Framework.Security.Authorization;
using LIMS.Services.Bali;
using LIMS.Services.Basic;
using LIMS.Services.Breed;
using LIMS.Services.Localization;
using LIMS.Services.LocalStructure;
using LIMS.Services.OtherOrganizations;
using LIMS.Services.Security;
using LIMS.Web.Areas.Admin.Extensions.Mapping;
using LIMS.Web.Areas.Admin.Helper;
using LIMS.Web.Areas.Admin.Models.Bali;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LIMS.Web.Areas.Admin.Controllers
{
    public class DeathVerificationController : BaseAdminController
    {
        private readonly IDeathVerificationService _DeathVerificationService;
        private readonly IUnitService _unitService;
        private readonly IBreedService _breedService;
        private readonly ILocalizationService _localizationService;
        private readonly ILanguageService _languageService;
        private readonly IWorkContext _workContext;
        private readonly IFiscalYearService _fiscalYearService;
        private readonly ICategoryService _CategoryService;
        private readonly ILocalLevelService _localLevelService;
        private readonly IBudgetService _budgetService;
        private readonly ILivestockBreedService _livestockBreedService;
        private readonly ILivestockSpeciesService _livestockSpeciesService;
        private readonly IOtherOrganizationService _otherOrganizationService;
        private readonly IHostingEnvironment _hostingEnvironment;

        public DeathVerificationController(ILocalizationService localizationService,
            IDeathVerificationService DeathVerificationService,
            ILanguageService languageService,
            IUnitService unitService,
            IBreedService breedService,
            IWorkContext workContext,
            IFiscalYearService fiscalYearService,
            ICategoryService CategoryService,
            ILocalLevelService localLevelService,
            ICategoryService categoryService,
            IBudgetService budgetService,
            ILivestockBreedService livestockBreedService,
            ILivestockSpeciesService livestockSpeciesService,
            IOtherOrganizationService otherOrganizationService,
            IHostingEnvironment hostingEnvironment
            )
        {
            _localizationService = localizationService;
            _DeathVerificationService = DeathVerificationService;
            _languageService = languageService;
            _unitService = unitService;
            _breedService = breedService;
            _workContext = workContext;
            _fiscalYearService = fiscalYearService;
            _CategoryService = CategoryService;
            _localLevelService = localLevelService;
            _CategoryService = categoryService;
            _budgetService = budgetService;
            _livestockBreedService = livestockBreedService;
            _livestockSpeciesService = livestockSpeciesService;
            _otherOrganizationService = otherOrganizationService;
            _hostingEnvironment = hostingEnvironment;
        }

        public IActionResult Index() => RedirectToAction("List");

        public async Task<IActionResult> List() {

            //var localLevels = await _localLevelService.GetLocalLevel("KATHMANDU");
            //var localLevelSelect = new SelectList(localLevels).ToList();
            //localLevelSelect.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            //ViewBag.LocalLevels = new SelectList(localLevelSelect, "Text","Text", ExecutionHelper.LocalLevel);

            var fiscalyear = await _fiscalYearService.GetCurrentFiscalYear();

            var fiscalYear = new SelectList(await _fiscalYearService.GetFiscalYear(), "Id", "NepaliFiscalYear", fiscalyear.Id).ToList();
            fiscalYear.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.FiscalYearId = fiscalYear;


            return View();

        }
        public IActionResult ReportIndex()
        {
            return View();
        }
       

        [PermissionAuthorizeAction(PermissionActionName.List)]
        [HttpPost]
        public async Task<IActionResult> List(DataSourceRequest command, string fiscalYear = "", string district = "", string locallevel = "")
        {
            var id = _workContext.CurrentCustomer.Id;
            var bali = await _DeathVerificationService.GetdeathVerification("", fiscalYear, locallevel, command.Page - 1, command.PageSize);
            var gridModel = new DataSourceResult {
                Data = bali,
                Total = bali.TotalCount
            };
            return Json(gridModel);
        }


        public async Task<IActionResult> Create()
        {
            var fiscalyear = await _fiscalYearService.GetCurrentFiscalYear();

            var fiscalYear = new SelectList(await _fiscalYearService.GetFiscalYear(), "Id", "NepaliFiscalYear", fiscalyear.Id).ToList();
            fiscalYear.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.FiscalYearId = fiscalYear;

            var ward = new WardHelper();
            var wardSelect = ward.GetWard();
            wardSelect.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.Ward = new SelectList(wardSelect, "Value", "Text");

            //var breedSelect = new SelectList(await _livestockBreedService.GetBreed(), "Id", "EnglishName").ToList();
            //breedSelect.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            //ViewBag.Breed = breedSelect;

            var speciesSelect = new SelectList(await _livestockSpeciesService.GetBreed(), "Id", "EnglishName").ToList();
            speciesSelect.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.Species = speciesSelect;


            var firmRegister = ExecutionHelper.GetFirmRegister();
            ViewBag.FirmRegister = new SelectList(firmRegister, "Value", "Text");

            DeathVerificationModel model = new DeathVerificationModel();
            model.LocalLevel = _workContext.CurrentCustomer.LocalLevel;
            model.EnglishDate = DateTime.Now;

            return View(model);
        }


        [PermissionAuthorizeAction(PermissionActionName.Edit)]
        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public async Task<IActionResult> Create(DeathVerificationModel model, bool continueEditing)
        {
            if (ModelState.IsValid)
            {

                var DeathVerification = model.ToEntity();
                DeathVerification.LivestockBreed = await _livestockBreedService.GetBreedById(model.LivestockBreedId);
                DeathVerification.LivestockSpecies = await _livestockSpeciesService.GetBreedById(model.LivestockSpeciesId);
                DeathVerification.FiscalYear = await _fiscalYearService.GetFiscalYearById(model.FiscalYearId);
                if (!String.IsNullOrEmpty(DeathVerification.OrganizationId))
                {
                    DeathVerification.Organization = await _otherOrganizationService.GetOtherOrganizationById(DeathVerification.OrganizationId);
                }

                await CreateCategoryByName(DeathVerification.CauseOfDeath,"Cause Of Death");
                await CreateCategoryByName(DeathVerification.Designation,"Designation");
                await CreateCategoryByName(DeathVerification.InsuranceCompany,"Insurance Company");

                DeathVerification.CreatedBy = _workContext.CurrentCustomer.Id;
                DeathVerification.CreatedAt = DateTime.Now;

                await _DeathVerificationService.InsertdeathVerification(DeathVerification);

                SuccessNotification(_localizationService.GetResource("Admin.Create.successful"));
                return continueEditing ? RedirectToAction("Edit", new { id = DeathVerification.Id }) : RedirectToAction("Index");
            }

            var fiscalyear = await _fiscalYearService.GetCurrentFiscalYear();

            var fiscalYear = new SelectList(await _fiscalYearService.GetFiscalYear(), "Id", "NepaliFiscalYear", fiscalyear.Id).ToList();
            fiscalYear.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.FiscalYearId = fiscalYear;

            var ward = new WardHelper();
            var wardSelect = ward.GetWard();
            wardSelect.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.Ward = new SelectList(wardSelect, "Value", "Text");

            //var breedSelect = new SelectList(await _livestockBreedService.GetBreed(), "Id", "EnglishName").ToList();
            //breedSelect.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            //ViewBag.Breed = breedSelect;

            var speciesSelect = new SelectList(await _livestockSpeciesService.GetBreed(), "Id", "EnglishName").ToList();
            speciesSelect.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.Species = speciesSelect;


            var firmRegister = ExecutionHelper.GetFirmRegister();
            ViewBag.FirmRegister = new SelectList(firmRegister, "Value", "Text");

            return View(model);
        }

        public async Task CreateCategoryByName(string term, string type)
        {
            var category = await _CategoryService.GetCategoryByNameType(term, type);

          
            if(category == null )
            {
                var newCat = new Category {
                    NameEnglish = term.Trim(),
                    NameNepali = term.Trim(),
                    Type = type.Trim()
                };

                await _CategoryService.InsertCategory(newCat);
            }

        }

        public async Task<IActionResult> Edit(string id)
        {
            var DeathVerification = await _DeathVerificationService.GetdeathVerificationById(id);
            if (DeathVerification == null)
                return RedirectToAction("List");
            var model = DeathVerification.ToModel();

            var firmRegister = ExecutionHelper.GetFirmRegister();
            ViewBag.FirmRegister = new SelectList(firmRegister, "Value", "Text");

            //var localLevels = await _localLevelService.GetLocalLevel("KATHMANDU");
            //var localLevelSelect = new SelectList(localLevels).ToList();
            //localLevelSelect.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            //ViewBag.LocalLevels = new SelectList(localLevelSelect, "Text","Text", ExecutionHelper.LocalLevel);

            var fiscalyear = await _fiscalYearService.GetCurrentFiscalYear();

            var fiscalYear = new SelectList(await _fiscalYearService.GetFiscalYear(), "Id", "NepaliFiscalYear", fiscalyear.Id).ToList();
            fiscalYear.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.FiscalYearId = fiscalYear;

            var ward = new WardHelper();
            var wardSelect = ward.GetWard();
            wardSelect.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.Ward = new SelectList(wardSelect, "Value", "Text");

            var breedSelect = new SelectList(await _livestockBreedService.GetBreed(), "Id", "EnglishName").ToList();
            breedSelect.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.Breed = breedSelect;

            var speciesSelect = new SelectList(await _livestockSpeciesService.GetBreed(), "Id", "EnglishName").ToList();
            speciesSelect.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.Species = speciesSelect;

            return View(model);
        }

        [PermissionAuthorizeAction(PermissionActionName.Edit)]
        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public async Task<IActionResult> Edit(DeathVerificationModel model, bool continueEditing)
        {
            var DeathVerification = await _DeathVerificationService.GetdeathVerificationById(model.Id);
            if (DeathVerification == null)
                //No blog post found with the specified id
                return RedirectToAction("List");

            if (ModelState.IsValid)
            {
                var m = model.ToEntity();
                m.LivestockBreed = await _livestockBreedService.GetBreedById(model.LivestockBreedId);
                m.LivestockSpecies = await _livestockSpeciesService.GetBreedById(model.LivestockSpeciesId);
                m.FiscalYear = await _fiscalYearService.GetFiscalYearById(model.FiscalYearId);
                if (!String.IsNullOrEmpty(m.OrganizationId))
                {
                    m.Organization = await _otherOrganizationService.GetOtherOrganizationById(DeathVerification.OrganizationId);
                }

                await CreateCategoryByName(DeathVerification.CauseOfDeath, "Cause Of Death");
                await CreateCategoryByName(DeathVerification.Designation, "Designation");
                await CreateCategoryByName(DeathVerification.InsuranceCompany, "Insurance Company");

                m.CreatedBy = _workContext.CurrentCustomer.Id;
                m.CreatedAt = DateTime.Now;

                await _DeathVerificationService.UpdatedeathVerification(m);

                SuccessNotification(_localizationService.GetResource("Admin.Update.Successful"));
                if (continueEditing)
                {
                    //selected tab
                    await SaveSelectedTabIndex();

                    return RedirectToAction("Edit", new { id = model.Id });
                }
                return RedirectToAction("Index");
            }

            var fiscalyear = await _fiscalYearService.GetCurrentFiscalYear();

            var fiscalYear = new SelectList(await _fiscalYearService.GetFiscalYear(), "Id", "NepaliFiscalYear", fiscalyear.Id).ToList();
            fiscalYear.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.FiscalYearId = fiscalYear;

            var ward = new WardHelper();
            var wardSelect = ward.GetWard();
            wardSelect.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.Ward = new SelectList(wardSelect, "Value", "Text");

            var breedSelect = new SelectList(await _livestockBreedService.GetBreed(), "Id", "EnglishName").ToList();
            breedSelect.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.Breed = breedSelect;

            var speciesSelect = new SelectList(await _livestockSpeciesService.GetBreed(), "Id", "EnglishName").ToList();
            speciesSelect.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.Species = speciesSelect;

            //If we got this far, something failed, redisplay form
            ViewBag.AllLanguages = await _languageService.GetAllLanguages(true);

            return RedirectToAction("Create");
        }

        public async Task<IActionResult> Report(string id)
        {
            var livestockWardWiseReportHtml = RenderViewComponentToString("DeathVerificationReport", new { id = id });

            ViewBag.RawHtml = livestockWardWiseReportHtml;
            return View();
        }
        public async Task<IActionResult> Upload(string id)
        {
            string uploads = Path.Combine(_hostingEnvironment.WebRootPath, "uploads/Insurance/" + id+ ".jpg");
            string uploadsPdf = Path.Combine(_hostingEnvironment.WebRootPath, "uploads/Insurance/" + id+ ".pdf");
            FileInfo fileInfo = new FileInfo(uploads);
            FileInfo fileInfoPdf = new FileInfo(uploadsPdf);
            
            string[] allowedExtensions = { ".jpg", ".jpeg", ".png", ".gif" }; // Example allowed extensions
            string filepath = "";
            string fileExtension = "";
            double filesize = 0;
           
            if (System.IO.File.Exists(uploads) )
            {
                filepath = "/uploads/insurance/" + id + ".jpg";
                fileExtension = System.IO.Path.GetExtension(uploads);
                filesize = (double)fileInfo.Length / (1024 * 1024);
            }
            if (System.IO.File.Exists(uploadsPdf))
            {
                filepath = "/uploads/insurance/" + id + ".pdf";
                fileExtension = System.IO.Path.GetExtension(uploadsPdf);
                filesize = (double)fileInfoPdf.Length / (1024 * 1024);
            }
            // Check magic numbers for various image formats


            var model = new DeathVerificationUploadModel {
                Id=id,
                FilePath =filepath,
                FileExtension = fileExtension,
                FileSize = filesize                
            };

            if (String.IsNullOrEmpty(id))
            {
                return RedirectToAction("Index");
            }
            else
            {
                return View(model);
            }
        }
        [HttpPost]
        public async Task<IActionResult> Upload(DeathVerificationUploadModel model)
        {
            if (!String.IsNullOrEmpty(model.Id))
            {
                string path = Path.Combine(_hostingEnvironment.WebRootPath, "uploads/Insurance/");

                if (!System.IO.Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                try
                {
                    string fileExtension = System.IO.Path.GetExtension(model.Image.FileName);
                    string[] allowedExtensions = { ".jpg", ".jpeg", ".png", ".gif" }; // Example allowed extensions


                    if (allowedExtensions.Contains(fileExtension.ToLower()))
                    {
                        fileExtension = ".jpg";
                    }
                    else
                    { fileExtension = ".pdf"; }
                    if (model.Image != null && model.Image.Length > 0)
                    {                       
                        
                        string uploads = Path.Combine(_hostingEnvironment.WebRootPath, "uploads/Insurance/"+model.Id+ fileExtension);
                        //Delete previously uploaded file if exists
                        if (fileExtension == ".jpg")
                        {
                            string uploadsPDF = Path.Combine(_hostingEnvironment.WebRootPath, "uploads/Insurance/" + model.Id + ".pdf");
                            if (System.IO.File.Exists(uploadsPDF))
                            {
                                System.IO.File.Delete(uploadsPDF);
                            }
                        
                        }
                        else {
                            string uploadsImage = Path.Combine(_hostingEnvironment.WebRootPath, "uploads/Insurance/" + model.Id + ".jpg");
                            if (System.IO.File.Exists(uploadsImage))
                            {
                                System.IO.File.Delete(uploadsImage);
                            }
                        }
                        using (Stream fileStream = new FileStream(uploads, FileMode.Create))
                        {
                            await model.Image.CopyToAsync(fileStream);
                        }                        
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.ToString());
                    return View(model);
                }
               
                return RedirectToAction("Index");
            }
            else
            {
                return View(model);
            }

        }

        public async Task<ActionResult> GetBreedBySpecies(string species = "")
        {
            var breed = await _breedService.GetBreedBySpeciesId(species);

            return Json(breed);
        }

        //public static bool IsPdf(byte[] buffer)
        //{
            
        //    // Check magic number for PDF
        //    return Encoding.ASCII.GetString(buffer).StartsWith("%PDF");
        //}

        //// Methods to check file signatures

        //private static bool IsJpeg(byte[] buffer)
        //{
        //    return buffer[0] == 0xFF && buffer[1] == 0xD8 && buffer[2] == 0xFF;
        //}

        //private static bool IsPng(byte[] buffer)
        //{
        //    return buffer[0] == 0x89 && buffer[1] == 0x50 && buffer[2] == 0x4E && buffer[3] == 0x47;
        //}

        //private static bool IsGif(byte[] buffer)
        //{
        //    return buffer[0] == 0x47 && buffer[1] == 0x49 && buffer[2] == 0x46;
        //}
        //public virtual async Task<IActionResult> CategoryAutoComplete(string term, string type)
        //{
        //    var result = await _CategoryService.GetCategoryByType(type, term);
        //    return Json(result);
        //}

    }
    }
