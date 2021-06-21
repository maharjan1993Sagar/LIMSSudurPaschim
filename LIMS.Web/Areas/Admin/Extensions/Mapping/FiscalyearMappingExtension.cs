using LIMS.Domain.BesicSetup;
using LIMS.Web.Areas.Admin.Models.BasicSetup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Web.Areas.Admin.Extensions.Mapping
{
    public static class FiscalyearMappingExtension
    {
        public static FiscalyearModel ToModel(this FiscalYear entity)
        {
            return entity.MapTo<FiscalYear, FiscalyearModel>();
        }

        public static FiscalYear ToEntity(this FiscalyearModel model)
        {
            return model.MapTo<FiscalyearModel, FiscalYear>();
        }

        public static FiscalYear ToEntity(this FiscalyearModel model, FiscalYear destination)
        {
            return model.MapTo(destination);
        }
    }
}
