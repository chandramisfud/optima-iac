using Repositories.Entities.Report;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Contracts
{
    public interface ISKPValidationRepo
    {
        Task<SKPValidationLandingPage> GetSKPValidationLandingPage(
            string period, int entityId, int distributorId, int budgetParentId, int channelId, string profileId, 
            int cancelStatus, string startFrom, string startTo, int submissionParam, int status,
            string search, string sortColumn, string sortDirection = "ASC", int pageNum = 0, int dataDisplayed = 10
        );

        Task<IList<SKPValidationEntityList>> GetEntityList();

        Task<IList<SKPValidationDistributorList>> GetDistributorList(int budgetid, int[] arrayParent);

        Task<IList<SKPValidationChannelList>> GetChannelList(string userid, int[] arrayParent);
    }
}
