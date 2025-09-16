using Repositories.Entities.Dtos;
using Repositories.Entities.Models;
using Repositories.Entities.Report;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Contracts
{
    public interface IFinDNDetailReportingRepo
    {
        Task<DNDetailReportingLandingPage> GetDNDetailReportingLandingPage(
           string period,
           int categoryId,
           int entityId,
           int distributorId,
           int channelId,
           int accountId, 
           string profileId,
           string search, 
           string sortColumn, 
           int pageNum = 0, 
           int dataDisplayed = 10, 
           string sortDirection = "ASC"
       );

        Task<IList<DNDetailReportingEntityList>> GetEntityList();
        Task<IList<DNDetailReportingDistributorList>> GetDistributorList(int budgetid, int[] arrayParent);
        Task<IList<DNDetailReportingSubAccountList>> GetSubAccountList(string profileId);
        Task<InvestmentNotifFinReport> CekInvestmentNotifDN(InvestmentNotifBodyFinReport body);
        Task<IList<object>> GetCategoryDropdownList();
        Task<object> GetDNOutStanding(string period, DataTable distributorId, string userProfile, int pageNumber, int pageSize, string search);
        Task<object> GetDistributorList();
    }
}
