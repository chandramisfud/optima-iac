using Repositories.Entities.Models;
using Repositories.Entities.Report;

namespace Repositories.Contracts
{
    public interface IFinAccrualReportRepo
    {
        Task<IList<AccrualEntityList>> GetEntityList();
        Task<IList<AccrualReportHeaderList>> GetPromoAccrualReportHeader(AccrualReportHeaderBody body);
        Task<IList<AccrualReportList>> GetPromoAccrualReportById(int id);
        Task<InvestmentNotifFinReport> CekInvestmentNotif(InvestmentNotifBodyFinReport body);
        Task<object> GetGroupBrandList(int entity);
        Task<AccrualReportLandingPage> GetAccrualReportLandingPage(int download, string period, int entityId, int distributorId, 
            int budgetParentId, int channelId, int grpBrandId, string profileId, string closingDate, string search, string sortColumn, 
            int pageNum = 0, int dataDisplayed = 10, string sortDirection = "ASC");
    }
}