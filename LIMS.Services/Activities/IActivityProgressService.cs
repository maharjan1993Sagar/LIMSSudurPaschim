using LIMS.Domain;
using LIMS.Domain.Activities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LIMS.Services.Activities
{
    public interface IActivityProgressService
    {
        Task<ActivityProgress> GetActivityProgressById(string id);

        Task<IPagedList<ActivityProgress>> GetActivityProgress(int pageIndex = 0, int pageSize = int.MaxValue);
        Task<IPagedList<ActivityProgress>> GetActivityProgress(string createdby,int pageIndex = 0, int pageSize = int.MaxValue);

        Task<IPagedList<ActivityProgress>> GetFilteredProgress(string createdby,string fiscalyear,string month,int pageIndex = 0, int pageSize = int.MaxValue);


        Task DeleteActivityProgress(ActivityProgress activityProgress);

        Task InsertActivityProgress(ActivityProgress activityProgress);

        Task UpdateActivityProgress(ActivityProgress activityProgress);
        Task InsertActivityProgressList(List<ActivityProgress> activityProgress);
        Task UpdateActivityProgressList(List<ActivityProgress> activityProgress);

    }
}
