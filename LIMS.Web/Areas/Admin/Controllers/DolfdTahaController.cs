using LIMS.Core;
using LIMS.Domain.Bali;
using LIMS.Domain.MoAMAC;
using LIMS.Framework.Kendoui;
using LIMS.Framework.Mvc.Filters;
using LIMS.Framework.Security.Authorization;
using LIMS.Services.Bali;
using LIMS.Services.Basic;
using LIMS.Services.Customers;
using LIMS.Services.Localization;
using LIMS.Services.MoAMAC;
using LIMS.Services.Security;
using LIMS.Web.Areas.Admin.Models.Bali;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Web.Areas.Admin.Controllers
{
    public class DolfdTahaController : BaseAdminController
    {

            private readonly ITahaDarbandiService _animalRegistrationService;
           
            private readonly ILocalizationService _localizationService;
            private readonly ILanguageService _languageService;
            private readonly IWorkContext _workContext;
            private readonly IFiscalYearService _fiscalYearService;
        private readonly ICustomerService _customerService;
        private readonly IVhlsecService _vhlsecService;
        private readonly IDolfdService _dolfdService;
        public DolfdTahaController(ILocalizationService localizationService,
                ITahaDarbandiService animalRegistrationService,
                ILanguageService languageService,
            
                IWorkContext workContext,
                IFiscalYearService fiscalYearService,
                IUnitService unitService,
                  ICustomerService customerService,
           IVhlsecService vhlsecService,
            IDolfdService dolfdService
                )
            {
                _localizationService = localizationService;
                _animalRegistrationService = animalRegistrationService;
                _languageService = languageService;
               
                _workContext = workContext;
                _fiscalYearService = fiscalYearService;
            _customerService = customerService;
            _vhlsecService = vhlsecService;
            _dolfdService = dolfdService;
          
           }

            public IActionResult Index() => RedirectToAction("List");

            public IActionResult List() => View();

            [PermissionAuthorizeAction(PermissionActionName.List)]
            [HttpPost]
            public async Task<IActionResult> List(DataSourceRequest command, string keyword = "")
            {
                var id = _workContext.CurrentCustomer.Id;
                var bali = await _animalRegistrationService.GetTahaData(id, keyword);
                var gridModel = new DataSourceResult {
                    Data = bali,
                    Total = bali.TotalCount
                };
                return Json(gridModel);
            }


            public IActionResult MarketList() => View();

            [PermissionAuthorizeAction(PermissionActionName.List)]
            [HttpPost]
            public async Task<IActionResult> MarketList(DataSourceRequest command)
            {
                var id = _workContext.CurrentCustomer.Id;
                var bali = await _animalRegistrationService.GetTahaData(id);
                var gridModel = new DataSourceResult {
                    Data = bali,
                    Total = bali.TotalCount
                };
                return Json(gridModel);
            }




            public async Task<IActionResult> Create()
            {
                 var c = await _fiscalYearService.GetCurrentFiscalYear();
                var fiscalYear = new SelectList(await _fiscalYearService.GetFiscalYear(), "Id", "NepaliFiscalYear",c.Id).ToList();
                fiscalYear.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
                ViewBag.FiscalYearId = fiscalYear;

              
            

                return View();
            }

            [PermissionAuthorizeAction(PermissionActionName.Edit)]
            [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
            public async Task<IActionResult> Create(DolfdTahaEntryModel model,IFormCollection col)
            {
               
                    var animalRegistration = new DolfdTahaEntry();
                   animalRegistration.FiscalYear = await _fiscalYearService.GetFiscalYearById(model.FiscalYearId);
            animalRegistration.sixthpad = col["sixthpad"].ToString();
            animalRegistration.sixthpadPurti = col["sixthpadPurti"].ToString();
            animalRegistration.tenthpad = col["tenthpad"].ToString();
            animalRegistration.tenthpadPurti = col["tenthpadPurti"].ToString();
            animalRegistration.fourthpad = col["fourthpad"].ToString();
            animalRegistration.Remarks = col["Remarks"].ToString();


            animalRegistration.fourththpadPurti = col["fourththpadPurti"].ToString();
            animalRegistration.eightthpad = col["eightthpad"].ToString();
            animalRegistration.eightthpadPurti = col["eightthpadPurti"].ToString();

            animalRegistration.eleventhpad = col["eleventhpad"].ToString();

            animalRegistration.eleventhpadPurti = col["eleventhpadPurti"].ToString();

            animalRegistration.twelvethpadD = col["twelvethpadD"].ToString();
            animalRegistration.twelvethpadPurti = col["twelvethpadPurti"].ToString();
           
            animalRegistration.TahaBihinNas = col["TahaBihinNas"].ToString();
            animalRegistration.TahaBihin = col["TahaBihin"].ToString();
            animalRegistration.TahaBihinNaspadPurti = col["TahaBihinNaspadPurti"].ToString();
            animalRegistration.TahaBihinpadPurti = col["TahaBihinpadPurti"].ToString();

            animalRegistration.CreatedBy = _workContext.CurrentCustomer.Id;
            if (string.IsNullOrEmpty(col["LivestockDataId"].ToString()))
            {
                await _animalRegistrationService.InsertTahaData(animalRegistration);
            }
            else
            {
                animalRegistration.Id = col["LivestockDataId"].ToString();
                await _animalRegistrationService.UpdateTahaData(animalRegistration);

            }
            SuccessNotification(_localizationService.GetResource("Admin.Create.successful"));
                   
            
                ViewBag.AllLanguages = await _languageService.GetAllLanguages(true);
                var fiscalYear = new SelectList(await _fiscalYearService.GetFiscalYear(), "Id", "NepaliFiscalYear").ToList();
                fiscalYear.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
                ViewBag.FiscalYearId = fiscalYear;
               
                return View(model);
            }

        public async Task<IActionResult> Report()
        {
            var fiscaly = await _fiscalYearService.GetCurrentFiscalYear();
            var fiscalYear = new SelectList(await _fiscalYearService.GetFiscalYear(), "Id", "NepaliFiscalYear", fiscaly.Id).ToList();

            fiscalYear.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
            ViewBag.FiscalYearId = fiscalYear;


            var role = _workContext.CurrentCustomer.CustomerRoles.Select(m => m.Name).ToList();
            if (role.Contains("MolmacAdmin") || role.Contains("MolmacUser"))
            {
                string entityId = _workContext.CurrentCustomer.EntityId;
                List<Dolfd> dolfdid = _dolfdService.GetDolfdByMolmacId(entityId).Result.ToList();

                var lss = new SelectList(dolfdid, "Id", "NameEnglish").ToList();
                lss.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
                ViewBag.dolfd = lss;
            }
            else if (role.Contains("DolfdAdmin") || role.Contains("DolfdUser") || role.Contains("AddAdmin") || role.Contains("AddUser"))
            {
                string entityId = _workContext.CurrentCustomer.EntityId;
                List<Vhlsec> dolfdid = _vhlsecService.GetVhlsecByDolfdId(entityId).Result.ToList();

                var lss = new SelectList(dolfdid, "Id", "NameEnglish").ToList();
                lss.Insert(0, new SelectListItem(_localizationService.GetResource("Admin.Common.Select"), ""));
                ViewBag.vhlsec = lss;
            }


            return View();
        }
        public async Task<ActionResult> GetTahaDarbandiByFiscalyear(string fiscalYear)
            {
            var cratedby = _workContext.CurrentCustomer.Id;
                var breed = await _animalRegistrationService.GetTahaData(cratedby, 0,int.MaxValue, fiscalYear);

                return Json(breed.ToList());
            }
        public async Task<ActionResult> GetTaha(string fiscalyear, string vhlsecid = "", string dolfdid = "")
        {
            var role = _workContext.CurrentCustomer.CustomerRoles.Select(m => m.Name).ToList();

            if (!string.IsNullOrEmpty(dolfdid) && string.IsNullOrEmpty(vhlsecid))
            {

                var id = _workContext.CurrentCustomer.Id;



                string entity = _workContext.CurrentCustomer.EntityId;
                List<string> entities = _vhlsecService.GetVhlsecByDolfdId(dolfdid).Result.Select(m => m.Id).ToList();
                var customers = _customerService.GetCustomerByLssId(entities, dolfdid);
                List<string> customerid = customers.Select(x => x.Id).ToList();

                var breed = await _animalRegistrationService.GetTahaData(customerid, 0, int.MaxValue, fiscalyear);

                return Json(breed.ToList());





            }
            else if (!string.IsNullOrEmpty(vhlsecid))
            {

                var id = _workContext.CurrentCustomer.Id;

                string entity = vhlsecid;
                var customers = _customerService.GetCustomerByLssId(null, entity);
                List<string> customerid = customers.Select(x => x.Id).ToList();

                var breed = await _animalRegistrationService.GetTahaData(customerid, 0, int.MaxValue, fiscalyear);

                return Json(breed.ToList());


            }
            else
            {

                var id = _workContext.CurrentCustomer.Id;


                List<string> customerid = (dynamic)null;
                if (role.Contains("MolmacAdmin") || role.Contains("MolmacAdmin"))
                {
                    string entity = _workContext.CurrentCustomer.EntityId;
                    List<string> entities = _dolfdService.GetDolfdByMolmacId(entity).Result.Select(m => m.Id).ToList();
                    List<string> lss = new List<string>();
                    foreach (var item in entities)
                    {
                        lss.AddRange(_vhlsecService.GetVhlsecByDolfdId(item).Result.Select(m => m.Id).ToList());
                    }
                    var customers = _customerService.GetCustomerByLssId(lss, entities, entity);
                    customerid = customers.Select(x => x.Id).ToList();

                }
                else if (role.Contains("DolfdAdmin") || role.Contains("DolfdUser") || role.Contains("AddAdmin") || role.Contains("AddUser"))
                {
                    string entity = _workContext.CurrentCustomer.EntityId;
                    List<string> entities = _vhlsecService.GetVhlsecByDolfdId(entity).Result.Select(m => m.Id).ToList();
                    var customers = _customerService.GetCustomerByLssId(entities, entity);
                    customerid = customers.Select(x => x.Id).ToList();


                }
                else
                {
                    customerid = new List<string>();
                    customerid.Add(_workContext.CurrentCustomer.Id);

                }
                var breed = await _animalRegistrationService.GetTahaData(customerid, 0, int.MaxValue, fiscalyear);

                return Json(breed.ToList());


            }
        }

    }
    }

