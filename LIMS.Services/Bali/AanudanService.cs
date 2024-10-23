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
        private readonly IRepository<AanudanKokaryakram> _baliRegisterRepository;
        private readonly IMediator _mediator;
        public AanudanService(IRepository<AanudanKokaryakram> baliRegisterRepository, IMediator mediator)
        {
            _baliRegisterRepository = baliRegisterRepository;
            _mediator = mediator;
        }
        public async Task DeletebaliRegister(AanudanKokaryakram baliRegister)
        {
            if (baliRegister == null)
                throw new ArgumentNullException("AanudanKokaryakram");

            await _baliRegisterRepository.DeleteAsync(baliRegister);

            //event notification
            await _mediator.EntityDeleted(baliRegister);
        }

        public async Task<IPagedList<AanudanKokaryakram>> GetbaliRegister(string createdby, int pageIndex = 0, int pageSize = int.MaxValue, string fiscalyear = "")
        {
            var query = _baliRegisterRepository.Table;
            query = query.Where(m => m.CreatedBy == createdby);
            if (!string.IsNullOrEmpty(fiscalyear))
            {
                query = query.Where(
                  m => m.FiscalYear.Id == fiscalyear
                );
            }

            return await PagedList<AanudanKokaryakram>.Create(query, pageIndex, pageSize);
        }
        
         public async Task<IPagedList<AanudanKokaryakram>> GetFilteredSubsidy(string id, string fiscalYear, string district, string program, int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = _baliRegisterRepository.Table;
            query = query.Where(m => m.CreatedBy == id);
            if(!String.IsNullOrEmpty(fiscalYear))
            {
                query = query.Where(m => m.FiscalYear.Id == fiscalYear || m.FiscalyearId == fiscalYear);
            }

            if (!string.IsNullOrEmpty(program))
            {
                query = query.Where(m => m.PujigatKharchaKharakram.Id == program);
            }
            if (!string.IsNullOrEmpty(district))
            {
                query = query.Where(m => m.District == district);
            }

            return await PagedList<AanudanKokaryakram>.Create(query, pageIndex, pageSize);

        }
        public async Task<SubsidyReportModel> GetSubsidyReportModel(string id, string fiscalYear, string localLevel, string budgetId, int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = _baliRegisterRepository.Table;
            if (!String.IsNullOrEmpty(id))
            {
                query = query.Where(m => m.CreatedBy == id);
            }
            if (!string.IsNullOrEmpty(fiscalYear))
            {
                query = query.Where(m => m.FiscalYear.Id == fiscalYear || m.FiscalyearId == fiscalYear);
            }

            if (!string.IsNullOrEmpty(budgetId))
            {
                query = query.Where(m => m.PujigatKharchaKaryakramId == budgetId);
            }
            if (!string.IsNullOrEmpty(localLevel))
            {
                query = query.Where(m => m.LocalLevel == localLevel);
            }

            var filteredAnudan = query.AsEnumerable().ToList();

            var distinctBudget = filteredAnudan.ToList().Select(m => m.PujigatKharchaKaryakramId).Distinct();

            var subsidy = new SubsidyReportModel();
            subsidy.FiscalYear = filteredAnudan.ToList().FirstOrDefault().FiscalYear.NepaliFiscalYear;
            subsidy.LocalLevel = localLevel;
            subsidy.Level = "नगर कार्यपालिकाको कार्यालय";
            subsidy.StartDate = "";
            subsidy.EndDate = "";
            subsidy.Address = "";
            subsidy.Ward = "";
            var lstRowData = new List<SubsidyRowData>();
            int i = 1;
            foreach (var item in distinctBudget)
            {
                var objData = filteredAnudan.ToList().FirstOrDefault(m => m.PujigatKharchaKaryakramId == item);
                var objSubsidyData = new SubsidyRowData {
                    BudgetTitle = objData.PujigatKharchaKharakram.Program,
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
            var query = _baliRegisterRepository.Table;
            query = query.Where(m => m.CreatedBy == id);
            if (!string.IsNullOrEmpty(fiscalYear))
            {
                query = query.Where(m=> m.FiscalYear.Id == fiscalYear);

            }
            if (!string.IsNullOrEmpty(type))
            {
                query = query.Where(m => m.PujigatKharchaKharakram.Type== type);
            }
            if (!string.IsNullOrEmpty(programType))
            {
                query = query.Where(m => m.PujigatKharchaKharakram.ProgramType.ToLower() == programType.ToLower());
            }

            return await PagedList<AanudanKokaryakram>.Create(query, pageIndex, pageSize);

        }
        public async Task<IPagedList<AanudanKokaryakram>> GetFilteredLabambitKrishak(List<string> id, string fiscalYear, string programType, string type, int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = _baliRegisterRepository.Table;
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
                query = query.Where(m => m.PujigatKharchaKharakram.ProgramType.ToLower() == programType.ToLower());
            }

            return await PagedList<AanudanKokaryakram>.Create(query, pageIndex, pageSize);

        }

        public async Task<IPagedList<AanudanKokaryakram>> GetbaliRegister(List<string> createdby, int pageIndex = 0, int pageSize = int.MaxValue, string fiscalyear = "")
        {
            var query = _baliRegisterRepository.Table;
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
            return _baliRegisterRepository.GetByIdAsync(id);
        }

        public async Task InsertbaliRegister(AanudanKokaryakram baliRegister)
        {
            if (baliRegister == null)
                throw new ArgumentNullException("Livestock");

            await _baliRegisterRepository.InsertAsync(baliRegister);

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

            await _baliRegisterRepository.UpdateAsync(baliRegister);

            //event notification
            await _mediator.EntityUpdated(baliRegister);
        }

        public Task UpdatebaliRegisterList(List<AanudanKokaryakram> baliRegisters)
        {
            throw new NotImplementedException();
        }
    }
}
