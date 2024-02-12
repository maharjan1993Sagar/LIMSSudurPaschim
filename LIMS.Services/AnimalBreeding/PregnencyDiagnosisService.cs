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
    public class PregnencyDiagnosisService : IPregnencyDiagnosisService
    {
        private readonly IRepository<PregnencyDiagnosis> _pregnencyDiagnosisRepository;
        private readonly IMediator _mediator;
        public PregnencyDiagnosisService(IRepository<PregnencyDiagnosis> pregnencyDiagnosisRepository, IMediator mediator)
        {
            _pregnencyDiagnosisRepository = pregnencyDiagnosisRepository;
            _mediator = mediator;
        }

        public async Task DeletePregnencyDiagnosis(PregnencyDiagnosis pregnencyDiagnosis)
        {
            if (pregnencyDiagnosis == null)
                throw new ArgumentNullException("PregnencyDiagnosis");
            await _pregnencyDiagnosisRepository.DeleteAsync(pregnencyDiagnosis);

            //event notification
            await _mediator.EntityDeleted(pregnencyDiagnosis);
        }

        public async Task<IPagedList<PregnencyDiagnosis>> GetPregnencyDiagnosis(int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = _pregnencyDiagnosisRepository.Table;
            return await PagedList<PregnencyDiagnosis>.Create(query, pageIndex, pageSize);
        }
        public async Task<IPagedList<PregnencyDiagnosis>> GetPregnencyDiagnosis(string createdby,int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = _pregnencyDiagnosisRepository.Table;
            if (!string.IsNullOrEmpty(createdby))
            {
                query = query.Where(m => m.CreatedBy == createdby);
            }
            return await PagedList<PregnencyDiagnosis>.Create(query, pageIndex, pageSize);
        }
        public async Task<IPagedList<PregnencyDiagnosis>> GetPregnencyDiagnosis(List<string> createdby, int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = _pregnencyDiagnosisRepository.Table;
            query = query.Where(m => createdby.Contains(m.CreatedBy));
            return await PagedList<PregnencyDiagnosis>.Create(query, pageIndex, pageSize);
        }


        public Task<PregnencyDiagnosis> GetPregnencyDiagnosisById(string id)
        {
            return _pregnencyDiagnosisRepository.GetByIdAsync(id);
        }
        public async Task<IPagedList<PregnencyDiagnosis>> GetPregnencyDiagnosisByAnimalId(string animalId, int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = _pregnencyDiagnosisRepository.Table;
            query = query.Where(m => m.AnimalRegistration.Id == animalId);
            return await PagedList<PregnencyDiagnosis>.Create(query, pageIndex, pageSize);
        }

        public async Task InsertPregnencyDiagnosis(PregnencyDiagnosis pregnencyDiagnosis)
        {
            if (pregnencyDiagnosis == null)
                throw new ArgumentNullException("PregnencyDiagnosis");
            await _pregnencyDiagnosisRepository.InsertAsync(pregnencyDiagnosis);

            //event notification
            await _mediator.EntityInserted(pregnencyDiagnosis);
        }

        public async Task UpdatePregnencyDiagnosis(PregnencyDiagnosis pregnencyDiagnosis)
        {
            if (pregnencyDiagnosis == null)
                throw new ArgumentNullException("PregnencyDiagnosis");
            await _pregnencyDiagnosisRepository.UpdateAsync(pregnencyDiagnosis);

            //event notification
            await _mediator.EntityUpdated(pregnencyDiagnosis);
        }

    }
}
