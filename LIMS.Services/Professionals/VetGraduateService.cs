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
  public  class VetGraduateService:IVetGraduateService
    {
        private readonly IRepository<VetGraduate> _VetGraduateRepository;
        private readonly IMediator _mediator;
        public VetGraduateService(IRepository<VetGraduate> vaccinerepository, IMediator mediator)
        {
            _VetGraduateRepository = vaccinerepository;
            _mediator = mediator;
        }
        public async Task DeleteVetGraduate(VetGraduate VetGraduate)
        {
            if (VetGraduate == null)
                throw new ArgumentNullException("VetGraduate");

            await _VetGraduateRepository.DeleteAsync(VetGraduate);

            //event notification
            await _mediator.EntityDeleted(VetGraduate);
        }

        public async Task<IPagedList<VetGraduate>> GetVetGraduate(int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = _VetGraduateRepository.Table;


            return await PagedList<VetGraduate>.Create(query, pageIndex, pageSize);
        }

        public Task<VetGraduate> GetVetGraduateById(string VaccinationId)
        {
            return _VetGraduateRepository.GetByIdAsync(VaccinationId);

        }

        public async Task InsertVetGraduate(VetGraduate VetGraduate)
        {
            if (VetGraduate == null)
                throw new ArgumentNullException("VetGraduate");

            await _VetGraduateRepository.InsertAsync(VetGraduate);

            //event notification
            await _mediator.EntityInserted(VetGraduate);
        }

        public async Task UpdateVetGraduate(VetGraduate VetGraduate)
        {
            if (VetGraduate == null)
                throw new ArgumentNullException("poll");

            await _VetGraduateRepository.UpdateAsync(VetGraduate);

            //event notification
            await _mediator.EntityUpdated(VetGraduate);
        }

    }
}
