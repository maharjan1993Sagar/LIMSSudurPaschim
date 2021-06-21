using AutoMapper;
using LIMS.Domain.Documents;
using LIMS.Core.Infrastructure.Mapper;
using LIMS.Web.Areas.Admin.Models.Documents;
using System.Collections.Generic;
using System.Linq;

namespace LIMS.Web.Areas.Admin.Infrastructure.Mapper.Profiles
{
    public class DocumentProfile : Profile, IMapperProfile
    {
        public DocumentProfile()
        {
            CreateMap<Document, DocumentModel>()
               .ForMember(dest => dest.AvailableStores, mo => mo.Ignore())
               .ForMember(dest => dest.SelectedStoreIds, mo => mo.Ignore())
               .ForMember(dest => dest.AvailableCustomerRoles, mo => mo.Ignore())
               .ForMember(dest => dest.SelectedCustomerRoleIds, mo => mo.Ignore())
               .ForMember(dest => dest.AvailableDocumentTypes, mo => mo.Ignore());
            CreateMap<DocumentModel, Document>()
               .ForMember(dest => dest.CreatedOnUtc, mo => mo.Ignore())
               .ForMember(dest => dest.UpdatedOnUtc, mo => mo.Ignore())
               .ForMember(dest => dest.CustomerRoles, mo => mo.MapFrom(x => x.SelectedCustomerRoleIds != null ? x.SelectedCustomerRoleIds.ToList() : new List<string>()))
               .ForMember(dest => dest.Stores, mo => mo.MapFrom(x => x.SelectedStoreIds != null ? x.SelectedStoreIds.ToList() : new List<string>()))
               .ForMember(dest => dest.Id, mo => mo.Ignore());
        }

        public int Order => 0;
    }
}