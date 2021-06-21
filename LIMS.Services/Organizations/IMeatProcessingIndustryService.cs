using LIMS.Domain;
using LIMS.Domain.Organizations;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LIMS.Services.Organizations
{
    public interface IMeatProcessingIndustryService
    {
        Task<MeatProcesssingIndustries> GetMeatProcessingIndustryById(string id);

        Task<IPagedList<MeatProcesssingIndustries>> GetMeatProcessingIndustry(string createdby,string fiscalyear, int pageIndex = 0, int pageSize = int.MaxValue);

        Task DeleteMeatProcessingIndustry(MeatProcesssingIndustries meatProcesssingIndustries);

        Task InsertMeatProcessingIndustry(MeatProcesssingIndustries meatProcesssingIndustries);
        Task InsertMeatProcessingIndustryList(List<MeatProcesssingIndustries> meatProcesssingIndustries);
        Task UpdateMeatProcessingIndustry(MeatProcesssingIndustries meatProcesssingIndustries);
        Task<IPagedList<MeatProcesssingIndustries>> GetMeatProcessingIndustryByType(string createdby, string type,string fiscalyear, int pageIndex = 0, int pageSize = int.MaxValue);
        Task<IPagedList<MeatProcesssingIndustries>> GetMeatProcessingIndustryByType(List<string> createdby, string type, string fiscalyear, int pageIndex = 0, int pageSize = int.MaxValue);

    }
}
