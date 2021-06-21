using LIMS.Domain.Users;
using LIMS.Web.Areas.Admin.Models.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Web.Areas.Admin.Extensions.Mapping
{
    public static class VaccinationUserMappingExtension
    {
        public static VaccinationUserModel ToModel(this VaccinationUser entity)
        {
            return entity.MapTo<VaccinationUser, VaccinationUserModel>();
        }

        public static VaccinationUser ToEntity(this VaccinationUserModel model)
        {
            return model.MapTo<VaccinationUserModel, VaccinationUser>();
        }
        public static VaccinationUser ToEntity(this VaccinationUserModel model, VaccinationUser destination)
        {
            return model.MapTo(destination);
        }
    }
}
