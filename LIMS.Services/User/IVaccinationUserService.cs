using LIMS.Domain;
using LIMS.Domain.Users;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LIMS.Services.User
{
    public interface IVaccinationUserService
    {
        Task<VaccinationUser> GetVaccinationUserById(string id);

        Task<IPagedList<VaccinationUser>> GetVaccinationUser(string createdby,int pageIndex = 0, int pageSize = int.MaxValue);

        Task DeleteVaccinationUser(VaccinationUser vaccinationUser);

        Task InsertVaccinationUser(VaccinationUser vaccinationUser);

        Task UpdateVaccinationUser(VaccinationUser vaccinationUser);
    }
}
