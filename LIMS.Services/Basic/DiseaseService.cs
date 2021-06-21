using LIMS.Domain;
using LIMS.Domain.BasicSetup;
using LIMS.Domain.BesicSetup;
using LIMS.Domain.Data;
using LIMS.Services.Events;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LIMS.Services.Basic
{
    public class DiseaseService:IDiseaseService
    {
        private readonly IRepository<Disease> _diseaseRepository;
        private readonly IMediator _mediator;
        public DiseaseService(IRepository<Disease> diseaseRepository, IMediator mediator)
        {
            _diseaseRepository = diseaseRepository;
            _mediator = mediator;
        }
        public async Task DeleteDisease(Disease disease)
        {
            if (disease == null)
                throw new ArgumentNullException("Disease");

            await _diseaseRepository.DeleteAsync(disease);

            //event notification
            await _mediator.EntityDeleted(disease);
        }

        public async Task<IPagedList<Disease>> GetDisease(int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = _diseaseRepository.Table;


            return await PagedList<Disease>.Create(query, pageIndex, pageSize);
        }

        public Task<Disease> GetDiseaseById(string Id)
        {
            return _diseaseRepository.GetByIdAsync(Id);

        }

        public async Task InsertDisease(Disease disease)
        {
            if (disease == null)
                throw new ArgumentNullException("Disease");

            await _diseaseRepository.InsertAsync(disease);

            //event notification
            await _mediator.EntityInserted(disease);
        }

        public async Task UpdateDisease(Disease disease)
        {
            if (disease == null)
                throw new ArgumentNullException("Disease");

            await _diseaseRepository.UpdateAsync(disease);

            //event notification
            await _mediator.EntityUpdated(disease);
        }

    }
}
