using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using LIMS.Domain;
using LIMS.Domain.DynamicMenu;
namespace LIMS.Services.DynamicMenu
{
    public interface IMainMenuService
    {
        Task<MainMenu> GetMainMenuById(string Id);

        Task<IPagedList<MainMenu>> GetMainMenu(int pageIndex = 0, int pageSize = int.MaxValue);

        Task<List<MainMenu>> GetAll();

        Task DeleteMainMenu(MainMenu menu);

        Task InsertMainMenu(MainMenu menu);

        Task UpdateMainMenu(MainMenu menu);

        Task UpdateMainMenu(List<MainMenu> menu);

     
    }
}
