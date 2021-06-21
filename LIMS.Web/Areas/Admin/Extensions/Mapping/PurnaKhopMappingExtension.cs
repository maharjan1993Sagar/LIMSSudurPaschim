using LIMS.Domain.AnimalHealth;
using LIMS.Web.Areas.Admin.Models.AnimalHealth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Web.Areas.Admin.Extensions.Mapping
{
    public static class PurnaKhopMappingExtension
    {
        public static PurnaKhopModel ToModel(this PurnaKhop entity)
        {
            return entity.MapTo<PurnaKhop, PurnaKhopModel>();
        }

        public static PurnaKhop ToEntity(this PurnaKhopModel model)
        {
            return model.MapTo<PurnaKhopModel, PurnaKhop>();
        }

        public static PurnaKhop ToEntity(this PurnaKhopModel model, PurnaKhop destination)
        {
            return model.MapTo(destination);
        }
    }
}
