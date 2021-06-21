using LIMS.Domain;
using LIMS.Domain.RationBalance;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LIMS.Services.RationBalance
{
    public interface IAnimalFeedService
    {
        Task<AnimalFeed> GetAnimalFeedById(string id);
        Task<IPagedList<AnimalFeed>> GetFeedLibraries(int pageIndex = 0, int PageSize = int.MaxValue);
        Task DeleteAnimalFeed(AnimalFeed animalFeed);
        Task InsertAnimalFeed(AnimalFeed animalFeed);
        Task InsertAnimalFeedList(IList<AnimalFeed> feedLibraries);
        Task UpdateAnimalFeed(AnimalFeed animalFeed);
        Task UpdateAnimalFeedList(IList<AnimalFeed> feedLibraries);

    }
}
