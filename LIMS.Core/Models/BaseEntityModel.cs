namespace LIMS.Core.Models
{
    /// <summary>
    /// Represents base LIMS entity model
    /// </summary>
    public partial class BaseEntityModel : BaseModel
    {
        /// <summary>
        /// Gets or sets model identifier
        /// </summary>
        public virtual string Id { get; set; }
    }
}
