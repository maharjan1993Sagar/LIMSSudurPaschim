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
    public class CanelClubeController:BaseAdminController
    {
        private readonly IOtherOrganizationService _organizationService;
        private readonly ICanelClubeService _canelClubeService;
        private readonly IFiscalYearService _fiscalYearService;
        private readonly ICustomerService _customerService;
        private readonly IWorkContext _context;
        private readonly ILocalizationService _localizationService;
        private readonly IVhlsecService _vhlsecService;
        private readonly ILssService _lssService;
        public CanelClubeController(IOtherOrganizationService organizationService,
            ICanelClubeService canelClubeService,
             IFiscalYearService fiscalYearService,
             ICustomerService customerService,
             IWorkContext context,
             ILocalizationService localizationService,
            IVhlsecService vhlsecService,
            ILssService lssService
            )
        {
            _organizationService = organizationService;
            _canelClubeService = canelClubeService;
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
        public async Task<IActionResult> List(DataSourceRequest command,string Keyword="")
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
                var hatchery = await _canelClubeService.GetCanelClube(createdby, Keyword, command.Page - 1, command.PageSize);
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
                var hatchery = await _canelClubeService.GetCanelClubeByUserList(userids,Keyword, command.Page - 1, command.PageSize);
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
                var hatchery = await _canelClubeService.GetCanelClubeByUserList(userids,  Keyword, command.Page - 1, command.PageSize);
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

            var organization = await _organizationService.GetOtherOrganizationByType(createdby, "Canel clube");
            var fiscalyear = new SelectList(await _fiscalYearService.GetFiscalYear(), "Id", "NepaliFiscalYear").ToList();
            fiscalyear.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.FiscalYearId = fiscalyear;
            var otherOrg = new CanelClubeModel();
            otherOrg.Organization = organization;
            return View(otherOrg);
        }
        [HttpPost]
        public async Task<ActionResult> Create(CanelClubeModel model, IFormCollection form)
        {
            var dogBreedToBeSold = form["DogBreedToBeSold"].ToList();
            var providedServices = form["ProvidedServices"].ToList();
            var maleManPower = form["MaleManPower"].ToList();
            var femaleManpower = form["FemaleManpower"].ToList();
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
            var otherOrganizationList = new List<CanelClube>();
            for (int i = 0; i < organizationIds.Count(); i++)
            {
                if (string.IsNullOrEmpty(organizationIds[i]))
                    continue;

                var otherOrganization = new CanelClube {
                    OtherOrganization = await _organizationService.GetOtherOrganizationById(organizationIds[i]),
                    OtherOrganizationId = organizationIds[i],
                    DogBreedToBeSold = dogBreedToBeSold[i],
                    ProvidedServices = providedServices[i],
                    MaleManPower=maleManPower[i],
                    FemaleManpower = femaleManpower[i],
                    Remarks = remarks[i],

                    FiscalYear = await _fiscalYearService.GetFiscalYearById(model.FiscalYearId),
                    FiscalYearId = model.FiscalYearId,
                    CreatedBy = createdby
                };
                otherOrganizationList.Add(otherOrganization);
            }
            await _canelClubeService.InsertCanelClubeList(otherOrganizationList);
            return RedirectToAction("List");
        }
    }
}
