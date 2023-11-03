using LIMS.Api.DTOs.AINR;
using LIMS.Api.DTOs.GeneralCMS;
using LIMS.Api.Queries.Models.Common;
using LIMS.Domain.Data;
using LIMS.Services.GeneralCMS;
using LIMS.Services.Media;
using MediatR;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using LIMS.Api.Extensions;
using LIMS.Services.NewsEvent;
using System.IO;
using LIMS.Services.DynamicMenu;

namespace LIMS.Api.Queries.Handlers.GeneralCMS
{
    public class GetNewsEventQueryHandler : IRequestHandler<GetQueryCMS<NewsEventTenderDto>, IList<NewsEventTenderDto>>
    {
        private readonly INewsEventService _newsEventService;
        private readonly IPictureService _pictureService;
        private readonly IMainMenuService _mainMenuService;
        private readonly ISubMenuService _subMenuService;
        private readonly ISubSubMenuService _subSubMenuService;
        public GetNewsEventQueryHandler(INewsEventService newsEventService,
                                       IMainMenuService mainMenuService,
                                       ISubMenuService subMenuService,
                                       ISubSubMenuService subSubMenuService,
                                     IPictureService pictureService)
        {
            _newsEventService = newsEventService;
            _pictureService = pictureService;
            _mainMenuService = mainMenuService;
            _subMenuService = subMenuService;
            _subSubMenuService = subSubMenuService;
        }
        public async Task<IList<NewsEventTenderDto>> Handle(GetQueryCMS<NewsEventTenderDto> request, CancellationToken cancellationToken)
        {
            if (!string.IsNullOrEmpty(request.UserId))
            {
                var newsEventTenders = await _newsEventService.GetAll();

                var newsEventDto = newsEventTenders.
                                        Where(m => m.UserId == request.UserId).
                                        Select(m => m.ToModel()).ToList();
                var mainmenu = await _mainMenuService.GetAll();
                var submenu = await _subMenuService.GetAll();
                var subsubmenu = await _subSubMenuService.GetAll();

                try
                {
                    newsEventDto.Where(m=>!string.IsNullOrEmpty(m.SubMenu) && string.IsNullOrEmpty(m.SubSubMenu)).ToList().ForEach(m => m.TypeName = submenu.Where(n => n.Id == m.SubMenu).FirstOrDefault().Name);
                    newsEventDto.Where(m => string.IsNullOrEmpty(m.SubMenu) && string.IsNullOrEmpty(m.SubSubMenu)).ToList().ForEach(m => m.TypeName = mainmenu.Where(n => n.Id == m.Type).FirstOrDefault().MainMenuName);
                    newsEventDto.Where(m => !string.IsNullOrEmpty(m.SubSubMenu)).ToList().ForEach(m => m.TypeName = subsubmenu.Where(n => n.Id == m.SubSubMenu).FirstOrDefault().SubSubMenuName);

                }
                catch (Exception)
                {
                    
                        newsEventDto.Where(m => string.IsNullOrEmpty(m.SubMenuName)).ToList().ForEach(m => m.TypeName = m.Type);

                    
                    //newsEventDto.ForEach(m => m.TypeName =m.Type);

                }
                return newsEventDto;

            }
            else
            {
                return new List<NewsEventTenderDto>();
            }
        }
    }
}

