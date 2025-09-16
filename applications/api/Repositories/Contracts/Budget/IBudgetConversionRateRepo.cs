using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Contracts
{
    public interface IBudgetConversionRateRepo
    {
        Task<object> GetAccountList(int[] subChannelId);
//        Task<object> GetBudgetConversionRateLP(string period, int[] channelId, int[] groupBrandId, string keyword, string sortColumn, string sortDirection, int pageNumber, int pageSize, string profileId);

        Task<object> SetBudgetConversionRateUpload(DataTable data, string profile, string email);
        Task<object> GetBudgetSSVolumeLP(string period, int[] channelId, int[] subChannelId, int[] accountId, int[] subAccountId, int[] regionId, int[] groupBrandId, string keyword, string sortColumn, string sortDirection, int pageNumber, int pageSize, string profileId);

        Task<object> GetChannelList();
        Task<object> GetGroupBrandList();
        Task<object> GetSubAccountList(int[] accountId);
        Task<object> GetSubChannelList(int[] channelId);
        Task<object> SetBudgetVolumeUpload(DataTable data, string profile, string email);
        Task<object> GetRegionList();
        Task<object> SetBudgetTTConsoleUploadRC(DataTable data, string profile, string email);
        Task<object> SetBudgetTTConsoleUploadDC(DataTable data, string profile, string email);
        Task<object> GetBudgetTTConsoleLP(string period, string profile, int[] categoryId, int[] subcategoryId, int[] subActivityTypeId, int[] activityId, int[] subActivityId, int[] channelId, int[] subChannelId, int[] accountId, int[] subAccountId, int[] distributorId, int[] groupBrandId, string keyword, string sortColumn, string sortDirection, int pageNumber, int pageSize);
        Task<object> GetDistributorList();
        Task<object> GetSubActivityTypeList(int[] categoryId);
        Task<object> GetSubActivityList(int[] subCategoryId, int[] activityId, int[] subActivityTypeId);

        Task<object> GetCategoryList();
        Task<object> GetBudgetTTConsoleHistory(string period, string profile, int[] categoryId, int[] subcategoryId, int[] subActivityTypeId, int[] activityId, int[] subActivityId, int[] channelId, int[] subChannelId, int[] accountId, int[] subAccountId, int[] distributorId, int[] groupBrandId, string keyword, string sortColumn, string sortDirection, int pageNumber, int pageSize);
        Task<object> GetUserProfileChannelList(string profile);
        Task<object> SetBudgetTTConsoleUpdate(int id, string period, int category, int subcategory, int channel, int subChannel, int account, int subAccount,
            int distributor, string distributorShortDesc, int subActivityType, int activity, int subActivity, int groupBrand, 
            string budgetName, decimal tt,  string profile, string email);
        Task<object> SetBudgetTTConsoleCreate(string period, int category, int subcategory, int channel, int subChannel, int account, int subAccount,
            int distributor, string distributorShortDesc, int subActivityType, int activity, int subActivity, int groupBrand, 
            string budgetName, decimal tt, string profile, string email);
        Task<object> GetBudgetTTConsoleByid(int id);
        
        Task<object> GetSubCategoryList(int[] categoryId, int[] subActivityTypeId);
        Task<object> GetActivityList(int[] SubActivityTypeId, int[] categoryId, int[] subCategoryId);
        Task<object> GetBudgetTTConsoleTemplate(string period, string profile, int[] categoryId, int[] subcategoryId, int[] subActivityTypeId, int[] activityId, int[] subActivityId, int[] channelId, int[] subChannelId, int[] accountId, int[] subAccountId, int[] distributorId, int[] groupBrandId, string keyword, string sortColumn, string sortDirection, int pageNumber, int pageSize);
        Task<object> SetBudgetPSInputUpload(DataTable data, string profile, string email);
        Task<object> GetBudgetPSInputLP(string period, int[] distributorid, int[] groupBrandId, string keyword, string sortColumn, string sortDirection, int pageNumber, int pageSize);
        Task<object> GetBudgetPSInputFilter();
        Task<object> GetBudgetPSInputTemplate(string period, int[] distributorid, int[] groupBrandId, string keyword, string sortColumn, string sortDirection, int pageNumber, int pageSize);
        Task<object> GetBudgetSSVolumeTemplate(string period, int[] channelId, int[] subChannelId, int[] accountId, int[] subAccountId, int[] regionId, int[] groupBrandId, string keyword, string sortColumn, string sortDirection, int pageNumber, int pageSize, string profileId);
        Task<object> GetConversionRateSubChannelList(int[] channel);
        Task<object> GetBudgetConversionRateLP(string period, int[] channelId, int[] subChannelId, int[] groupBrandId, string keyword, string sortColumn, string sortDirection, int pageNumber, int pageSize, string profileId);
    }
}
