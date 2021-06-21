using System.Threading.Tasks;

namespace LIMS.Services.Tasks
{
    public interface IScheduleTask
    {
        Task Execute();
    }
}
