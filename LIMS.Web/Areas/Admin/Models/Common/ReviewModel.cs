using LIMS.Core.ModelBinding;
using LIMS.Core.Models;

namespace LIMS.Web.Areas.Admin.Models.Common
{
    public partial class ReviewModel : BaseEntityModel
    {
        /// <summary>
        /// Gets or sets the title
        /// </summary>
        [LIMSResourceDisplayName("Admin.Customers.Customers.Reviews.Title")]
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the review text
        /// </summary>
        [LIMSResourceDisplayName("Admin.Customers.Customers.Reviews.Review")]
        public string ReviewText { get; set; }
    }
}