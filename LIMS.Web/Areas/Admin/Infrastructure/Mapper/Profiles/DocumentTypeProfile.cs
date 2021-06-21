using AutoMapper;
using LIMS.Domain.Documents;
using LIMS.Core.Infrastructure.Mapper;
using LIMS.Web.Areas.Admin.Models.Documents;

namespace LIMS.Web.Areas.Admin.Infrastructure.Mapper.Profiles
{
    public class DocumentTypeProfile : Profile, IMapperProfile
    {
        public DocumentTypeProfile()
        {
            CreateMap<DocumentType, DocumentTypeModel>();
            CreateMap<DocumentTypeModel, DocumentType>()
                .ForMember(dest => dest.Id, mo => mo.Ignore());
        }

        public int Order => 0;
    }
}