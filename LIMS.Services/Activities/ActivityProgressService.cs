using LIMS.Domain;
using LIMS.Domain.Activities;
using LIMS.Domain.Data;
using LIMS.Services.Events;
using MediatR;
using MongoDB.Driver.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LIMS.Services.Activities
{
   public class ActivityProgressService:IActivityProgressService
    {
        private readonly IRepository<ActivityProgress> _activityProgressRepository;
        private readonly IMediator _mediator;
        public ActivityProgressService(IRepository<ActivityProgress> activityProgressRepository, IMediator mediator)
        {
            _activityProgressRepository = activityProgressRepository;
            _mediator = mediator;
        }
        public async Task DeleteActivityProgress(ActivityProgress activityProgress)
        {
            if (activityProgress == null)
                throw new ArgumentNullException("ActivityProgress");

            await _activityProgressRepository.DeleteAsync(activityProgress);

            //event notification
            await _mediator.EntityDeleted(activityProgress);
        }

        public async Task<IPagedList<ActivityProgress>> GetActivityProgress(int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = _activityProgressRepository.Table;


            return await PagedList<ActivityProgress>.Create(query, pageIndex, pageSize);
        }
        public async Task<IPagedList<ActivityProgress>> GetActivityProgress(string createdby,int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = _activityProgressRepository.Table;
            if (!string.IsNullOrEmpty(createdby))
            {
                query = query.Where(m => m.CreatedBy == createdby);
            }
            return await PagedList<ActivityProgress>.Create(query, pageIndex, pageSize);
        }
        public async Task<IPagedList<ActivityProgress>> GetFilteredProgress(string createdby, string fiscalyear,string month, int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = _activityProgressRepository.Table;
            if(!string.IsNullOrEmpty(createdby))
            {
                query = query.Where(m => m.CreatedBy == createdby);
            }
            
            query = query.Where(m =>
                                m.FiscalYearId == fiscalyear &&
                                m.Month==month
                                );
            return await PagedList<ActivityProgress>.Create(query, pageIndex, pageSize);
        }

        public Task<ActivityProgress> GetActivityProgressById(string Id)
        {
            return _activityProgressRepository.GetByIdAsync(Id);

        }

        public async Task InsertActivityProgress(ActivityProgress activityProgress)
        {
            if (activityProgress == null)
                throw new ArgumentNullException("ActivityProgress");

            await _activityProgressRepository.InsertAsync(activityProgress);

            //event notification
            await _mediator.EntityInserted(activityProgress);
        }

        public async Task UpdateActivityProgress(ActivityProgress activityProgress)
        {
            if (activityProgress == null)
                throw new ArgumentNullException("ActivityProgress");

            await _activityProgressRepository.UpdateAsync(activityProgress);

            //event notification
            await _mediator.EntityUpdated(activityProgress);
        }
        public async Task InsertActivityProgressList(List<ActivityProgress> activityProgresss)
        {
            if (activityProgresss.Count < 1)
                throw new ArgumentNullException("ActivityProgress");
            await _activityProgressRepository.InsertManyAsync(activityProgresss);


        }
        public async Task UpdateActivityProgressList(List<ActivityProgress> activityProgresss)
        {
            if (activityProgresss.Count < 1)
                throw new ArgumentNullException("ActivityProgress");
            foreach (var item in activityProgresss)
            {
                await _activityProgressRepository.UpdateAsync(item);
            }


        }

        //public async Task<IPagedList<ActivityProgress>> GetFilteredActivityProgress(string fiscalyearId, string Quater, string activityProgresstype, string createdBy, int pageIndex = 0, int pageSize = int.MaxValue)
        //{
        //    var query = _activityProgressRepository.Table;
        //    query = query.Where(m =>
        //      m.Quater == Quater &&
        //      m.ActivityProgressType == activityProgresstype &&
        //      m.FiscalYear.Id == fiscalyearId &&
        //      m.CreatedBy == createdBy
        //    );
        //    return await PagedList<ActivityProgress>.Create(query, pageIndex, pageSize);
        //}
    }
}
