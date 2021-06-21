using AutoMapper;
using LIMS.Domain.Logging;
using LIMS.Core.Infrastructure.Mapper;
using LIMS.Web.Areas.Admin.Models.Logging;

namespace LIMS.Web.Areas.Admin.Infrastructure.Mapper.Profiles
{
    public class LogProfile : Profile, IMapperProfile
    {
        public LogProfile()
        {
            CreateMap<Log, LogModel>()
                .ForMember(dest => dest.CustomerEmail, mo => mo.Ignore())
                .ForMember(dest => dest.CreatedOn, mo => mo.Ignore());

            CreateMap<LogModel, Log>()
                .ForMember(dest => dest.Id, mo => mo.Ignore())
                .ForMember(dest => dest.CreatedOnUtc, mo => mo.Ignore())
                .ForMember(dest => dest.LogLevelId, mo => mo.Ignore());
        }

        public int Order => 0;
    }
}