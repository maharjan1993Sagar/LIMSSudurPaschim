using LIMS.Domain;
using LIMS.Domain.Bali;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LIMS.Services.Bali
{
    public interface ISubSectorService
    {
        Task<SubSector> GetSubSectorById(string id);
        //Task<IPagedList<SubSector>> GetSubSector(string createdby,  string fiscalYearId, string LocalLevel, string district, int pageIndex = 0, int pageSize = int.MaxValue, string fiscalyear = "");
        Task<IPagedList<SubSector>> GetSubSector(string createdby, int pageIndex = 0, int pageSize = int.MaxValue, string fiscalyear = "");
        Task<IPagedList<SubSector>> GetSubSector(int pageIndex = 0, int pageSize = int.MaxValue, string fiscalyear = "");

        Task<IPagedList<SubSector>> GetSubSector(List<string> createdby, int pageIndex = 0, int pageSize = int.MaxValue, string fiscalyear = "");

        Task DeleteSubSector(SubSector SubSector);

        Task InsertSubSector(SubSector SubSector);
        Task InsertSubSectorList(List<SubSector> livestocks);

        Task UpdateSubSector(SubSector SubSector);
        Task UpdateSubSectorList(List<SubSector> livestocks);
    }
}
