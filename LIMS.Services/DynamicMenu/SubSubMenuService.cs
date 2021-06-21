using LIMS.Domain;
using LIMS.Domain.DynamicMenu;
using LIMS.Domain.Data;
using LIMS.Services.Events;
using MediatR;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LIMS.Services.DynamicMenu
{
    public class SubSubMenuService : ISubSubMenuService
    {
        private readonly IRepository<SubSubMenu> _menuRepository;
        private readonly IMediator _mediator;
        public SubSubMenuService(IRepository<SubSubMenu> menuRepository, IMediator mediator)
        {
            _menuRepository = menuRepository;
            _mediator = mediator;
        }
        public async Task DeleteSubSubMenu(SubSubMenu menu)
        {
            if (menu == null)
                throw new ArgumentNullException("SubSubMenu");

            await _menuRepository.DeleteAsync(menu);

            //event notification
            await _mediator.EntityDeleted(menu);
        }

        public async Task<IPagedList<SubSubMenu>> GetSubSubMenu(int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = _menuRepository.Table;
            return await PagedList<SubSubMenu>.Create(query, pageIndex, pageSize);
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
