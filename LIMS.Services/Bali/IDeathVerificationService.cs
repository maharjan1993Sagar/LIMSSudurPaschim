using LIMS.Domain;
using LIMS.Domain.Bali;
using LIMS.Domain.Report;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LIMS.Services.Bali
{
    public interface IDeathVerificationService
    {
        Task<DeathVerification> GetdeathVerificationById(string id);
        Task<IPagedList<DeathVerification>> GetdeathVerification(string createdby,string fiscalYear, string localLevel, int pageIndex = 0, int pageSize = int.MaxValue, string fiscalyear = "");
        Task<IPagedList<DeathVerification>> GetdeathVerification(List<string> createdby, int pageIndex = 0, int pageSize = int.MaxValue, string fiscalyear = "");

        Task DeletedeathVerification(DeathVerification deathVerification);

        Task InsertdeathVerification(DeathVerification deathVerification);
        Task InsertdeathVerificationList(List<DeathVerification> livestocks);
        Task UpdatedeathVerification(DeathVerification deathVerification);
        Task UpdatedeathVerificationList(List<DeathVerification> livestocks);
        //Task<IPagedList<DeathVerification>> GetFilteredSubsidy(string id, string fiscalYear, string localLevel, string budgetId, string xetra,int pageIndex = 0, int pageSize = int.MaxValue);
        //Task<SubsidyReportModel> GetSubsidyReportModel(string id, string fiscalYear, string localLevel, string budgetId, int pageIndex = 0, int pageSize = int.MaxValue);
        //Task<IPagedList<DeathVerification>> GetFilteredLabambitKrishak(string id, string fiscalYear, string programType, string type, int pageIndex = 0, int pageSize = int.MaxValue);
        //Task<IPagedList<DeathVerification>> GetFilteredLabambitKrishak(List<string> id, string fiscalYear, string programType, string type, int pageIndex = 0, int pageSize = int.MaxValue);

    }
}
