using LIMS.Domain;
using LIMS.Domain.BasicSetup;
using LIMS.Domain.BesicSetup;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LIMS.Services.Basic
{
    public interface IDiseaseService
    {
        Task<Disease> GetDiseaseById(string Id);
        Task<IPagedList<Disease>> GetDisease(
           int pageIndex = 0, int pageSize = int.MaxValue);
        Task DeleteDisease(Disease disease);


        Task InsertDisease(Disease disease);


        Task UpdateDisease(Disease disease);
    }
}
