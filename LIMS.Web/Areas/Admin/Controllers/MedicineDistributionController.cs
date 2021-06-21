using LIMS.Core;
using LIMS.Domain.MedicineInventory;
using LIMS.Domain.Organizations;
using LIMS.Domain.Seo;
using LIMS.Domain.Vaccination;
using LIMS.Framework.Kendoui;
using LIMS.Framework.Security.Authorization;
using LIMS.Services.Basic;
using LIMS.Services.Customers;
using LIMS.Services.Localization;
using LIMS.Services.MedicineInventory;
using LIMS.Services.Organizations;
using LIMS.Services.Security;
using LIMS.Services.Stores;
using LIMS.Services.Vaccination;
using LIMS.Web.Areas.Admin.Helper;
using LIMS.Web.Areas.Admin.Models.MedicineInventory;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Web.Areas.Admin.Controllers
{
    public class MedicineDistributionController : BaseAdminController
    {
        #region fields
        private readonly IVaccinationTypeService _vaccinationTypeService;
        private readonly IFiscalYearService _fiscalYearService;
        private readonly ILanguageService _languageService;
        private readonly ILocalizationService _localizationService;
        private readonly IStoreService _storeService;
        private readonly IWorkContext _workContext;
        private readonly SeoSettings _seoSettings;
        private readonly IReceivedMedicineService _receivedMedicineService;
        private readonly IUnitService _unitService;
        private readonly ICustomerService _customerService;
        private readonly IOrganizationService _organizationService;
        #endregion fields
        #region ctor
        public MedicineDistributionController(
            IVaccinationTypeService vaccinationTypeService,
            ILanguageService languageService,
            ILocalizationService localizationService,
            IStoreService storeService,
            IWorkContext workContext,
            SeoSettings seoSettings,
            IMedicineProgressService medicineProgressService,
            IFiscalYearService fiscalYearService,
            IUnitService unitService,
            ICustomerService customerService,
            IReceivedMedicineService receivedMedicineService,

             IOrganizationService organizationService
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
            _receivedMedicineService = receivedMedicineService;
            _organizationService = organizationService;

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
            if (roles.Contains(RoleHelper.LssAdmin))
            {
                createdBy = _workContext.CurrentCustomer.Id;
            }
            else
            {
                string adminemail = _workContext.CurrentCustomer.CreatedBy;
                var admin = await _customerService.GetCustomerByEmail(adminemail);
                createdBy = admin.Id;
            }
            var distributedMedicines = await _receivedMedicineService.GetReceivedMedicine(createdBy, command.Page-1, command.PageSize);
            var distributedMedicinesList = new List<MedicineDistributionListModel>();
            foreach (var item in distributedMedicines)
            {
                distributedMedicinesList.Add(new MedicineDistributionListModel() {
                    Id=item.Id,
                    MedicineName = (item.VaccinationType == null) ? "" : item.VaccinationType.MedicalName,
                    Quantity = item.Quantity,
                    UnitId = (item.Unit != null) ? item.Unit.Id : string.Empty,
                    Unit = (item.Unit == null) ? "" : item.Unit.UnitNameEnglish,
                    Fiscalyear = (item.FiscalYear == null) ? "" : item.FiscalYear.EnglishFiscalYear,
                    FiscalYearId = (item.FiscalYear != null) ? item.FiscalYear.Id : string.Empty
                });
            }

            var gridModel = new DataSourceResult {
                Data = distributedMedicinesList,
                Total = distributedMedicines.TotalCount
            };
            return Json(gridModel);

        }


        public async Task<ActionResult> Create()
        {
            var unit = new SelectList(await _unitService.GetUnit(), "Id", "UnitShortName").ToList();
            unit.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            var fiscalyear = new SelectList(await _fiscalYearService.GetFiscalYear(), "Id", "NepaliFiscalYear").ToList();
            fiscalyear.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            MonthHelper month = new MonthHelper();
            var months = month.GetMonths();
            months.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.Months = months;
            ViewBag.FiscalYearId = fiscalyear;

            ViewBag.UnitId = unit;
            List<string> roles = _workContext.CurrentCustomer.CustomerRoles.Select(x => x.Name).ToList();
            var vaccination = new List<VaccinationType>();
            if (roles.Contains(RoleHelper.LssAdmin))
            {
                var createdBy = _workContext.CurrentCustomer.Id;

                var vaccine = await _vaccinationTypeService.GetVaccination(createdBy);
                vaccination = vaccine.ToList();

            }
            else
            {
                string adminemail = _workContext.CurrentCustomer.CreatedBy;
                var admin = await _customerService.GetCustomerByEmail(adminemail);
                string createdBy = admin.Id;

                var vaccine = await _vaccinationTypeService.GetVaccination(createdBy);
                vaccination = vaccine.ToList();

            }
            //var lss = new SelectList(await _organizationService.GetOrganization(), "Id", "NameEnglish").ToList();
            //lss.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            //ViewBag.OrganizationId = lss;
            ReceivedMedicineModel model = new ReceivedMedicineModel();

            model.VaccinationType = vaccination;
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Create(ReceivedMedicineModel model, IFormCollection form)
        {
            var medicine = form["MedicineId"].ToList();
            var quantity = form["Quantity"].ToList();
            var unit = form["UnitId"].ToList();
            var existingDistributeMedicine = form["DistributeMedicineId"].ToList();
            //string createdby = _workContext.CurrentCustomer.Id;
            var receivedMedicines = new List<ReceivedMedicine>();
            var updateMedicines = new List<ReceivedMedicine>();
            var addMedicines = new List<ReceivedMedicine>();
            List<string> roles = _workContext.CurrentCustomer.CustomerRoles.Select(x => x.Name).ToList();
            string createdBy = null;
            if (roles.Contains(RoleHelper.LssAdmin))
            {
                 createdBy = _workContext.CurrentCustomer.Id;
            }
            else
            {
                string adminemail = _workContext.CurrentCustomer.CreatedBy;
                var admin = await _customerService.GetCustomerByEmail(adminemail);
                 createdBy = admin.Id;

          
            

            }
            if (model.OrganizationId==null)
            {
                var organization = new Organization();
                organization.NameEnglish = form["OrganizationName"];
                organization.CreatedBy = createdBy;
                await _organizationService.InsertOrganization(organization);
                model.OrganizationId = organization.Id;

            }
            for (int i = 0; i < medicine.Count; i++)
            {
                var distributeMedicine = new ReceivedMedicine {
                    MedicineId = medicine[i],
                    VaccinationType = await _vaccinationTypeService.GetVaccinationTypeById(medicine[i]),
                    FiscalYearId = model.FiscalYearId,
                    FiscalYear = await _fiscalYearService.GetFiscalYearById(model.FiscalYearId),
                    Quantity = quantity[i],
                    Unit = await _unitService.GetUnitById(unit[i]),
                    Organization = await _organizationService.GetOrganizationById(model.OrganizationId),
                    OrganizationId = model.OrganizationId,
                    CreatedBy = createdBy,
                    Month = model.Month,


                };
                if (!string.IsNullOrEmpty(existingDistributeMedicine[i]))
                {
                    distributeMedicine.Id = existingDistributeMedicine[i];
                    updateMedicines.Add(distributeMedicine);
                }
                else
                {
                    addMedicines.Add(distributeMedicine);
                }

            }
            if (updateMedicines.Count > 0)
            {
                await _receivedMedicineService.UpdateReceivedMedicineList(updateMedicines);
            }
            if (addMedicines.Count > 0)
            {
                await _receivedMedicineService.InsertReceivedMedicineList(addMedicines);
            }

            return RedirectToAction("Index");
        }
        [HttpPost]
        public async Task<IActionResult> GetDistributedMedicine(string fiscalyear, string month, string organization)
        {
            string createdby = "";
            List<string> roles = _workContext.CurrentCustomer.CustomerRoles.Select(x => x.Name).ToList();
            if (roles.Contains(RoleHelper.LssAdmin))
            {
                createdby = _workContext.CurrentCustomer.Id;
            }
            else
            {
                string adminemail = _workContext.CurrentCustomer.CreatedBy;
                var admin = await _customerService.GetCustomerByEmail(adminemail);
                createdby = admin.Id;
            }
            var distributedMedicine = await _receivedMedicineService.GetReceivedMedicine(createdby, fiscalyear, month, organization);
            return Json(distributedMedicine);

        }


        [HttpPost]
        public async Task<IActionResult> UpdateDistributedMedicine(MedicineDistributionListModel model)
        {
            var distriuted = await _receivedMedicineService.GetReceivedMedicineById(model.Id);
            distriuted.Unit = await _unitService.GetUnitById(model.UnitId);
            distriuted.Quantity = model.Quantity;
            distriuted.FiscalYearId = model.FiscalYearId;
            distriuted.FiscalYear = await _fiscalYearService.GetFiscalYearById(model.FiscalYearId);
            await _receivedMedicineService.UpdateReceivedMedicine(distriuted);
            return Json(model);
        }


        #endregion MedicineDistribution
    }
}
