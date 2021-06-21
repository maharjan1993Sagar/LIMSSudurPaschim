using LIMS.Domain;
using LIMS.Domain.Users;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LIMS.Services.User
{
    public interface INlboUserService
    {
        Task<NlboUser> GetNlboUserById(string id);

        Task<IPagedList<NlboUser>> GetNlboUser(int pageIndex = 0, int pageSize = int.MaxValue);

        Task DeleteNlboUser(NlboUser nlboUser);

        Task InsertNlboUser(NlboUser nlboUser);

        Task UpdateNlboUser(NlboUser nlboUser);
    }
}
