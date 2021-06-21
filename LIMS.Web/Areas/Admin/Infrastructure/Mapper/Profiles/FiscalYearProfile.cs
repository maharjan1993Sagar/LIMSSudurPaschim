using AutoMapper;
using LIMS.Core.Infrastructure.Mapper;
using LIMS.Domain.BasicSetup;
using LIMS.Domain.BesicSetup;
using LIMS.Domain.Professionals;
using LIMS.Web.Areas.Admin.Models.BasicSetup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Web.Areas.Admin.Infrastructure.Mapper.Profiles
{
    public class FiscalYearProfile: Profile, IMapperProfile
    {
        public FiscalYearProfile()
        {
            CreateMap<FiscalyearModel, FiscalYear>().ReverseMap();
            CreateMap<FiscalYearForGraphModel, FiscalYearForGraphSetup>().ReverseMap();

        }


        public int Order => 0;
    }
}
