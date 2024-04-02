using LIMS.Core;
using LIMS.Domain.MedicineInventory;
using LIMS.Domain.Seo;
using LIMS.Domain.Vaccination;
using LIMS.Framework.Kendoui;
using LIMS.Framework.Security.Authorization;
using LIMS.Services.Basic;
using LIMS.Services.Customers;
using LIMS.Services.Localization;
using LIMS.Services.MedicineInventory;
using LIMS.Services.Security;
using LIMS.Services.Stores;
using LIMS.Services.Vaccination;
using LIMS.Web.Areas.Admin.Helper;
using LIMS.Web.Areas.Admin.Models.MedicineInventory;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Web.Areas.Admin.Controllers
{
    public class ReceivedMedicineController : BaseAdminController
    {
        #region fields
        private readonly IVaccinationTypeService _vaccinationTypeService;
        private readonly IFiscalYearService _fiscalYearService;
        private readonly ILanguageService _languageService;
        private readonly ILocalizationService _localizationService;
        private readonly IStoreService _storeService;
        private readonly IWorkContext _workContext;
        private readonly SeoSettings _seoSettings;
        private readonly IMedicineProgressService _medicineProgressService;
        private readonly IUnitService _unitService;
        private readonly ICustomerService _customerService;
        #endregion fields
        #region ctor
        public ReceivedMedicineController(
            IVaccinationTypeService vaccinationTypeService,
            ILanguageService languageService,
            ILocalizationService localizationService,
            IStoreService storeService,
            IWorkContext workContext,
            SeoSettings seoSettings,
            IMedicineProgressService medicineProgressService,
            IFiscalYearService fiscalYearService,
            IUnitService unitService,
            ICustomerService customerService

            )
        {
            _vaccinationTypeService = vaccinationTypeService;
            _languageService = languageService;
            _localizationService = localizationService;
            _storeService = storeService;
            _workContext = workContext;
            _seoSettings = seoSettings;
            _medicineProgressService = medicineProgressService;
            _fiscalYearService = fiscalYearService;
            _unitService = unitService;
            _customerService = customerService;

        }

        #endregion ctor

        #region ReceivedMedicine
        public IActionResult Index() => RedirectToAction("List");

        public IActionResult List() => View();

        [PermissionAuthorizeAction(PermissionActionName.List)]
        [HttpPost]
        public async Task<IActionResult> List(DataSourceRequest command, string keyword = "")
        {
            List<string> roles = _workContext.CurrentCustomer.CustomerRoles.Select(x => x.Name).ToList();
            string createdBy = null;
            //if (roles.Contains(RoleHelper.LssAdmin))
            //{
            //    createdBy = _workContext.CurrentCustomer.Id;
            //}
            //else
            //{
            //    string adminemail = _workContext.CurrentCustomer.CreatedBy;
            //    var admin = await _customerService.GetCustomerByEmail(adminemail);
            //    createdBy = admin.Id;
            //}

            var medicineProgresses = await _medicineProgressService.GetMedicineProgress(createdBy, command.Page-1, command.PageSize);

            var receivedMedicine = new List<ReceivedMedicineListModel>();
            foreach (var item in medicineProgresses)
            {
                receivedMedicine.Add(new ReceivedMedicineListModel {
                    Id = item.Id,
                    MedicineName = (item.Vaccination == null) ? " " : item.Vaccination.MedicalName,
                    UnitId = (item.Unit != null) ? item.Unit.Id : string.Empty,
                    Unit = (item.Unit == null) ? "" : item.Unit.UnitNameEnglish,
                    Quantity = item.Quantity,
                    Month = item.Month,
                    Fiscalyear = (item.FiscalYear == null) ? "" : item.FiscalYear.EnglishFiscalYear,
                    FiscalYearId = (item.FiscalYear != null) ? item.FiscalYear.Id : string.Empty
                });
            }

            var gridModel = new DataSourceResult {
                Data = receivedMedicine,
                Total = medicineProgresses.TotalCount
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
           // ViewBag.FiscalYearId = fiscalyear;

            ViewBag.UnitId = unit;
            List<string> roles = _workContext.CurrentCustomer.CustomerRoles.Select(x => x.Name).ToList();
            var vaccination = new List<VaccinationType>();
            var vaccine = await _vaccinationTypeService.GetVaccination("");
            vaccination = vaccine.ToList();
            //if (roles.Contains(RoleHelper.LssAdmin))
            //{
            //    var createdBy = _workContext.CurrentCustomer.Id;

            //    var vaccine = await _vaccinationTypeService.GetVaccination(createdBy);
            //    vaccination = vaccine.ToList();

            //}
            //else
            //{
            //    string adminemail = _workContext.CurrentCustomer.CreatedBy;
            //    var admin = await _customerService.GetCustomerByEmail(adminemail);
            //    string createdBy = admin.Id;

            //    var vaccine = await _vaccinationTypeService.GetVaccination(createdBy);
            //    vaccination = vaccine.ToList();
            //}
            var model = new MedicineProgressModel();
            model.VaccinationType = vaccination;
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(MedicineProgressModel model, IFormCollection form)
        {
            var medicine = form["MedicineId"].ToList();
            var quantity = form["Quantity"].ToList();
            var unit = form["UnitId"].ToList();
            var existingReceivedId = form["ReceivedMedicineId"].ToList();
            string createdby = null;
            var updateReceivedMedicines = new List<MedicineProgress>();
            var addReceivedMedicines = new List<MedicineProgress>();

            List<string> roles = _workContext.CurrentCustomer.CustomerRoles.Select(x => x.Name).ToList();
            //if (roles.Contains(RoleHelper.LssAdmin))
            //{
            //    createdby = _workContext.CurrentCustomer.Id;
            //}
            //else
            //{
            //    string adminemail = _workContext.CurrentCustomer.CreatedBy;
            //    var admin = await _customerService.GetCustomerByEmail(adminemail);
            //    createdby = admin.Id;
            //}
            //var receivedMedicines = new List<MedicineProgress>();
            for (int i = 0; i < medicine.Count; i++)
            {
                var receivedMedicine = new MedicineProgress {
                    MedicineId = medicine[i],
                    Vaccination = await _vaccinationTypeService.GetVaccinationTypeById(medicine[i]),
                    FiscalYearId = model.FiscalYearId,
                    FiscalYear = await _fiscalYearService.GetFiscalYearById(model.FiscalYearId),
                    Quantity = quantity[i],
                    Unit = await _unitService.GetUnitById(unit[i]),
                    CreatedBy = createdby,
                    Month = model.Month,
                    Date=model.Date

                };
                if (!string.IsNullOrEmpty(existingReceivedId[i]))
                {
                    receivedMedicine.Id = existingReceivedId[i];
                    updateReceivedMedicines.Add(receivedMedicine);
                }
                else
                {
                    addReceivedMedicines.Add(receivedMedicine);
                }

                //  receivedMedicines.Add(receivedMedicine);
            }
            if (updateReceivedMedicines.Count > 0)
            {
                await _medicineProgressService.UpdateMedicineProgressList(updateReceivedMedicines);
            }
            else
            {
                await _medicineProgressService.InsertMedicineProgressList(addReceivedMedicines);
            }
            return RedirectToAction("Index");
        }


        [HttpPost]
        public async Task<IActionResult> GetReceivedMedicine(string fiscalyear, string month)
        {
            string createdby = "";
            var roles = _workContext.CurrentCustomer.CustomerRoles.Select(x => x.Name).ToList();
            //if (roles.Contains(RoleHelper.LssAdmin))
            //{
            //    createdby = _workContext.CurrentCustomer.Id;
            //}
            //else
            //{
            //    string adminemail = _workContext.CurrentCustomer.CreatedBy;
            //    var admin = await _customerService.GetCustomerByEmail(adminemail);
            //    createdby = admin.Id;
            //}
            var receivedMedicine = await _medicineProgressService.GetMedicineProgress(createdby, month, fiscalyear);
            return Json(receivedMedicine.ToList());
        }
        #endregion ReceivedMedicine

        [HttpPost]
        public async Task<IActionResult> UpdateReceivedMedicine(ReceivedMedicineListModel model)
        {
            var receivedMedicine = await _medicineProgressService.GetMedicineProgressById(model.Id);
            receivedMedicine.UnitId = model.UnitId;
            receivedMedicine.Unit = await _unitService.GetUnitById(model.UnitId);
            receivedMedicine.Quantity = model.Quantity;
            receivedMedicine.FiscalYearId = model.FiscalYearId;
            receivedMedicine.FiscalYear = await _fiscalYearService.GetFiscalYearById(model.FiscalYearId);
            await _medicineProgressService.UpdateMedicineProgress(receivedMedicine);
            return Json(model);
        }
    }
}
