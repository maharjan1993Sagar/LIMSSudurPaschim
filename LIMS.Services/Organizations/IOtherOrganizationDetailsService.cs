using LIMS.Domain;
using LIMS.Domain.Organizations;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LIMS.Services.Organizations
{
    public interface IOtherOrganizationDetailsService
    {
        Task<OtherOrganizationDetails> GetOtherOrganizationById(string id);

        Task<IPagedList<OtherOrganizationDetails>> GetOtherOrganization(string createdby, int pageIndex = 0, int pageSize = int.MaxValue);
        Task<IPagedList<OtherOrganizationDetails>> GetOtherFilteredOrganization(string createdby,string type, string fiscalyear = "", int pageIndex = 0, int pageSize = int.MaxValue);
        Task<IPagedList<OtherOrganizationDetails>> GetOtherFilteredOrganization(List<string> createdby, string type, string fiscalyear = "", int pageIndex = 0, int pageSize = int.MaxValue);

        Task DeleteOtherOrganization(OtherOrganizationDetails otherOrganizationDetails);

        Task InsertOtherOrganization(OtherOrganizationDetails otherOrganizationDetails);
        Task InsertOtherOrganizationList(List<OtherOrganizationDetails> otherOrganizationDetails);
        Task UpdateOtherOrganization(OtherOrganizationDetails otherOrganizationDetails);
    }
}
