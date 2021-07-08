using LIMS.Domain.Breed;
using LIMS.Domain.DynamicMenu;
using LIMS.Domain.NewsEvent;
using LIMS.Web.Areas.Admin.Models.DynamicMenu;
using LIMS.Web.Areas.Admin.Models.NewsEvent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Web.Areas.Admin.Extensions.Mapping
{
    public static class FileMappingExtension
    {
        public static NewsEventFileModel ToModel(this NewsEventFile entity)
        {
            return entity.MapTo<NewsEventFile, NewsEventFileModel>();
        }

        public static NewsEventFile ToEntity(this NewsEventFileModel model)
        {
            return model.MapTo<NewsEventFileModel, NewsEventFile>();
        }

        public static NewsEventFile ToEntity(this NewsEventFileModel model, NewsEventFile destination)
        {
            return model.MapTo(destination);
        }
    }
}
