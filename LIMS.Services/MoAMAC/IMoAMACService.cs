using LIMS.Domain;
using System.Threading.Tasks;

namespace LIMS.Services.MoAMAC
{
    public interface IMoAMACService
    {
        Task<Domain.MoAMAC.MoAMAC> GetMoAMACById(string id);

        Task<IPagedList<Domain.MoAMAC.MoAMAC>> GetMoAMAC(int pageIndex = 0, int pageSize = int.MaxValue);

        Task DeleteMoAMAC(Domain.MoAMAC.MoAMAC moAMAC);

        Task InsertMoAMAC(Domain.MoAMAC.MoAMAC moAMAC);

        Task UpdateMoAMAC(Domain.MoAMAC.MoAMAC moAMAC);
    }
}
