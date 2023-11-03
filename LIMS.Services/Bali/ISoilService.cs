using LIMS.Domain;
using LIMS.Domain.Bali;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LIMS.Services.Bali
{
    public interface ISoilService
    {
        Task<Soil> GetsoilById(string id);
        Task<IPagedList<Soil>> Getsoil(string createdby,  string fiscalYearId, string LocalLevel, string district, int pageIndex = 0, int pageSize = int.MaxValue, string fiscalyear = "");
        Task<IPagedList<Soil>> Getsoil(string createdby, int pageIndex = 0, int pageSize = int.MaxValue, string fiscalyear = "");

        Task<IPagedList<Soil>> Getsoil(List<string> createdby, int pageIndex = 0, int pageSize = int.MaxValue, string fiscalyear = "");

        Task Deletesoil(Soil soil);

        Task Insertsoil(Soil soil);
        Task InsertsoilList(List<Soil> livestocks);

        Task Updatesoil(Soil soil);
        Task UpdatesoilList(List<Soil> livestocks);
    }
}
