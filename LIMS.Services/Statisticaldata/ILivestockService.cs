using LIMS.Domain;
using LIMS.Domain.Breed;
using LIMS.Domain.StatisticalData;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LIMS.Services.Statisticaldata
{
    public interface ILivestockService
    {
        Task<Livestock> GetLivestockById(string id);
        Task<IPagedList<Livestock>> GetLivestock(string createdby ,int pageIndex = 0, int pageSize = int.MaxValue, string fiscalyear="");
        Task<IPagedList<Livestock>> GetFilteredLivestock(string createdby,string speciesId,string breedType,string fiscalYearId,string quater,string month,int pageIndex = 0, int pageSize = int.MaxValue);
        Task<IPagedList<Livestock>> GetFilteredLivestock(string createdby, string speciesId, string breedType, string fiscalYearId, string quater,string ward,string month, int pageIndex = 0, int pageSize = int.MaxValue);

        Task DeleteLivestock(Livestock livestock);

        Task InsertLivestock(Livestock livestock);
        Task InsertLivestockList(List<Livestock> livestocks);

        Task UpdateLivestock(Livestock livestock);
        Task UpdateLivestockList(List<Livestock> livestocks);
    }
}
