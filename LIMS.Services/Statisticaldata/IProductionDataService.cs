using LIMS.Domain;
using LIMS.Domain.StatisticalData;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LIMS.Services.Statisticaldata
{
    public interface IProductionionDataService
    {
        Task<Production> GetProductionById(string id);

        Task<IPagedList<Production>> GetProduction(int pageIndex = 0, int pageSize = int.MaxValue);
        Task<IPagedList<Production>> GetProduction(string createdby,int pageIndex = 0, int pageSize = int.MaxValue);
        Task<IPagedList<Production>> GetProductionByKeyword(string createdby,string keyword, int pageIndex = 0, int pageSize = int.MaxValue);

        Task<IPagedList<Production>> GetProduction(List<string> createdby,string type, int pageIndex = 0, int pageSize = int.MaxValue);

        Task<IPagedList<Production>> GetProduction(string createdBy,string fiscalyear,int pageIndex = 0, int pageSize = int.MaxValue);


        Task DeleteProduction(Production production);

        Task InsertProduction(Production production);

        Task UpdateProduction(Production production);
        Task InsertProductionList(List<Production> production);
        Task UpdateProductionList(List<Production> production);
        Task<IPagedList<Production>> GetFilteredProduction( string fiscalyearId, string productiontype, string createdBy,string district,string locallevel,string ward,string farmId, int pageIndex = 0, int pageSize = int.MaxValue);
        Task<IPagedList<Production>> GetFilteredProduction(string createdby, string type, string fiscalYearId, string LocalLevel, string district, int pageIndex = 0, int pageSize = int.MaxValue);
    }
}
