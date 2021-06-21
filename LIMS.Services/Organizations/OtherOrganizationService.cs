using LIMS.Domain;
using LIMS.Domain.Data;
using LIMS.Domain.Organizations;
using LIMS.Services.Events;
using MediatR;
using MongoDB.Driver.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LIMS.Services.OtherOrganizations
{
    public class OtherOrganizationService:IOtherOrganizationService
    {
        private readonly IRepository<OtherOrganization> _otherOrganizationRepository;
        private readonly IMediator _mediator;
        public OtherOrganizationService(IRepository<OtherOrganization> otherOrganizationRepository, IMediator mediator)
        {
            _otherOrganizationRepository = otherOrganizationRepository;
            _mediator = mediator;
        }
        public async Task DeleteOtherOrganization(OtherOrganization otherOrganization)
        {
            if (otherOrganization == null)
                throw new ArgumentNullException("OtherOrganization");
            await _otherOrganizationRepository.DeleteAsync(otherOrganization);

            //event notification
            await _mediator.EntityDeleted(otherOrganization);
        }

        public async Task<IPagedList<OtherOrganization>> GetOtherOrganization(string createdby, int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = _otherOrganizationRepository.Table;
            query = query.Where(m => m.CreatedBy == createdby);
            return await PagedList<OtherOrganization>.Create(query, pageIndex, pageSize);
        }

        public Task<OtherOrganization> GetOtherOrganizationById(string id)
        {
            return _otherOrganizationRepository.GetByIdAsync(id);
        }

        public async Task InsertOtherOrganization(OtherOrganization otherOrganization)
        {
            if (otherOrganization == null)
                throw new ArgumentNullException("OtherOrganization");
            await _otherOrganizationRepository.InsertAsync(otherOrganization);

            //event notification
            await _mediator.EntityInserted(otherOrganization);
        }

        public async Task UpdateOtherOrganization(OtherOrganization otherOrganization)
        {
            if (otherOrganization == null)
                throw new ArgumentNullException("OtherOrganization");
            await _otherOrganizationRepository.UpdateAsync(otherOrganization);

            //event notification
            await _mediator.EntityUpdated(otherOrganization);
        }
        public async Task<List<OtherOrganization>> GetOtherOrganizationByType(string createdby, string type)
        {
            var query = _otherOrganizationRepository.Table;
            query = query.Where(m => m.CreatedBy == createdby&& m.Type==type);
            return query.ToList();
        }


    }
}
