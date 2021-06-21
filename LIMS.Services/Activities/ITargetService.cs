using LIMS.Domain;
using LIMS.Domain.Activities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LIMS.Services.Activities
{
   public interface ITargetRegisterService
    {
        Task<TargetRegister> GetTargetRegisterById(string id);

        Task<IPagedList<TargetRegister>> GetTargetRegister(int pageIndex = 0, int pageSize = int.MaxValue);
        Task<IPagedList<TargetRegister>> GetFilteredTarget(string createdby,string fiscalyear,int pageIndex = 0, int pageSize = int.MaxValue);

        Task DeleteTargetRegister(TargetRegister targetRegister);

        Task InsertTargetRegister(TargetRegister targetRegister);

        Task UpdateTargetRegister(TargetRegister targetRegister);
        Task InsertTargetRegisterList(List<TargetRegister> targetRegister);
        Task UpdateTargetRegisterList(List<TargetRegister> targetRegister);
    }
}
