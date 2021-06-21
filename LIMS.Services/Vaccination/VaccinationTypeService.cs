using LIMS.Domain;
using LIMS.Domain.Data;
using LIMS.Domain.Vaccination;
using LIMS.Services.Events;
using MediatR;
using MongoDB.Driver.Linq;
using System;
using System.Threading.Tasks;

namespace LIMS.Services.Vaccination
{
    public partial class VaccinationTypeService : IVaccinationTypeService
    {
        private readonly IRepository<Domain.Vaccination.VaccinationType> _vaccinationTypeRepository;
        private readonly IMediator _mediator;

        public VaccinationTypeService(IRepository<Domain.Vaccination.VaccinationType> vaccineRepository, IMediator mediator)
        {
            _vaccinationTypeRepository = vaccineRepository;
            _mediator = mediator;
        }

        public async Task DeleteVaccinationType(Domain.Vaccination.VaccinationType vaccinationType)
        {
            if (vaccinationType == null)
                throw new ArgumentNullException("VaccinationType");
            await _vaccinationTypeRepository.DeleteAsync(vaccinationType);

            //event notification
            await _mediator.EntityDeleted(vaccinationType);
        }

        public async Task<IPagedList<Domain.Vaccination.VaccinationType>> GetVaccination(string createdBy="",int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = _vaccinationTypeRepository.Table;
            if (!string.IsNullOrEmpty(createdBy))
            {
                query = query.Where(m => m.CreatedBy == createdBy);
            }
            return await PagedList<Domain.Vaccination.VaccinationType>.Create(query, pageIndex, pageSize);
        }

        public Task<Domain.Vaccination.VaccinationType> GetVaccinationTypeById(string vaccinationId)
        {
            return _vaccinationTypeRepository.GetByIdAsync(vaccinationId);
        }

        public async Task InsertVaccinationType(Domain.Vaccination.VaccinationType vaccinationType)
        {
            if (vaccinationType == null)
                throw new ArgumentNullException("VaccinationType");
            await _vaccinationTypeRepository.InsertAsync(vaccinationType);

            //event notification
            await _mediator.EntityInserted(vaccinationType);
        }

        public async Task UpdateVaccinationType(Domain.Vaccination.VaccinationType vaccinationType)
        {
            if (vaccinationType == null)
                throw new ArgumentNullException("VaccinationType");
            await _vaccinationTypeRepository.UpdateAsync(vaccinationType);

            //event notification
            await _mediator.EntityUpdated(vaccinationType);
        }
        public async Task<IPagedList<VaccinationType>> FiletrVaccinationType(string type, int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = _vaccinationTypeRepository.Table;
           
                query = query.Where(m => m.Type == type);
          
            return await PagedList<Domain.Vaccination.VaccinationType>.Create(query, pageIndex, pageSize);
        }
    }
}
