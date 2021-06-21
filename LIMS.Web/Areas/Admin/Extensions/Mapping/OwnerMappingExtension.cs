using LIMS.Domain.AInR;
using LIMS.Web.Areas.Admin.Models.AInR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Web.Areas.Admin.Extensions.Mapping
{
    public static  class OwnerMappingExtension
    {
        public static OwnerModel ToModel(this Owner entity)
        {
            return entity.MapTo<Owner, OwnerModel>();
        }

        public static Owner ToEntity(this OwnerModel model)
        {
            return model.MapTo<OwnerModel, Owner>();
        }
        public static Owner ToEntity(this OwnerModel model, Owner destination)
        {
            return model.MapTo(destination);
        }
    }
}
