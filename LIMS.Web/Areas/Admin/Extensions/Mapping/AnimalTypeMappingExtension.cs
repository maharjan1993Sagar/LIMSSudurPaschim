using LIMS.Domain.Breed;
using LIMS.Web.Areas.Admin.Models.Breed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Web.Areas.Admin.Extensions.Mapping
{
    public static class AnimalTypeMappingExtension
    {
        public static AnimalTypeModel ToModel(this AnimalType entity)
        {
            return entity.MapTo<AnimalType, AnimalTypeModel>();
        }

        public static AnimalType ToEntity(this AnimalTypeModel model)
        {
            return model.MapTo<AnimalTypeModel, AnimalType>();
        }

        public static AnimalType ToEntity(this AnimalTypeModel model, AnimalType destination)
        {
            return model.MapTo(destination);
        }
    }
}
