using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using LIMS.Services.GeneralCMS;
using LIMS.Services.NewsEvent;
using MediatR;
using LIMS.Web.Models.Website;

namespace LIMS.Web.Controllers
{
    public partial class HomeController : BasePublicController
    {
        string userId = "600fd784c61fcbfa6ca65586";
        private readonly IContactUsService _contactUsService;
        private readonly IEmployeeService _employeeService;
        private readonly IPageContentService _pageContentService;
        private readonly IImportantLinksService _importantLinksService;
        private readonly INewsEventService _newsEventTenderService;
        private readonly IMediator _mediator;
        private readonly IBannerService _bannerService;
        // public virtual IActionResult Index() => View();

        public HomeController(IMediator mediator,
                              IContactUsService contactUsService,
                              IPageContentService pageContentService,
                              IImportantLinksService importantLinkService,
                              INewsEventService newsEventTenderService,
                              IBannerService bannerService,
                              IEmployeeService employeeService)
        {
            _mediator = mediator;
            _contactUsService = contactUsService;
            _pageContentService = pageContentService;
            _importantLinksService = importantLinkService;
            _newsEventTenderService = newsEventTenderService;
            _bannerService = bannerService;
            _employeeService = employeeService;
        }

        public async Task<IActionResult> Index()
        {
            var banners = await _bannerService.GetAll();
            var pageContent = await _bannerService.GetAll();
            var employee = await _employeeService.GetAll();
            var contactUs= await _contactUsService.GetAll();
            var importantLinks = await _importantLinksService.GetAll();
            var newsEvent= await _newsEventTenderService.GetAll();


            var homeModel = new HomeModel();
            //homeModel.PageContent=


            return View();
        }
    }
}
