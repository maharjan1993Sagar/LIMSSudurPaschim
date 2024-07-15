using LIMS.Domain;
using LIMS.Domain.Bali;
using LIMS.Domain.BasicSetup;
using LIMS.Domain.BesicSetup;
using LIMS.Domain.Data;
using LIMS.Services.Events;
using MediatR;
using MongoDB.Driver.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LIMS.Services.Basic
{
    public class PujigatKharchaKharakramService : IPujigatKharchaKharakramService
    {
        private readonly IRepository<PujigatKharchaKharakram> _pujigatKharchaKharakramRepository;
        private readonly IRepository<MainActivityCode> _mainActivityCodeRepository;
        private readonly IMediator _mediator;
        public PujigatKharchaKharakramService(IRepository<PujigatKharchaKharakram> pujigatKharchaKharakramRepository,
                                               IRepository<MainActivityCode> mainActivityCodeRepository, IMediator mediator)
        {
            _pujigatKharchaKharakramRepository = pujigatKharchaKharakramRepository;
            _mainActivityCodeRepository = mainActivityCodeRepository;
            _mediator = mediator;
        }
        public async Task DeletePujigatKharchaKharakram(PujigatKharchaKharakram pujigatKharchaKharakram)
        {
            if (pujigatKharchaKharakram == null)
                throw new ArgumentNullException("pujigatKharchaKharakram");

            await _pujigatKharchaKharakramRepository.DeleteAsync(pujigatKharchaKharakram);

            //event notification
            await _mediator.EntityDeleted(pujigatKharchaKharakram);
        }

        public async Task<IPagedList<PujigatKharchaKharakram>> GetPujigatKharchaKharakram(int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = _pujigatKharchaKharakramRepository.Table;


            return await PagedList<PujigatKharchaKharakram>.Create(query, pageIndex, pageSize);
        }

        public async Task<IPagedList<PujigatKharchaKharakram>> GetPujigatKharchaKharakram(string createdby, string keyword = "", int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = _pujigatKharchaKharakramRepository.Table;
            query = query.Where(m => m.CreatedBy == createdby);
            if (!string.IsNullOrEmpty(keyword))
            {
                query = query.Where(m => m.Limbis_Code.Contains(keyword) || m.kharchaCode.Contains(keyword) || m.Program.Contains(keyword));
            }
            return await PagedList<PujigatKharchaKharakram>.Create(query, pageIndex, pageSize);
        }
        public async Task<IPagedList<PujigatKharchaKharakram>> GetPujigatKharchaKharakramSelect(string createdby, string keyword = "", int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = _pujigatKharchaKharakramRepository.Table;
            var queryCodes = _mainActivityCodeRepository.Table;
            var mainActivity = queryCodes.Select(m => m.Limbis_Code)
                                           .Distinct().ToList();

            query = query.Where(m => m.CreatedBy == createdby && mainActivity.Contains(m.kharchaCode));
           
            //query = query.Where(m => m.CreatedBy == createdby && (m.kharchaCode == "22522" || m.kharchaCode == "26413" || m.kharchaCode == "26423" || m.kharchaCode == "31159"
            //|| m.kharchaCode == "22512"
            //));
          
            if (!string.IsNullOrEmpty(keyword))
            {
                query = query.Where(m => m.Limbis_Code.Contains(keyword) || m.kharchaCode.Contains(keyword) || m.Program.Contains(keyword));
            }
            return await PagedList<PujigatKharchaKharakram>.Create(query, pageIndex, pageSize);
        }



        public async  Task<IPagedList<PujigatKharchaKharakram>> GetPujigatKharchaKharakram(List<string> createdby, int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = _pujigatKharchaKharakramRepository.Table;
            query = query.Where(m =>createdby.Contains(m.CreatedBy));

            return await PagedList<PujigatKharchaKharakram>.Create(query, pageIndex, pageSize);
        }

        //public async Task<IPagedList<PujigatKharchaKharakram>> GetPujigatKharchaKharakram(List<string> createdby, string programtype, string type,string fiscalyear, int pageIndex = 0, int pageSize = int.MaxValue)
        //{
        //    var query = _pujigatKharchaKharakramRepository.Table;
        //    query = query.Where(m => createdby.Contains(m.CreatedBy)&&m.FiscalYear.Id==fiscalyear&&m.ProgramType==programtype&&m.Type==type);

        //    return await PagedList<PujigatKharchaKharakram>.Create(query, pageIndex, pageSize);
        //}

        public async Task<IPagedList<PujigatKharchaKharakram>> GetPujigatKharchaKharakram(
            string createdby,
           string fiscalYear,
            string programtype="", 
            string type="",
            
            int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = _pujigatKharchaKharakramRepository.Table;
            query = query.Where(m => m.CreatedBy== createdby && m.FiscalYear.Id == fiscalYear );
            if(!string.IsNullOrEmpty(programtype))
            {
                query=query.Where(m => m.ProgramType == programtype);
            }
            if (!string.IsNullOrEmpty(type))
            {
                query= query.Where(m => m.Type == type);
            }

            return await PagedList<PujigatKharchaKharakram>.Create(query, pageIndex, pageSize);
        }

        public async Task<IPagedList<PujigatKharchaKharakram>> GetNitigatKharakram(
          string createdby,
         string fiscalYear,
          string programtype = "",
          string type = "",

          int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = _pujigatKharchaKharakramRepository.Table;
            query = query.Where(m => m.CreatedBy == createdby && !string.IsNullOrEmpty(m.IsNitiTathaKaryaKram) && m.FiscalYear.Id == fiscalYear);
            if (!string.IsNullOrEmpty(programtype))
            {
                query = query.Where(m => m.ProgramType == programtype);
            }
            if (!string.IsNullOrEmpty(type))
            {
                query = query.Where(m => m.Type == type);
            }

            return await PagedList<PujigatKharchaKharakram>.Create(query, pageIndex, pageSize);
        }
        public async Task<IPagedList<PujigatKharchaKharakram>> GetMainKharakram(
          string createdby,
         string fiscalYear,
          string programtype = "",
          string type = "",

          int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = _pujigatKharchaKharakramRepository.Table;
            var queryCodes = _mainActivityCodeRepository.Table;
            var mainActivity = queryCodes.Select(m => m.Limbis_Code).Distinct().ToList();
            query = query.Where(m => m.CreatedBy == createdby && mainActivity.Contains(m.kharchaCode) && m.FiscalYear.Id == fiscalYear);

            //query = query.Where(m => m.CreatedBy == createdby &&(m.kharchaCode== "22512" || m.kharchaCode=="22522" || m.kharchaCode=="26413"||m.kharchaCode== "26423") && m.FiscalYear.Id == fiscalYear);
            if (!string.IsNullOrEmpty(programtype))
            {
                query = query.Where(m => m.ProgramType == programtype);
            }
            if (!string.IsNullOrEmpty(type))
            {
                query = query.Where(m => m.Type == type);
            }

            return await PagedList<PujigatKharchaKharakram>.Create(query, pageIndex, pageSize);
        }
        public async Task<IPagedList<PujigatKharchaKharakram>> GetPujigatKharchaKharakram(
           List<string> createdby,
          string fiscalYear,
           string programtype = "",
           string type = "",

           int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = _pujigatKharchaKharakramRepository.Table;
            query = query.Where(m => createdby.Contains(m.CreatedBy)  && m.FiscalYear.Id == fiscalYear);
            if (!string.IsNullOrEmpty(programtype))
            {
                query = query.Where(m => m.ProgramType == programtype);
            }
            if (!string.IsNullOrEmpty(type))
            {
                query = query.Where(m => m.Type == type);
            }

            return await PagedList<PujigatKharchaKharakram>.Create(query, pageIndex, pageSize);
        }

        public Task<PujigatKharchaKharakram> GetPujigatKharchaKharakramById(string Id)
        {
            return _pujigatKharchaKharakramRepository.GetByIdAsync(Id);

        }
        public async Task<bool> GetPujigatKharchaKharakramByLmBIsCode(string Id)
        {
            var query= _pujigatKharchaKharakramRepository.Table;
            query =  query.Where(m => m.Limbis_Code == Id);
           if(await query.CountAsync()>0)
            {
                return true;
            }

            return false;
        }
        

        public async Task InsertPujigatKharchaKharakram(PujigatKharchaKharakram pujigatKharchaKharakram)
        {
            if (pujigatKharchaKharakram == null)
                throw new ArgumentNullException("pujigatKharchaKharakram");

            await _pujigatKharchaKharakramRepository.InsertAsync(pujigatKharchaKharakram);

            //event notification
            await _mediator.EntityInserted(pujigatKharchaKharakram);
        }

        public async Task UpdatePujigatKharchaKharakram(PujigatKharchaKharakram pujigatKharchaKharakram)
        {
            if (pujigatKharchaKharakram == null)
                throw new ArgumentNullException("pujigatKharchaKharakram");

            await _pujigatKharchaKharakramRepository.UpdateAsync(pujigatKharchaKharakram);

            //event notification
            await _mediator.EntityUpdated(pujigatKharchaKharakram);
        }

        public async Task<List<string>> GetLimbis_Code()
        {
            var query =_pujigatKharchaKharakramRepository.Table;

            var distinctCode = query.Select(m => m.kharchaCode).ToList();

            distinctCode = distinctCode.Where(m => !String.IsNullOrEmpty(m))
                            .Distinct().ToList();

            return distinctCode;

        }

    }
}
