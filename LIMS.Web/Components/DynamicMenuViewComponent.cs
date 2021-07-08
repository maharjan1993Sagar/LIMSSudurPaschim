using LIMS.Core;
using LIMS.Framework.Components;
using LIMS.Services.DynamicMenu;
using LIMS.Web.Models.DynamicMenu;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Web.Components
{
    public class DynamicMenuViewComponent : BaseViewComponent
    {
        string userId = "600fd784c61fcbfa6ca65586";
        private readonly IMediator _mediator;
        private readonly IWorkContext _workContext;
        private readonly IMainMenuService _mainMenu;
        private readonly ISubMenuService _subMenu;
        private readonly ISubSubMenuService _subSubMenu;
        public DynamicMenuViewComponent(IMediator mediator, IWorkContext workContext,
                                        IMainMenuService mainMenu, ISubMenuService subMenu, ISubSubMenuService subSubMenu)
        {
            _mediator = mediator;
            _workContext = workContext;
            _mainMenu = mainMenu;
            _subMenu = subMenu;
            _subSubMenu = subSubMenu;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var mainMenus = await _mainMenu.GetByUser(userId);
            var subMenus = await _subMenu.GetAll();
            var subSubMenus = await _subSubMenu.GetAll();

            var listMainMenu = new List<MainMenuModel>();
            var listSubMenu = new List<SubMenuModel>();

            foreach (var item in mainMenus.ToList())
            {
                var objMainMenu = new MainMenuModel {
                    HasSubMenu = item.HasSubMenu,
                    IsActive = item.IsActive,
                    MainMenuName = item.MainMenuName,
                    MainMenuId = item.MainMenuId,
                    SerialNo = item.SerialNo,
                    Url = item.Url,
                    UserId = item.UserId
                };

                var filSubMenu = subMenus.Where(m => m.MainMenuId == item.Id&& m.UserId==userId);

                if (filSubMenu.Any())
                {
                    listSubMenu = new List<SubMenuModel>();
                    foreach (var itemSub in filSubMenu)
                    {
                        var objSubMenu = new SubMenuModel {
                            HasSubSubMenu = itemSub.HasSubSubMenu,
                            IsActive = itemSub.IsActive,
                            Name = itemSub.Name,
                            SerialNo = itemSub.SerialNo,
                            Url = itemSub.Url
                        };
                        var filSubSubMenus = subSubMenus.Where(m => m.SubMenuId == itemSub.Id&&m.UserId==userId);
                        if (filSubSubMenus.Any())
                        {
                            objSubMenu.SubSubMenus = filSubSubMenus
                                                                    .Select(m => new SubSubMenuModel {
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
            return View(listMainMenu);
        }
    }
}
