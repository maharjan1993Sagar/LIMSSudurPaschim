using LIMS.Domain;
using LIMS.Domain.AInR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LIMS.Services.Ainr
{
    public interface IOwnerService
    {
        Task<Owner> GetOwnerById(string id);

        Task<IPagedList<Owner>> GetOwner(int pageIndex = 0, int pageSize = int.MaxValue);
        Task<IPagedList<Owner>> GetOwner(string farmid,int pageIndex = 0, int pageSize = int.MaxValue);

        Task DeleteOwner(Owner owner);

        Task InsertOwner(Owner owner);

        Task UpdateOwner(Owner owner);
        Task<IPagedList<Owner>> GetOwnerByFarmId(string farmId, int pageIndex = 0, int pageSize = int.MaxValue);
    }
}
