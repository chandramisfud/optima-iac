using Repositories.Entities.Report;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Contracts
{
    public interface IFinDNDisplayRepo
    {
        Task<DNDisplayLandingPage> GetDNDisplayLandingPage(
            string period, int entityId, int distributorId, int channelId, int accountId, string profileId, string isDNManual,
            string search, string sortColumn, string sortDirection = "ASC", int pageNum = 0, int dataDisplayed = 10
        );

        Task<DNDisplayDataById> GetDNDisplayData(int id);

        Task<IList<DNDisplayEntityList>> GetEntityList();

        Task<IList<DNDisplayDistributorList>> GetDistributorList(int budgetid, int[] arrayParent);

        Task<IList<DNDisplayTaxLevelList>> GetTaxLevelList();

        Task<IList<DNDisplaySellingPointList>> GetSellingPointList(string profileId);

        Task<DNDisplayPrint> GetDNPrint(int id);
    }
}
