using LIMS.Domain;
using LIMS.Domain.StatisticalData;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LIMS.Services.Statisticaldata
{
    public interface IFishProductionService
    {
        Task<FishProduction> GetFishProductionById(string id);
        Task<IPagedList<FishProduction>> GetFishProduction(string createdby, int pageIndex = 0, int pageSize = int.MaxValue, string fiscalyear = "");
        Task<IPagedList<FishProduction>> GetFilteredFishProduction(string createdby,  string fiscalYearId, string quater, int pageIndex = 0, int pageSize = int.MaxValue);
        Task<IPagedList<FishProduction>> GetFilteredFishProduction(string createdby,  string fiscalYearId, string quater, string natureOfProduction, string localevel = "", string ward = "", string farmid = "", int pageIndex = 0, int pageSize = int.MaxValue);
        Task<IPagedList<FishProduction>> GetFishProduction(List<string> createdby, string fiscalyear, int pageIndex = 0, int pageSize = int.MaxValue);

        Task DeleteFishProduction(FishProduction fishProduction);

        Task InsertFishProduction(FishProduction fishProduction);
        Task InsertFishProductionList(List<FishProduction> fishProductions);

        Task UpdateFishProduction(FishProduction fishProduction);
        Task UpdateFishProductionList(List<FishProduction> fishProductions);

    }
}
