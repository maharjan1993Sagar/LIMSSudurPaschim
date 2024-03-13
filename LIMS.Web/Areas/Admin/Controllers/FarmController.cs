using LIMS.Core;
using LIMS.Domain.AInR;
using LIMS.Framework.Kendoui;
using LIMS.Framework.Mvc;
using LIMS.Framework.Mvc.Filters;
using LIMS.Framework.Security.Authorization;
using LIMS.Services.Ainr;
using LIMS.Services.Customers;
using LIMS.Services.Localization;
using LIMS.Services.LocalStructure;
using LIMS.Services.Media;
using LIMS.Services.MoAMAC;
using LIMS.Services.Security;
using LIMS.Web.Areas.Admin.Extensions.Mapping;
using LIMS.Web.Areas.Admin.Helper;
using LIMS.Web.Areas.Admin.Models.AInR;
using LIMS.Web.Areas.Admin.Models.Livestock;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using static LIMS.Web.Areas.Admin.Models.AInR.FarmModel;

namespace LIMS.Web.Areas.Admin.Controllers
{
    public class FarmController : BaseAdminController
    {
        private readonly IFarmService _farmService;
        private readonly IPictureService _pictureService;
        private readonly ILocalizationService _localizationService;
        private readonly ILanguageService _languageService;
        private readonly IWorkContext _workContext;
        private readonly IVhlsecService _vhlsecService;
        private readonly ILssService _lssService;
        private readonly ICustomerService _customerService;
        private readonly IMediator _mediator;
        private readonly IStoreContext _storeContext;
        private readonly ILocalLevelService _localLevelService;


        public FarmController(ILocalizationService localizationService, IFarmService farmService, IPictureService pictureService,
            ILanguageService languageService, IWorkContext workContext, IVhlsecService vhlsecService, 
            ILssService lssService, ICustomerService customerService, IMediator mediator,
            IStoreContext storeContext, ILocalLevelService localLevelService)
        {
            _localizationService = localizationService;
            _farmService = farmService;
            _pictureService = pictureService;
            _languageService = languageService;
            _workContext = workContext;
            _vhlsecService = vhlsecService;
            _lssService = lssService;
            _customerService = customerService;
            _mediator = mediator;
            _storeContext = storeContext;
            _localLevelService = localLevelService;
        }
        public IActionResult Index() => RedirectToAction("List");

        public IActionResult List() => View();

        [PermissionAuthorizeAction(PermissionActionName.List)]
        [HttpPost]
        public async Task<IActionResult> List(DataSourceRequest command, FarmListModel model)
        {
            List<string> roles = _workContext.CurrentCustomer.CustomerRoles.Select(x => x.Name).ToList();

            //if (roles.Contains("VhlsecUser") || roles.Contains("VhlsecAdmin"))
            //{
                string vhlsecid = _workContext.CurrentCustomer.EntityId;
                List<string> lssId = _lssService.GetLssByVhlsecId(vhlsecid).Result.Select(m => m.Id).ToList();
                var customers = _customerService.GetCustomerByLssId(lssId, vhlsecid);
                List<string> customerid = customers.Select(x => x.Id).ToList();
                var farm = await _farmService.GetFarmByLssId(null, model.Keyword, command.Page-1, command.PageSize);
                var currentuser = _workContext.CurrentCustomer.Id;
                var gridModel = new DataSourceResult {
                    Data = farm,
                    Total = farm.TotalCount
                };
                return Json(gridModel);
            //}
            //if (roles.Contains(RoleHelper.NlboAdmin) || roles.Contains(RoleHelper.NlboUser))
            //{
            //    string nlboId = _workContext.CurrentCustomer.EntityId;
            //    var customers = _customerService.GetCustomerByLssId(null, nlboId);
            //    var pprsCustomer = _customerService.GetNlboUsers();
            //    List<string> pprsCustomerId = pprsCustomer.Select(x => x.Id).ToList();
            //    List<string> customerid = customers.Select(x => x.Id).ToList();
            //   customerid.AddRange(pprsCustomerId);
            //    var farm = await _farmService.GetFarmByLssId(customerid, model.Keyword, command.Page - 1, command.PageSize);
            //    var currentuser = _workContext.CurrentCustomer.Id;
            //    var gridModel = new DataSourceResult {
            //        Data = farm,
            //        Total = farm.TotalCount
            //    };
            //    return Json(gridModel);
            //}
            //else
            //{
              
            //    var currentuser = _workContext.CurrentCustomer.EntityId;
            //   var  allCustomer = await _customerService.GetAllCustomers();
            //    List<string> customerid = allCustomer.Where(m=>m.EntityId==currentuser).Select(x => x.Id).ToList();
            //    var farm = await _farmService.GetFarmByLssId(customerid, model.Keyword, command.Page-1, command.PageSize);
            //    var gridModel = new DataSourceResult {
            //        Data = farm,
            //        Total = farm.TotalCount
            //    };
            //    return Json(gridModel);
            //}
        }


