using LIMS.Domain;
using LIMS.Domain.AnimalHealth;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LIMS.Services.AnimalHealth
{
   public interface ITreatmentService
    {
        Task<TreatMent> GetTreatmentById(string Id);

        Task<IPagedList<TreatMent>> GetTreatment(int pageIndex = 0, int pageSize = int.MaxValue);
        Task<IPagedList<TreatMent>> GetTreatmentByAnimalId(string animalId, int pageIndex = 0, int pageSize = int.MaxValue);

        Task DeleteTreatment(TreatMent treatment);

        Task InsertTreatment(TreatMent vacination);

        Task UpdateTreatment(TreatMent vacination);
    }
}
