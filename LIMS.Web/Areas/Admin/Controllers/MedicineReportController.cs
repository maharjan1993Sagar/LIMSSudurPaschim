using LIMS.Core;
using LIMS.Domain.MedicineInventory;
using LIMS.Domain.Vaccination;
using LIMS.Services.Basic;
using LIMS.Services.Customers;
using LIMS.Services.Localization;
using LIMS.Services.MedicineInventory;
using LIMS.Services.Vaccination;
using LIMS.Web.Areas.Admin.Helper;
using LIMS.Web.Areas.Admin.Models;
using LIMS.Web.Areas.Admin.Models.MedicineInventory;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Web.Areas.Admin.Controllers
{
    public class MedicineReportController : BaseAdminController
    {
        public readonly IMedicineProgressService _medicineProgress;
        public readonly IReceivedMedicineService _receivedMedicine;
        public readonly IVaccinationTypeService _vaccinationTypeService;
        public readonly IWorkContext _workContext;
        public readonly ICustomerService _customerService;
        public readonly IFiscalYearService _fiscalYearService;
        public readonly ILocalizationService _localizationService;

        public MedicineReportController(
            IMedicineProgressService medicineProgress,
             IReceivedMedicineService receivedMedicine,
             IWorkContext workContext,
             IVaccinationTypeService vaccinationTypeService,
              ICustomerService customerService,
              IFiscalYearService fiscalYearService,
              ILocalizationService localizationService


            )
        {
            _medicineProgress = medicineProgress;
            _receivedMedicine = receivedMedicine;
            _workContext = workContext;
            _vaccinationTypeService = vaccinationTypeService;
            _customerService = customerService;
            _fiscalYearService = fiscalYearService;
            _localizationService = localizationService;
           
        }
        public async Task<IActionResult> Index()
        {
            var fiscalyear = new SelectList(await _fiscalYearService.GetFiscalYear(), "Id", "NepaliFiscalYear").ToList();
            fiscalyear.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            MonthHelper month = new MonthHelper();
            var months = month.GetMonths();
            months.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.Months = months;
            ViewBag.FiscalYearId = fiscalyear;
            return View();
        }
        public async Task<IActionResult> MedicineReport(string fiscalyear,string month) 
        {
            List<string> roles = _workContext.CurrentCustomer.CustomerRoles.Select(x => x.Name).ToList();
            var vaccination = new List<VaccinationType>();
            string createdBy="";
            var vaccine = await _vaccinationTypeService.GetVaccination(createdBy);
            vaccination = vaccine.ToList();
            //if (roles.Contains(RoleHelper.LssAdmin))
            //{
            //  createdBy = _workContext.CurrentCustomer.Id;
            //    var vaccine = await _vaccinationTypeService.GetVaccination(createdBy);
            //    vaccination = vaccine.ToList();

            //}
            //else
            //{
            //    string adminemail = _workContext.CurrentCustomer.CreatedBy;
            //    var admin = await _customerService.GetCustomerByEmail(adminemail);
            //     createdBy = admin.Id;

            //    var vaccine = await _vaccinationTypeService.GetVaccination(createdBy);
            //    vaccination = vaccine.ToList();

            //}
            var customer = _workContext.CurrentCustomer;
            ViewBag.LocalLevel = customer.LocalLevel;
            ViewBag.Address = customer.Addresses;
            ViewBag.Level = ExecutionHelper.LevelEnglish;
            var vaccinationReport = new List<BaseMedicineReportModel>();
            var receivedMedicine = await _medicineProgress.GetMedicineProgress(createdBy);
            var sentmedicine = await _receivedMedicine.GetReceivedMedicine(createdBy);
            var monthsentmedicine = await _receivedMedicine.GetReceivedMedicine(createdBy, fiscalyear, month);
            var year = await _fiscalYearService.GetFiscalYearById(fiscalyear);
            foreach (var item in vaccination)
            {
              
                int totalMedicine;
                int distributedMedicine;
                if (receivedMedicine.Where(m => m.Vaccination.Id == item.Id).Count()!= 0)
                {
                     totalMedicine = receivedMedicine.Where(m => m.Vaccination.Id == item.Id).Sum(m => Convert.ToInt32((string.IsNullOrEmpty(m.Quantity)) ? "0" : m.Quantity));

                }
                else
                {
                     totalMedicine = 0;
                }
                if(sentmedicine.Where(m => m.Month != month && m.FiscalYear.Id == fiscalyear && m.VaccinationType.Id == item.Id).Count()!=0)
                {
                     distributedMedicine = sentmedicine.Where(m => m.Month != month && m.FiscalYear.Id == fiscalyear && m.VaccinationType.Id == item.Id).Sum(m => Convert.ToInt32((string.IsNullOrEmpty(m.Quantity)) ? "0" : m.Quantity));

                }
                else
                {
                     distributedMedicine = 0;
                }
                var stock = totalMedicine - distributedMedicine;
                var totaldistributedMedicine = sentmedicine.Where(m=> m.FiscalYear.Id == fiscalyear && m.VaccinationType.Id == item.Id).Sum(m => Convert.ToInt32((string.IsNullOrEmpty(m.Quantity)) ? "0" : m.Quantity));
                var remainingStock = totalMedicine - totaldistributedMedicine;

                var distributedThisMonth= sentmedicine.Where(m => m.FiscalYear.Id == fiscalyear && m.VaccinationType.Id == item.Id&&m.Month==month).Sum(m => Convert.ToInt32((string.IsNullOrEmpty(m.Quantity)) ? "0" : m.Quantity));
                string unit;
                try
                {
                     unit = receivedMedicine.Where(m => m.Vaccination.Id == item.Id).Select(m => m.Unit.UnitNameEnglish).FirstOrDefault();
                }
                catch(Exception)
                {
                    unit = "";
                }

                vaccinationReport.Add(new BaseMedicineReportModel {
                    MedicineName = item.MedicalName,
                    TotalStock = stock.ToString(),
                    Distribution = distributedThisMonth.ToString(),
                   Unit=unit,
                   RemaningStock=remainingStock.ToString(),


                }); 
            }

            var medicineReportModel = new MedicineReportModel();
            medicineReportModel.Month = month;
            medicineReportModel.FiscalYear = year.NepaliFiscalYear;
            medicineReportModel.BaseMedicineReportModels = vaccinationReport;
            return View(medicineReportModel);


        }
    }
}
