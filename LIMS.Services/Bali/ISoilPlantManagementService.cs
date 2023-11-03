using LIMS.Domain;
using LIMS.Domain.Bali;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LIMS.Services.Bali
{
    public interface IPlantSoilManagementService
    {

        Task<PlantSoilManagement> GetsoilById(string id);
        Task<IPagedList<PlantSoilManagement>> Getsoil(string createdby, string fiscalYearId, string LocalLevel, string district, int pageIndex = 0, int pageSize = int.MaxValue, string fiscalyear = "");
        Task<IPagedList<PlantSoilManagement>> Getsoil(string createdby, int pageIndex = 0, int pageSize = int.MaxValue, string fiscalyear = "");

        Task<IPagedList<PlantSoilManagement>> Getsoil(List<string> createdby, int pageIndex = 0, int pageSize = int.MaxValue, string fiscalyear = "");

        Task Deletesoil(PlantSoilManagement soil);

        Task Insertsoil(PlantSoilManagement soil);
        Task InsertsoilList(List<PlantSoilManagement> livestocks);

        Task Updatesoil(PlantSoilManagement soil);
        Task UpdatesoilList(List<PlantSoilManagement> livestocks);
    }
}
