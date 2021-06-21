using LIMS.Domain;
using LIMS.Domain.Professionals;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LIMS.Services.Professionals
{
   public interface IVetGraduateService
    {
        Task<VetGraduate> GetVetGraduateById(string VaccinationId);
        Task<IPagedList<VetGraduate>> GetVetGraduate(
           int pageIndex = 0, int pageSize = int.MaxValue);
        Task DeleteVetGraduate(VetGraduate VetGraduate);


        Task InsertVetGraduate(VetGraduate VetGraduate);


        Task UpdateVetGraduate(VetGraduate VetGraduate);
    }
}
