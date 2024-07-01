using LIMS.Domain;
using LIMS.Domain.Data;
using LIMS.Domain.LocalStructure;
using MediatR;
using MongoDB.Driver.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using System.Data;
namespace LIMS.Services.LocalStructure
{
    public  class LocalLevelService:ILocalLevelService
    {
        private readonly IRepository<LocalLevels> _localLevelsRepositoty;
        private readonly IMediator _mediator;
        public LocalLevelService(IRepository<LocalLevels> localLevelsRepositoty, IMediator mediator)
        {
            _localLevelsRepositoty = localLevelsRepositoty;
            _mediator = mediator;
        }

        public async Task<IList<string>> GetDistrict(string province)
        {
            var query = _localLevelsRepositoty.Table;
            var all = query.ToList();            
            if (!String.IsNullOrEmpty(province))
            {
                all = query.Where(m => m.Province == province).ToList();               
            }

            var district= all.Select(m=>m.District.Trim()).Distinct().ToList();
            return district;          
        }

        public async Task<IList<string>> GetLocalLevel(string district)
        {
            var query = _localLevelsRepositoty.Table;
            if (!String.IsNullOrEmpty(district))
            {
                query = query.Where(m => m.District == district);
            }
            var local = query.Select(m => m.Municipality).Distinct().ToList();
            return local;
        }

        public async Task<IList<string>> GetProvience()
        {

            var query = _localLevelsRepositoty.Table;
            var province = query.Select(m => m.Province).Where(m=>m!=null&& m!="").Distinct().ToList();
            return province;

            ////return await PagedList<string>.Create(query, pageIndex, pageSize);
        }
        public async Task<IList<LocalLevels>> GetAllProvience()
        {
            var query = _localLevelsRepositoty.Table;
            var provience = query.ToList();

            return provience; 
        }
        public string GetNepaliDistrict(string district)
        {
            var query = _localLevelsRepositoty.Table;
            var provience = query.Where(m => m.District == district).FirstOrDefault();
            if(provience!=null)
            {
                return provience.DistrictNepali;
            }
            

            return district;
        }
    }
}
