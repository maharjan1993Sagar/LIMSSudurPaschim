using LIMS.Domain.Breed;
using LIMS.Domain.GeneralCMS;
using LIMS.Web.Areas.Admin.Models.GeneralCMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Web.Areas.Admin.Extensions.Mapping
{
    public static class GalleryMappingExtension
    {
        public static GalleryModel ToModel(this Gallery entity)
        {
            return entity.MapTo<Gallery, GalleryModel>();
        }

        public static Gallery ToEntity(this GalleryModel model)
        {
            return model.MapTo<GalleryModel, Gallery>();
        }

        public static Gallery ToEntity(this GalleryModel model, Gallery destination)
        {
            return model.MapTo(destination);
        }
    }
}
