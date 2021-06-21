using LIMS.Domain;
using LIMS.Domain.Data;
using LIMS.Domain.Services;
using LIMS.Services.Events;
using MediatR;
using MongoDB.Driver.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LIMS.Services.AnimalBreeding
{
    public class PregnencyTerminationService:IPregnencyTerminationService
    {
        private readonly IRepository<PregnencyTermination> _pregnencyTerminationRepository;
        private readonly IMediator _mediator;
        public PregnencyTerminationService(IRepository<PregnencyTermination> pregnencyTerminationRepository, IMediator mediator)
        {
            _pregnencyTerminationRepository = pregnencyTerminationRepository;
            _mediator = mediator;
        }

        public async Task DeletePregnencyTermination(PregnencyTermination pregnencyTermination)
        {
            if (pregnencyTermination == null)
                throw new ArgumentNullException("PregnencyTermination");
            await _pregnencyTerminationRepository.DeleteAsync(pregnencyTermination);

            //event notification
            await _mediator.EntityDeleted(pregnencyTermination);
        }

        public async Task<IPagedList<PregnencyTermination>> GetPregnencyTermination(int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = _pregnencyTerminationRepository.Table;
            return await PagedList<PregnencyTermination>.Create(query, pageIndex, pageSize);
        }


        public Task<PregnencyTermination> GetPregnencyTerminationById(string id)
        {
            return _pregnencyTerminationRepository.GetByIdAsync(id);
        }
        public async Task<IPagedList<PregnencyTermination>> GetPregnencyTerminationByAnimalId(string animalId, int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = _pregnencyTerminationRepository.Table;
            query = query.Where(m => m.AnimalRegistration.Id == animalId);
            return await PagedList<PregnencyTermination>.Create(query, pageIndex, pageSize);
        }

        public async Task InsertPregnencyTermination(PregnencyTermination pregnencyTermination)
        {
            if (pregnencyTermination == null)
                throw new ArgumentNullException("PregnencyTermination");
            await _pregnencyTerminationRepository.InsertAsync(pregnencyTermination);

            //event notification
            await _mediator.EntityInserted(pregnencyTermination);
        }

        public async Task UpdatePregnencyTermination(PregnencyTermination pregnencyTermination)
        {
            if (pregnencyTermination == null)
                throw new ArgumentNullException("PregnencyTermination");
            await _pregnencyTerminationRepository.UpdateAsync(pregnencyTermination);

            //event notification
            await _mediator.EntityUpdated(pregnencyTermination);
        }

    }
}
