using LIMS.Domain;
using LIMS.Domain.AInR;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LIMS.Services.Ainr
{
    public interface IFarmService
    {
        Task<Farm> GetFarmById(string id);

        Task<IPagedList<Farm>> SearchFarm(string keyword = "", int pageIndex = 0, int pageSize = int.MaxValue);

        Task DeleteFarm(Farm farm);

        Task InsertFarm(Farm farm);

        Task UpdateFarm(Farm farm);

        Task DeleteFarmPicture(FarmPicture farmPicture);

        Task InsertFarmPicture(FarmPicture farmPicture);

        Task UpdateFarmPicture(FarmPicture farmPicture);
        Task<PagedList<Farm>> GetPPRsFram(string keyword = "", int pageIndex = 0, int pageSize = int.MaxValue);

        Task<IPagedList<Farm>> GetFarmByLssId(List<string> customerid, string keyword = "", int pageIndex = 0, int pageSize = int.MaxValue);
        Task<PagedList<Farm>> GetFarmByCreatedBy(string createdBy, int pageIndex = 0, int pageSize = int.MaxValue);
        Task DeleteFarmGrass(FarmGrass farmGrass);
        Task InsertFarmGrass(FarmGrass farmGrass);

        Task UpdateFarmGrass(FarmGrass farmGrass);
        Task DeleteFarmShed(FarmShed farmShed);
        Task InsertFarmShed(FarmShed farmShed);

        Task UpdateFarmShed(FarmShed farmShed);
        int GetFarmCountByLssId(List<string> customerid);


    }
}
