using LIMS.Domain;
using LIMS.Domain.Bali;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LIMS.Services.Bali
{
    public interface ITalimService
    {
        Task<Talim> GettalimById(string id);
        Task<IPagedList<Talim>> Gettalim(string createdby="" , string fiscalyear = "", string budgetId = "", int pageIndex = 0, int pageSize = int.MaxValue);
        Task<IPagedList<Talim>> Gettalim(List<string> createdby, int pageIndex = 0, int pageSize = int.MaxValue, string fiscalyear = "");

        Task Deletetalim(Talim talim);

        Task Inserttalim(Talim talim);
        Task InserttalimList(List<Talim> livestocks);

        Task Updatetalim(Talim talim);
        Task UpdatetalimList(List<Talim> livestocks);
    }
}
