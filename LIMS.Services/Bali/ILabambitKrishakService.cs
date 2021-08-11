using LIMS.Domain;
using LIMS.Domain.Bali;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LIMS.Services.Bali
{
    public interface ILabambitKrishakService
    {
        Task<LabambitKrishakHaru> GetLabambitKrishakHaruById(string id);
        Task<IPagedList<LabambitKrishakHaru>> GetLabambitKrishakHaru(string createdby, int pageIndex = 0, int pageSize = int.MaxValue, string fiscalyear = "");
        Task<IPagedList<LabambitKrishakHaru>> GetLabambitKrishakHaru(List<string> createdby, int pageIndex = 0, int pageSize = int.MaxValue, string fiscalyear = "");

        Task DeleteLabambitKrishakHaru(LabambitKrishakHaru LabambitKrishakHaru);

        Task InsertLabambitKrishakHaru(LabambitKrishakHaru LabambitKrishakHaru);
        Task InsertLabambitKrishakHaruList(List<LabambitKrishakHaru> livestocks);

        Task UpdateLabambitKrishakHaru(LabambitKrishakHaru LabambitKrishakHaru);
        Task UpdateLabambitKrishakHaruList(List<LabambitKrishakHaru> livestocks);
    }
}
