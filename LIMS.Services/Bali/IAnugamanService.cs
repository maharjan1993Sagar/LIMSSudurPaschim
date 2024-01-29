using LIMS.Domain;
using LIMS.Domain.Bali;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LIMS.Services.Bali
{
    public interface IAnugamanService
    {
        Task<Anugaman> GetAnugamanById(string id);
        Task<IPagedList<Anugaman>> GetAnugaman(string createdby,  string fiscalYearId, string LocalLevel, string district, int pageIndex = 0, int pageSize = int.MaxValue, string fiscalyear = "");
        Task<IPagedList<Anugaman>> GetAnugaman(string createdby, int pageIndex = 0, int pageSize = int.MaxValue, string fiscalyear = "");

        Task<IPagedList<Anugaman>> GetAnugaman(List<string> createdby, int pageIndex = 0, int pageSize = int.MaxValue, string fiscalyear = "");

        Task DeleteAnugaman(Anugaman Anugaman);

        Task InsertAnugaman(Anugaman Anugaman);
        Task InsertAnugamanList(List<Anugaman> livestocks);

        Task UpdateAnugaman(Anugaman Anugaman);
        Task UpdateAnugamanList(List<Anugaman> livestocks);
    }
}
