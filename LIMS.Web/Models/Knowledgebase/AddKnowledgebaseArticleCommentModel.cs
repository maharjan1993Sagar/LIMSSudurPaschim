using LIMS.Core.ModelBinding;
using LIMS.Core.Models;

namespace LIMS.Web.Models.Knowledgebase
{
    public partial class AddKnowledgebaseArticleCommentModel : BaseEntityModel
    {
        [LIMSResourceDisplayName("Knowledgebase.Article.CommentText")]
        public string CommentText { get; set; }

        public bool DisplayCaptcha { get; set; }
    }
}
