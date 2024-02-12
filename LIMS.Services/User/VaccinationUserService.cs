using LIMS.Domain;
using LIMS.Domain.Data;
using LIMS.Domain.Users;
using LIMS.Services.Events;
using MediatR;
using MongoDB.Driver.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LIMS.Services.User
{
    public class VaccinationUserService:IVaccinationUserService
    {
        private readonly IRepository<VaccinationUser> _vaccinationUserRepository;
        private readonly IMediator _mediator;
        public VaccinationUserService(IRepository<VaccinationUser> vaccinationUserRepository, IMediator mediator)
        {
            _vaccinationUserRepository = vaccinationUserRepository;
            _mediator = mediator;
        }
        public async Task DeleteVaccinationUser(VaccinationUser vaccinationUser)
        {
            if (vaccinationUser == null)
                throw new ArgumentNullException("VaccinationUser");
            await _vaccinationUserRepository.DeleteAsync(vaccinationUser);

            //event notification
            await _mediator.EntityDeleted(vaccinationUser);
        }

        public async Task<IPagedList<VaccinationUser>> GetVaccinationUser(string createdby,int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = _vaccinationUserRepository.Table;
            if (!string.IsNullOrEmpty(createdby))
            {
                query = query.Where(m => m.CreatedBy == createdby);
            }
            return await PagedList<VaccinationUser>.Create(query, pageIndex, pageSize);
        }

        public Task<VaccinationUser> GetVaccinationUserById(string id)
        {
            return _vaccinationUserRepository.GetByIdAsync(id);
        }

        public async Task InsertVaccinationUser(VaccinationUser vaccinationUser)
        {
            if (vaccinationUser == null)
                throw new ArgumentNullException("vaccinationUser");
            await _vaccinationUserRepository.InsertAsync(vaccinationUser);

            //event notification
            await _mediator.EntityInserted(vaccinationUser);
        }

        public async Task UpdateVaccinationUser(VaccinationUser vaccinationUser)
        {
            if (vaccinationUser == null)
                throw new ArgumentNullException("vaccinationUser");
            await _vaccinationUserRepository.UpdateAsync(vaccinationUser);

            //event notification
            await _mediator.EntityUpdated(vaccinationUser);
        }

    }
}
