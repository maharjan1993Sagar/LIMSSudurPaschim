using LIMS.Domain;
using LIMS.Domain.DynamicMenu;
using LIMS.Domain.Data;
using LIMS.Services.Events;
using MediatR;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LIMS.Core;

namespace LIMS.Services.DynamicMenu
{
    public class SubMenuService : ISubMenuService
    {
        private readonly IRepository<SubMenu> _menuRepository;
        private readonly IWorkContext _workContext;
        private readonly IMediator _mediator;
        public SubMenuService(IRepository<SubMenu> menuRepository, IMediator mediator, IWorkContext workContext)
        {
            _menuRepository = menuRepository;
            _mediator = mediator;
            _workContext = workContext;
        }

        public async Task<List<SubMenu>> GetAll()
        {
            var menus = _menuRepository.Table;

            return menus.ToList();
        }
        public async Task DeleteSubMenu(SubMenu menu)
        {
            if (menu == null)
                throw new ArgumentNullException("SubMenu");

            await _menuRepository.DeleteAsync(menu);

            //event notification
            await _mediator.EntityDeleted(menu);
        }

        public async Task<IPagedList<SubMenu>> GetSubMenu(int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = _menuRepository.Table;
            return await PagedList<SubMenu>.Create(query, pageIndex, pageSize);
        }
         public async Task<IPagedList<SubMenu>> GetSubMenuByUser
          (int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var userId = _workContext.CurrentCustomer.Id;
            var query = _menuRepository.Collection;
            var filter = Builders<SubMenu>.Filter.Eq("UserId", userId);
            return await PagedList<SubMenu>.Create(query,filter, pageIndex, pageSize);
        }

        public Task<SubMenu> GetSubMenuById(string Id)
        {
            return _menuRepository.GetByIdAsync(Id);
        }



        public async Task InsertSubMenu(SubMenu menu)
        {
            if (menu == null)
                throw new ArgumentNullException("SubMenu");
            await _menuRepository.InsertAsync(menu);

            //event notification
            //   await _mediator.EntityInserted(breedReg);
        }

        public async Task UpdateSubMenu(SubMenu menu)
        {
            if (menu == null)
                throw new ArgumentNullException("SubMenu");
            await _menuRepository.UpdateAsync(menu);

            //event notification
            //await _mediator.EntityUpdated(breedReg);
        }

        public async Task UpdateSubMenu(List<SubMenu> menu)
        {
            if (menu == null)
                throw new ArgumentNullException("SubMenu");

            await _menuRepository.UpdateAsync(menu);
        }


    }


}
