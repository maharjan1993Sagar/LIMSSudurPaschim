using LIMS.Domain;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using LIMS.Domain.Organizations;
namespace LIMS.Services.Organizations
{
    public interface IOrganizationService
    {
        Task<Organization> GetOrganizationById(string id);

        Task<IPagedList<Organization>> GetOrganization(string createdby,int pageIndex = 0, int pageSize = int.MaxValue);

        Task DeleteOrganization(Organization organization);

        Task InsertOrganization(Organization organization);
        Task UpdateOrganization(Organization organization);
    }
}
