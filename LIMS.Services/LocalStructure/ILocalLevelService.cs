using LIMS.Domain;
using LIMS.Domain.LocalStructure;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LIMS.Services.LocalStructure
{
    public interface ILocalLevelService
    {
        Task<IList<string>> GetProvience(
          );
        Task<IList<string>> GetDistrict(string province);
        Task<IList<string>> GetLocalLevel(string district);
        Task<IList<LocalLevels>> GetAllProvience();
        string GetNepaliDistrict(string district);
    }
}
