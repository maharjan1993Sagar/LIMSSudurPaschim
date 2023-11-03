using LIMS.Domain;
using LIMS.Domain.Bali;
using LIMS.Domain.Data;
using LIMS.Services.Events;
using MediatR;
using MongoDB.Driver.Linq;
using System;
using System.Collections.Generic;
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
            
                query = query.Where(m => m.FiscalYear.Id == fiscalYear);

            
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
                query = query.Where(m => m.PujigatKharchaKharakram.ProgramType == programType);
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
                query = query.Where(m => m.PujigatKharchaKharakram.ProgramType == programType);
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
