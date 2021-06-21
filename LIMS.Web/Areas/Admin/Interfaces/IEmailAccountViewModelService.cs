using LIMS.Domain.Messages;
using LIMS.Web.Areas.Admin.Models.Messages;
using System.Threading.Tasks;

namespace LIMS.Web.Areas.Admin.Interfaces
{
    public interface IEmailAccountViewModelService
    {
        EmailAccountModel PrepareEmailAccountModel();
        Task<EmailAccount> InsertEmailAccountModel(EmailAccountModel model);
        Task<EmailAccount> UpdateEmailAccountModel(EmailAccount emailAccount, EmailAccountModel model);
        Task<EmailAccount> ChangePasswordEmailAccountModel(EmailAccount emailAccount, EmailAccountModel model);
        Task SendTestEmail(EmailAccount emailAccount, EmailAccountModel model);
    }
}
