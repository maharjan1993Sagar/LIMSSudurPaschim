using LIMS.Core;
using LIMS.Domain.Organizations;
using LIMS.Framework.Kendoui;
using LIMS.Services.Basic;
using LIMS.Services.Customers;
using LIMS.Services.Localization;
using LIMS.Services.MoAMAC;
using LIMS.Services.Organizations;
using LIMS.Services.OtherOrganizations;
using LIMS.Web.Areas.Admin.Helper;
using LIMS.Web.Areas.Admin.Models.Organization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Web.Areas.Admin.Controllers
{
    public class DiaryShopController : BaseAdminController
    {
        private readonly IOtherOrganizationService _organizationService;
        private readonly IDIaryIndustryService _diaryIndustryService;
        private readonly IFiscalYearService _fiscalYearService;
        private readonly ICustomerService _customerService;
        private readonly IWorkContext _context;
        private readonly ILocalizationService _localizationService;
        private readonly IVhlsecService _vhlsecService;
        private readonly ILssService _lssService;
        public DiaryShopController(IOtherOrganizationService organizationService,
             IDIaryIndustryService diaryIndustryService,
             IFiscalYearService fiscalYearService,
             ICustomerService customerService,
             IWorkContext context,
             ILocalizationService localizationService,
             IVhlsecService vhlsecService,
             ILssService lssService


            )
        {
            _organizationService = organizationService;
            _diaryIndustryService = diaryIndustryService;
            _fiscalYearService = fiscalYearService;
            _context = context;
            _customerService = customerService;
            _localizationService = localizationService;
            _vhlsecService = vhlsecService;
            _lssService = lssService;
        }
        public IActionResult Index() => RedirectToAction("List");

        public IActionResult List() => View();
        [HttpPost]
        public async Task<IActionResult> List(DataSourceRequest command,string Keyword)
        {
            string createdby = null;
            List<string> roles = _context.CurrentCustomer.CustomerRoles.Select(x => x.Name).ToList();
            //if (roles.Contains(RoleHelper.LssAdmin) || roles.Contains(RoleHelper.VhlsecAdmin) || roles.Contains(RoleHelper.DolfdAdmin))
            //{
            //    createdby = _context.CurrentCustomer.Id;
            //}
            //else
            //{
            //    string adminemail = _context.CurrentCustomer.CreatedBy;
            //    var admin = await _customerService.GetCustomerByEmail(adminemail);
            //    createdby = admin.Id;
            //}
            //if (roles.Contains(RoleHelper.LssAdmin) || roles.Contains(RoleHelper.LssUser))
            //{
                var hatchery = await _diaryIndustryService.GetDiaryIndustryAndShopByFiscalyear(createdby, "Diary shop", Keyword, command.Page - 1, command.PageSize);
                var gridModel = new DataSourceResult {
                    Data = hatchery,
                    ExtraData = Keyword,
                    Total = hatchery.TotalCount
                };
                return Json(gridModel);

            //}
            //else if (roles.Contains(RoleHelper.VhlsecAdmin) || roles.Contains(RoleHelper.VhlsecUser))
            //{
            //    string entityId = _context.CurrentCustomer.EntityId;
            //    var lssid = await _lssService.GetLssByVhlsecId(entityId);
            //    var lssids = lssid.Select(m => m.Id).ToList();
            //    var customer = _customerService.GetCustomerByLssId(lssids, entityId);
            //    var userids = customer.Select(m => m.Id).ToList();
            //    var hatchery = await _diaryIndustryService.GetDiaryIndustryAndShopByFiscalyear(userids, "Diary shop", Keyword, command.Page - 1, command.PageSize);
            //    var gridModel = new DataSourceResult {
            //        Data = hatchery,
            //        ExtraData = Keyword,
            //        Total = hatchery.TotalCount
            //    };
            //    return Json(gridModel);
            //}
            //else if (roles.Contains(RoleHelper.DolfdAdmin) || roles.Contains(RoleHelper.DolfdUser))
            //{
            //    string entityId = _context.CurrentCustomer.EntityId;
            //    List<string> vhlsecId = _vhlsecService.GetVhlsecByDolfdId(entityId).Result.Select(m => m.Id).ToList();
            //    var LssIds = new List<string>();
            //    foreach (var item in vhlsecId)
            //    {
            //        LssIds.AddRange(_lssService.GetLssByVhlsecId(item).Result.Select(m => m.Id).ToList());
            //    }
            //    var userids = _customerService.GetCustomerByLssId(LssIds, vhlsecId, entityId).Select(m => m.Id).ToList();
            //    var hatchery = await _diaryIndustryService.GetDiaryIndustryAndShopByFiscalyear(userids, "Diary shop", Keyword, command.Page - 1, command.PageSize);
            //    var gridModel = new DataSourceResult {
            //        Data = hatchery,
            //        ExtraData = Keyword,
            //        Total = hatchery.TotalCount
            //    };
            //    return Json(gridModel);
            //}
            //else
            //{
            //    return Json(null);
            //}
           
        }
        public async Task<ActionResult> Create()
        {
            string createdby = null;
            List<string> roles = _context.CurrentCustomer.CustomerRoles.Select(x => x.Name).ToList();
            //if (roles.Contains(RoleHelper.LssAdmin))
            //{
            //    createdby = _context.CurrentCustomer.Id;
            //}
            //else
            //{
            //    string adminemail = _context.CurrentCustomer.CreatedBy;
            //    var admin = await _customerService.GetCustomerByEmail(adminemail);
            //    createdby = admin.Id;
            //}

            var organization = await _organizationService.GetOtherOrganizationByType(createdby, "Diary shop");
            var fiscalyear = await _fiscalYearService.GetCurrentFiscalYear();

            var fiscalYear = new SelectList(await _fiscalYearService.GetFiscalYear(), "Id", "NepaliFiscalYear", fiscalyear.Id).ToList();
            fiscalYear.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.FiscalYearId = fiscalYear;
            var feedIndustry = new DiaryIndustryAndShopModel();
            feedIndustry.Organization = organization;
            return View(feedIndustry);
        }
        [HttpPost]
        public async Task<ActionResult> Create(DiaryIndustryAndShopModel model, IFormCollection form)
        {
            var milk = form["Milk"].ToList();
            var curd = form["Curd"].ToList();
            var Cheese= form["Cheese"].ToList();
            var churpi = form["Churpi"].ToList();
            var icecream= form["IceCream"].ToList();
            var mithai = form["Mithai"].ToList();
            var panir= form["Panir"].ToList();
            var ghew = form["Ghew"].ToList();
            var others= form["Others"].ToList();
        
            var remarks = form["Remarks"].ToList();
            var organizationIds = form["OtherOrganizationId"].ToList();
            var yearlyBusinessIncome = form["YearlyBusinessIncome"].ToList();
            string createdby = null;
            List<string> roles = _context.CurrentCustomer.CustomerRoles.Select(x => x.Name).ToList();
            //if (roles.Contains(RoleHelper.LssAdmin) || roles.Contains(RoleHelper.VhlsecAdmin) || roles.Contains(RoleHelper.DolfdAdmin))
            //{
            //    createdby = _context.CurrentCustomer.Id;
            //}
            //else
            //{
            //    string adminemail = _context.CurrentCustomer.CreatedBy;
            //    var admin = await _customerService.GetCustomerByEmail(adminemail);
            //    createdby = admin.Id;
            //}
            var diaryIndustries = new List<DiaryIndustryAndShop>();
            for (int i = 0; i < organizationIds.Count(); i++)
            {
                if (string.IsNullOrEmpty(organizationIds[i]))
                    continue;

                var diary = new DiaryIndustryAndShop {
                    OtherOrganization = await _organizationService.GetOtherOrganizationById(organizationIds[i]),
                    OtherOrganizationId = organizationIds[i],
                    Milk=milk[i],
                    Panir=panir[i],
                    Mithai=mithai[i],
                    Curd=curd[i],
                    Cheese=Cheese[i],
                    IceCream=icecream[i],
                    Ghew=ghew[i],
                    Others=others[i],
                    Churpi=churpi[i],
                    YearlyBusinessIncome= yearlyBusinessIncome[i],
                    CreatedBy = createdby,
                    FiscalYear = await _fiscalYearService.GetFiscalYearById(model.FiscalYearId),
                    FiscalYearId = model.FiscalYearId
                };
                diaryIndustries.Add(diary);
            }
            await _diaryIndustryService.InsertDiaryIndustryAndShopList(diaryIndustries);
            return RedirectToAction("List");
        }
    }
}
