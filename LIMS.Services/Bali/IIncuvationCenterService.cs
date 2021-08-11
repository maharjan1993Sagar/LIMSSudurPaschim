using LIMS.Domain;
using LIMS.Domain.Bali;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LIMS.Services.Bali
{
    public interface IIncuvationCenterService
    {
        Task<IncubationCenter> GetincuvationCenterById(string id);
        Task<IPagedList<IncubationCenter>> GetincuvationCenter(string createdby, int pageIndex = 0, int pageSize = int.MaxValue, string fiscalyear = "");
        Task<IPagedList<IncubationCenter>> GetincuvationCenter(List<string> createdby, int pageIndex = 0, int pageSize = int.MaxValue, string fiscalyear = "");

        Task DeleteincuvationCenter(IncubationCenter incuvationCenter);

        Task InsertincuvationCenter(IncubationCenter incuvationCenter);
        Task InsertincuvationCenterList(List<IncubationCenter> livestocks);

        Task UpdateincuvationCenter(IncubationCenter incuvationCenter);
        Task UpdateincuvationCenterList(List<IncubationCenter> livestocks);

    }
}
