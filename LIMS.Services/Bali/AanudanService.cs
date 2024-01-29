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
    public class AanudanService:IAnudanService
    {
        private readonly IRepository<AanudanKokaryakram> _anudanRepository;
        private readonly IMediator _mediator;
        public AanudanService(IRepository<AanudanKokaryakram> anudanRepository, IMediator mediator)
        {
            _anudanRepository = anudanRepository;
            _mediator = mediator;
        }
        public async Task DeletebaliRegister(AanudanKokaryakram baliRegister)
        {
            if (baliRegister == null)
                throw new ArgumentNullException("AanudanKokaryakram");

            await _anudanRepository.DeleteAsync(baliRegister);

            //event notification
            await _mediator.EntityDeleted(baliRegister);
        }

        public async Task<IPagedList<AanudanKokaryakram>> GetbaliRegister(string createdby, int pageIndex = 0, int pageSize = int.MaxValue, string fiscalyear = "")
        {
            var query = _anudanRepository.Table;
            query = query.Where(m => m.CreatedBy == createdby);
            if (!string.IsNullOrEmpty(fiscalyear))
            {
                query = query.Where(
                  m => m.FiscalYear.Id == fiscalyear
                );
            }

            return await PagedList<AanudanKokaryakram>.Create(query, pageIndex, pageSize);
        }
        
         public async Task<IPagedList<AanudanKokaryakram>> GetFilteredSubsidy(string id, string fiscalYear, string localLevel, string budgetId, int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = _anudanRepository.Table;
            if (!String.IsNullOrEmpty(id))
            {
                query = query.Where(m => m.CreatedBy == id);
            }
            if (!string.IsNullOrEmpty(fiscalYear))
            {
                query = query.Where(m => m.FiscalYear.Id == fiscalYear);
            }
            
            if (!string.IsNullOrEmpty(budgetId))
            {
                query = query.Where(m => m.BudgetId == budgetId);
            }
            if (!string.IsNullOrEmpty(localLevel))
            {
                query = query.Where(m => m.LocalLevel == localLevel);
            }

            return await PagedList<AanudanKokaryakram>.Create(query, pageIndex, pageSize);

        }

        public async Task<SubsidyReportModel> GetSubsidyReportModel(string id, string fiscalYear, string localLevel, string budgetId, int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = _anudanRepository.Table;
            if (!String.IsNullOrEmpty(id))
            {
                query = query.Where(m => m.CreatedBy == id);
            }
            if (!string.IsNullOrEmpty(fiscalYear))
            {
                query = query.Where(m => m.FiscalYear.Id == fiscalYear);
            }

            if (!string.IsNullOrEmpty(budgetId))
            {
                query = query.Where(m => m.BudgetId == budgetId);
            }
            if (!string.IsNullOrEmpty(localLevel))
            {
                query = query.Where(m => m.LocalLevel == localLevel);
            }

            var filteredAnudan = query.AsEnumerable().ToList();

            var distinctBudget = filteredAnudan.ToList().Select(m => m.BudgetId).Distinct();

            var subsidy = new SubsidyReportModel();
            subsidy.FiscalYear = filteredAnudan.ToList().FirstOrDefault().FiscalYear.NepaliFiscalYear;
            subsidy.LocalLevel = localLevel;
            subsidy.Level = "नगर कार्यपालिकाको कार्यालय";
            subsidy.StartDate = "";
            subsidy.EndDate = "";
            subsidy.Address = "काठमाडौँ";
            subsidy.Ward = "";
            var lstRowData = new List<SubsidyRowData>();
            int i = 1;
            foreach (var item in distinctBudget)
            {
                var objData = filteredAnudan.ToList().FirstOrDefault(m => m.BudgetId == item);
                var objSubsidyData = new SubsidyRowData {
                    BudgetTitle = objData.Budget.ActivityName,
                    MainActivity = objData.ExpectedOutput,
                    Remarks = objData.Remaks,
                    SN = i.ToString()
                };
                lstRowData.Add(objSubsidyData);
                i++;
            }

            subsidy.Rows = lstRowData;

            return subsidy;

        }

        public async Task<IPagedList<AanudanKokaryakram>> GetFilteredLabambitKrishak(string id, string fiscalYear, string programType, string type, int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = _anudanRepository.Table;
            if (!string.IsNullOrEmpty(id))
            {
                query = query.Where(m => m.CreatedBy == id);
            }
            if (!string.IsNullOrEmpty(fiscalYear))
            {
                query = query.Where(m=> m.FiscalyearId == fiscalYear);
            }
            if (!string.IsNullOrEmpty(programType))
            {
                query = query.Where(m => m.Budget.SourceOfFund== programType);
            }
            if (!string.IsNullOrEmpty(type))
            {
                query = query.Where(m => m.Budget.TypeOfExpen == type);
            }

            return await PagedList<AanudanKokaryakram>.Create(query, pageIndex, pageSize);

        }
        public async Task<IPagedList<AanudanKokaryakram>> GetFilteredLabambitKrishak(List<string> id, string fiscalYear, string programType, string type, int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = _anudanRepository.Table;
            query = query.Where(m =>id.Contains(m.CreatedBy));
            if (!string.IsNullOrEmpty(fiscalYear))
            {
                query = query.Where(m => m.FiscalYear.Id == fiscalYear);

            }
            if (!string.IsNullOrEmpty(type))
            {
                query = query.Where(m => m.PujigatKharchaKharakram.Type == type);
            }
            if (!string.IsNullOrEmpty(programType))
            {
                query = query.Where(m => m.PujigatKharchaKharakram.ProgramType == programType);
            }

            return await PagedList<AanudanKokaryakram>.Create(query, pageIndex, pageSize);

        }

        public async Task<IPagedList<AanudanKokaryakram>> GetbaliRegister(List<string> createdby, int pageIndex = 0, int pageSize = int.MaxValue, string fiscalyear = "")
        {
            var query = _anudanRepository.Table;
            query = query.Where(m => createdby.Contains(m.CreatedBy));
            if (!string.IsNullOrEmpty(fiscalyear))
            {
                query = query.Where(
                  m => m.FiscalYear.Id == fiscalyear
                );
            }

            return await PagedList<AanudanKokaryakram>.Create(query, pageIndex, pageSize);
        }

        public Task<AanudanKokaryakram> GetbaliRegisterById(string id)
        {
            return _anudanRepository.GetByIdAsync(id);
        }

        public async Task InsertbaliRegister(AanudanKokaryakram baliRegister)
        {
            if (baliRegister == null)
                throw new ArgumentNullException("Livestock");

            await _anudanRepository.InsertAsync(baliRegister);

            //event notification
            await _mediator.EntityInserted(baliRegister);
        }

        public Task InsertbaliRegisterList(List<AanudanKokaryakram> baliRegisters)
        {
            throw new NotImplementedException();
        }

        public async Task UpdatebaliRegister(AanudanKokaryakram baliRegister)
        {
            if (baliRegister == null)
                throw new ArgumentNullException("baliregister");

            await _anudanRepository.UpdateAsync(baliRegister);

            //event notification
            await _mediator.EntityUpdated(baliRegister);
        }

        public Task UpdatebaliRegisterList(List<AanudanKokaryakram> baliRegisters)
        {
            throw new NotImplementedException();
        }
    }
}
