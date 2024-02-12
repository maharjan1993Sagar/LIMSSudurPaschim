using LIMS.Domain;
using LIMS.Domain.Bali;
using LIMS.Domain.Data;
using LIMS.Services.Events;
using MediatR;
using MongoDB.Driver.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LIMS.Services.Bali
{
    public class PlantSoilManagementService:IPlantSoilManagementService
    {
        private readonly IRepository<PlantSoilManagement> _soilRepository;
        private readonly IMediator _mediator;
        public PlantSoilManagementService(IRepository<PlantSoilManagement> soilRepository, IMediator mediator)
        {
            _soilRepository = soilRepository;
            _mediator = mediator;
        }
        public async Task Deletesoil(PlantSoilManagement soil)
        {
            if (soil == null)
                throw new ArgumentNullException("PlantSoilManagement");

            await _soilRepository.DeleteAsync(soil);

            //event notification
            await _mediator.EntityDeleted(soil);
        }
        public async Task<IPagedList<PlantSoilManagement>> Getsoil(string createdby, int pageIndex = 0, int pageSize = int.MaxValue, string fiscalyear = "")
        {
            var query = _soilRepository.Table;
            if (!string.IsNullOrEmpty(createdby))
            {
                query = query.Where(m => m.CreatedBy == createdby);
            }
            //if (!string.IsNullOrEmpty(fiscalyear))
            //{
            //    query = query.Where(
            //      m => m.fis.Id == fiscalyear
            //    );
            //}

            return await PagedList<PlantSoilManagement>.Create(query, pageIndex, pageSize);
        }

        public async Task<IPagedList<PlantSoilManagement>> Getsoil(string createdby, string fiscalYear = "", string district = "", string locallevel = "", int pageIndex = 0, int pageSize = int.MaxValue, string fiscalyear = "")
        {
            var query = _soilRepository.Table;
            if (!string.IsNullOrEmpty(createdby))
            {
                query = query.Where(m => m.CreatedBy == createdby);
            }
            return await PagedList<PlantSoilManagement>.Create(query, pageIndex, pageSize);
        }

        public async Task<IPagedList<PlantSoilManagement>> Getsoil(List<string> createdby, int pageIndex = 0, int pageSize = int.MaxValue, string fiscalyear = "")
        {
            var query = _soilRepository.Table;
            query = query.Where(m => createdby.Contains(m.CreatedBy));
            //if (!string.IsNullOrEmpty(fiscalyear))
            //{
            //    query = query.Where(
            //      m => m.FiscalYear.Id == fiscalyear
            //    );
            //}

            return await PagedList<PlantSoilManagement>.Create(query, pageIndex, pageSize);
        }

        public Task<PlantSoilManagement> GetsoilById(string id)
        {
            return _soilRepository.GetByIdAsync(id);
        }

        public async Task Insertsoil(PlantSoilManagement soil)
        {
            if (soil == null)
                throw new ArgumentNullException("Livestock");

            await _soilRepository.InsertAsync(soil);

            //event notification
            await _mediator.EntityInserted(soil);
        }

        public Task InsertsoilList(List<PlantSoilManagement> soils)
        {
            throw new NotImplementedException();
        }

        public async Task Updatesoil(PlantSoilManagement soil)
        {
            if (soil == null)
                throw new ArgumentNullException("baliregister");

            await _soilRepository.UpdateAsync(soil);

            //event notification
            await _mediator.EntityUpdated(soil);
        }

        public Task UpdatesoilList(List<PlantSoilManagement> soils)
        {
            throw new NotImplementedException();
        }
    }
}
