using LIMS.Api.DTOs.AINR;
using LIMS.Api.DTOs.GeneralCMS;
using LIMS.Api.Extensions;
using LIMS.Api.Queries.Models.Common;
using LIMS.Core;
using LIMS.Domain.Data;
using LIMS.Services.Ainr;
using LIMS.Services.DynamicMenu;
using MediatR;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LIMS.Api.Queries.Handlers.GeneralCMS
{
    public class DynamicMenuQueryHandler : IRequestHandler<GetQueryCMS<MainMenuDto>, IList<MainMenuDto>>
    {
        private readonly IMainMenuService _mainMenuService;
        private readonly ISubMenuService _subMenuService;
        private readonly ISubSubMenuService _subSubMenuService;
        private readonly IWorkContext _workContext;
        public DynamicMenuQueryHandler(IMainMenuService mainMenuService,
                                        ISubMenuService subMenuService,
                                        ISubSubMenuService subSubMenuService,
                                        IWorkContext workContext)
        {
            _mainMenuService = mainMenuService;
            _subMenuService = subMenuService;
            _subSubMenuService = subSubMenuService;
            _workContext = workContext;
        }
        public async Task<IList<MainMenuDto>> Handle(GetQueryCMS<MainMenuDto> request, CancellationToken cancellationToken)
        {
            if (!string.IsNullOrEmpty(request.UserId))
            {
                var mainMenus = await _mainMenuService.GetByUser(request.UserId);
                var subMenus = await _subMenuService.GetAll();
                var subSubMenus = await _subSubMenuService.GetAll();

                var listMainMenu = new List<MainMenuDto>();
                var listSubMenu = new List<SubMenuDto>();

                foreach (var item in mainMenus)
                {
                    var objMainMenu = new MainMenuDto {
                        HasSubMenu = item.HasSubMenu,
                        IsActive = item.IsActive,
                        MainMenuName = item.MainMenuName,
                        MainMenuId = item.MainMenuId,
                        SerialNo = item.SerialNo,
                        Url = item.Url,
                        UserId = item.UserId
                    };

                    var filSubMenu = subMenus.Where(m => m.MainMenuId == item.Id && m.UserId == request.UserId && m.IsActive).OrderBy(m=>m.SerialNo);

                    if (filSubMenu.Any())
                    {
                        listSubMenu = new List<SubMenuDto>();
                        foreach (var itemSub in filSubMenu)
                        {
                            var objSubMenu = new SubMenuDto {
                                HasSubSubMenu = itemSub.HasSubSubMenu,
                                IsActive = itemSub.IsActive,
                                Name = itemSub.Name,
                                SerialNo = itemSub.SerialNo,
                                Url = itemSub.Url
                            };
                            var filSubSubMenus = subSubMenus.Where(m => m.SubMenuId == itemSub.Id && m.UserId == request.UserId && m.IsActive).OrderBy(m=>m.SerialNo);
                            if (filSubSubMenus.Any())
                            {
                                objSubMenu.SubSubMenus = filSubSubMenus
                                                                        .Select(m => new SubSubMenuDto {
                                                                            IsActive = m.IsActive,
                                                                            SerialNo = m.SerialNo,
                                                                            SubSubMenuName = m.SubSubMenuName,
                                                                            Url = m.Url,
                                                                            UserId = m.UserId
                                                                        }).ToList();
                            }
                            listSubMenu.Add(objSubMenu);

                        }
                        objMainMenu.SubMenus = listSubMenu;
                    }

                    listMainMenu.Add(objMainMenu);

                }
                listMainMenu = listMainMenu.Where(m => m.IsActive).ToList();
                listMainMenu = listMainMenu.OrderBy(m => m.SerialNo).ToList();
              
                return listMainMenu;
            }
            else
            {
                return new List<MainMenuDto>();
            }
           
        }

    }
}
