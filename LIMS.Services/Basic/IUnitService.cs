using LIMS.Domain;
using LIMS.Domain.BasicSetup;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LIMS.Services.Basic
{
  public  interface IUnitService
    {
        Task<Unit> GetUnitById(string Id);
        Task<IPagedList<Unit>> GetUnit(
           int pageIndex = 0, int pageSize = int.MaxValue);
        Task DeleteUnit(Unit unit);


        Task InsertUnit(Unit unit);


        Task UpdateUnit(Unit unit);

    }
}
