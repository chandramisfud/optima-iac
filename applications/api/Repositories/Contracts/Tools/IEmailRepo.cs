using Repositories.Entities.Dtos;
using Repositories.Entities.Models;

namespace Repositories.Contracts
{
    public interface IToolsEmailRepository
    {
        Task SendEmail(EmailBody emailBodyDto);
        Task<IList<EmailResult>> GetEmailConfig(EmailBodyReq body);
        Task<ResendEmailApproval> resendEmailApproval();
    }
}