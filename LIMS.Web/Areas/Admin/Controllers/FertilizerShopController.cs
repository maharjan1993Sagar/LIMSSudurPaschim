﻿using LIMS.Core;
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
    public class FertilizerShopController:BaseAdminController
    {
        private readonly IOtherOrganizationService _organizationService;
        private readonly IFertilizerShopService _FertilizerShopService;
        private readonly IFiscalYearService _fiscalYearService;
        private readonly ICustomerService _customerService;
        private readonly IWorkContext _context;
        private readonly ILocalizationService _localizationService;
        private readonly ILssService _lssService;
        private readonly IVhlsecService _vhlsecService;

        public FertilizerShopController(IOtherOrganizationService organizationService,
            IFertilizerShopService FertilizerShopService,
             IFiscalYearService fiscalYearService,
             ICustomerService customerService,
             IWorkContext context,
             ILocalizationService localizationService,
             ILssService lssService,
             IVhlsecService vhlsecService

            )
        {
            _organizationService = organizationService;
            _FertilizerShopService = FertilizerShopService;
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
                var hatchery = await _FertilizerShopService.GetFertilizerShop("", Keyword, command.Page - 1, command.PageSize);
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
            //    var hatchery = await _FertilizerShopService.GetFertilizerShop(userids, Keyword, command.Page - 1, command.PageSize);
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
            //    var hatchery = await _FertilizerShopService.GetFertilizerShop(userids, Keyword, command.Page - 1, command.PageSize);
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

            var organization = await _organizationService.GetOtherOrganizationByType("", "Fertilizer Shop");
            var fiscalyear = await _fiscalYearService.GetCurrentFiscalYear();

            var fiscalYear = new SelectList(await _fiscalYearService.GetFiscalYear(), "Id", "NepaliFiscalYear", fiscalyear.Id).ToList();
            fiscalYear.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.FiscalYearId = fiscalYear;
            var otherOrg = new FertilizerShopModel();
            otherOrg.Organization = organization;
            return View(otherOrg);
        }
        [HttpPost]
        public async Task<ActionResult> Create(FertilizerShopModel model, IFormCollection form)
        {           
            var fertilizer1 = form["Fertilizer1"].ToList();
            var fertilizer2 = form["Fertilizer2"].ToList();
            var fertilizer3 = form["Fertilizer3"].ToList();
            var fertilizerOther = form["FertilizerOther"].ToList();
            var remarks = form["Remarks"].ToList();
            var ids = form["Id"].ToList();
            var organizationIds = form["OtherOrganizationId"].ToList();
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
            var otherOrganizationList = new List<FertilizerShop>();
            var updateOrganizationList = new List<FertilizerShop>();
            for (int i = 0; i < organizationIds.Count(); i++)
            {
                if (string.IsNullOrEmpty(organizationIds[i]))
                    continue;
                var otherOrganization = await _FertilizerShopService.GetFertilizerShopById(ids[i]);

                if (otherOrganization != null)
                {
                    otherOrganization.OtherOrganization = await _organizationService.GetOtherOrganizationById(organizationIds[i]);
                    otherOrganization.OtherOrganizationId = organizationIds[i];
                        otherOrganization.Fertilizer1 = Convert.ToDecimal(fertilizer1[i]);
                        otherOrganization.Fertilizer2 = Convert.ToDecimal(fertilizer2[i]);
                        otherOrganization.Fertilizer3 = Convert.ToDecimal(fertilizer3[i]);
                        otherOrganization.FertilizerOther = Convert.ToDecimal(fertilizerOther[i]);
                        otherOrganization.Remarks = remarks[i];
                        otherOrganization.FiscalYear = await _fiscalYearService.GetFiscalYearById(model.FiscalYearId);
                        otherOrganization.FiscalYearId = model.FiscalYearId;
                        otherOrganization.CreatedBy = createdby;
                    updateOrganizationList.Add(otherOrganization);
                }
                else
                {
                    otherOrganization = new FertilizerShop {
                        OtherOrganization = await _organizationService.GetOtherOrganizationById(organizationIds[i]),
                        OtherOrganizationId = organizationIds[i],
                        Fertilizer1 = Convert.ToDecimal(fertilizer1[i]),
                        Fertilizer2 = Convert.ToDecimal(fertilizer2[i]),
                        Fertilizer3 = Convert.ToDecimal(fertilizer3[i]),
                        FertilizerOther = Convert.ToDecimal(fertilizerOther[i]),
                        Remarks = remarks[i],
                        FiscalYear = await _fiscalYearService.GetFiscalYearById(model.FiscalYearId),
                        FiscalYearId = model.FiscalYearId,
                        CreatedBy = createdby
                    };
                    otherOrganizationList.Add(otherOrganization);
                }
            }
            if (otherOrganizationList.Count > 0)
            {
                await _FertilizerShopService.InsertFertilizerShopList(otherOrganizationList);
            }
            if(updateOrganizationList.Count>0)
            {
                await _FertilizerShopService.UpdateFertilizerShopList(updateOrganizationList);
            }
            return RedirectToAction("List");
        }

        [HttpPost]
        public async Task<ActionResult> GetByFiscalYear(string fiscalyear = "")
        {
            var lstCoOperative = await _FertilizerShopService.GetFertilizerShop("", fiscalyear);

            var allOrganizations = await _organizationService.GetOtherOrganizationByType("", "Fertilizer Shop");

            foreach (var item in allOrganizations)
            {
                if (!lstCoOperative.Any(m => m.OtherOrganizationId == item.Id))
                {
                    lstCoOperative.Add(new FertilizerShop {
                        Id="",
                        OtherOrganizationId = item.Id,
                        OtherOrganization = item,
                        Fertilizer1 = 0,
                        Fertilizer2 = 0,
                        Fertilizer3 = 0,
                        FertilizerOther=0,
                        Remarks = ""
                    });
                }
            }

            return Json(lstCoOperative);
        }

    }
}