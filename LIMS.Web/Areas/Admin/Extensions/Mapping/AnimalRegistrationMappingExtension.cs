using LIMS.Domain.AInR;
using LIMS.Web.Areas.Admin.Models.AInR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Web.Areas.Admin.Extensions.Mapping
{
    public static class AnimalRegistrationMappingExtension
    {
        public static AnimalRegistrationModel ToModel(this AnimalRegistration entity)
        {
            return entity.MapTo<AnimalRegistration, AnimalRegistrationModel>();
        }

        public static AnimalRegistration ToEntity(this AnimalRegistrationModel model)
        {
            return model.MapTo<AnimalRegistrationModel, AnimalRegistration>();
        }
        public static AnimalRegistration ToEntity(this AnimalRegistrationModel model, AnimalRegistration destination)
        {
            return model.MapTo(destination);
        }
    }
}
