using LIMS.Core.Caching;
using LIMS.Domain;
using LIMS.Domain.AInR;
using LIMS.Domain.Data;
using LIMS.Services.Events;
using MediatR;
using MongoDB.Driver.Linq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LIMS.Services.Ainr
{
    public class EarTagService : IEarTagService
    {
        #region Fields
        private readonly IRepository<EarTag> _earTagRepository;
        private readonly IMediator _mediator;
        private readonly ICacheManager _cacheManager;
        #endregion

        #region Ctor
        public EarTagService(IRepository<EarTag> earTagRepository, IMediator mediator, ICacheManager cacheManager)
        {
            _earTagRepository = earTagRepository;
            _mediator = mediator;
            _cacheManager = cacheManager;
        }
        #endregion

        public virtual async Task DeleteEarTag(EarTag earTag)
        {
            if (earTag == null)
                throw new ArgumentNullException("EarTag");
            await _earTagRepository.DeleteAsync(earTag);

            //event notification
            await _mediator.EntityDeleted(earTag);
        }

        public virtual Task<EarTag> GetEarTagById(string id)
        {
            return _earTagRepository.GetByIdAsync(id);
        }

        public virtual async Task InsertEarTag(EarTag earTag)
        {
            if (earTag == null)
                throw new ArgumentNullException("EarTag");
            await _earTagRepository.InsertAsync(earTag);

            //event notification
            await _mediator.EntityInserted(earTag);
        }

        public virtual async Task<IPagedList<EarTag>> SearchEarTag(string keyword = "", int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = _earTagRepository.Table;
            if (!string.IsNullOrEmpty(keyword))
            {
                keyword = keyword.ToLower();
                query = query.Where(et => et.EarTagNo.ToLower().Contains(keyword));
            }
            query = query.OrderByDescending(et => et.SerialNo);
            return await PagedList<EarTag>.Create(query, pageIndex, pageSize);
        }

        public virtual async Task UpdateEarTag(EarTag earTag)
        {
            if (earTag == null)
                throw new ArgumentNullException("EarTag");
            await _earTagRepository.UpdateAsync(earTag);

            //event notification
            await _mediator.EntityUpdated(earTag);
        }

        public virtual async Task<IList<EarTag>> GetEarTags(int from = 0, int to = 0)
        {
            var query = _earTagRepository.Table.Where(x => x.SerialNo >= from && x.SerialNo <= to);
            query = query.OrderByDescending(et => et.SerialNo);
            return await PagedList<EarTag>.Create(query, 0, int.MaxValue);
        }
    }
}
