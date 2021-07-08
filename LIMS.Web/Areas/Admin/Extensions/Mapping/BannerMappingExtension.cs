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
    public static class BannerMappingExtension
    {
        public static BannerModel ToModel(this Banner entity)
        {
            return entity.MapTo<Banner, BannerModel>();
        }

        public static Banner ToEntity(this BannerModel model)
        {
            return model.MapTo<BannerModel, Banner>();
        }

        public static Banner ToEntity(this BannerModel model, Banner destination)
        {
            return model.MapTo(destination);
        }
    }
}
