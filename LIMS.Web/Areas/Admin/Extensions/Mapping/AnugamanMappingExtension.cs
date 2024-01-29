using LIMS.Domain.Bali;
using LIMS.Domain.BasicSetup;
using LIMS.Web.Areas.Admin.Models.Bali;
using LIMS.Web.Areas.Admin.Models.BasicSetup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Web.Areas.Admin.Extensions.Mapping
{
    public static class AnugamanMappingExtension
    {
        public static AnugamanModel ToModel(this Anugaman entity)
        {
            return entity.MapTo<Anugaman, AnugamanModel>();
        }

        public static Anugaman ToEntity(this AnugamanModel model)
        {
            return model.MapTo<AnugamanModel, Anugaman>();
        }

        public static Anugaman ToEntity(this AnugamanModel model, Anugaman destination)
        {
            return model.MapTo(destination);
        }
    }
}
