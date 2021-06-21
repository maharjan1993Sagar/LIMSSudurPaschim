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
    public class OtherOrganizationDetailsService: IOtherOrganizationDetailsService
    {
        private readonly IRepository<OtherOrganizationDetails> _otherOrganizationRepository;
        private readonly IMediator _mediator;
        public OtherOrganizationDetailsService(IRepository<OtherOrganizationDetails> otherOrganizationRepository, IMediator mediator)
        {
            _otherOrganizationRepository = otherOrganizationRepository;
            _mediator = mediator;
        }
        public async Task DeleteOtherOrganization(OtherOrganizationDetails OtherOrganizationDetails)
        {
            if (OtherOrganizationDetails == null)
                throw new ArgumentNullException("OtherOrganizationDetails");
            await _otherOrganizationRepository.DeleteAsync(OtherOrganizationDetails);

            //event notification
            await _mediator.EntityDeleted(OtherOrganizationDetails);
        }

        public async Task<IPagedList<OtherOrganizationDetails>> GetOtherOrganization(string createdby, int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = _otherOrganizationRepository.Table;
            query = query.Where(m => m.CreatedBy == createdby);
            return await PagedList<OtherOrganizationDetails>.Create(query, pageIndex, pageSize);
        }
        public async Task<IPagedList<OtherOrganizationDetails>> GetOtherFilteredOrganization(string createdby,string type, string fiscalyear = "",int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = _otherOrganizationRepository.Table;
            query = query.Where(m => m.CreatedBy == createdby && m.OtherOrganization.Type==type);
            if(!string.IsNullOrEmpty(fiscalyear))
            {
                query = query.Where(m => m.FiscalYearId == fiscalyear);
            }
            return await PagedList<OtherOrganizationDetails>.Create(query, pageIndex, pageSize);
        }
        public async Task<IPagedList<OtherOrganizationDetails>> GetOtherFilteredOrganization(List<string> createdby, string type, string fiscalyear = "", int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = _otherOrganizationRepository.Table;
            query = query.Where(m =>createdby.Contains(m.CreatedBy) && m.OtherOrganization.Type == type);
            if (!string.IsNullOrEmpty(fiscalyear))
            {
                query = query.Where(m => m.FiscalYearId == fiscalyear);
            }
            return await PagedList<OtherOrganizationDetails>.Create(query, pageIndex, pageSize);
        }

        public Task<OtherOrganizationDetails> GetOtherOrganizationById(string id)
        {
            return _otherOrganizationRepository.GetByIdAsync(id);
        }

        public async Task InsertOtherOrganization(OtherOrganizationDetails OtherOrganizationDetails)
        {
            if (OtherOrganizationDetails == null)
                throw new ArgumentNullException("OtherOrganizationDetails");
            await _otherOrganizationRepository.InsertAsync(OtherOrganizationDetails);

            //event notification
            await _mediator.EntityInserted(OtherOrganizationDetails);
        }
        public async Task InsertOtherOrganizationList(List<OtherOrganizationDetails> OtherOrganizationDetails)
        {
            if (OtherOrganizationDetails == null)
                throw new ArgumentNullException("OtherOrganizationDetails");
            await _otherOrganizationRepository.InsertManyAsync(OtherOrganizationDetails);

        }

        public async Task UpdateOtherOrganization(OtherOrganizationDetails OtherOrganizationDetails)
        {
            if (OtherOrganizationDetails == null)
                throw new ArgumentNullException("OtherOrganizationDetails");
            await _otherOrganizationRepository.UpdateAsync(OtherOrganizationDetails);

            //event notification
            await _mediator.EntityUpdated(OtherOrganizationDetails);
        }

    }
}
