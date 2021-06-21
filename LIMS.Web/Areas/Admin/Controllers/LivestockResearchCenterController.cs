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
    public class LivestockResearchCenterController : BaseAdminController
    {
        private readonly IOtherOrganizationService _organizationService;
        private readonly ILivestockResearchCenterService _livestockResearchCenterService;
        private readonly IFiscalYearService _fiscalYearService;
        private readonly ICustomerService _customerService;
        private readonly IWorkContext _context;
        private readonly ILocalizationService _localizationService;
        private readonly ILssService _lssService;
        private readonly IVhlsecService _vhlsecService;

        public LivestockResearchCenterController(IOtherOrganizationService organizationService,
             ILivestockResearchCenterService livestockResearchCenterService,
             IFiscalYearService fiscalYearService,
             ICustomerService customerService,
             IWorkContext context,
             ILocalizationService localizationService,
            ILssService lssService,
            IVhlsecService vhlsecService

            )
        {
            _organizationService = organizationService;
            _livestockResearchCenterService = livestockResearchCenterService;
            _fiscalYearService = fiscalYearService;
            _context = context;
            _customerService = customerService;
            _localizationService = localizationService;
            _lssService = lssService;
            _vhlsecService = vhlsecService;
        }
        public IActionResult Index() => RedirectToAction("List");

        public IActionResult List() => View();
        [HttpPost]
        public async Task<IActionResult> List(DataSourceRequest command,string Keyword)
        {
            string createdby = null;
            List<string> roles = _context.CurrentCustomer.CustomerRoles.Select(x => x.Name).ToList();
            if (roles.Contains(RoleHelper.LssAdmin) || roles.Contains(RoleHelper.VhlsecAdmin) || roles.Contains(RoleHelper.DolfdAdmin))
            {
                createdby = _context.CurrentCustomer.Id;
            }
            else
            {
                string adminemail = _context.CurrentCustomer.CreatedBy;
                var admin = await _customerService.GetCustomerByEmail(adminemail);
                createdby = admin.Id;
            }
            if (roles.Contains(RoleHelper.LssAdmin) || roles.Contains(RoleHelper.LssUser))
            {
                var hatchery = await _livestockResearchCenterService.GetLivestockResearchCenter(createdby,Keyword, command.Page - 1, command.PageSize);
                var gridModel = new DataSourceResult {
                    Data = hatchery,
                    ExtraData = Keyword,
                    Total = hatchery.TotalCount
                };
                return Json(gridModel);

            }
            else if (roles.Contains(RoleHelper.VhlsecAdmin) || roles.Contains(RoleHelper.VhlsecUser))
            {
                string entityId = _context.CurrentCustomer.EntityId;
                var lssid = await _lssService.GetLssByVhlsecId(entityId);
                var lssids = lssid.Select(m => m.Id).ToList();
                var customer = _customerService.GetCustomerByLssId(lssids, entityId);
                var userids = customer.Select(m => m.Id).ToList();
                var hatchery = await _livestockResearchCenterService.GetLivestockResearchCenter(userids,Keyword, command.Page - 1, command.PageSize);
                var gridModel = new DataSourceResult {
                    Data = hatchery,
                    ExtraData = Keyword,
                    Total = hatchery.TotalCount
                };
                return Json(gridModel);
            }
            else if (roles.Contains(RoleHelper.DolfdAdmin) || roles.Contains(RoleHelper.DolfdUser))
            {
                string entityId = _context.CurrentCustomer.EntityId;
                List<string> vhlsecId = _vhlsecService.GetVhlsecByDolfdId(entityId).Result.Select(m => m.Id).ToList();
                var LssIds = new List<string>();
                foreach (var item in vhlsecId)
                {
                    LssIds.AddRange(_lssService.GetLssByVhlsecId(item).Result.Select(m => m.Id).ToList());
                }
                var userids = _customerService.GetCustomerByLssId(LssIds, vhlsecId, entityId).Select(m => m.Id).ToList();
                var hatchery = await _livestockResearchCenterService.GetLivestockResearchCenter(userids, Keyword, command.Page - 1, command.PageSize);
                var gridModel = new DataSourceResult {
                    Data = hatchery,
                    ExtraData = Keyword,
                    Total = hatchery.TotalCount
                };
                return Json(gridModel);
            }
            else
            {
                return Json(null);
            }
            
        }
        public async Task<ActionResult> Create()
        {
            string createdby = null;
            List<string> roles = _context.CurrentCustomer.CustomerRoles.Select(x => x.Name).ToList();
            if (roles.Contains(RoleHelper.LssAdmin))
            {
                createdby = _context.CurrentCustomer.Id;
            }
            else
            {
                string adminemail = _context.CurrentCustomer.CreatedBy;
                var admin = await _customerService.GetCustomerByEmail(adminemail);
                createdby = admin.Id;
            }

            var organization = await _organizationService.GetOtherOrganizationByType(createdby, "Livestock Resource Center");
            var fiscalyear = new SelectList(await _fiscalYearService.GetFiscalYear(), "Id", "NepaliFiscalYear").ToList();
            fiscalyear.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.FiscalYearId = fiscalyear;
            var livestockResearchCenter = new LivestockResearchCenterModel();
            livestockResearchCenter.Organization = organization;
            return View(livestockResearchCenter);
        }
        [HttpPost]
        public async Task<ActionResult> Create(LivestockResearchCenterModel model, IFormCollection form)
        {
            var type = form["Type"].ToList();
            var totalNoOfLivestock = form["TotalNoOfLivestock"].ToList();
            var totalAreaForFeedAndFodder = form["TotalAreaForFeedAndFodder"].ToList();
            var manufacturedGoods = form["ManufacturedGoods"].ToList();
            var selledLivestockQuantity = form["SelledLivestockQuantity"].ToList();
            var remarks = form["Remarks"].ToList();
            var organizationIds = form["OtherOrganizationId"].ToList();
            string createdby = null;
            List<string> roles = _context.CurrentCustomer.CustomerRoles.Select(x => x.Name).ToList();
            if (roles.Contains(RoleHelper.LssAdmin) || roles.Contains(RoleHelper.VhlsecAdmin) || roles.Contains(RoleHelper.DolfdAdmin))
            {
                createdby = _context.CurrentCustomer.Id;
            }
            else
            {
                string adminemail = _context.CurrentCustomer.CreatedBy;
                var admin = await _customerService.GetCustomerByEmail(adminemail);
                createdby = admin.Id;
            }
            var feedIndustries = new List<LivestockResearchCenter>();
            for (int i = 0; i < organizationIds.Count(); i++)
            {
                if (string.IsNullOrEmpty(organizationIds[i]))
                    continue;

                var otherOrganization = new LivestockResearchCenter {
                    OtherOrganization = await _organizationService.GetOtherOrganizationById(organizationIds[i]),
                    OtherOrganizationId = organizationIds[i],
                    Type = type[i],
                    ManufacturedGoods = manufacturedGoods[i],
                    TotalNoOfLivestock = totalNoOfLivestock[i],
                    SelledLivestockQuantity = selledLivestockQuantity[i],
                    TotalAreaForFeedAndFodder=totalAreaForFeedAndFodder[i],
                    Remarks = remarks[i],
                    CreatedBy = createdby,
                    FiscalYear = await _fiscalYearService.GetFiscalYearById(model.FiscalYearId),
                    FiscalYearId = model.FiscalYearId
                };
                feedIndustries.Add(otherOrganization);
            }
            await _livestockResearchCenterService.InsertLivestockResearchCenterList(feedIndustries);
            return RedirectToAction("List");
        }

    }
}
