//using LIMS.Web.Areas.Admin.Models.Services;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;

//namespace LIMS.Web.Areas.Admin.Extensions.Mapping
//{
//    public class HeatRecordingMappingExtension
//    {
//        public static HeatRecordingModel ToModel(this HeatRecording entity)
//        {
//            return entity.MapTo<BlogCategory, BlogCategoryModel>();
//        }

//        public static BlogCategory ToEntity(this BlogCategoryModel model)
//        {
//            return model.MapTo<BlogCategoryModel, BlogCategory>();
//        }

//        public static BlogCategory ToEntity(this BlogCategoryModel model, BlogCategory destination)
//        {
//            return model.MapTo(destination);
//        }
//    }
//}
