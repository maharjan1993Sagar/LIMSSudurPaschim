using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using LIMS.Domain;
using LIMS.Domain.Data;
using LIMS.Domain.Organizations;
using LIMS.Services.Events;
using MediatR;
using MongoDB.Driver.Linq;


namespace LIMS.Services.Organizations
{
    public class OrganizationService:IOrganizationService
    {

        private readonly IRepository<Organization> _organizationRepository;
        private readonly IMediator _mediator;
        public OrganizationService(IRepository<Organization> organizationRepository, IMediator mediator)
        {
            _organizationRepository = organizationRepository;
            _mediator = mediator;
        }
        public async Task DeleteOrganization(Organization organization)
        {
            if (organization == null)
                throw new ArgumentNullException("Organization");
            await _organizationRepository.DeleteAsync(organization);

            //event notification
            await _mediator.EntityDeleted(organization);
        }

        public async Task<IPagedList<Organization>> GetOrganization(string createdby,int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = _organizationRepository.Table;
            if (!string.IsNullOrEmpty(createdby))
            {
                query = query.Where(m => m.CreatedBy == createdby);
            }
            return await PagedList<Organization>.Create(query, pageIndex, pageSize);
        }

        public Task<Organization> GetOrganizationById(string id)
        {
            return _organizationRepository.GetByIdAsync(id);
        }

        public async Task InsertOrganization(Organization organization)
        {
            if (organization == null)
                throw new ArgumentNullException("Organization");
            await _organizationRepository.InsertAsync(organization);

            //event notification
            await _mediator.EntityInserted(organization);
        }

        public async Task UpdateOrganization(Organization organization)
        {
            if (organization == null)
                throw new ArgumentNullException("Organization");
            await _organizationRepository.UpdateAsync(organization);

            //event notification
            await _mediator.EntityUpdated(organization);
        }
     


    }
}
