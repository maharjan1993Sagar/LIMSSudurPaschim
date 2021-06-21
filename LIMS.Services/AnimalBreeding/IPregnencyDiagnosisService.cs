using LIMS.Domain;
using LIMS.Domain.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LIMS.Services.AnimalBreeding
{
    public interface IPregnencyDiagnosisService
    {
        Task<PregnencyDiagnosis> GetPregnencyDiagnosisById(string Id);

        Task<IPagedList<PregnencyDiagnosis>> GetPregnencyDiagnosis(int pageIndex = 0, int pageSize = int.MaxValue);
        Task<IPagedList<PregnencyDiagnosis>> GetPregnencyDiagnosis(string createdby,int pageIndex = 0, int pageSize = int.MaxValue);
        Task<IPagedList<PregnencyDiagnosis>> GetPregnencyDiagnosis(List<string> createdby, int pageIndex = 0, int pageSize = int.MaxValue);

        Task<IPagedList<PregnencyDiagnosis>> GetPregnencyDiagnosisByAnimalId(string animalId, int pageIndex = 0, int pageSize = int.MaxValue);

        Task DeletePregnencyDiagnosis(PregnencyDiagnosis pregnencyDiagnosis);

        Task InsertPregnencyDiagnosis(PregnencyDiagnosis pregnencyDiagnosis);

        Task UpdatePregnencyDiagnosis(PregnencyDiagnosis pregnencyDiagnosis);
    }
}
