using LIMS.Domain;
using LIMS.Domain.Data;
using LIMS.Domain.Organizations;
using LIMS.Services.Events;
using MediatR;
using MongoDB.Driver.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LIMS.Services.Organizations
{
    public class TechSchoolService:ITechSchoolService
    {
        private readonly IRepository<TechSchool> _techSchoolRepository;
        private readonly IMediator _mediator;
        public TechSchoolService(IRepository<TechSchool> techSchoolRepository, IMediator mediator)
        {
            _techSchoolRepository = techSchoolRepository;
            _mediator = mediator;
        }
        public async Task DeleteTechSchool(TechSchool TechSchool)
        {
            if (TechSchool == null)
                throw new ArgumentNullException("TechSchool");
            await _techSchoolRepository.DeleteAsync(TechSchool);

            //event notification
            await _mediator.EntityDeleted(TechSchool);
        }

        public async Task<IPagedList<TechSchool>> GetTechSchool(string createdby,string fiscalyear, int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = _techSchoolRepository.Table;
            if (!string.IsNullOrEmpty(createdby))
            {
                query = query.Where(m => m.CreatedBy == createdby);
            }
            if (!string.IsNullOrEmpty(fiscalyear))
            {
                query = query.Where(m => m.FiscalYear.Id == fiscalyear);

            }
            return await PagedList<TechSchool>.Create(query, pageIndex, pageSize);
        }
        public async Task<IPagedList<TechSchool>> GetTechSchool(List<string> createdby, string fiscalyear, int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = _techSchoolRepository.Table;
            query = query.Where(m => createdby.Contains(m.CreatedBy) );
            if (!string.IsNullOrEmpty(fiscalyear))
            {
                query = query.Where(m => m.FiscalYear.Id == fiscalyear);

            }
            return await PagedList<TechSchool>.Create(query, pageIndex, pageSize);
        }

        public Task<TechSchool> GetTechSchoolById(string id)
        {
            return _techSchoolRepository.GetByIdAsync(id);
        }

        public async Task InsertTechSchool(TechSchool TechSchool)
        {
            if (TechSchool == null)
                throw new ArgumentNullException("TechSchool");
            await _techSchoolRepository.InsertAsync(TechSchool);

            //event notification
            await _mediator.EntityInserted(TechSchool);
        }
        public async Task InsertTechSchoolList(List<TechSchool> TechSchool)
        {
            if (TechSchool == null)
                throw new ArgumentNullException("TechSchool");
            await _techSchoolRepository.InsertManyAsync(TechSchool);

        }

        public async Task UpdateTechSchool(TechSchool TechSchool)
        {
            if (TechSchool == null)
                throw new ArgumentNullException("TechSchool");
            await _techSchoolRepository.UpdateAsync(TechSchool);

            //event notification
            await _mediator.EntityUpdated(TechSchool);
        }
    }
}
