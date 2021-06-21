using LIMS.Domain.BasicSetup;
using LIMS.Web.Areas.Admin.Models.BasicSetup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Web.Areas.Admin.Extensions.Mapping
{
    public static class FiscalYearGraphMappingExtension
    {
        public static FiscalYearForGraphSetup ToEntity(this FiscalYearForGraphModel model)
        {
            return model.MapTo<FiscalYearForGraphModel, FiscalYearForGraphSetup>();
        }
        public static FiscalYearForGraphModel ToModel(this FiscalYearForGraphSetup model)
        {
            return model.MapTo<FiscalYearForGraphSetup,FiscalYearForGraphModel>();
        }
        public static FiscalYearForGraphSetup ToEntity(this FiscalYearForGraphModel source,FiscalYearForGraphSetup destination)
        {
            return source.MapTo(destination);
        }
    }
}
