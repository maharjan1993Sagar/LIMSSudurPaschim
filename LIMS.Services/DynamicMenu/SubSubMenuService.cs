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
    public class SubSubMenuService : ISubSubMenuService
    {
        private readonly IRepository<SubSubMenu> _menuRepository;
        private readonly IMediator _mediator;
        private readonly IWorkContext _workContext;
        public SubSubMenuService(IRepository<SubSubMenu> menuRepository, IMediator mediator, IWorkContext workContext)
        {
            _menuRepository = menuRepository;
            _mediator = mediator;
            _workContext = workContext;
        }
        public async Task DeleteSubSubMenu(SubSubMenu menu)
        {
            if (menu == null)
                throw new ArgumentNullException("SubSubMenu");

            await _menuRepository.DeleteAsync(menu);

            //event notification
            await _mediator.EntityDeleted(menu);
        }

        public async Task<List<SubSubMenu>> GetAll()
        {
            var subMenus = _menuRepository.Table;
            return subMenus.ToList();

        }
        public async Task<IPagedList<SubSubMenu>> GetSubSubMenu(int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = _menuRepository.Table;
            return await PagedList<SubSubMenu>.Create(query, pageIndex, pageSize);
        }

        public async Task<IPagedList<SubSubMenu>> GetSubSubMenuByUser(int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var userId = _workContext.CurrentCustomer.Id;
            var query = _menuRepository.Collection;
            var filter = Builders<SubSubMenu>.Filter.Eq("UserId", userId);
            return await PagedList<SubSubMenu>.Create(query, filter, pageIndex, pageSize);
        }

        public Task<SubSubMenu> GetSubSubMenuById(string Id)

        {
            return _menuRepository.GetByIdAsync(Id);
        }



        public async Task InsertSubSubMenu(SubSubMenu menu)
        {
            if (menu == null)
                throw new ArgumentNullException("SubSubMenu");
            await _menuRepository.InsertAsync(menu);

            //event notification
            //   await _mediator.EntityInserted(breedReg);
        }

        public async Task UpdateSubSubMenu(SubSubMenu menu)
        {
            if (menu == null)
                throw new ArgumentNullException("SubSubMenu");
            await _menuRepository.UpdateAsync(menu);

            //event notification
            //await _mediator.EntityUpdated(breedReg);
        }

        public async Task UpdateSubSubMenu(List<SubSubMenu> menu)
        {
            if (menu == null)
                throw new ArgumentNullException("SubSubMenu");

            await _menuRepository.UpdateAsync(menu);
        }


    }


}
