using LIMS.Core;
using LIMS.Domain.Seo;
using LIMS.Domain.Vaccination;
using LIMS.Domain.VaccinationInventory;
using LIMS.Framework.Kendoui;
using LIMS.Framework.Security.Authorization;
using LIMS.Services.Basic;
using LIMS.Services.Customers;
using LIMS.Services.Localization;
using LIMS.Services.Security;
using LIMS.Services.Stores;
using LIMS.Services.Vaccination;
using LIMS.Services.VaccinationInventory;
using LIMS.Web.Areas.Admin.Helper;
using LIMS.Web.Areas.Admin.Models.MedicineInventory;
using LIMS.Web.Areas.Admin.Models.VaccinationInventory;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Web.Areas.Admin.Controllers
{
    public class ReceivedVaccinationController : BaseAdminController
    {
        #region fields
        private readonly IVaccinationTypeService _vaccinationTypeService;
        private readonly IFiscalYearService _fiscalYearService;
        private readonly ILanguageService _languageService;
        private readonly ILocalizationService _localizationService;
        private readonly IStoreService _storeService;
        private readonly IWorkContext _workContext;
        private readonly SeoSettings _seoSettings;
        private readonly IReceivedVaccinationService _receivedVaccinationService;
        private readonly IUnitService _unitService;
        private readonly ICustomerService _customerService;
        #endregion fields
        #region ctor
        public ReceivedVaccinationController(
            IVaccinationTypeService vaccinationTypeService,
            ILanguageService languageService,
            ILocalizationService localizationService,
            IStoreService storeService,
            IWorkContext workContext,
            SeoSettings seoSettings,
            IFiscalYearService fiscalYearService,
            IUnitService unitService,
            ICustomerService customerService,
            IReceivedVaccinationService receivedVaccinationService



            )
        {
            _vaccinationTypeService = vaccinationTypeService;
            _languageService = languageService;
            _localizationService = localizationService;
            _storeService = storeService;
            _workContext = workContext;
            _seoSettings = seoSettings;
            _fiscalYearService = fiscalYearService;
            _unitService = unitService;
            _customerService = customerService;
            _receivedVaccinationService = receivedVaccinationService;


        }

        #endregion ctor
        #region MedicineDistribution
        public IActionResult Index() => RedirectToAction("List");

        public IActionResult List() => View();

        [PermissionAuthorizeAction(PermissionActionName.List)]
        [HttpPost]
        public async Task<IActionResult> List(DataSourceRequest command, string keyword = "")
        {
            List<string> roles = _workContext.CurrentCustomer.CustomerRoles.Select(x => x.Name).ToList();
            string createdBy = null;
            //if (roles.Contains(RoleHelper.LssAdmin) || (roles.Contains(RoleHelper.VhlsecAdmin)))
            //{
            //    createdBy = _workContext.CurrentCustomer.Id;
            //}
            //else
            //{
            //    string adminemail = _workContext.CurrentCustomer.CreatedBy;
            //    var admin = await _customerService.GetCustomerByEmail(adminemail);
            //    createdBy = admin.Id;
            //}
            var receivedVaccines = await _receivedVaccinationService.GetReceivedVaccine(createdBy, command.Page - 1, command.PageSize);
            var receivedVaccineLists = new List<ReceivedVaccineListModel>();
            foreach (var item in receivedVaccines)
            {
                receivedVaccineLists.Add(new ReceivedVaccineListModel() {
                    Id = item.Id,
                    VaccineName = (item.Vaccination == null) ? "" : item.Vaccination.MedicalName,
                    Quantity = item.Quantity,
                    UnitId = (item.Unit != null) ? item.Unit.Id : string.Empty,
                    Unit = (item.Unit == null) ? "" : item.Unit.UnitNameEnglish,
                    Fiscalyear = (item.FiscalYear == null) ? "" : item.FiscalYear.EnglishFiscalYear,
                    FiscalYearId = (item.FiscalYear != null) ? item.FiscalYear.Id : string.Empty,
                  
                });
            }

            var gridModel = new DataSourceResult {
                Data = receivedVaccineLists,
                Total = receivedVaccines.TotalCount
            };
            return Json(gridModel);

        }


        public async Task<ActionResult> Create()
        {
            var unit = new SelectList(await _unitService.GetUnit(), "Id", "UnitShortName").ToList();
            unit.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            var fiscalyear = await _fiscalYearService.GetCurrentFiscalYear();

            var fiscalYear = new SelectList(await _fiscalYearService.GetFiscalYear(), "Id", "NepaliFiscalYear", fiscalyear.Id).ToList();
            fiscalYear.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.FiscalYearId = fiscalYear;
            MonthHelper month = new MonthHelper();
            var months = month.GetMonths();
            months.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.Months = months;
            //ViewBag.FiscalYearId = fiscalyear;
            ViewBag.UnitId = unit;

            var purpose = VaccinationPurposeHelper.GetVaccinationPurpose();
            purpose.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.Purpose = purpose;
            var received = VaccinationReceivedHelper.GetVaccinationReceived();
            received.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.Received = received;
            List<string> roles = _workContext.CurrentCustomer.CustomerRoles.Select(x => x.Name).ToList();
            var vaccination = new List<VaccinationType>();
            var vaccine = await _vaccinationTypeService.FiletrVaccinationType("Vaccine");
            vaccination = vaccine.ToList();
            ReceivedVaccineModel model = new ReceivedVaccineModel();

            model.VaccinationType = vaccination;
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Create(ReceivedVaccineModel model, IFormCollection form)
        {
            var medicine = form["VaccinationTypeId"].ToList();
            var quantity = form["Quantity"].ToList();
            var unit = form["UnitId"].ToList();
            var propose = form["Propose"].ToList();

            var receivedVaccines= new List<ReceivedVaccine>();
           
            List<string> roles = _workContext.CurrentCustomer.CustomerRoles.Select(x => x.Name).ToList();
            string createdBy = null;
            //if (roles.Contains(RoleHelper.LssAdmin)||roles.Contains(RoleHelper.VhlsecAdmin))
            //{
            //    createdBy = _workContext.CurrentCustomer.Id;
            //}
            //else
            //{
            //    string adminemail = _workContext.CurrentCustomer.CreatedBy;
            //    var admin = await _customerService.GetCustomerByEmail(adminemail);
            //    createdBy = admin.Id;
            //}
           
            for (int i = 0; i < medicine.Count; i++)
            {
                var receivedVaccine = new ReceivedVaccine {
                    VaccinationTypeId = medicine[i],
                    Vaccination = await _vaccinationTypeService.GetVaccinationTypeById(medicine[i]),
                    FiscalYearId = model.FiscalYearId,
                    FiscalYear = await _fiscalYearService.GetFiscalYearById(model.FiscalYearId),
                    Quantity = quantity[i],
                    Unit = await _unitService.GetUnitById(unit[i]),
                    Propose=model.Propose,
                    Amount=model.Amount,
                    ReceivedBy=model.ReceivedBy,
                    CreatedBy = createdBy,
                  ReceivedFrom=model.ReceivedFrom

                };
               
                    receivedVaccines.Add(receivedVaccine);
                

            }
           
            if (receivedVaccines.Count > 0)
            {
                await _receivedVaccinationService.InsertReceivedVaccine(receivedVaccines);
            }

            return RedirectToAction("Index");
        }
     

        [HttpPost]
        public async Task<IActionResult> UpdateReceivedVaccination(ReceivedVaccineListModel model)
        {
            var receivedVaccine = await _receivedVaccinationService.GetReceivedVaccineById(model.Id);
            receivedVaccine.Unit = await _unitService.GetUnitById(model.UnitId);
            receivedVaccine.Quantity = model.Quantity;
            receivedVaccine.FiscalYearId = model.FiscalYearId;
            receivedVaccine.FiscalYear = await _fiscalYearService.GetFiscalYearById(model.FiscalYearId);
            await _receivedVaccinationService.UpdateReceivedVaccine(receivedVaccine);
            return Json(model);
        }


        #endregion MedicineDistribution
    }
}
