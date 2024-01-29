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
    public class AnugamanService:IAnugamanService
    {
        private readonly IRepository<Anugaman> _AnugamanRepository;
        private readonly IMediator _mediator;
        public AnugamanService(IRepository<Anugaman> AnugamanRepository, IMediator mediator)
        {
            _AnugamanRepository = AnugamanRepository;
            _mediator = mediator;
        }
        public async Task DeleteAnugaman(Anugaman Anugaman)
        {
            if (Anugaman == null)
                throw new ArgumentNullException("Anugaman");

            await _AnugamanRepository.DeleteAsync(Anugaman);

            //event notification
            await _mediator.EntityDeleted(Anugaman);
        }
        public async Task<IPagedList<Anugaman>> GetAnugaman(string createdby, int pageIndex = 0, int pageSize = int.MaxValue, string fiscalyear = "")
        {
            var query = _AnugamanRepository.Table;
            query = query.Where(m => m.CreatedBy == createdby);
            //if (!string.IsNullOrEmpty(fiscalyear))
            //{
            //    query = query.Where(
            //      m => m.fis.Id == fiscalyear
            //    );
            //}

            return await PagedList<Anugaman>.Create(query, pageIndex, pageSize);
        }

        public async Task<IPagedList<Anugaman>> GetAnugaman(string createdby, string fiscalYear = "", string district = "", string locallevel = "", int pageIndex = 0, int pageSize = int.MaxValue, string fiscalyear = "")
        {
            var query = _AnugamanRepository.Table;
            if(!String.IsNullOrEmpty(createdby))
            {
                query = query.Where(m => m.CreatedBy == createdby);
            }
            if (!String.IsNullOrEmpty(fiscalyear))
            {
                query = query.Where(m => m.FiscalYearId == fiscalYear);
            }
            if (!string.IsNullOrEmpty(district))
            {
                query = query.Where(
                  m => m.District == district
                );
            }
            if (!string.IsNullOrEmpty(locallevel))
            {
                query = query.Where(
                  m => m.LocalLevel == locallevel
                );
            }

            return await PagedList<Anugaman>.Create(query, pageIndex, pageSize);
        }

        public async Task<IPagedList<Anugaman>> GetAnugaman(List<string> createdby, int pageIndex = 0, int pageSize = int.MaxValue, string fiscalyear = "")
        {
            var query = _AnugamanRepository.Table;
            query = query.Where(m => createdby.Contains(m.CreatedBy));
            //if (!string.IsNullOrEmpty(fiscalyear))
            //{
            //    query = query.Where(
            //      m => m.FiscalYear.Id == fiscalyear
            //    );
            //}

            return await PagedList<Anugaman>.Create(query, pageIndex, pageSize);
        }

        public Task<Anugaman> GetAnugamanById(string id)
        {
            return _AnugamanRepository.GetByIdAsync(id);
        }

        public async Task InsertAnugaman(Anugaman Anugaman)
        {
            if (Anugaman == null)
                throw new ArgumentNullException("Livestock");

            await _AnugamanRepository.InsertAsync(Anugaman);

            //event notification
            await _mediator.EntityInserted(Anugaman);
        }

        public Task InsertAnugamanList(List<Anugaman> Anugamans)
        {
            throw new NotImplementedException();
        }

        public async Task UpdateAnugaman(Anugaman Anugaman)
        {
            if (Anugaman == null)
                throw new ArgumentNullException("baliregister");

            await _AnugamanRepository.UpdateAsync(Anugaman);

            //event notification
            await _mediator.EntityUpdated(Anugaman);
        }

        public Task UpdateAnugamanList(List<Anugaman> Anugamans)
        {
            throw new NotImplementedException();
        }
    }
}
