using LIMS.Domain;
using LIMS.Domain.Organizations;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LIMS.Services.Organizations
{
    public interface ITechSchoolService
    {
        Task<TechSchool> GetTechSchoolById(string id);

        Task<IPagedList<TechSchool>> GetTechSchool(string createdby,string fiscalyear, int pageIndex = 0, int pageSize = int.MaxValue);
        Task<IPagedList<TechSchool>> GetTechSchool(List<string> createdby, string fiscalyear, int pageIndex = 0, int pageSize = int.MaxValue);

        Task DeleteTechSchool(TechSchool techSchool);

        Task InsertTechSchool(TechSchool techSchool);
        Task InsertTechSchoolList(List<TechSchool> techSchoolAndShop);

        Task UpdateTechSchool(TechSchool techSchool);
    }
}
