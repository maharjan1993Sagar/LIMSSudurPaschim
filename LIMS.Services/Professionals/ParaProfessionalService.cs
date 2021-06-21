using LIMS.Domain;
using LIMS.Domain.Data;
using LIMS.Domain.Professionals;
using LIMS.Services.Events;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LIMS.Services.Professionals
{
  public  class ParaProfessionalService:IParaProfessionalService
    {
        private readonly IRepository<ParaProfessionals> _ParaProfessionalsRepository;
        private readonly IMediator _mediator;
        public ParaProfessionalService(IRepository<ParaProfessionals> vaccinerepository, IMediator mediator)
        {
            _ParaProfessionalsRepository = vaccinerepository;
            _mediator = mediator;
        }
        public async Task DeleteParaProfessionals(ParaProfessionals ParaProfessionals)
        {
            if (ParaProfessionals == null)
                throw new ArgumentNullException("Paraprofessionals");

            await _ParaProfessionalsRepository.DeleteAsync(ParaProfessionals);

            //event notification
            await _mediator.EntityDeleted(ParaProfessionals);
        }

        public async Task<IPagedList<ParaProfessionals>> GetParaProfessionals(int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = _ParaProfessionalsRepository.Table;


            return await PagedList<ParaProfessionals>.Create(query, pageIndex, pageSize);
        }

        public Task<ParaProfessionals> GetParaProfessionalsById(string VaccinationId)
        {
            return _ParaProfessionalsRepository.GetByIdAsync(VaccinationId);

        }

        public async Task InsertParaProfessionals(ParaProfessionals ParaProfessionals)
        {
            if (ParaProfessionals == null)
                throw new ArgumentNullException("ParaProfessionals");

            await _ParaProfessionalsRepository.InsertAsync(ParaProfessionals);

            //event notification
            await _mediator.EntityInserted(ParaProfessionals);
        }

        public async Task UpdateParaProfessionals(ParaProfessionals ParaProfessionals)
        {
            if (ParaProfessionals == null)
                throw new ArgumentNullException("poll");

            await _ParaProfessionalsRepository.UpdateAsync(ParaProfessionals);

            //event notification
            await _mediator.EntityUpdated(ParaProfessionals);
        }

    }
}
