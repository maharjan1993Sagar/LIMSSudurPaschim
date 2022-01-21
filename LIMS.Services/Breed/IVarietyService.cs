using LIMS.Domain;
using LIMS.Domain.Breed;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LIMS.Services.Breed
{
    public interface IVarietyService
    {

        Task<CropsProduction> GetBreedById(string Id);

        Task<IPagedList<CropsProduction>> GetBreed(string createdby, int pageIndex = 0, int pageSize = int.MaxValue);

        Task DeleteBreed(CropsProduction CropsProduction);

        Task InsertBreed(CropsProduction CropsProduction);
        Task InsertBreed(List<CropsProduction> CropsProduction);


        Task UpdateBreed(CropsProduction CropsProduction);

        Task UpdateBreed(List<CropsProduction> Varietys);
        Task<IPagedList<CropsProduction>> GetFilteredProduction(string createdby, string type, string fiscalYearId, string LocalLevel, string district, int pageIndex = 0, int pageSize = int.MaxValue);
        Task<PagedList<CropsProduction>> GetFilteredLivestock(string createdby, string district, string locallevel, string fiscalyear,int ward, int pageIndex = 0, int pageSize = int.MaxValue);
    }
}
