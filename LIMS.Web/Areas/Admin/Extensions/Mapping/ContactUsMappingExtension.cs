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
    public static class ContactUsMappingExtension
    {
        public static ContactUsModel ToModel(this ContactUs entity)
        {
            return entity.MapTo<ContactUs, ContactUsModel>();
        }

        public static ContactUs ToEntity(this ContactUsModel model)
        {
            return model.MapTo<ContactUsModel, ContactUs>();
        }

        public static ContactUs ToEntity(this ContactUsModel model, ContactUs destination)
        {
            return model.MapTo(destination);
        }
    }
}
