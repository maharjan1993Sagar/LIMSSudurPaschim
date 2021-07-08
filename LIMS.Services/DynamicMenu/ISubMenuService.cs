using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using LIMS.Domain;
using LIMS.Domain.DynamicMenu;
namespace LIMS.Services.DynamicMenu
{
    public interface ISubMenuService
    {
        Task<List<SubMenu>> GetAll();
        Task<SubMenu> GetSubMenuById(string Id);

        Task<IPagedList<SubMenu>> GetSubMenu(int pageIndex = 0, int pageSize = int.MaxValue);
        Task<IPagedList<SubMenu>> GetSubMenuByUser(int pageIndex = 0, int pageSize = int.MaxValue);

        Task DeleteSubMenu(SubMenu menu);

        Task InsertSubMenu(SubMenu menu);

        Task UpdateSubMenu(SubMenu menu);

        Task UpdateSubMenu(List<SubMenu> menu);

     
    }
}
