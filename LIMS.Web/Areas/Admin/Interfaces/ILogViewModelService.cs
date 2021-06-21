using LIMS.Domain.Logging;
using LIMS.Web.Areas.Admin.Models.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LIMS.Web.Areas.Admin.Interfaces
{
    public interface ILogViewModelService
    {
        LogListModel PrepareLogListModel();
        Task<(IEnumerable<LogModel> logModels, int totalCount)> PrepareLogModel(LogListModel model, int pageIndex, int pageSize);
        Task<LogModel> PrepareLogModel(Log log);
    }
}
