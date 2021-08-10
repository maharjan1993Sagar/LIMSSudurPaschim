using LIMS.Framework.Kendoui;
using LIMS.Framework.Security.Authorization;
using LIMS.Services.Security;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Web.Areas.Admin.Controllers
{
    public class ProgressReport : BaseAdminController
    {
        public Program MyProperty { get; set; }
        public ProgressReport()
        {

        }
        public IActionResult ProgramSummery()
        {
            return View();
        }
        [PermissionAuthorizeAction(PermissionActionName.List)]
        [HttpPost]
        public async Task<IActionResult> ProgramSummery(DataSourceRequest command)
        {
            //var pageContent = await _pageContentService.GetPageContentByUser(command.Page - 1, command.PageSize);

            //var gridModel = new DataSourceResult {
            //    Data = pageContent,
            //    Total = pageContent.TotalCount
            //};
            return Json("as");
        }
    }
}
