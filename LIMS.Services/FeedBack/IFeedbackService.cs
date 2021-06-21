using LIMS.Domain;
using LIMS.Domain.Feedback;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LIMS.Services.FeedBack
{
   public interface IFeedbackService
    {
        Task<Feedback> GetFeedbackById(string Id);
        Task<IPagedList<Feedback>> GetFeedback(
           int pageIndex = 0, int pageSize = int.MaxValue);
        Task DeleteFeedback(Feedback feedback);


        Task InsertFeedback(Feedback feedback);


        Task UpdateFeedback(Feedback feedback);
    }
}
