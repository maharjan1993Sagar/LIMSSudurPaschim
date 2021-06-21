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
    public class ActivityService : IActivityService
    {
        private readonly IRepository<Activity> _activityRepository;
        private readonly IMediator _mediator;
        public ActivityService(IRepository<Activity> ActivityRepository, IMediator mediator)
        {
            _activityRepository = ActivityRepository;
            _mediator = mediator;
        }

        public async Task DeleteActivity(Activity activity)
        {
            if (activity == null)
                throw new ArgumentNullException("Activity");
            await _activityRepository.DeleteAsync(activity);

            //event notification
            await _mediator.EntityDeleted(activity);
        }

        public async Task<IPagedList<Activity>> GetActivity(string createdby = "", int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = _activityRepository.Table;
            if (!string.IsNullOrEmpty(createdby))
            {
                query = query.Where(m => m.CreatedBy == createdby);
            }
            return await PagedList<Activity>.Create(query, pageIndex, pageSize);
        }
        public async Task<IPagedList<Activity>> GetActivityByFiscalYear(string createdby, string fiscalyear, int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = _activityRepository.Table;

            query = query.Where(
                m => m.CreatedBy == createdby &&
                m.FiscalYear.Id == fiscalyear
                );
                
                
                
              
            return await PagedList<Activity>.Create(query, pageIndex, pageSize);
        }


        public Task<Activity> GetActivityById(string id)
        {
            return _activityRepository.GetByIdAsync(id);
        }

        public async Task InsertActivity(Activity activity)
        {
            if (activity == null)
                throw new ArgumentNullException("Activity");
            await _activityRepository.InsertAsync(activity);

            //event notification
            await _mediator.EntityInserted(activity);
        }

        public async Task UpdateActivity(Activity activity)
        {
            if (activity == null)
                throw new ArgumentNullException("Activity");
            await _activityRepository.UpdateAsync(activity);

            //event notification
            await _mediator.EntityUpdated(activity);
        }

    }
}
