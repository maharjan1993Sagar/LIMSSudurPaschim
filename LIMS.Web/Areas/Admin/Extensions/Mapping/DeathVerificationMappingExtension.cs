using LIMS.Domain.Bali;
using LIMS.Web.Areas.Admin.Models.Bali;
using LIMS.Web.Areas.Admin.Models.Bali.Aanudan;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Web.Areas.Admin.Extensions.Mapping
{
    public static class DeathVerificationMappingExtension
    {
        public static DeathVerificationModel ToModel(this DeathVerification entity)
        {
            return entity.MapTo<DeathVerification, DeathVerificationModel>();
        }

        public static DeathVerification ToEntity(this DeathVerificationModel model)
        {
            return model.MapTo<DeathVerificationModel, DeathVerification>();
        }       
        public static DeathVerification ToEntity(this DeathVerificationModel model, DeathVerification destination)
        {
            return model.MapTo(destination);
        }
    }
}
