using LIMS.Core;
using LIMS.Domain.StatisticalData;
using LIMS.Framework.Kendoui;
using LIMS.Framework.Security.Authorization;
using LIMS.Services.Ainr;
using LIMS.Services.AnimalHealth;
using LIMS.Services.Basic;
using LIMS.Services.Breed;
using LIMS.Services.Customers;
using LIMS.Services.Localization;
using LIMS.Services.LocalStructure;
using LIMS.Services.MoAMAC;
using LIMS.Services.Security;
using LIMS.Services.Statisticaldata;
using LIMS.Services.User;
using LIMS.Services.Vaccination;
using LIMS.Web.Areas.Admin.Helper;
using LIMS.Web.Areas.Admin.Models.Livestock;
using LIMS.Web.Areas.Admin.Models.StatisticalData;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Web.Areas.Admin.Controllers
{
    public class TreatmentController:BaseAdminController
    {

        #region fields
        private readonly ILocalizationService _localizationService;
        private readonly ISpeciesService _speciesService;
        private readonly IUnitService _unitService;
        public readonly IFiscalYearService _fiscalYearService;
        public readonly IBreedService _breedService;
        public readonly IWorkContext _workContext;
        public readonly IServiceData _serviceData;
        public readonly ILssService _lssService;
        public readonly ICustomerService _customerService;
        public readonly IFarmService _farmService;
        public readonly IServiceProviderService _serviceProvider;
        public readonly ILocalLevelService _localLevelService;
        public readonly ILivestockSpeciesService _livestockSpeciesService;


        public readonly IVaccinationTypeService _vaccinationService;
        #endregion fields
        #region ctor
        public TreatmentController(ILocalizationService localizationService,
            ISpeciesService speciesService,
             IUnitService unitService,
              IFiscalYearService fiscalYearService,
              IBreedService breedService,
              IWorkContext workContext,
              IServiceData serviceData,
              ILssService lssService,
              ICustomerService customerService,
              IFarmService farmService,
               IVaccinationTypeService vaccinationService,
               IServiceProviderService serviceProvider,
               ILocalLevelService localLevelService,
               ILivestockSpeciesService livestockSpeciesService
            )
        {
            _localizationService = localizationService;
            _speciesService = speciesService;
            _unitService = unitService;
            _fiscalYearService = fiscalYearService;
            _breedService = breedService;
            _workContext = workContext;
            _serviceData = serviceData;
            _lssService = lssService;
            _customerService = customerService;
            _farmService = farmService;
            _vaccinationService = vaccinationService;
            _serviceProvider = serviceProvider;
            _localLevelService = localLevelService;
            _livestockSpeciesService = livestockSpeciesService;
        }
        #endregion ctor
        #region service
        public IActionResult Index() => RedirectToAction("List");

        public async Task<IActionResult> List()
        {

            var createdby = _workContext.CurrentCustomer.EntityId;
            //var createdbys = _customerService.GetCustomerByLssId(null, createdby);
            //var ids = createdbys.Select(m => m.Id).ToList();

            FarmListModel currentfiscal = new FarmListModel();
            var fiscalyear = await _fiscalYearService.GetCurrentFiscalYear();
            currentfiscal.CurrentFiscalYear = fiscalyear.Id;
            var dropdownitem = new SelectList(await _fiscalYearService.GetFiscalYear(), "Id", "NepaliFiscalYear", fiscalyear.Id).ToList();
            dropdownitem.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.common.select"), ""));

            ViewBag.FiscalYearId = dropdownitem;


            var specie = await _livestockSpeciesService.GetBreed();
            var aiSpecies = specie.Where(m => m.Purposes.Contains("AI"));
            var species = new SelectList(aiSpecies, "Id", "NepaliName").ToList();
            species.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.SpeciesId = species;

            var month = new MonthHelper();
            var months = month.GetMonths();
            months.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.Month = months;

            return View(currentfiscal);
        }

        [PermissionAuthorizeAction(PermissionActionName.List)]
        [HttpPost]
        public async Task<IActionResult> List(DataSourceRequest command, string Fiscalyear, string Month, string SpeciesId)
        {
            string user = _workContext.CurrentCustomer.Id;
            List<string> roles = _workContext.CurrentCustomer.CustomerRoles.Select(x => x.Name).ToList();
            string createdby = null;
            //if (roles.Contains(RoleHelper.LssAdmin) || roles.Contains(RoleHelper.VhlsecAdmin) || roles.Contains(RoleHelper.DolfdAdmin))
            //{
            //    createdby = _workContext.CurrentCustomer.Id;
            //}
            //else
            //{
            //    string adminemail = _workContext.CurrentCustomer.CreatedBy;
            //    var admin = await _customerService.GetCustomerByEmail(adminemail);
            //    createdby = adminemail;
            //}

            var fiscalyear = await _fiscalYearService.GetCurrentFiscalYear();
            var CurrentFiscalYear = fiscalyear.Id;
            var service = (dynamic)null;
            //if (Fiscalyear != null)
            //{

            //    service = await _serviceData.GetFilteredByFarmListModel("", "Treatment", SpeciesId, "", Fiscalyear, Month, command.Page - 1, command.PageSize);

            //}
            //else
            //{
                service = await _serviceData.GetFilteredByFarmListModel("", "Treatment", SpeciesId, "", CurrentFiscalYear, Month, command.Page - 1, command.PageSize);

            //}
            var gridModel = new DataSourceResult {
                Data = service,
                Total = service.TotalCount
            };
            return Json(gridModel);

        }

        public async Task<ActionResult> Create()
        {
            
            var unit = new SelectList(await _unitService.GetUnit(), "Id", "UnitShortName").ToList();
            unit.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            var createdby = _workContext.CurrentCustomer.EntityId;
            //var createdbys = _customerService.GetCustomerByLssId(null, createdby);
            //var ids = createdbys.Select(m => m.Id).ToList();
            var treatmentType = new List<SelectListItem>() {
                new SelectListItem{
                    Text="Medical treatment",
                    Value="Medical treatment"
                },
                new SelectListItem{
                    Text="Minor surgical",
                    Value="Minor surgical"
                },
                new SelectListItem{
                    Text="Major surgical",
                    Value="Major surgical"
                },
                new SelectListItem{
                    Text="Infertility management",
                    Value="Infertility management"
                }

            };
            treatmentType.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.TreatmentType = treatmentType;


            var tex = new SelectList(await _serviceProvider.GetServiceProvider(), "Id", "NameEnglish").ToList();
            tex.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.Technicians = tex;

            MonthHelper month = new MonthHelper();
            var months = month.GetMonths();
            months.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.Months = months;

            var provience = ProvinceHelper.GetProvince();
            provience.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.provience = provience;

            var specie = await _livestockSpeciesService.GetBreed();
            var aiSpecies = specie.Where(m => m.Purposes.Contains("AI"));
            var species = new SelectList(aiSpecies, "Id", "NepaliName").ToList();
            species.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.SpeciesId = species;


            var fiscalyear = await _fiscalYearService.GetCurrentFiscalYear();

            var fiscalYear = new SelectList(await _fiscalYearService.GetFiscalYear(), "Id", "NepaliFiscalYear", fiscalyear.Id).ToList();
            fiscalYear.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.FiscalYearId = fiscalYear;

            ViewBag.UnitId = unit;
           
            WardHelper wardHelper = new WardHelper();
            var ward = wardHelper.GetWard();
            ward.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));

            ViewBag.Ward = ward;
            ServicesModel model = new ServicesModel();
            model.District = _workContext.CurrentCustomer.District;
            model.LocalLevel = _workContext.CurrentCustomer.LocalLevel;
            model.LivestockSpecies = aiSpecies.ToList();

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(ServicesModel model, IFormCollection form)
        {
            var speciesids = form["SpeciesId"].ToList();
            var quantities = form["Quantity"].ToList();
          //  var units = form["Unit"].ToList();

            var existingServiceId = form["ServiceDataId"].ToList();
            string createdby = null;
            //List<string> roles = _workContext.CurrentCustomer.CustomerRoles.Select(x => x.Name).ToList();
            //if (roles.Contains(RoleHelper.LssAdmin) || roles.Contains(RoleHelper.VhlsecAdmin) || roles.Contains(RoleHelper.DolfdAdmin))
            //{
            //    createdby = _workContext.CurrentCustomer.Id;
            //}
            //else
            //{
            //    string adminemail = _workContext.CurrentCustomer.CreatedBy;
            //    var admin = await _customerService.GetCustomerByEmail(adminemail);
            //    createdby = adminemail;
            //}
            var addServices = new List<ServicesData>();
            var updateServices = new List<ServicesData>();

            for (int i = 0; i < speciesids.Count(); i++)
            {
                if (string.IsNullOrEmpty(quantities[i]))
                    continue;
                var service = (dynamic)null;

                service = new ServicesData {
                   // Unit = await _unitService.GetUnitById(units[i]),
                    FiscalYear = await _fiscalYearService.GetFiscalYearById(model.FiscalYear),
                    LivestockSpecies = await _livestockSpeciesService.GetBreedById(speciesids[i]),
                    Provience = model.Provience,
                    District = model.District,
                    LocalLevel = _workContext.CurrentCustomer.LocalLevel,
                    ServicesType = "Treatment",
                    CreatedBy = createdby,
                    Month = model.Month,
                    Quantity = quantities[i],
                    TreatmentType=model.TreatmentType
                };

                var isExistingRecord = await _serviceData.GetServiceById(existingServiceId[i]);
                               
                if (isExistingRecord !=null)
                {
                    service.Id = existingServiceId[i];
                    updateServices.Add(service);
                }
                else
                {
                    addServices.Add(service);
                }

            }

            if (updateServices.Count > 0)
                await _serviceData.UpdateServiceList(updateServices);
            if (addServices.Count > 0)
                await _serviceData.InsertServiceList(addServices);



            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> GetServiceData(string fiscalyearId, string month, string district, string locallevel,string treatmenttype="")
        {
            string createdby = null;
            List<string> roles = _workContext.CurrentCustomer.CustomerRoles.Select(x => x.Name).ToList();
            
            var servicesData = await _serviceData.GetFilteredService(fiscalyearId, month, "Treatment", "", "", locallevel,"", treatmenttype);
            var species = await _livestockSpeciesService.GetBreed();
            var aiSpecies = species.Where(m => m.Purposes.Contains("AI"));


            var lstServiceData = new List<ServicesData>();

            foreach (var item in aiSpecies)
            {
                if (servicesData.Any(m => m.LivestockSpecies.Id == item.Id))
                {
                    lstServiceData.Add(servicesData.FirstOrDefault(m => m.LivestockSpecies.Id == item.Id));
                }
                else
                {
                    var objServiceData = new ServicesData{LivestockSpecies =item };
                    objServiceData.Id = "";
                     lstServiceData.Add(objServiceData);
                }
            }
            
            return Json(lstServiceData);


        }

        public async Task<IActionResult> GetAiTechnician()
        {
            var createdby = _workContext.CurrentCustomer.EntityId;
            var createdbys = _customerService.GetCustomerByLssId(null, createdby);
            //var ids = createdbys.Select(m => m.Id).ToList();

            var tex = await _serviceProvider.GetServiceProvider();
            var texs = tex.Where(m => m.Designation == "JT(inseminator)" || m.Designation == "JTA(inseminator)");
            return Json(texs.ToList());

        }

        public async Task<IActionResult> GetAllTechnician()
        {
            var createdby = _workContext.CurrentCustomer.EntityId;
            var createdbys = _customerService.GetCustomerByLssId(null, createdby);
            //var ids = createdbys.Select(m => m.Id).ToList();

            var tex = await _serviceProvider.GetServiceProvider();
            var texs = tex.Where(m => m.Designation != "JT(inseminator)" || m.Designation != "JTA(inseminator)");

            return Json(texs.ToList());

        }
        public async Task<IActionResult> Update(string id, string Quantity)
        {
            var ai = await _serviceData.GetServiceById(id);
            if (ai != null)
            {
                if (ai != null)
                {
                    ai.Quantity = Quantity;
                }
                await _serviceData.UpdateService(ai);
                return Json(true);
            }
            return null;
        }
        #endregion


    }
}
