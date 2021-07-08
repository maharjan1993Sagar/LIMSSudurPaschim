using LIMS.Domain.Breed;
using LIMS.Domain.GeneralCMS;
using LIMS.Web.Areas.Admin.Models.Breed;
using LIMS.Web.Areas.Admin.Models.GeneralCMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Web.Areas.Admin.Extensions.Mapping
{
    public static class PageContentMappingExtension
    {
        public static PageContentModel ToModel(this PageContent entity)
        {
            return entity.MapTo<PageContent, PageContentModel>();
        }

        public static PageContent ToEntity(this PageContentModel model)
        {
            return model.MapTo<PageContentModel, PageContent>();
        }

        public static PageContent ToEntity(this PageContentModel model, PageContent destination)
        {
            return model.MapTo(destination);
        }
    }
}
