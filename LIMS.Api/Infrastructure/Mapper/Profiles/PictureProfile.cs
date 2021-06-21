using AutoMapper;
using LIMS.Api.DTOs.Common;
using LIMS.Domain.Media;
using LIMS.Core.Infrastructure.Mapper;

namespace LIMS.Api.Infrastructure.Mapper
{
    public class PictureProfile : Profile, IMapperProfile
    {
        public PictureProfile()
        {
            CreateMap<PictureDto, Picture>()
                .ForMember(dest => dest.GenericAttributes, mo => mo.Ignore());

            CreateMap<Picture, PictureDto>()
                .ForMember(dest => dest.PictureBinary, mo => mo.Ignore());
        }

        public int Order => 1;
    }
}