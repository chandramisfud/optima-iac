using Repositories.Entities;
using Repositories.Entities.Models;
using Repositories.Entities.Models.DN;
using Repositories.Entities.Models.Promo;
using Repositories.Entities.Models.PromoApproval;
using Repositories.Entities.Report;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Contracts
{
    public interface IPromoSKPValidationRepository
    {

        Task<IList<SKPValidationEntityList>> GetEntityList();

        Task<IList<SKPValidationDistributorList>> GetDistributorList(int budgetid, int[] arrayParent);

        Task<IList<SKPValidationChannelList>> GetChannelList(string userid, int[] arrayParent);
        Task<PromoSelectSKP> GetPromoWithSKP(int Id, string LongDesc);
        Task<ErrorMessageDto> UpdateApprovalPromoWithSKPFlagging(PromoSKP promoSKP);
        Task<IList<SKPValidationView>> GetPromoListSKPFlagging(string periode, int entity, int distributor, int BudgetParent, int channel, bool cancelstatus, DateTime start_from, DateTime start_to, int status, string userid);
        Task<SKPValidationLandingPage> GetSKPValidationDownload(string period, int entityId, int distributorId, int budgetParentId, int channelId, string profileId, int cancelStatus, string startFrom, string startTo, int submissionParam, int status, string keyword, string sortColumn, string sortDirection = "ASC", int pageNum = 0, int dataDisplayed = 10);
    }
}
