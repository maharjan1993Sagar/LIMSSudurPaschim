using AutoMapper;
using LIMS.Api.DTOs.FeedBack;
using LIMS.Core.Infrastructure.Mapper;
using LIMS.Domain.Feedback;

namespace LIMS.Api.Infrastructure.Mapper.Profiles
{
    public class FeedbackProfile: Profile, IMapperProfile
    {
        public FeedbackProfile()
        {
            CreateMap<Feedback, FeedbackDto>().ReverseMap();
        }

        public int Order { get; set; } = 0;
    }
}
