using LIMS.Domain;
using LIMS.Domain.AInR;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LIMS.Services.Ainr
{
    public interface IEarTagService
    {
        Task<EarTag> GetEarTagById(string id);

        Task<IPagedList<EarTag>> SearchEarTag(string keyword = "", int pageIndex = 0, int pageSize = int.MaxValue);

        Task DeleteEarTag(EarTag farm);

        Task InsertEarTag(EarTag farm);

        Task UpdateEarTag(EarTag farm);

        Task<IList<EarTag>> GetEarTags(int from = 0, int to = 0);
    }
}
