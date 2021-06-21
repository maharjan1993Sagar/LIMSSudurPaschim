using LIMS.Domain.RationBalance;
using LIMS.Web.Areas.Admin.Models.RashanBalance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Web.Areas.Admin.Extensions.Mapping
{
    public static class FeedLibraryMappingExtension
    {
        public static FeedLibrary ToEntity(this FeedLibraryModel model)
        {
            return model.MapTo<FeedLibraryModel, FeedLibrary>();
        }
        public static FeedLibraryModel ToModel(this FeedLibrary entity)
        {
            return entity.MapTo<FeedLibrary, FeedLibraryModel>();
        }
        public static FeedLibrary ToEntity(this FeedLibraryModel source, FeedLibrary destination)
        {
            return source.MapTo(destination);
        }
    }
}