        public async Task<IActionResult> Create()
        {
            List<SelectListItem> Category = GetCategory();
            Category.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));

            List<SelectListItem> Education = GetEducation();
            Education.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));

            List<SelectListItem> FarmType = GetFarmType();
            FarmType.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            List<SelectListItem> EthinicGroup = GetEthinicGroup();

            var Provience = GetProvinceList();
            Provience.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            EthinicGroup.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            var natureOfWork = NatureOfWork.GetNatureOfWork();
            natureOfWork.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.NatureOfWork = natureOfWork;
            ViewBag.Provience = Provience;
            ViewBag.Education = Education;
            ViewBag.FarmType = FarmType;
            ViewBag.Category = Category;
            ViewBag.EthinicGroup = EthinicGroup;

            var localLevels = await _localLevelService.GetLocalLevel("KATHMANDU");
            var localLevelSelect = new SelectList(localLevels).ToList();
            localLevelSelect.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.LocalLevels = new SelectList(localLevelSelect, "Text", "Text", ExecutionHelper.LocalLevel);

            var farmModel = new FarmModel();
            return View(farmModel);
        }
        [PermissionAuthorizeAction(PermissionActionName.Edit)]
        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public async Task<IActionResult> Create(FarmModel model, bool continueEditing)
        {
            if (ModelState.IsValid)
            {
                var farm = model.ToEntity();
                farm.CreatedAt = DateTime.Now.ToString();
                farm.CreatedBy = _workContext.CurrentCustomer.Id;
                farm.Source = _workContext.CurrentCustomer.OrgName;
                farm.MobileNo = model.MoblileNo;
                await _farmService.InsertFarm(farm);
                SuccessNotification(_localizationService.GetResource("Admin.farm.Added"));
                return continueEditing ? RedirectToAction("Edit", new { id = farm.Id }) : RedirectToAction("List");
            }
            var natureOfWork = NatureOfWork.GetNatureOfWork();
            natureOfWork.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.NatureOfWork = natureOfWork;

            ViewBag.AllLanguages = await _languageService.GetAllLanguages(true);
            List<SelectListItem> Category = GetCategory();
            Category.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));

            List<SelectListItem> Education = GetEducation();
            Education.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));

            List<SelectListItem> FarmType = GetFarmType();
            FarmType.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            List<SelectListItem> EthinicGroup = GetEthinicGroup();

            var Provience = GetProvinceList();
            Provience.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            EthinicGroup.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.Provience = Provience;

            var localLevels = await _localLevelService.GetLocalLevel("KATHMANDU");
            var localLevelSelect = new SelectList(localLevels).ToList();
            localLevelSelect.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.LocalLevels = new SelectList(localLevelSelect, "Text","Text", ExecutionHelper.LocalLevel);

            ViewBag.Education = Education;
            ViewBag.FarmType = FarmType;
            ViewBag.Category = Category;
            ViewBag.EthinicGroup = EthinicGroup;

            //var localLevels = await _localLevelService.GetLocalLevel("KATHMANDU");
            //var localLevelSelect = new SelectList(localLevels).ToList();
            //localLevelSelect.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            //ViewBag.LocalLevels = new SelectList(localLevelSelect, "Text", "Text", ExecutionHelper.LocalLevel);


            return View(model);
        }

        public async Task<IActionResult> Edit(string id)
        {
            var farm = await _farmService.GetFarmById(id);
            if (farm == null)
                return RedirectToAction("List");
            var model = farm.ToModel();
            List<SelectListItem> Category = GetCategory();
            Category.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));

            List<SelectListItem> Education = GetEducation();
            Education.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));

            List<SelectListItem> FarmType = GetFarmType();
            FarmType.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            List<SelectListItem> EthinicGroup = GetEthinicGroup();

            var Provience = GetProvinceList();
            Provience.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            EthinicGroup.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.Provience = Provience;
            var natureOfWork = NatureOfWork.GetNatureOfWork();
            natureOfWork.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.NatureOfWork = natureOfWork;
            var grasstype = FodderHelper.GetGrassType();
            grasstype.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.GrassType = grasstype;
            var season = SeasonHelper.GetSeason();
            season.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.Season = season;
            var shedType = SHedTypeHelper.GetShedType();
            shedType.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));

            var localLevels = await _localLevelService.GetLocalLevel("KATHMANDU");
            var localLevelSelect = new SelectList(localLevels).ToList();
            localLevelSelect.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.LocalLevels = new SelectList(localLevelSelect, "Text","Text", ExecutionHelper.LocalLevel);

            ViewBag.ShedType = shedType;
            ViewBag.Education = Education;
            ViewBag.FarmType = FarmType;
            ViewBag.Category = Category;
            ViewBag.EthinicGroup = EthinicGroup;
            ViewBag.AllLanguages = await _languageService.GetAllLanguages(true);

            return View(model);
        }

        [PermissionAuthorizeAction(PermissionActionName.Edit)]
        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public async Task<IActionResult> Edit(FarmModel model, bool continueEditing)
        {
            var farm = await _farmService.GetFarmById(model.Id);
            if (farm == null)
                //No blog post found with the specified id
                return RedirectToAction("List");

            if (ModelState.IsValid)
            {
                var m = model.ToEntity(farm);
                m.MobileNo = model.MoblileNo;
                await _farmService.UpdateFarm(m);

                SuccessNotification(_localizationService.GetResource("Admin.farm.Updated"));
                if (continueEditing)
                {
                    //selected tab
                    await SaveSelectedTabIndex();

                    return RedirectToAction("Edit", new { id = model.Id });
                }
                return RedirectToAction("List");
            }
            List<SelectListItem> Category = GetCategory();
            Category.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));

            List<SelectListItem> Education = GetEducation();
            Education.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));

            List<SelectListItem> FarmType = GetFarmType();
            FarmType.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            List<SelectListItem> EthinicGroup = GetEthinicGroup();

            var Provience = GetProvinceList();
            Provience.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            EthinicGroup.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.Provience = Provience;
            ViewBag.Education = Education;
            ViewBag.FarmType = FarmType;
            ViewBag.Category = Category;
            ViewBag.EthinicGroup = EthinicGroup;
            var natureOfWork = NatureOfWork.GetNatureOfWork();
            natureOfWork.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.NatureOfWork = natureOfWork;
            var grasstype = FodderHelper.GetGrassType();
            grasstype.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.GrassType = grasstype;
            var season = SeasonHelper.GetSeason();
            season.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.Season = season;
            var shedType = SHedTypeHelper.GetShedType();
            shedType.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.ShedType = shedType;

            var localLevels = await _localLevelService.GetLocalLevel("KATHMANDU");
            var localLevelSelect = new SelectList(localLevels).ToList();
            localLevelSelect.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.LocalLevels = new SelectList(localLevelSelect, "Text","Text", ExecutionHelper.LocalLevel);

            //If we got this far, something failed, redisplay form
            ViewBag.AllLanguages = await _languageService.GetAllLanguages(true);

            return View(model);
        }

        private List<SelectListItem> GetProvinceList()
        {
            return new List<SelectListItem> {

                new SelectListItem { Text = _localizationService.GetResource("Common.Province.Four"), Value = "Province 4", Selected = true },

            };
        }
        private List<SelectListItem> GetCategory()
        {
            return new List<SelectListItem>{
                new SelectListItem{Text="Farmer",Value="farmer"},
                new SelectListItem{Text="Farm", Value="farm" },
                new SelectListItem{Text="Groups", Value="groups" },
                new SelectListItem{Text="Co-operative", Value="co-operative" },
            };
        }
        private List<SelectListItem> GetEducation()
        {
            return new List<SelectListItem>() {
                new SelectListItem{Text="Bachelor",Value="Bachelor"},
                new SelectListItem{Text="+2", Value="+2" },
                 new SelectListItem{Text="Secondary level", Value="Secondary level" },
                  new SelectListItem{Text="Below class 8", Value="Below class 8" }
            };

        }
        private List<SelectListItem> GetFarmType()
        {
            return new List<SelectListItem>() {
                new SelectListItem{Text="Public", Value="Public" },
                 new SelectListItem{Text="Private", Value="Private" },
                new SelectListItem{Text="Semi-Public",Value="SemiPublic"},
            };
        }

        private List<SelectListItem> GetEthinicGroup()
        {
            return new List<SelectListItem>() {
            new SelectListItem { Text = "Dalit", Value = "Dalit" },
                 new SelectListItem { Text = "JanaJati", Value = "JanaJati" },
                new SelectListItem { Text = "Aanya", Value = "Aanya" },
           };
        }

        #region Farm pictures
        public async Task<IActionResult> FarmPictureAdd(string pictureId, int displayOrder,
            string overrideAltAttribute, string overrideTitleAttribute,
            string farmId)
        {
            if (string.IsNullOrEmpty(pictureId))
                throw new ArgumentException();

            var farm = await _farmService.GetFarmById(farmId);

            var picture = await _pictureService.GetPictureById(pictureId);
            if (picture == null)
                throw new ArgumentException("No picture found with the specified id");

            await _pictureService.UpdatePicture(picture.Id,
                await _pictureService.LoadPictureBinary(picture),
                picture.MimeType,
                picture.SeoFilename,
                overrideAltAttribute,
                overrideTitleAttribute);

            await _farmService.InsertFarmPicture(new FarmPicture {
                PictureId = pictureId,
                FarmId = farm.Id,
                DisplayOrder = displayOrder,
                AltAttribute = overrideAltAttribute,
                MimeType = picture.MimeType,
                SeoFilename = picture.SeoFilename,
                TitleAttribute = overrideTitleAttribute
            });

            await _pictureService.SetSeoFilename(pictureId, _pictureService.GetPictureSeName(farm.NameEnglish));

            return Json(new { Result = true });
        }

        [PermissionAuthorizeAction(PermissionActionName.Preview)]
        [HttpPost]
        public async Task<IActionResult> FarmPictureList(DataSourceRequest command, string farmId)
        {
            var farm = await _farmService.GetFarmById(farmId);

            var farmPicturesModel = new List<FarmModel.FarmPictureModel>();
            foreach (var x in farm.FarmPictures.OrderBy(x => x.DisplayOrder))
            {
                var picture = await _pictureService.GetPictureById(x.PictureId);
                var m = new FarmModel.FarmPictureModel {
                    Id = x.Id,
                    FarmId = farm.Id,
                    PictureId = x.PictureId,
                    PictureUrl = picture != null ? await _pictureService.GetPictureUrl(picture) : null,
                    OverrideAltAttribute = picture?.AltAttribute,
                    OverrideTitleAttribute = picture?.TitleAttribute,
                    DisplayOrder = x.DisplayOrder
                };
                farmPicturesModel.Add(m);
            }

            var gridModel = new DataSourceResult {
                Data = farmPicturesModel,
                Total = farmPicturesModel.Count
            };

            return Json(gridModel);
        }

        [PermissionAuthorizeAction(PermissionActionName.Edit)]
        [HttpPost]
        public async Task<IActionResult> FarmPictureUpdate(FarmModel.FarmPictureModel model)
        {
            if (ModelState.IsValid)
            {
                var farm = await _farmService.GetFarmById(model.FarmId);

                var farmPicture = farm.FarmPictures.Where(x => x.Id == model.Id).FirstOrDefault();
                if (farmPicture == null)
                    throw new ArgumentException("No farm picture found with the specified id");
                farmPicture.FarmId = farm.Id;

                var picture = await _pictureService.GetPictureById(farmPicture.PictureId);
                if (picture == null)
                    throw new ArgumentException("No picture found with the specified id");

                farmPicture.DisplayOrder = model.DisplayOrder;
                farmPicture.MimeType = picture.MimeType;
                farmPicture.SeoFilename = picture.SeoFilename;
                farmPicture.AltAttribute = model.OverrideAltAttribute;
                farmPicture.TitleAttribute = model.OverrideTitleAttribute;

                await _farmService.UpdateFarmPicture(farmPicture);

                await _pictureService.UpdatePicture(picture.Id,
                    await _pictureService.LoadPictureBinary(picture),
                    picture.MimeType,
                    picture.SeoFilename,
                    model.OverrideAltAttribute,
                    model.OverrideTitleAttribute);

                return new NullJsonResult();
            }
            return ErrorForKendoGridJson(ModelState);
        }

        [PermissionAuthorizeAction(PermissionActionName.Edit)]
        [HttpPost]
        public async Task<IActionResult> FarmPictureDelete(FarmModel.FarmPictureModel model)
        {
            if (ModelState.IsValid)
            {
                var farm = await _farmService.GetFarmById(model.FarmId);

                var farmPicture = farm.FarmPictures.Where(x => x.Id == model.Id).FirstOrDefault();
                if (farmPicture == null)
                    throw new ArgumentException("No farm picture found with the specified id");
                farmPicture.FarmId = farm.Id;

                var pictureId = farmPicture.PictureId;
                await _farmService.DeleteFarmPicture(farmPicture);

                var picture = await _pictureService.GetPictureById(pictureId);
                if (picture != null)
                    await _pictureService.DeletePicture(picture);

                return new NullJsonResult();
            }
            return ErrorForKendoGridJson(ModelState);
        }
        #endregion

        #region Farm Autocomplete
        public virtual async Task<IActionResult> SearchFarmAutoComplete(string term)
        {
            List<string> roles = _workContext.CurrentCustomer.CustomerRoles.Select(x => x.Name).ToList();

            var result = await _farmService.SearchFarm(term);
            if (roles.Contains("VhlsecUser") || roles.Contains("VhlsecAdmin"))
            {
                string vhlsecid = _workContext.CurrentCustomer.EntityId;
                List<string> lssId = _lssService.GetLssByVhlsecId(vhlsecid).Result.Select(m => m.Id).ToList();
                var customers = _customerService.GetCustomerByLssId(lssId, vhlsecid);
                List<string> customerid = customers.Select(x => x.Id).ToList();
                var farm = await _farmService.GetFarmByLssId(customerid, term);
                var currentuser = _workContext.CurrentCustomer.Id;
                //var gridModel = new DataSourceResult {
                //    Data = farm,
                //    Total = farm.TotalCount
                //};
                return Json(farm);
            }
            else
            {

                var currentuser = _workContext.CurrentCustomer.EntityId;
                var allCustomer = await _customerService.GetAllCustomers();
                List<string> customerid = allCustomer.Where(m => m.EntityId == currentuser).Select(x => x.Id).ToList();
                var farm = await _farmService.GetFarmByLssId(customerid, term);
                //var gridModel = new DataSourceResult {
                //    Data = farm,
                //    Total = farm.Count()
                //};
                return Json(farm);
            }
            //return Json(result);
        }
        #endregion

        #region Farm Grasss
        public async Task<IActionResult> FarmGrassAdd(string Type,
          string TotalArea,string farmId,string Season,string GrassName,string NoOfTree)
        {
        var farm = await _farmService.GetFarmById(farmId);
            await _farmService.InsertFarmGrass(new FarmGrass {
                FarmId = farm.Id,
               TotalArea=TotalArea,
              Type=Type,
              GrassName=GrassName,
              Season=Season,
              NoOfTree=NoOfTree
            });
            return Json(new { Result = true });
        }

        [PermissionAuthorizeAction(PermissionActionName.Preview)]
        [HttpPost]
        public async Task<IActionResult> FarmGrassList(DataSourceRequest command, string farmId)
        {
            var farm = await _farmService.GetFarmById(farmId);

            var farmGrasssModel = new List<FarmModel.FarmGrassModel>();
            foreach (var x in farm.FarmGrasses)
            {
               
                var m = new FarmModel.FarmGrassModel {
                    Id = x.Id,
                    FarmId = farm.Id,
                    TotalArea=x.TotalArea,
                    Type=x.Type,
                    GrassName=x.GrassName,
                    Season=x.Season
                };
                farmGrasssModel.Add(m);
            }

            var gridModel = new DataSourceResult {
                Data = farmGrasssModel,
                Total = farmGrasssModel.Count
            };

            return Json(gridModel);
        }

        [PermissionAuthorizeAction(PermissionActionName.Edit)]
        [HttpPost]
        public async Task<IActionResult> FarmGrassUpdate(FarmModel.FarmGrassModel model)
        {
            if (ModelState.IsValid)
            {
                var farm = await _farmService.GetFarmById(model.FarmId);

                var farmGrass = farm.FarmGrasses.Where(x => x.Id == model.Id).FirstOrDefault();
                if (farmGrass == null)
                    throw new ArgumentException("No farm Grass found with the specified id");
                farmGrass.FarmId = farm.Id;


                farmGrass.Type = model.Type;
                farmGrass.TotalArea = model.TotalArea;
                farmGrass.GrassName = model.GrassName;
                await _farmService.UpdateFarmGrass(farmGrass);

               

                return new NullJsonResult();
            }
            return ErrorForKendoGridJson(ModelState);
        }

        [PermissionAuthorizeAction(PermissionActionName.Edit)]
        [HttpPost]
        public async Task<IActionResult> FarmGrassDelete(FarmModel.FarmGrassModel model)
        {
            if (ModelState.IsValid)
            {
                var farm = await _farmService.GetFarmById(model.FarmId);

                var farmGrass = farm.FarmGrasses.Where(x => x.Id == model.Id).FirstOrDefault();
                if (farmGrass == null)
                    throw new ArgumentException("No farm Grass found with the specified id");
                farmGrass.FarmId = farm.Id;
                await _farmService.DeleteFarmGrass(farmGrass);
                return new NullJsonResult();
            }
            return ErrorForKendoGridJson(ModelState);
        }
        #endregion


        #region Farm Shed
        public async Task<IActionResult> FarmShedAdd( FarmShedModel model,string farmId)
        {
            
            var farm = await _farmService.GetFarmById(farmId);
            double l = 0;
            double.TryParse(model.Length, out l);
            double b = 0;
            double.TryParse(model.Bredth, out b);
            double h = 0;
            double.TryParse(model.Height, out h);
            await _farmService.InsertFarmShed(new FarmShed {
                FarmId = farm.Id,
                Length = model.Length,
                Bredth=model.Bredth,
                Height=model.Height,
                Volume=Convert.ToString(l*b*h),
                ConstructedDate=model.ConstructedDate,
                Type = model.Type
            });
            return Json(new { Result = true });
        }

        [PermissionAuthorizeAction(PermissionActionName.Preview)]
        [HttpPost]
        public async Task<IActionResult> FarmShedList(DataSourceRequest command, string farmId)
        {
            var farm = await _farmService.GetFarmById(farmId);

            var farmShedsModel = new List<FarmModel.FarmShedModel>();
            foreach (var x in farm.FarmSheds)
            {

                var m = new FarmModel.FarmShedModel {
                    Id = x.Id,
                    FarmId = farm.Id,
                    Length = x.Length,
                    Bredth = x.Bredth,
                    Height = x.Height,
                    Volume = x.Volume,
                   Type=x.Type,
                   ConstructedDate=x.ConstructedDate
                };
                farmShedsModel.Add(m);
            }

            var gridModel = new DataSourceResult {
                Data = farmShedsModel,
                Total = farmShedsModel.Count
            };

            return Json(gridModel);
        }

        [PermissionAuthorizeAction(PermissionActionName.Edit)]
        [HttpPost]
        public async Task<IActionResult> FarmShedUpdate(FarmShedModel model,string constructedDate)
        {
            if (ModelState.IsValid)
            {
                var farm = await _farmService.GetFarmById(model.FarmId);
                var farmShed = farm.FarmSheds.Where(x => x.Id == model.Id).FirstOrDefault();
                if (farmShed == null)
                    throw new ArgumentException("No farm Shed found with the specified id");
                double l = 0;
                double.TryParse(model.Length, out l);
                double b = 0;
                double.TryParse(model.Bredth, out b);
                double h = 0;
                double.TryParse(model.Height, out h);
                farmShed.FarmId = farm.Id;
                farmShed.Type = model.Type;
                farmShed.Length = model.Length;
                farmShed.Bredth = model.Bredth;
                farmShed.Height = model.Height;
                farmShed.Volume = model.Volume;
                farmShed.Type = model.Type;
                farmShed.ConstructedDate = model.ConstructedDate;
                await _farmService.UpdateFarmShed(farmShed);



                return new NullJsonResult();
            }
            return ErrorForKendoGridJson(ModelState);
        }

        [PermissionAuthorizeAction(PermissionActionName.Edit)]
        [HttpPost]
        public async Task<IActionResult> FarmShedDelete(FarmModel.FarmShedModel model)
        {
            if (ModelState.IsValid)
            {
                var farm = await _farmService.GetFarmById(model.FarmId);

                var farmShed = farm.FarmSheds.Where(x => x.Id == model.Id).FirstOrDefault();
                if (farmShed == null)
                    throw new ArgumentException("No farm Shed found with the specified id");
                farmShed.FarmId = farm.Id;
                await _farmService.DeleteFarmShed(farmShed);
                return new NullJsonResult();
            }
            return ErrorForKendoGridJson(ModelState);
        }
        #endregion

        public IActionResult PPRSList()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> PPRSList(DataSourceRequest command, FarmListModel model)
        {
            var farm = await _farmService.GetPPRsFram(model.Keyword, command.Page - 1, command.PageSize);
            var gridModel = new DataSourceResult {
                Data = farm,
                Total = farm.TotalCount
            };
            return Json(gridModel);

        }


    }
}
