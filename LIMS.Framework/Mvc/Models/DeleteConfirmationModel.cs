using LIMS.Core.Models;

namespace LIMS.Framework.Mvc.Models
{
    public class DeleteConfirmationModel : BaseEntityModel
    {
        public string ControllerName { get; set; }
        public string ActionName { get; set; }
        public string WindowId { get; set; }
    }
}