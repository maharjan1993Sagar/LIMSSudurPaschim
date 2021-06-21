using LIMS.Domain;
using LIMS.Domain.Organizations;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LIMS.Services.OtherOrganizations
{
   public interface IOtherOrganizationService
    {
        Task<OtherOrganization> GetOtherOrganizationById(string id);

        Task<IPagedList<OtherOrganization>> GetOtherOrganization(string createdby, int pageIndex = 0, int pageSize = int.MaxValue);

        Task DeleteOtherOrganization(OtherOrganization otherOrganization);

        Task InsertOtherOrganization(OtherOrganization otherOrganization);
        Task UpdateOtherOrganization(OtherOrganization otherOrganization);
        Task<List<OtherOrganization>> GetOtherOrganizationByType(string createdby,string type);

    }
}
