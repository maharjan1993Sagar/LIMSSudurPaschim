using LIMS.Domain;
using LIMS.Domain.Bali;
using LIMS.Domain.Report;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LIMS.Services.Bali
{
    public interface IAnudanService
    {
        Task<AanudanKokaryakram> GetbaliRegisterById(string id);
        Task<IPagedList<AanudanKokaryakram>> GetbaliRegister(string createdby, int pageIndex = 0, int pageSize = int.MaxValue, string fiscalyear = "");
        Task<IPagedList<AanudanKokaryakram>> GetbaliRegister(List<string> createdby, int pageIndex = 0, int pageSize = int.MaxValue, string fiscalyear = "");

        Task DeletebaliRegister(AanudanKokaryakram baliRegister);

        Task InsertbaliRegister(AanudanKokaryakram baliRegister);
        Task InsertbaliRegisterList(List<AanudanKokaryakram> livestocks);

        Task UpdatebaliRegister(AanudanKokaryakram baliRegister);
        Task UpdatebaliRegisterList(List<AanudanKokaryakram> livestocks);
        Task<IPagedList<AanudanKokaryakram>> GetFilteredSubsidy(string id, string fiscalYear, string district, string program, int pageIndex = 0, int pageSize = int.MaxValue);
        Task<SubsidyReportModel> GetSubsidyReportModel(string id, string fiscalYear, string localLevel, string budgetId, int pageIndex = 0, int pageSize = int.MaxValue);

        Task<IPagedList<AanudanKokaryakram>> GetFilteredLabambitKrishak(string id, string fiscalYear, string programType, string type, int pageIndex = 0, int pageSize = int.MaxValue);
        Task<IPagedList<AanudanKokaryakram>> GetFilteredLabambitKrishak(List<string> id, string fiscalYear, string programType, string type, int pageIndex = 0, int pageSize = int.MaxValue);

    }
}
