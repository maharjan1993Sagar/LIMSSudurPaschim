using LIMS.Domain;
using LIMS.Domain.Data;
using LIMS.Domain.Services;
using LIMS.Services.Events;
using MediatR;
using MongoDB.Driver.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LIMS.Services.AnimalBreeding
{
    public class AiService:IAiService
    {
        private readonly IRepository<AIService> _aiRepository;
        private readonly IMediator _mediator;
        public AiService(IRepository<AIService> aiRepository, IMediator mediator)
        {
            _aiRepository = aiRepository;
            _mediator = mediator;
        }

        public async Task DeleteAI(AIService ai)
        {
            if (ai == null)
                throw new ArgumentNullException("AI");
            await _aiRepository.DeleteAsync(ai);

            //event notification
            await _mediator.EntityDeleted(ai);
        }

        public async Task<IPagedList<AIService>> GetAI(int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = _aiRepository.Table;
            return await PagedList<AIService>.Create(query, pageIndex, pageSize);
        }
        public async Task<IPagedList<AIService>> GetAI(string createdBy,int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = _aiRepository.Table;
            if (!string.IsNullOrEmpty(createdBy))
            {
                query = query.Where(m => m.CreatedBy == createdBy);
            }
            return await PagedList<AIService>.Create(query, pageIndex, pageSize);
        }
        public async Task<IPagedList<AIService>> GetAI(List<string> createdBy, int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = _aiRepository.Table;
            
            query = query.Where(t => createdBy.Contains(t.CreatedBy));
            return await PagedList<AIService>.Create(query, pageIndex, pageSize);
        }


        public Task<AIService> GetAIById(string id)
        {
            return _aiRepository.GetByIdAsync(id);
        }
        public async Task<IPagedList<AIService>> GetAIByAnimalId(string animalId, int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = _aiRepository.Table;
            query = query.Where(m => m.AnimalRegistration.Id == animalId);
            return await PagedList<AIService>.Create(query, pageIndex, pageSize);
        }

        public async Task InsertAI(AIService ai)
        {
            if (ai == null)
                throw new ArgumentNullException("AI");
            await _aiRepository.InsertAsync(ai);

            //event notification
            await _mediator.EntityInserted(ai);
        }

        public async Task UpdateAI(AIService ai)
        {
            if (ai == null)
                throw new ArgumentNullException("AI");
            await _aiRepository.UpdateAsync(ai);

            //event notification
            await _mediator.EntityUpdated(ai);
        }
        public async Task<IPagedList<AIService>> GetAIServiceByCustomerIds(List<string> customerid, string keyword = "", int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = _aiRepository.Table.Where(t => customerid.Contains(t.CreatedBy));

           
            return await PagedList<AIService>.Create(query, pageIndex, pageSize);
        }
        public int GetAiCountByCustomerIds(List<string> customerid)
        {
            var query =  _aiRepository.Table.Where(t => customerid.Contains(t.CreatedBy)).ToList();
            return query.Count();

            
        }


    }
}
