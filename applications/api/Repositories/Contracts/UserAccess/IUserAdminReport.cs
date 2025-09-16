using Repositories.Entities.UserAccess;

namespace Repositories.Contracts
{
    public interface IUserAdminReportRepository
    {
        Task<UserAdminReportLandingPage> GetUserAdminReportLandingPage(string search, int dataDisplayed = 10, int pageNum = 0);
    }
}
