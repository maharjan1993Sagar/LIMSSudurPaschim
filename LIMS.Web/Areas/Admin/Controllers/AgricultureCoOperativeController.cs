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
    public class AgricultureCoOperativeController : BaseAdminController
    {
        private readonly IOtherOrganizationService _organizationService;
        private readonly IAgricultureCoOperativeService _AgricultureCoOperativeService;
        private readonly IFiscalYearService _fiscalYearService;
        private readonly ICustomerService _customerService;
        private readonly IWorkContext _context;
        private readonly ILocalizationService _localizationService;
        private readonly ILssService _lssService;
        private readonly IVhlsecService _vhlsecService;

        public AgricultureCoOperativeController(IOtherOrganizationService organizationService,
            IAgricultureCoOperativeService AgricultureCoOperativeService,
             IFiscalYearService fiscalYearService,
             ICustomerService customerService,
             IWorkContext context,
             ILocalizationService localizationService,
             ILssService lssService,
             IVhlsecService vhlsecService

            )
        {
            _organizationService = organizationService;
            _AgricultureCoOperativeService = AgricultureCoOperativeService;
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
        public async Task<IActionResult> List(DataSourceRequest command, string Keyword)
        {
            string createdby = null;
            List<string> roles = _context.CurrentCustomer.CustomerRoles.Select(x => x.Name).ToList();
          
            var hatchery = await _AgricultureCoOperativeService.GetAgricultureCoOperative("", Keyword, command.Page - 1, command.PageSize);
            var gridModel = new DataSourceResult {
                Data = hatchery,
                ExtraData = Keyword,
                Total = hatchery.TotalCount
            };
            return Json(gridModel);

           

        }
        public async Task<ActionResult> Create()
        {
            string createdby = null;
            List<string> roles = _context.CurrentCustomer.CustomerRoles.Select(x => x.Name).ToList();

            var organization = await _organizationService.GetOtherOrganizationByType("", "Agriculture CoOperative");


            var currFiscalYear = await _fiscalYearService.GetCurrentFiscalYear();
            var fiscalyear = new SelectList(await _fiscalYearService.GetFiscalYear(), "Id", "NepaliFiscalYear",currFiscalYear.NepaliFiscalYear).ToList();
            fiscalyear.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.FiscalYearId = fiscalyear;
          
            var otherOrg = new AgricultureCoOperativeModel();
            otherOrg.Organization = organization;

            var lstCoOperative = await _AgricultureCoOperativeService.GetAgricultureCoOperative("", currFiscalYear.Id);

            var allOrganizations = await _organizationService.GetOtherOrganizationByType("", "Agriculture CoOperative");

            foreach (var item in allOrganizations)
            {
                if (!lstCoOperative.Any(m => m.OtherOrganizationId == item.Id))
                {
                    lstCoOperative.Add(new AgricultureCoOperative {
                        OtherOrganizationId = item.Id,
                        OtherOrganization = item,
                        FemaleMembers = 0,
                        MaleMembers = 0,
                        OthersMembers = 0,
                        Remarks = ""
                    });
                }
            }

            otherOrg.AllOrganization = lstCoOperative.ToList();

            return View(otherOrg);
        }
        [HttpPost]
        public async Task<ActionResult> Create(AgricultureCoOperativeModel model, IFormCollection form)
        {
            var maleMembers = form["MaleMembers"].ToList();
            var femaleMembers = form["FemaleMembers"].ToList();
            var OtherMembers = form["OthersMembers"].ToList();
            var remarks = form["Remarks"].ToList();
            var organizationIds = form["OtherOrganizationId"].ToList();
            var ids = form["Id"].ToList();
            string createdby = null;
            List<string> roles = _context.CurrentCustomer.CustomerRoles.Select(x => x.Name).ToList();


            var otherOrganizationList = new List<AgricultureCoOperative>();
            var updateOrganiationList = new List<AgricultureCoOperative>();
            for (int i = 0; i < ids.Count(); i++)
            {
                if (string.IsNullOrEmpty(organizationIds[i]))
                    continue;
                var agriById = await _AgricultureCoOperativeService.GetAgricultureCoOperativeById(ids[i]);

                if (agriById != null)
                {
                    agriById.OtherOrganization = await _organizationService.GetOtherOrganizationById(organizationIds[i]);
                    agriById.OtherOrganizationId = organizationIds[i];
                    agriById.MaleMembers = Convert.ToInt32(maleMembers[i]);
                    agriById.FemaleMembers = Convert.ToInt32(femaleMembers[i]);
                    agriById.OthersMembers = Convert.ToInt32(OtherMembers[i]);
                    agriById.Remarks = remarks[i];
                    agriById.FiscalYear = await _fiscalYearService.GetFiscalYearById(model.FiscalYearId);
                    agriById.FiscalYearId = model.FiscalYearId;

                    updateOrganiationList.Add(agriById);

                }
                else
                {
                    var otherOrganization = new AgricultureCoOperative {
                        OtherOrganization = await _organizationService.GetOtherOrganizationById(organizationIds[i]),
                        OtherOrganizationId = organizationIds[i],
                        MaleMembers = Convert.ToInt32(maleMembers[i]),
                        FemaleMembers = Convert.ToInt32(femaleMembers[i]),
                        OthersMembers = Convert.ToInt32(OtherMembers[i]),
                        Remarks = remarks[i],
                        FiscalYear = await _fiscalYearService.GetFiscalYearById(model.FiscalYearId),
                        FiscalYearId = model.FiscalYearId,
                        CreatedBy = createdby
                    };


                    otherOrganizationList.Add(otherOrganization);
                }
            }
            await _AgricultureCoOperativeService.InsertAgricultureCoOperativeList(otherOrganizationList);

           await _AgricultureCoOperativeService.UpdateAgricultureCoOperativeList(updateOrganiationList);

            return RedirectToAction("List");
        }

        [HttpPost]
        public async Task<ActionResult> GetByFiscalYear(string fiscalyear = "")
        {
            var lstCoOperative = await _AgricultureCoOperativeService.GetAgricultureCoOperative("", fiscalyear);

            var allOrganizations = await _organizationService.GetOtherOrganizationByType("", "Agriculture CoOperative");

            foreach (var item in allOrganizations)
            {
                if (!lstCoOperative.Any(m => m.OtherOrganizationId == item.Id))
                {
                    lstCoOperative.Add(new AgricultureCoOperative {
                    OtherOrganizationId = item.Id,
                    OtherOrganization = item,
                    FemaleMembers=0,
                    MaleMembers = 0,
                    OthersMembers = 0,
                    Remarks = ""
                    });
                }
            }

            return Json(lstCoOperative);
        }

        

    }
}
