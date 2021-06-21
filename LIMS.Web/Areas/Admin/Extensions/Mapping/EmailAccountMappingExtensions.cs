using LIMS.Domain.Messages;
using LIMS.Web.Areas.Admin.Models.Messages;

namespace LIMS.Web.Areas.Admin.Extensions
{
    public static class EmailAccountMappingExtensions
    {
        public static EmailAccountModel ToModel(this EmailAccount entity)
        {
            return entity.MapTo<EmailAccount, EmailAccountModel>();
        }

        public static EmailAccount ToEntity(this EmailAccountModel model)
        {
            return model.MapTo<EmailAccountModel, EmailAccount>();
        }

        public static EmailAccount ToEntity(this EmailAccountModel model, EmailAccount destination)
        {
            return model.MapTo(destination);
        }
    }
}