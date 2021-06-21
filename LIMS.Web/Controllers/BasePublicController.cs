using LIMS.Framework.Controllers;
using LIMS.Framework.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

namespace LIMS.Web.Controllers
{
    [CheckAccessPublicStore]
    [CheckAccessClosedStore]
    [CheckLanguageSeoCode]
    public abstract partial class BasePublicController : BaseController
    {
        protected virtual IActionResult InvokeHttp404()
        {
            Response.StatusCode = 404;
            return new EmptyResult();
        }
    }
}
