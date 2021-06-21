using LIMS.Core.Models;

using LIMS.Web.Models.Knowledgebase;
using System.Collections.Generic;

namespace LIMS.Web.Models.Common
{
    public partial class SitemapModel : BaseModel
    {
        public SitemapModel()
        {
         
            KnowledgebaseArticles = new List<KnowledgebaseItemModel>();
        }

        public IList<KnowledgebaseItemModel> KnowledgebaseArticles { get; set; }

        public bool NewsEnabled { get; set; }
        public bool BlogEnabled { get; set; }
        public bool ForumEnabled { get; set; }
        public bool KnowledgebaseEnabled { get; set; }
    }
}