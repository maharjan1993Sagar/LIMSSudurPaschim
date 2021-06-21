using LIMS.Domain.Breed;
using LIMS.Domain.Users;
using LIMS.Web.Areas.Admin.Models.Breed;
using LIMS.Web.Areas.Admin.Models.MoAMAC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Web.Areas.Admin.Extensions.Mapping
{
    public static class NlboUserMappingExtension
    {
        public static NlboUserModel ToModel(this NlboUser entity)
        {
            return entity.MapTo<NlboUser, NlboUserModel>();
        }

        public static NlboUser ToEntity(this NlboUserModel model)
        {
            return model.MapTo<NlboUserModel, NlboUser>();
        }

        public static NlboUser ToEntity(this NlboUserModel model, NlboUser destination)
        {
            return model.MapTo(destination);
        }
    }
}
