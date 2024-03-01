using LIMS.Domain;
using LIMS.Domain.Bali;
using LIMS.Domain.Data;
using LIMS.Domain.Report;
using LIMS.Services.Events;
using MediatR;
using MongoDB.Driver.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LIMS.Services.Bali
{
    public class DeathVerificationService:IDeathVerificationService
    {
        private readonly IRepository<DeathVerification> _deathRepository;
        private readonly IMediator _mediator;
        public DeathVerificationService(IRepository<DeathVerification> anudanRepository, IMediator mediator)
        {
            _deathRepository = anudanRepository;
            _mediator = mediator;
        }
        public async Task DeletedeathVerification(DeathVerification deathVerification)
        {
            if (deathVerification == null)
                throw new ArgumentNullException("DeathVerification");

            await _deathRepository.DeleteAsync(deathVerification);

            //event notification
            await _mediator.EntityDeleted(deathVerification);
        }

        public async Task<IPagedList<DeathVerification>> GetdeathVerification(string createdby,string fiscalYear, string localLevel, int pageIndex = 0, int pageSize = int.MaxValue, string fiscalyear = "")
        {
            var query = _deathRepository.Table;
            if (!string.IsNullOrEmpty(createdby))
            {
                query = query.Where(m => m.CreatedBy == createdby);
            }
            if (!string.IsNullOrEmpty(fiscalyear))
            {
                query = query.Where(
                  m => m.FiscalYearId == fiscalyear
                );
            }
            if (!string.IsNullOrEmpty(localLevel))
            {
                query = query.Where(
                  m => m.LocalLevel == localLevel
                );
            }

            return await PagedList<DeathVerification>.Create(query, pageIndex, pageSize);
        }
        
         public async Task<IPagedList<DeathVerification>> GetFilteredSubsidy(string id, string fiscalYear, string localLevel, string budgetId,string xetra, int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = _deathRepository.Table;
            if (!String.IsNullOrEmpty(id))
            {
                query = query.Where(m => m.CreatedBy == id);
            }
            if (!string.IsNullOrEmpty(fiscalYear))
            {
                query = query.Where(m => m.FiscalYear.Id == fiscalYear);
            }
            
            //if (!string.IsNullOrEmpty(budgetId))
            //{
            //    query = query.Where(m => m.BudgetId == budgetId);
            //}
            //if (!string.IsNullOrEmpty(localLevel))
            //{
            //    query = query.Where(m => m.LocalLevel == localLevel);
            //}
            //if (!string.IsNullOrEmpty(xetra))
            //{
            //    query = query.Where(m => m.Budget.Xetra == xetra);
            //}

            return await PagedList<DeathVerification>.Create(query, pageIndex, pageSize);

        }

        //public async Task<SubsidyReportModel> GetSubsidyReportModel(string id, string fiscalYear, string localLevel, string budgetId, int pageIndex = 0, int pageSize = int.MaxValue)
        //{
        //    var query = _deathRepository.Table;
        //    if (!String.IsNullOrEmpty(id))
        //    {
        //        query = query.Where(m => m.CreatedBy == id);
        //    }
        //    if (!string.IsNullOrEmpty(fiscalYear))
        //    {
        //        query = query.Where(m => m.FiscalYear.Id == fiscalYear);
        //    }

        //    //if (!string.IsNullOrEmpty(budgetId))
        //    //{
        //    //    query = query.Where(m => m.BudgetId == budgetId);
        //    //}
        //    if (!string.IsNullOrEmpty(localLevel))
        //    {
        //        query = query.Where(m => m.LocalLevel == localLevel);
        //    }

        //    var filteredAnudan = query.AsEnumerable().ToList();

        //    //var distinctBudget = filteredAnudan.ToList().Select(m => m.BudgetId).Distinct();

        //    var subsidy = new SubsidyReportModel();
        //    subsidy.FiscalYear = filteredAnudan.ToList().FirstOrDefault().FiscalYear.NepaliFiscalYear;
        //    subsidy.LocalLevel = localLevel;
        //    subsidy.Level = "नगर कार्यपालिकाको कार्यालय";
        //    subsidy.StartDate = "";
        //    subsidy.EndDate = "";
        //    subsidy.Address = "काठमाडौँ";
        //    subsidy.Ward = "";
        //    var lstRowData = new List<SubsidyRowData>();
        //    int i = 1;
        //    foreach (var item in distinctBudget)
        //    {
        //        var objData = filteredAnudan.ToList().FirstOrDefault(m => m.BudgetId == item);
        //        var objSubsidyData = new SubsidyRowData {
        //            BudgetTitle = objData.Budget.ActivityName,
        //            MainActivity = objData.ExpectedOutput,
        //            Remarks = objData.Remaks,
        //            SN = i.ToString()
        //        };
        //        lstRowData.Add(objSubsidyData);
        //        i++;
        //    }

        //    subsidy.Rows = lstRowData;

        //    return subsidy;

        //}

        //public async Task<IPagedList<DeathVerification>> GetFilteredLabambitKrishak(string id, string fiscalYear, string programType, string type, int pageIndex = 0, int pageSize = int.MaxValue)
        //{
        //    var query = _deathRepository.Table;
        //    if (!string.IsNullOrEmpty(id))
        //    {
        //        query = query.Where(m => m.CreatedBy == id);
        //    }
        //    if (!string.IsNullOrEmpty(fiscalYear))
        //    {
        //        query = query.Where(m=> m.FiscalyearId == fiscalYear);
        //    }
        //    if (!string.IsNullOrEmpty(programType))
        //    {
        //        query = query.Where(m => m.Budget.SourceOfFund== programType);
        //    }
        //    if (!string.IsNullOrEmpty(type))
        //    {
        //        query = query.Where(m => m.Budget.TypeOfExpen == type);
        //    }

        //    return await PagedList<DeathVerification>.Create(query, pageIndex, pageSize);

        //}
        //public async Task<IPagedList<DeathVerification>> GetFilteredLabambitKrishak(List<string> id, string fiscalYear, string programType, string type, int pageIndex = 0, int pageSize = int.MaxValue)
        //{
        //    var query = _deathRepository.Table;
        //    query = query.Where(m =>id.Contains(m.CreatedBy));
        //    if (!string.IsNullOrEmpty(fiscalYear))
        //    {
        //        query = query.Where(m => m.FiscalYear.Id == fiscalYear);

        //    }
        //    if (!string.IsNullOrEmpty(type))
        //    {
        //        query = query.Where(m => m.PujigatKharchaKharakram.Type == type);
        //    }
        //    if (!string.IsNullOrEmpty(programType))
        //    {
        //        query = query.Where(m => m.PujigatKharchaKharakram.ProgramType == programType);
        //    }

        //    return await PagedList<DeathVerification>.Create(query, pageIndex, pageSize);

        //}

        public async Task<IPagedList<DeathVerification>> GetdeathVerification(List<string> createdby, int pageIndex = 0, int pageSize = int.MaxValue, string fiscalyear = "")
        {
            var query = _deathRepository.Table;
            query = query.Where(m => createdby.Contains(m.CreatedBy));
            if (!string.IsNullOrEmpty(fiscalyear))
            {
                query = query.Where(
                  m => m.FiscalYear.Id == fiscalyear
                );
            }

            return await PagedList<DeathVerification>.Create(query, pageIndex, pageSize);
        }

        public Task<DeathVerification> GetdeathVerificationById(string id)
        {
            return _deathRepository.GetByIdAsync(id);
        }

        public async Task InsertdeathVerification(DeathVerification deathVerification)
        {
            if (deathVerification == null)
                throw new ArgumentNullException("Livestock");

            await _deathRepository.InsertAsync(deathVerification);

            //event notification
            await _mediator.EntityInserted(deathVerification);
        }

        public Task InsertdeathVerificationList(List<DeathVerification> deathVerifications)
        {
            throw new NotImplementedException();
        }

        public async Task UpdatedeathVerification(DeathVerification deathVerification)
        {
            if (deathVerification == null)
                throw new ArgumentNullException("deathVerification");

            await _deathRepository.UpdateAsync(deathVerification);

            //event notification
            await _mediator.EntityUpdated(deathVerification);
        }

        public Task UpdatedeathVerificationList(List<DeathVerification> deathVerifications)
        {
            throw new NotImplementedException();
        }
    }
}
