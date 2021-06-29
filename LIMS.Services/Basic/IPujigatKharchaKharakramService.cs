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
        Task<IPagedList<PujigatKharchaKharakram>> GetPujigatKharchaKharakram(
           int pageIndex = 0, int pageSize = int.MaxValue);
        Task DeletePujigatKharchaKharakram(PujigatKharchaKharakram pujigatKharchaKharakram);


        Task InsertPujigatKharchaKharakram(PujigatKharchaKharakram pujigatKharchaKharakram);


        Task UpdatePujigatKharchaKharakram(PujigatKharchaKharakram pujigatKharchaKharakram);
    }
}
