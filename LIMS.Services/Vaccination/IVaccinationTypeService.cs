using LIMS.Domain;
using LIMS.Domain.Vaccination;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LIMS.Services.Vaccination
{
    public partial interface IVaccinationTypeService
    {
        Task<VaccinationType> GetVaccinationTypeById(string vaccinationId);

        Task<IPagedList<VaccinationType>> GetVaccination(string createdBy="",int pageIndex = 0, int pageSize = int.MaxValue);

        Task DeleteVaccinationType(VaccinationType vaccinationType);

        Task InsertVaccinationType(VaccinationType vaccinationType);

        Task UpdateVaccinationType(VaccinationType vaccinationType);
        Task<IPagedList<VaccinationType>> FiletrVaccinationType(string type, int pageIndex = 0, int pageSize = int.MaxValue);
    }
}
