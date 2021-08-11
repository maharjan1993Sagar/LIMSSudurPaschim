using LIMS.Domain;
using LIMS.Domain.Bali;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LIMS.Services.Bali
{
    public interface IBaliRegisterService
    {
        Task<BaliRegister> GetbaliRegisterById(string id);
        Task<IPagedList<BaliRegister>> GetbaliRegister(string createdby, int pageIndex = 0, int pageSize = int.MaxValue, string fiscalyear = "");
        Task<IPagedList<BaliRegister>> GetbaliRegister(List<string> createdby, int pageIndex = 0, int pageSize = int.MaxValue, string fiscalyear = "");

        Task DeletebaliRegister(BaliRegister baliRegister);

        Task InsertbaliRegister(BaliRegister baliRegister);
        Task InsertbaliRegisterList(List<BaliRegister> livestocks);

        Task UpdatebaliRegister(BaliRegister baliRegister);
        Task UpdatebaliRegisterList(List<BaliRegister> livestocks);
    }
}
