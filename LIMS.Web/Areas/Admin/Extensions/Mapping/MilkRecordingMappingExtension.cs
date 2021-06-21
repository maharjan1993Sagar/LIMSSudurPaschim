using LIMS.Domain.Recording;
using LIMS.Web.Areas.Admin.Models.Recording;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Web.Areas.Admin.Extensions.Mapping
{
    public static class MilkRecordingMappingExtension
    {
        public static MilkRecordingModel ToModel(this MilkRecording entity)
        {
            return entity.MapTo<MilkRecording, MilkRecordingModel>();
        }

        public static MilkRecording ToEntity(this MilkRecordingModel model)
        {
            return model.MapTo<MilkRecordingModel, MilkRecording>();
        }

        public static MilkRecording ToEntity(this MilkRecordingModel model, MilkRecording destination)
        {
            return model.MapTo(destination);
        }
    }
}
