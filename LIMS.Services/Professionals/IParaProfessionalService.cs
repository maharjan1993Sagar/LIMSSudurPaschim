using LIMS.Domain;
using LIMS.Domain.Professionals;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LIMS.Services.Professionals
{
   public  interface IParaProfessionalService
    {
        Task<ParaProfessionals> GetParaProfessionalsById(string VaccinationId);
        Task<IPagedList<ParaProfessionals>> GetParaProfessionals(
           int pageIndex = 0, int pageSize = int.MaxValue);
        Task DeleteParaProfessionals(ParaProfessionals ParaProfessionals);


        Task InsertParaProfessionals(ParaProfessionals ParaProfessionals);


        Task UpdateParaProfessionals(ParaProfessionals ParaProfessionals);
    }
}
