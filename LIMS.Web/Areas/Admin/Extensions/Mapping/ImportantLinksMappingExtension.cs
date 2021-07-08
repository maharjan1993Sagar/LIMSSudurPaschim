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
    public static class ImportantLinksMappingExtension
    {
        public static ImportantLinksModel ToModel(this ImportantLinks entity)
        {
            return entity.MapTo<ImportantLinks, ImportantLinksModel>();
        }

        public static ImportantLinks ToEntity(this ImportantLinksModel model)
        {
            return model.MapTo<ImportantLinksModel, ImportantLinks>();
        }

        public static ImportantLinks ToEntity(this ImportantLinksModel model, ImportantLinks destination)
        {
            return model.MapTo(destination);
        }
    }
}
