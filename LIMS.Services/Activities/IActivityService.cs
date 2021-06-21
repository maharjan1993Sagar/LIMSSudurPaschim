using LIMS.Domain;
using LIMS.Domain.Activities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LIMS.Services.Activities
{
    public interface IActivityService
    {
        Task<Activity> GetActivityById(string id);

        Task<IPagedList<Activity>> GetActivity(string createdby="",int pageIndex = 0, int pageSize = int.MaxValue);
        Task<IPagedList<Activity>> GetActivityByFiscalYear(string createdby,string fiscalyear, int pageIndex = 0, int pageSize = int.MaxValue);


        Task DeleteActivity(Activity activity);

        Task InsertActivity(Activity activity);

        Task UpdateActivity(Activity activity);
        
    }
}
