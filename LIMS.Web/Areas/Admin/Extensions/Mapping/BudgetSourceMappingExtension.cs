using LIMS.Domain.Bali;
using LIMS.Web.Areas.Admin.Models.Bali;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Web.Areas.Admin.Extensions.Mapping
{
    public static class BudgetSourceMappingExtension
    {

        public static BudgetSourceModel ToModel(this BudgetSource bali)
        {
            return bali.MapTo<BudgetSource, BudgetSourceModel>();
        }
        public static BudgetSource ToEntity(this BudgetSourceModel bali)
        {
            return bali.MapTo<BudgetSourceModel, BudgetSource>();
        }
        public static BudgetSource ToEntity(this BudgetSourceModel source, BudgetSource destination)
        {
            return source.MapTo(destination);
        }
    }
}
