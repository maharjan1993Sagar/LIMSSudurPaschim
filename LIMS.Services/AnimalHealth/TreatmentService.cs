using LIMS.Domain;
using LIMS.Domain.AnimalHealth;
using LIMS.Domain.Data;
using LIMS.Domain.Services;
using LIMS.Services.Events;
using MediatR;
using MongoDB.Driver.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LIMS.Services.AnimalHealth
{
   public class TreatMentService:ITreatmentService
    {
        private readonly IRepository<TreatMent> _treatMentRepository;
        private readonly IMediator _mediator;
        public TreatMentService(IRepository<TreatMent> treatMentRepository, IMediator mediator)
        {
            _treatMentRepository = treatMentRepository;
            _mediator = mediator;
        }

        public async Task DeleteTreatment(TreatMent treatMent)
        {
            if (treatMent == null)
                throw new ArgumentNullException("TreatMent");
            await _treatMentRepository.DeleteAsync(treatMent);

            //event notification
            await _mediator.EntityDeleted(treatMent);
        }

        public async Task<IPagedList<TreatMent>> GetTreatment(int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = _treatMentRepository.Table;
            return await PagedList<TreatMent>.Create(query, pageIndex, pageSize);
        }


        public Task<TreatMent> GetTreatmentById(string id)
        {
            return _treatMentRepository.GetByIdAsync(id);
        }
        public async Task<IPagedList<TreatMent>> GetTreatmentByAnimalId(string animalId, int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = _treatMentRepository.Table;
            query = query.Where(m => m.AnimalRegistration.Id == animalId);
            return await PagedList<TreatMent>.Create(query, pageIndex, pageSize);
        }

        public async Task InsertTreatment(TreatMent treatMent)
        {
            if (treatMent == null)
                throw new ArgumentNullException("TreatMent");
            await _treatMentRepository.InsertAsync(treatMent);

            //event notification
            await _mediator.EntityInserted(treatMent);
        }

        public async Task UpdateTreatment(TreatMent treatMent)
        {
            if (treatMent == null)
                throw new ArgumentNullException("TreatMent");
            await _treatMentRepository.UpdateAsync(treatMent);

            //event notification
            await _mediator.EntityUpdated(treatMent);
        }


    }
}
