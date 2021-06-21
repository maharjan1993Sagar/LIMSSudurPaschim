using System.ComponentModel.DataAnnotations;

namespace LIMS.Api.Models
{
    public partial class BaseApiEntityModel
    {
        [Key]
        public string Id { get; set; }
    }
}
