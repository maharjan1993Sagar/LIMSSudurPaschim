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
    public class SoilService:ISoilService
    {
        private readonly IRepository<Soil> _soilRepository;
        private readonly IMediator _mediator;
        public SoilService(IRepository<Soil> soilRepository, IMediator mediator)
        {
            _soilRepository = soilRepository;
            _mediator = mediator;
        }
        public async Task Deletesoil(Soil soil)
        {
            if (soil == null)
                throw new ArgumentNullException("Soil");

            await _soilRepository.DeleteAsync(soil);

            //event notification
            await _mediator.EntityDeleted(soil);
        }

        public async Task<IPagedList<Soil>> Getsoil(string createdby, int pageIndex = 0, int pageSize = int.MaxValue, string fiscalyear = "")
        {
            var query = _soilRepository.Table;
            query = query.Where(m => m.CreatedBy == createdby);
            //if (!string.IsNullOrEmpty(fiscalyear))
            //{
            //    query = query.Where(
            //      m => m.fis.Id == fiscalyear
            //    );
            //}

            return await PagedList<Soil>.Create(query, pageIndex, pageSize);
        }

        public async Task<IPagedList<Soil>> Getsoil(List<string> createdby, int pageIndex = 0, int pageSize = int.MaxValue, string fiscalyear = "")
        {
            var query = _soilRepository.Table;
            query = query.Where(m => createdby.Contains(m.CreatedBy));
            //if (!string.IsNullOrEmpty(fiscalyear))
            //{
            //    query = query.Where(
            //      m => m.FiscalYear.Id == fiscalyear
            //    );
            //}

            return await PagedList<Soil>.Create(query, pageIndex, pageSize);
        }

        public Task<Soil> GetsoilById(string id)
        {
            return _soilRepository.GetByIdAsync(id);
        }

        public async Task Insertsoil(Soil soil)
        {
            if (soil == null)
                throw new ArgumentNullException("Livestock");

            await _soilRepository.InsertAsync(soil);

            //event notification
            await _mediator.EntityInserted(soil);
        }

        public Task InsertsoilList(List<Soil> soils)
        {
            throw new NotImplementedException();
        }

        public async Task Updatesoil(Soil soil)
        {
            if (soil == null)
                throw new ArgumentNullException("baliregister");

            await _soilRepository.UpdateAsync(soil);

            //event notification
            await _mediator.EntityUpdated(soil);
        }

        public Task UpdatesoilList(List<Soil> soils)
        {
            throw new NotImplementedException();
        }
    }
}
