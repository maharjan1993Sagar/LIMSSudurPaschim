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
    public class MainMenuService : IMainMenuService
    {
        private readonly IRepository<MainMenu> _menuRepository;
        private readonly IWorkContext _workContext;
        private readonly IMediator _mediator;
        public MainMenuService(IRepository<MainMenu> menuRepository, IMediator mediator, IWorkContext workContext)
        {
            _menuRepository = menuRepository;
            _mediator = mediator;
            _workContext = workContext;
        }
        public async Task<List<MainMenu>> GetAll()
        {
            var menus = _menuRepository.Table;

            return menus.ToList();

        }
        public async Task<List<MainMenu>> GetByUser(string userId)
        {

            var menus = await _menuRepository.Collection.FindAsync(
                       Builders<MainMenu>.Filter.Eq(v => v.UserId, userId));
            return menus.ToList();
        }
        public async Task DeleteMainMenu(MainMenu menu)
        {
            if (menu == null)
                throw new ArgumentNullException("MainMenu");

            await _menuRepository.DeleteAsync(menu);

            //event notification
            await _mediator.EntityDeleted(menu);
        }

        public async Task<IPagedList<MainMenu>> GetMainMenu(int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = _menuRepository.Table;
            return await PagedList<MainMenu>.Create(query, pageIndex, pageSize);
        }

        public async Task<IPagedList<MainMenu>> GetMainMenuByUser(int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var userId = _workContext.CurrentCustomer.Id;
            var collection = _menuRepository.Collection;
            var filter = Builders<MainMenu>.Filter.Eq("UserId", userId);
            return await PagedList<MainMenu>.Create(collection, filter, pageIndex, pageSize);
        }

        public Task<MainMenu> GetMainMenuById(string Id)
        {
            return _menuRepository.GetByIdAsync(Id);
        }



        public async Task InsertMainMenu(MainMenu menu)
        {
            if (menu == null)
                throw new ArgumentNullException("MainMenu");
            await _menuRepository.InsertAsync(menu);

            //event notification
            await _mediator.EntityInserted(menu);
        }

        public async Task UpdateMainMenu(MainMenu menu)
        {
            if (menu == null)
                throw new ArgumentNullException("MainMenu");
            await _menuRepository.UpdateAsync(menu);

            //event notification
            await _mediator.EntityUpdated(menu);
        }

        public async Task UpdateMainMenu(List<MainMenu> menu)
        {
            if (menu == null)
                throw new ArgumentNullException("MainMenu");

            await _menuRepository.UpdateAsync(menu);
        }


    }


}
