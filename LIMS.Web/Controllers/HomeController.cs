using Microsoft.AspNetCore.Mvc;

namespace LIMS.Web.Controllers
{
    public partial class HomeController : BasePublicController
    {
        public virtual IActionResult Index() => View();
    }
}
