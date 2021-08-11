using LIMS.Domain.Bali;
using LIMS.Web.Areas.Admin.Models.Bali;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Web.Areas.Admin.Extensions.Mapping
{
    public static class LabambitKrishakExtension
    {
        public static LabambitKrishakModel ToModel(this LabambitKrishakHaru entity)
        {
            return entity.MapTo<LabambitKrishakHaru, LabambitKrishakModel>();
        }

        public static LabambitKrishakHaru ToEntity(this LabambitKrishakModel model)
        {
            return model.MapTo<LabambitKrishakModel, LabambitKrishakHaru>();
        }

        public static LabambitKrishakHaru ToEntity(this LabambitKrishakModel model, LabambitKrishakHaru destination)
        {
            return model.MapTo(destination);
        }
    }
}
