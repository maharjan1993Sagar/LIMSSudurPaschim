using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using LIMS.Domain;
using LIMS.Domain.DynamicMenu;
namespace LIMS.Services.DynamicMenu
{
    public interface ISubSubMenuService
    {
        Task<SubSubMenu> GetSubSubMenuById(string Id);

        Task<IPagedList<SubSubMenu>> GetSubSubMenu(int pageIndex = 0, int pageSize = int.MaxValue);

        Task DeleteSubSubMenu(SubSubMenu menu);

        Task InsertSubSubMenu(SubSubMenu menu);

        Task UpdateSubSubMenu(SubSubMenu menu);

        Task UpdateSubSubMenu(List<SubSubMenu> menu);

     
    }
}
