using LIMS.Domain;
using LIMS.Domain.Breed;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LIMS.Services.Breed
{
    public interface ISubsidyTypeService
    {
        Task<SubsidyType> GetSubsidyTypeById(string id);

        Task<IPagedList<SubsidyType>> GetSubsidyType(int pageIndex = 0, int pageSize = int.MaxValue);

        Task DeleteSubsidyType(SubsidyType SubsidyType);

        Task InsertSubsidyType(SubsidyType SubsidyType);

        Task UpdateSubsidyType(SubsidyType SubsidyType);
    }
}
