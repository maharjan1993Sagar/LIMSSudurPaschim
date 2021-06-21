using LIMS.Domain.NewsEvent;
using LIMS.Web.Areas.Admin.Models.NewsEvent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Web.Areas.Admin.Extensions.Mapping
{
    public static class NewsEventTenderMappingExtension
    {
        public static NewsEventTenderModel ToModel(this NewsEventTender entity)
        {
            return entity.MapTo<NewsEventTender, NewsEventTenderModel>();
        }

        public static NewsEventTender ToEntity(this NewsEventTenderModel model)
        {
            return model.MapTo<NewsEventTenderModel, NewsEventTender>();
        }

        public static NewsEventTender ToEntity(this NewsEventTenderModel model, NewsEventTender destination)
        {
            return model.MapTo(destination);
        }
    }
}
