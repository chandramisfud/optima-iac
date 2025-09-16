using Repositories.Entities.Models;
using Repositories.Entities.Report;
using System.Data;

public interface IAccrualReportRepo
{
    Task<AccrualReportLandingPage> GetAccrualReportLandingPage(int download, string period, int entityId, int distributorId, int budgetParentId, 
        int channelId, int grpBrandId, string profileId, string closingDate, string search, string sortColumn, int pageNum = 0, int dataDisplayed = 10, 
        string sortDirection = "ASC");
    Task<object> GetChannelSummary(string year, DataTable category, DataTable groupBrand, DataTable channel, DataTable subActivityType, string profileid, int start, int length, string filter, string search);
    Task<object> GetChannelSummaryDownload(string year, DataTable category, DataTable groupBrand, DataTable channel, DataTable subActivityType, string profileid, int start, int length, string filter, string search);
    Task<IList<AccrualEntityList>> GetEntityList();
    Task<object> GetGroupBrandList(int entity);
    Task<object> GetTTControlRCDCFilter();
}