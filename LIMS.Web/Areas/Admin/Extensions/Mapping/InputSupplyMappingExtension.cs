using LIMS.Domain.Bali;
using LIMS.Domain.BasicSetup;
using LIMS.Web.Areas.Admin.Models.Bali;
using LIMS.Web.Areas.Admin.Models.BasicSetup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Web.Areas.Admin.Extensions.Mapping
{
    public static class InputSupplyMappingExtension
    {
        public static InputSupplyModel ToModel(this InputSupply entity)
        {
            return entity.MapTo<InputSupply, InputSupplyModel>();
        }

        public static InputSupply ToEntity(this InputSupplyModel model)
        {
            return model.MapTo<InputSupplyModel, InputSupply>();
        }

        public static InputSupply ToEntity(this InputSupplyModel model, InputSupply destination)
        {
            return model.MapTo(destination);
        }
    }
}
