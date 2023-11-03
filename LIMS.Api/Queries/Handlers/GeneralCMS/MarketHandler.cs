using LIMS.Api.DTOs;
using LIMS.Api.Extensions;
using LIMS.Api.Queries.Models.Common;
using LIMS.Services.Bali;
using LIMS.Services.Basic;
using LIMS.Services.Breed;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LIMS.Api.Queries.Handlers.GeneralCMS
{
    public class MarketHandler : IRequestHandler<GetQueryCMS<MarketDto>, IList<MarketDto>>
    {
        private readonly IMarketDataService _SoilService;
        private readonly IFiscalYearService _fiscalYearService;
        private readonly IBreedService _breedService;

        public MarketHandler(IMarketDataService SoilService, IFiscalYearService fiscalYearService, IBreedService breedService)
        {
            _SoilService = SoilService;
            _fiscalYearService = fiscalYearService;
            _breedService = breedService;
        }
        public async Task<IList<MarketDto>> Handle(GetQueryCMS<MarketDto> request, CancellationToken cancellationToken)
        {
           
                var mainMenus = await _SoilService.GetmarketData();
                mainMenus.ToList();
                List<MarketDto> r = new List<MarketDto>();
                foreach (var item in mainMenus.ToList().OrderByDescending(m => m.CreatedAt))
                {
                    MarketDto re = new MarketDto();
                    re = item.ToModel();
                try
                {
                    re.UnitId = _fiscalYearService.GetFiscalYearById(item.FiscalYearId).Result.CurrentFiscalYear.ToString();
                }
                catch
                {
                    re.UnitId = "false";
                }
                re.FiscalYearId =  _fiscalYearService.GetFiscalYearById(item.FiscalYearId).Result.NepaliFiscalYear;
               
                    re.BreedId = _breedService.GetBreedById(item.BreedId).Result.EnglishName;

                r.Add(re);
                }
                return r;
            
        }
    }
}
