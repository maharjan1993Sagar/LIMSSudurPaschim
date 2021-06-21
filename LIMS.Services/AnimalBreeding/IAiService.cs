using LIMS.Domain;
using LIMS.Domain.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LIMS.Services.AnimalBreeding
{
   public interface IAiService
    {
        Task<AIService> GetAIById(string Id);

        Task<IPagedList<AIService>> GetAI(int pageIndex = 0, int pageSize = int.MaxValue);
        Task<IPagedList<AIService>> GetAI(string createdBy,int pageIndex = 0, int pageSize = int.MaxValue);
        Task<IPagedList<AIService>> GetAI(List<string> createdBy, int pageIndex = 0, int pageSize = int.MaxValue);

        Task<IPagedList<AIService>> GetAIByAnimalId(string animalId,int pageIndex = 0, int pageSize = int.MaxValue);

        Task DeleteAI(AIService aiService);

        Task InsertAI(AIService aiService);

        Task UpdateAI(AIService aiService);
        Task<IPagedList<AIService>> GetAIServiceByCustomerIds(List<string> customerid, string keyword = "", int pageIndex = 0, int pageSize = int.MaxValue);
        int GetAiCountByCustomerIds(List<string> customerid);
    }
}
