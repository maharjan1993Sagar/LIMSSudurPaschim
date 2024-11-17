using LIMS.Domain;
using LIMS.Domain.Bali;
using LIMS.Domain.BasicSetup;
using LIMS.Domain.BesicSetup;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LIMS.Services.Basic
{
    public interface IPujigatKharchaKharakramService
    {
        Task<PujigatKharchaKharakram> GetPujigatKharchaKharakramById(string Id);
        Task<bool> GetPujigatKharchaKharakramByLmBIsCode(string Id);

        Task<IPagedList<PujigatKharchaKharakram>> GetPujigatKharchaKharakram(
           int pageIndex = 0, int pageSize = int.MaxValue);
        Task<IPagedList<PujigatKharchaKharakram>> GetPujigatKharchaKharakramSelect(string createdby, string keyword = "", int pageIndex = 0, int pageSize = int.MaxValue);
        Task<IPagedList<PujigatKharchaKharakram>> GetPujigatKharchaKharakram(string createdby,
          string keyword="", int pageIndex = 0, int pageSize = int.MaxValue);

        Task<IPagedList<PujigatKharchaKharakram>> GetPujigatKharchaKharakram(List<string> createdby,
          int pageIndex = 0, int pageSize = int.MaxValue);
        Task<IPagedList<PujigatKharchaKharakram>> GetPujigatKharchaKharakram(List<string> createdby,
              string fiscalyear,
            string programtype = "",

            string type = "",

        int pageIndex = 0, int pageSize = int.MaxValue);
        Task<IPagedList<PujigatKharchaKharakram>> GetPujigatKharchaKharakram(string createdby,
              string fiscalyear,
            string programtype="",          
            string type="",
             string budgetSourceId = "",
            string subSectorId = "",
        int pageIndex = 0, int pageSize = int.MaxValue
           );
        Task<IPagedList<PujigatKharchaKharakram>> GetNitigatKharakram(string createdby,
            string fiscalyear,
          string programtype = "",

          string type = "",

      int pageIndex = 0, int pageSize = int.MaxValue);
        Task<IPagedList<PujigatKharchaKharakram>> GetMainKharakram(string createdby,
         string fiscalyear,
       string programtype = "",
       string type = "",
       string budgetSourceId="",
       string subSectorId ="",
   int pageIndex = 0, int pageSize = int.MaxValue);
        Task DeletePujigatKharchaKharakram(PujigatKharchaKharakram pujigatKharchaKharakram);


        Task InsertPujigatKharchaKharakram(PujigatKharchaKharakram pujigatKharchaKharakram);


        Task UpdatePujigatKharchaKharakram(PujigatKharchaKharakram pujigatKharchaKharakram);

        Task<List<string>> GetLimbis_Code();
    }
}
