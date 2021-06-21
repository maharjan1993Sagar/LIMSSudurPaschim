using LIMS.Domain;
using LIMS.Domain.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LIMS.Services.AnimalBreeding
{
    public interface IPregnencyTerminationService
    {
        Task<PregnencyTermination> GetPregnencyTerminationById(string Id);

        Task<IPagedList<PregnencyTermination>> GetPregnencyTermination(int pageIndex = 0, int pageSize = int.MaxValue);

        Task DeletePregnencyTermination(PregnencyTermination pregnencyTermination);

        Task InsertPregnencyTermination(PregnencyTermination pregnencyTermination);

        Task UpdatePregnencyTermination(PregnencyTermination pregnencyTermination);
        Task<IPagedList<PregnencyTermination>> GetPregnencyTerminationByAnimalId(string animalId, int pageIndex = 0, int pageSize = int.MaxValue);
    }
}
