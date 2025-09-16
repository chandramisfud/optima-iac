using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;
using System.Data;
using Repositories.Contracts;
using Dapper;
//using static Dapper.SqlMapper;
using Repositories.Entities.Models;

namespace Repositories.Repos
{
    public class BudgetConversionRateRepository : IBudgetConversionRateRepo
    {
        readonly IConfiguration __config;
        public BudgetConversionRateRepository(IConfiguration config)
        {
            __config = config;
        }
        public IDbConnection Connection
        {
            get
            {
                return new SqlConnection(__config.GetConnectionString("DefaultConnection"));
            }
        }

        //CONVERSION RATE
        public async Task<object> GetBudgetConversionRateLP(string period, int[] channelId, 
            int[] subChannelId, int[] groupBrandId,
            string keyword, string sortColumn, string sortDirection, int pageNumber, int pageSize, string profileId)
        {
            using IDbConnection conn = Connection;
            try
            {

                var __param = new DynamicParameters();
                __param.Add("@period", period);

                DataTable dtChannel = new("ArrayIntType");
                dtChannel.Columns.Add("keyid");
                if (channelId != null)
                {
                    foreach (var channel in channelId)
                    {
                        dtChannel.Rows.Add(channel);
                    }
                }
                else
                {
                    dtChannel.Rows.Add(0);
                }

                DataTable dtSubChannel = new("ArrayIntType");
                dtSubChannel.Columns.Add("keyid");
                if (subChannelId != null)
                {
                    foreach (var channel in subChannelId)
                    {
                        dtSubChannel.Rows.Add(channel);
                    }
                }
                else
                {
                    dtSubChannel.Rows.Add(0);
                }

                DataTable dtBrand = new("ArrayIntType");
                dtBrand.Columns.Add("keyid");
                if (groupBrandId != null)
                {
                    foreach (var brand in groupBrandId)
                    {
                        dtBrand.Rows.Add(brand);
                    }
                }
                else
                {
                    dtBrand.Rows.Add(0);
                }
                __param.Add("@channelid", dtChannel.AsTableValuedParameter());
                __param.Add("@subchannelid", dtSubChannel.AsTableValuedParameter());
                __param.Add("@groupbrandid", dtBrand.AsTableValuedParameter());


                __param.Add("@txtSearch", keyword);
                __param.Add("@sort", sortDirection);
                __param.Add("@order", sortColumn);
                __param.Add("@start", pageNumber);
                __param.Add("@length", pageSize);
                __param.Add("@profileid", profileId);
                BaseLP baseLP = new BaseLP();
                var _res = await conn.QueryMultipleAsync("ip_ss_conversion_rate_list", __param, commandType: CommandType.StoredProcedure, commandTimeout: 180);
                baseLP = _res.Read<BaseLP>().First();
                baseLP.Data = _res.Read<object>().ToList();

                return baseLP;
            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }  
        public async Task<object> SetBudgetConversionRateUpload(DataTable data, string profile, string email)
        {
            using IDbConnection conn = Connection;
            try
            {

                var __param = new DynamicParameters();
               
                __param.Add("@sscovertionrate", data.AsTableValuedParameter());
                __param.Add("@profileid", profile);
                __param.Add("@email", email);


               var __reader = await conn.QueryMultipleAsync("ip_ss_conversion_rate_upload", __param, commandType: CommandType.StoredProcedure, commandTimeout: 180);

                var __res = new
                {
                    errMessage = __reader.Read().ToList(),
                    uploadStat = __reader.Read().ToList(),
                    promoList = __reader.Read().ToList()
                };
                return __res;
            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }
        public async Task<object> SetBudgetVolumeUpload(DataTable data, string profile, string email)
        {
            using IDbConnection conn = Connection;
            try
            {

                var __param = new DynamicParameters();

                __param.Add("@ssvolume", data.AsTableValuedParameter());
                __param.Add("@profileid", profile);
                __param.Add("@email", email);


                var __reader = await conn.QueryMultipleAsync("ip_ss_volume_upload", __param, commandType: CommandType.StoredProcedure, commandTimeout: 180);
                var __res = new
                {
                    errMessage = __reader.Read().ToList(),
                    uploadStat = __reader.Read().ToList(),
                    promoList = __reader.Read().ToList()
                };

                return __res;
            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }

        public async Task<object> GetChannelList()
        {
            using IDbConnection conn = Connection;
            try
            {
                var __query = @" SELECT id, refId, shortDesc, longDesc FROM tbmst_channel 
                                 WHERE ISNULL(IsDelete, 0) = 0";
                return await conn.QueryAsync<object>(__query);

            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }

        public async Task<object> GetConversionRateSubChannelList(int[] channel)
        {
            using (IDbConnection conn = Connection)
            {
                var sql = "";
                if (channel == null || channel.Length == 0)
                {


                    sql = @"
                    SELECT  B.Id channelId, B.LongDesc channelLongDesc,
                            A.Id subChannelId, A.LongDesc subChannelLongDesc		  
                    FROM tbmst_subchannel AS A LEFT JOIN
                    tbmst_channel AS B ON A.ChannelId = B.Id 
                    WHERE ISNULL(B.IsDelete, 0) = 0 AND ISNULL(A.IsDelete, 0) = 0 @channel";
                }
                else
                {
                    sql = @"
                    SELECT  B.Id channelId, B.LongDesc channelLongDesc,
		                    A.Id subChannelId, A.LongDesc subChannelLongDesc		  
                    FROM tbmst_subchannel AS A LEFT JOIN
                    tbmst_channel AS B ON A.ChannelId = B.Id 
                    WHERE ISNULL(B.IsDelete, 0) = 0 AND ISNULL(A.IsDelete, 0) = 0
                    AND ChannelId IN @channel";
                }

                return await conn.QueryAsync<object>(sql, new { channel = channel });
            }
        }

        public async Task<object> GetUserProfileChannelList(string profile)
        {
            using IDbConnection conn = Connection;
            try
            {
                var __query = @" SELECT C.id, C.refId, shortDesc, longDesc 
                                FROM tbset_user_channel_sstt UC
                                LEFT JOIN tbmst_channel C on C.Id = UC.channelId 
                                WHERE ISNULL(C.IsDelete, 0) = 0 AND UC.userId = @profile ";
                return await conn.QueryAsync<object>(__query, new { profile = profile});

            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }
        public async Task<object> GetGroupBrandList()
        {
            using IDbConnection conn = Connection;
            try
            {
                var __query = @" SELECT * FROM tbmst_brand_group 
                                 WHERE ISNULL(IsDeleted, 0) = 0";
                return await conn.QueryAsync<object>(__query);

            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }

        public async Task<object> GetAccountList(int[] subChannelId)
        {
            using IDbConnection conn = Connection;
            try
            {
                string paramList = string.Join(", ", subChannelId);
                var __query = @" select A.id, A.RefId refId, B.Id subChannelId, B.LongDesc subChannel,  A.ShortDesc shortDesc, A.LongDesc longDesc
                                FROM tbmst_account AS A 
                                LEFT JOIN tbmst_subchannel AS B ON B.Id = A.SubChannelId 
                                WHERE ISNULL(A.IsDelete, 0) = 0";

                if (subChannelId.Length > 0)
                {
                    __query += " AND subChannelId IN (" + paramList + ") ";
                }
                return await conn.QueryAsync<object>(__query);

            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }

        public async Task<object> GetSubAccountList(int[] accountId)
        {
            using IDbConnection conn = Connection;
            try
            {
                string paramList = string.Join(", ", accountId);
                var __query = @" SELECT a.id, A.RefId refId, B.Id accountId, B.LongDesc account, 
                A.Id subAccountId, A.ShortDesc shortDesc,  A.LongDesc longDesc
                FROM tbmst_subaccount AS A 
                LEFT JOIN tbmst_account AS B ON A.AccountId = B.Id 
                WHERE ISNULL(A.IsDelete, 0) = 0  ";
                if (accountId.Length > 0)
                {
                    __query += " AND B.Id IN ("+ paramList + ") ";
                }

                return await conn.QueryAsync<object>(__query);

            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }

        public async Task<object> GetSubChannelList(int[] channelId)
        {
            using IDbConnection conn = Connection;
            try
            {
                string paramList = string.Join(", ", channelId);
                var __query = @" SELECT A.id, A.RefId refId, A.ChannelId channelId, B.LongDesc channel, 
                                A.Id subChannelId, A.ShortDesc shortDesc,  A.LongDesc longDesc 
                                FROM tbmst_subchannel AS A 
                                LEFT JOIN tbmst_channel AS B ON A.ChannelId = B.Id 
                                WHERE ISNULL(A.IsDelete, 0) = 0";
                if (channelId.Length > 0)
                {
                    __query += "AND B.Id IN (" + paramList + ") ";
                }

                return await conn.QueryAsync<object>(__query);

            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }
        public async Task<object> GetRegionList()
        {
            using IDbConnection conn = Connection;
            try
            {
                var __query = @" SELECT id, refId, shortDesc, longDesc FROM tbmst_region
                                 WHERE ISNULL(IsDelete, 0) = 0";

                return await conn.QueryAsync<object>(__query);

            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }

        public async Task<object> GetDistributorList()
        {
            using IDbConnection conn = Connection;
            try
            {
               
                var __query = @" SELECT id,  longDesc, shortDesc
                            FROM tbmst_distributor
                            WHERE  ISNULL(IsDeleted, 0) = 0 ";
              
                return await conn.QueryAsync<object>(__query);

            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }

        public async Task<object> GetSubActivityTypeList(int[] categoryId)
        {
            using IDbConnection conn = Connection;
            try
            {
                string paramList = string.Join(", ", categoryId);
                var __query = @" SELECT DISTINCT subActivityTypeId id, subActivityTypeDesc longDesc
                                FROM vw_activity_active 
                                WHERE CategoryDesc!='Retailer Cost'";
                if (categoryId.Length > 0)
                {
                    __query += " AND CategoryId IN (" + paramList + ") ";
                }

                __query += " UNION " +
                           " SELECT DISTINCT subActivityTypeId id, subActivityTypeDesc longDesc" +
                           " FROM vw_activity_active " +
                           " WHERE CategoryDesc='Retailer Cost' AND SubActivityTypeDesc='Contractual' ";
                if (categoryId.Length > 0)
                {
                    __query += " AND CategoryId IN (" + paramList + ") ";
                }
                return await conn.QueryAsync<object>(__query);

            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }

        public async Task<object> GetActivityList(int[] SubActivityTypeId, int[] categoryId, int[] subCategoryId)
        {
            using IDbConnection conn = Connection;
            try
            {
                var __query = @" SELECT DISTINCT activityId id, ActivityDesc longDesc
                                 from vw_activity_active WHERE 1=1 ";

                string actList = string.Join(", ", SubActivityTypeId);
                if (SubActivityTypeId.Length > 0)
                {
                    __query += " AND SubActivityTypeId IN (" + actList + ") ";
                }

                string paramList = string.Join(", ", categoryId);
                if (categoryId.Length > 0)
                {
                    __query += " AND CategoryId IN (" + paramList + ") ";
                }

                string subcat = string.Join(", ", subCategoryId);
                if (subCategoryId.Length > 0)
                {
                    __query += " AND SubCategoryId IN (" + subcat + ") ";
                }
                return await conn.QueryAsync<object>(__query);

            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }

        public async Task<object> GetSubActivityList(int[] subCategoryId, int[] activityId, int[] subActivityTypeId)
        {
            using IDbConnection conn = Connection;
            try
            {
                var __query = @" SELECT DISTINCT subActivityId id, subActivityDesc longDesc
                                 from vw_activity_active WHERE 1=1 ";

                string actList = string.Join(", ", subActivityTypeId);
                if (subActivityTypeId.Length > 0)
                {
                    __query += " AND SubActivityTypeId IN (" + actList + ") ";
                }

                string catList = string.Join(", ", subCategoryId);
                if (subCategoryId.Length > 0)
                {
                    __query += " AND subCategoryId IN (" + catList + ") ";
                }

                string activityList = string.Join(", ", activityId);
                if (activityId.Length > 0)
                {
                    __query += " AND ActivityId IN (" + activityList + ") ";
                }
                return await conn.QueryAsync<object>(__query);

            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }

        public async Task<object> GetCategoryList()
        {
            using IDbConnection conn = Connection;
            try
            {            
                var __query = @" SELECT DISTINCT e.id, e.shortDesc, e.longDesc
                                    FROM tbmst_category e
                                 WHERE ISNULL(e.IsDeleted, 0) = 0  ";
   
                return await conn.QueryAsync<object>(__query);

            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }

        public async Task<object> GetSubCategoryList(int[] categoryId, int[] subActivityTypeId)
        {
            using IDbConnection conn = Connection;
            try
            {
                string catList = string.Join(", ", categoryId);
                var __query = @" SELECT DISTINCT  SubCategoryId id, SubCategoryDesc longDesc
                                 from vw_activity_active WHERE 1=1 ";
                if (categoryId.Length > 0)
                {
                    __query += "AND CategoryId IN (" + catList + ") ";
                }

                string subActList = string.Join(", ", subActivityTypeId);
                if (subActivityTypeId.Length > 0)
                {
                    __query += "AND subActivityTypeId IN (" + subActList + ") ";
                }
                return await conn.QueryAsync<object>(__query);

            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }
        public async Task<object> GetBudgetSSVolumeLP(string period, int[] channelId,int[] subChannelId, 
            int[] accountId,int[] subAccountId, int[] regionId, int[] groupBrandId,
           string keyword, string sortColumn, string sortDirection, int pageNumber, int pageSize, string profileId)
        {
            using IDbConnection conn = Connection;
            try
            {

                var __param = new DynamicParameters();
                __param.Add("@period", period);

                DataTable dtChannel = new("ArrayIntType");
                dtChannel.Columns.Add("keyid");
                if (channelId != null)
                {
                    foreach (var channel in channelId)
                    {
                        dtChannel.Rows.Add(channel);
                    }
                }
                else
                {
                    dtChannel.Rows.Add(0);
                }

                DataTable dtBrand = new("ArrayIntType");
                dtBrand.Columns.Add("keyid");
                if (groupBrandId != null)
                {
                    foreach (var brand in groupBrandId)
                    {
                        dtBrand.Rows.Add(brand);
                    }
                }
                else
                {
                    dtBrand.Rows.Add(0);
                }

                DataTable dtAccount = new("ArrayIntType");
                dtAccount.Columns.Add("keyid");
                if (accountId != null)
                {
                    foreach (var item in accountId)
                    {
                        dtAccount.Rows.Add(item);
                    }
                }
                else
                {
                    dtAccount.Rows.Add(0);
                }

                DataTable dtSubAccount = new("ArrayIntType");
                dtSubAccount.Columns.Add("keyid");
                if (subAccountId != null)
                {
                    foreach (var item in subAccountId)
                    {
                        dtSubAccount.Rows.Add(item);
                    }
                }
                else
                {
                    dtSubAccount.Rows.Add(0);
                }

                DataTable dtSubChannel = new("ArrayIntType");
                dtSubChannel.Columns.Add("keyid");
                if (subChannelId != null)
                {
                    foreach (var item in subChannelId)
                    {
                        dtSubChannel.Rows.Add(item);
                    }
                }
                else
                {
                    dtSubChannel.Rows.Add(0);
                }

                DataTable dtRegion = new("ArrayIntType");
                dtRegion.Columns.Add("keyid");
                if (regionId != null)
                {
                    foreach (var item in regionId)
                    {
                        dtRegion.Rows.Add(item);
                    }
                }
                else
                {
                    dtRegion.Rows.Add(0);
                }
                __param.Add("@channelid", dtChannel.AsTableValuedParameter());
                __param.Add("@groupbrandid", dtBrand.AsTableValuedParameter());
                __param.Add("@subchannelid", dtSubChannel.AsTableValuedParameter());
                __param.Add("@accountid", dtAccount.AsTableValuedParameter());
                __param.Add("@subaccountid", dtSubAccount.AsTableValuedParameter());
                __param.Add("@regionid", dtRegion.AsTableValuedParameter());
                __param.Add("@profileid", profileId);

                __param.Add("@txtSearch", keyword);
                __param.Add("@sort", sortDirection);
                __param.Add("@order", sortColumn);
                __param.Add("@start", pageNumber);
                __param.Add("@length", pageSize);
                BaseLP baseLP = new BaseLP();
                var _res = await conn.QueryMultipleAsync("ip_ss_volume_list", __param, commandType: CommandType.StoredProcedure, commandTimeout: 180);
                baseLP = _res.Read<BaseLP>().First();
                baseLP.Data = _res.Read<object>().ToList();

                return baseLP;
            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }

        public async Task<object> GetBudgetSSVolumeTemplate(string period, int[] channelId, int[] subChannelId,
               int[] accountId, int[] subAccountId, int[] regionId, int[] groupBrandId,
              string keyword, string sortColumn, string sortDirection, int pageNumber, int pageSize,
              string profileId)
        {
            using IDbConnection conn = Connection;
            try
            {

                var __param = new DynamicParameters();
                __param.Add("@period", period);

                DataTable dtChannel = new("ArrayIntType");
                dtChannel.Columns.Add("keyid");
                if (channelId != null)
                {
                    foreach (var channel in channelId)
                    {
                        dtChannel.Rows.Add(channel);
                    }
                }
                else
                {
                    dtChannel.Rows.Add(0);
                }

                DataTable dtBrand = new("ArrayIntType");
                dtBrand.Columns.Add("keyid");
                if (groupBrandId != null)
                {
                    foreach (var brand in groupBrandId)
                    {
                        dtBrand.Rows.Add(brand);
                    }
                }
                else
                {
                    dtBrand.Rows.Add(0);
                }

                DataTable dtAccount = new("ArrayIntType");
                dtAccount.Columns.Add("keyid");
                if (accountId != null)
                {
                    foreach (var item in accountId)
                    {
                        dtAccount.Rows.Add(item);
                    }
                }
                else
                {
                    dtAccount.Rows.Add(0);
                }

                DataTable dtSubAccount = new("ArrayIntType");
                dtSubAccount.Columns.Add("keyid");
                if (subAccountId != null)
                {
                    foreach (var item in subAccountId)
                    {
                        dtSubAccount.Rows.Add(item);
                    }
                }
                else
                {
                    dtSubAccount.Rows.Add(0);
                }

                DataTable dtSubChannel = new("ArrayIntType");
                dtSubChannel.Columns.Add("keyid");
                if (subChannelId != null)
                {
                    foreach (var item in subChannelId)
                    {
                        dtSubChannel.Rows.Add(item);
                    }
                }
                else
                {
                    dtSubChannel.Rows.Add(0);
                }

                DataTable dtRegion = new("ArrayIntType");
                dtRegion.Columns.Add("keyid");
                if (regionId != null)
                {
                    foreach (var item in regionId)
                    {
                        dtRegion.Rows.Add(item);
                    }
                }
                else
                {
                    dtRegion.Rows.Add(0);
                }
                __param.Add("@channelid", dtChannel.AsTableValuedParameter());
                __param.Add("@groupbrandid", dtBrand.AsTableValuedParameter());
                __param.Add("@subchannelid", dtSubChannel.AsTableValuedParameter());
                __param.Add("@accountid", dtAccount.AsTableValuedParameter());
                __param.Add("@subaccountid", dtSubAccount.AsTableValuedParameter());
                __param.Add("@regionid", dtRegion.AsTableValuedParameter());
                __param.Add("@profileid", profileId);


                __param.Add("@txtSearch", keyword);
                __param.Add("@sort", sortDirection);
                __param.Add("@order", sortColumn);
                __param.Add("@start", pageNumber);
                __param.Add("@length", pageSize);
                BudgetSSVolumeTemplate baseLP = new BudgetSSVolumeTemplate();
                var _res = await conn.QueryMultipleAsync("ip_ss_volume_tmp_list", __param, commandType: CommandType.StoredProcedure, commandTimeout: 180);
                baseLP = _res.Read<BudgetSSVolumeTemplate>().First();
                baseLP.Data = _res.Read<object>().ToList();
                baseLP.Region = _res.Read<object>().ToList();

                return baseLP;
            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }
        public async Task<object> GetBudgetTTConsoleLP(string period, string profile, int[] categoryId, int[] subcategoryId,
            int[] subActivityTypeId, int[] activityId, int[] subActivityId, int[] channelId, int[] subChannelId, 
            int[] accountId,int[] subAccountId, int[] distributorId, int[] groupBrandId,
            string keyword, string sortColumn, string sortDirection, int pageNumber, int pageSize)
        {
            using IDbConnection conn = Connection;
            try
            {

                var __param = new DynamicParameters();
             

                DataTable dtChannel = new("ArrayIntType");
                dtChannel.Columns.Add("keyid");
                if (channelId != null)
                {
                    foreach (var channel in channelId)
                    {
                        dtChannel.Rows.Add(channel);
                    }
                }
                else
                {
                    dtChannel.Rows.Add(0);
                }

                DataTable dtBrand = new("ArrayIntType");
                dtBrand.Columns.Add("keyid");
                if (groupBrandId != null)
                {
                    foreach (var brand in groupBrandId)
                    {
                        dtBrand.Rows.Add(brand);
                    }
                }
                else
                {
                    dtBrand.Rows.Add(0);
                }

                DataTable dtAccount = new("ArrayIntType");
                dtAccount.Columns.Add("keyid");
                if (accountId != null)
                {
                    foreach (var item in accountId)
                    {
                        dtAccount.Rows.Add(item);
                    }
                }
                else
                {
                    dtAccount.Rows.Add(0);
                }

                DataTable dtSubAccount = new("ArrayIntType");
                dtSubAccount.Columns.Add("keyid");
                if (subAccountId != null)
                {
                    foreach (var item in subAccountId)
                    {
                        dtSubAccount.Rows.Add(item);
                    }
                }
                else
                {
                    dtSubAccount.Rows.Add(0);
                }

                DataTable dtSubChannel = new("ArrayIntType");
                dtSubChannel.Columns.Add("keyid");
                if (subChannelId != null)
                {
                    foreach (var item in subChannelId)
                    {
                        dtSubChannel.Rows.Add(item);
                    }
                }
                else
                {
                    dtSubChannel.Rows.Add(0);
                }

                DataTable dtCategory = new("ArrayIntType");
                dtCategory.Columns.Add("keyid");
                if (categoryId != null)
                {
                    foreach (var item in categoryId)
                    {
                        dtCategory.Rows.Add(item);
                    }
                }
                else
                {
                    dtCategory.Rows.Add(0);
                }

                DataTable dtSubCategory = new("ArrayIntType");
                dtSubCategory.Columns.Add("keyid");
                if (subcategoryId != null)
                {
                    foreach (var item in subcategoryId)
                    {
                        dtSubCategory.Rows.Add(item);
                    }
                }
                else
                {
                    dtSubCategory.Rows.Add(0);
                }

                DataTable dtsubActivityType = new("ArrayIntType");
                dtsubActivityType.Columns.Add("keyid");
                if (subActivityTypeId != null)
                {
                    foreach (var item in subActivityTypeId)
                    {
                        dtsubActivityType.Rows.Add(item);
                    }
                }
                else
                {
                    dtsubActivityType.Rows.Add(0);
                }
                               
                DataTable dtActivity = new("ArrayIntType");
                dtActivity.Columns.Add("keyid");
                if (activityId != null)
                {
                    foreach (var item in activityId)
                    {
                        dtActivity.Rows.Add(item);
                    }
                }
                else
                {
                    dtActivity.Rows.Add(0);
                }                
                
                DataTable dtSubActivity = new("ArrayIntType");
                dtSubActivity.Columns.Add("keyid");
                if (subActivityId != null)
                {
                    foreach (var item in subActivityId)
                    {
                        dtSubActivity.Rows.Add(item);
                    }
                }
                else
                {
                    dtSubActivity.Rows.Add(0);
                }

                DataTable dtDistributor = new("ArrayIntType");
                dtDistributor.Columns.Add("keyid");
                if (distributorId != null)
                {
                    foreach (var item in distributorId)
                    {
                        dtDistributor.Rows.Add(item);
                    }
                }
                else
                {
                    dtDistributor.Rows.Add(0);
                }

                __param.Add("@period", period);
                __param.Add("@profileId", profile);
                __param.Add("@categoryId", dtCategory.AsTableValuedParameter());
                __param.Add("@subcategoryId", dtSubCategory.AsTableValuedParameter());
                __param.Add("@subActivityTypeId", dtsubActivityType.AsTableValuedParameter());
                __param.Add("@activityId", dtActivity.AsTableValuedParameter());
                __param.Add("@subActivityId", dtSubActivity.AsTableValuedParameter());
                __param.Add("@channelId", dtChannel.AsTableValuedParameter());
                __param.Add("@subChannelId", dtSubChannel.AsTableValuedParameter());
                      
                __param.Add("@accountId", dtAccount.AsTableValuedParameter());
                __param.Add("@subAccountId", dtSubAccount.AsTableValuedParameter());
                __param.Add("@distributorId", dtDistributor.AsTableValuedParameter());
                __param.Add("@groupBrandId", dtBrand.AsTableValuedParameter());

                __param.Add("@txtSearch", keyword);
                __param.Add("@sort", sortDirection);
                __param.Add("@order", sortColumn);
                __param.Add("@start", pageNumber);
                __param.Add("@length", pageSize);
                BaseLP baseLP = new BaseLP();
                var _res = await conn.QueryMultipleAsync("ip_ss_tt_list", __param, commandType: CommandType.StoredProcedure, commandTimeout: 180);
                baseLP = _res.Read<BaseLP>().First();
                baseLP.Data = _res.Read<object>().ToList();

                return baseLP;
            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }
        public async Task<object> GetBudgetTTConsoleHistory(string period, string profile, int[] categoryId, int[] subcategoryId,
          int[] subActivityTypeId, int[] activityId, int[] subActivityId, int[] channelId, int[] subChannelId,
          int[] accountId, int[] subAccountId, int[] distributorId, int[] groupBrandId,
          string keyword, string sortColumn, string sortDirection, int pageNumber, int pageSize)
        {
            using IDbConnection conn = Connection;
            try
            {

                var __param = new DynamicParameters();


                DataTable dtChannel = new("ArrayIntType");
                dtChannel.Columns.Add("keyid");
                if (channelId != null)
                {
                    foreach (var channel in channelId)
                    {
                        dtChannel.Rows.Add(channel);
                    }
                }
                else
                {
                    dtChannel.Rows.Add(0);
                }

                DataTable dtBrand = new("ArrayIntType");
                dtBrand.Columns.Add("keyid");
                if (groupBrandId != null)
                {
                    foreach (var brand in groupBrandId)
                    {
                        dtBrand.Rows.Add(brand);
                    }
                }
                else
                {
                    dtBrand.Rows.Add(0);
                }

                DataTable dtAccount = new("ArrayIntType");
                dtAccount.Columns.Add("keyid");
                if (accountId != null)
                {
                    foreach (var item in accountId)
                    {
                        dtAccount.Rows.Add(item);
                    }
                }
                else
                {
                    dtAccount.Rows.Add(0);
                }

                DataTable dtSubAccount = new("ArrayIntType");
                dtSubAccount.Columns.Add("keyid");
                if (subAccountId != null)
                {
                    foreach (var item in subAccountId)
                    {
                        dtSubAccount.Rows.Add(item);
                    }
                }
                else
                {
                    dtSubAccount.Rows.Add(0);
                }

                DataTable dtSubChannel = new("ArrayIntType");
                dtSubChannel.Columns.Add("keyid");
                if (subChannelId != null)
                {
                    foreach (var item in subChannelId)
                    {
                        dtSubChannel.Rows.Add(item);
                    }
                }
                else
                {
                    dtSubChannel.Rows.Add(0);
                }

                DataTable dtCategory = new("ArrayIntType");
                dtCategory.Columns.Add("keyid");
                if (categoryId != null)
                {
                    foreach (var item in categoryId)
                    {
                        dtCategory.Rows.Add(item);
                    }
                }
                else
                {
                    dtCategory.Rows.Add(0);
                }

                DataTable dtSubCategory = new("ArrayIntType");
                dtSubCategory.Columns.Add("keyid");
                if (subcategoryId != null)
                {
                    foreach (var item in subcategoryId)
                    {
                        dtSubCategory.Rows.Add(item);
                    }
                }
                else
                {
                    dtSubCategory.Rows.Add(0);
                }

                DataTable dtsubActivityType = new("ArrayIntType");
                dtsubActivityType.Columns.Add("keyid");
                if (subActivityTypeId != null)
                {
                    foreach (var item in subActivityTypeId)
                    {
                        dtsubActivityType.Rows.Add(item);
                    }
                }
                else
                {
                    dtsubActivityType.Rows.Add(0);
                }

                DataTable dtActivity = new("ArrayIntType");
                dtActivity.Columns.Add("keyid");
                if (activityId != null)
                {
                    foreach (var item in activityId)
                    {
                        dtActivity.Rows.Add(item);
                    }
                }
                else
                {
                    dtActivity.Rows.Add(0);
                }

                DataTable dtSubActivity = new("ArrayIntType");
                dtSubActivity.Columns.Add("keyid");
                if (subActivityId != null)
                {
                    foreach (var item in subActivityId)
                    {
                        dtSubActivity.Rows.Add(item);
                    }
                }
                else
                {
                    dtSubActivity.Rows.Add(0);
                }

                DataTable dtDistributor = new("ArrayIntType");
                dtDistributor.Columns.Add("keyid");
                if (distributorId != null)
                {
                    foreach (var item in distributorId)
                    {
                        dtDistributor.Rows.Add(item);
                    }
                }
                else
                {
                    dtDistributor.Rows.Add(0);
                }

                __param.Add("@period", period);
                __param.Add("@profileId", profile);
                __param.Add("@categoryId", dtCategory.AsTableValuedParameter());
                __param.Add("@subcategoryId", dtSubCategory.AsTableValuedParameter());
                __param.Add("@subActivityTypeId", dtsubActivityType.AsTableValuedParameter());
                __param.Add("@activityId", dtActivity.AsTableValuedParameter());
                __param.Add("@subActivityId", dtSubActivity.AsTableValuedParameter());
                __param.Add("@channelId", dtChannel.AsTableValuedParameter());
                __param.Add("@subChannelId", dtSubChannel.AsTableValuedParameter());

                __param.Add("@accountId", dtAccount.AsTableValuedParameter());
                __param.Add("@subAccountId", dtSubAccount.AsTableValuedParameter());
                __param.Add("@distributorId", dtDistributor.AsTableValuedParameter());
                __param.Add("@groupBrandId", dtBrand.AsTableValuedParameter());

                __param.Add("@txtSearch", keyword);
                __param.Add("@sort", sortDirection);
                __param.Add("@order", sortColumn);
                __param.Add("@start", pageNumber);
                __param.Add("@length", pageSize);
                BaseLP baseLP = new BaseLP();
                var _res = await conn.QueryMultipleAsync("ip_ss_tt_his_list", __param, commandType: CommandType.StoredProcedure, commandTimeout: 180);
                baseLP = _res.Read<BaseLP>().First();
                baseLP.Data = _res.Read<object>().ToList();

                return baseLP;
            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }

        public async Task<object> GetBudgetTTConsoleTemplate(string period, string profile, int[] categoryId, int[] subcategoryId,
                int[] subActivityTypeId, int[] activityId, int[] subActivityId, int[] channelId, int[] subChannelId,
                int[] accountId, int[] subAccountId, int[] distributorId, int[] groupBrandId,
                string keyword, string sortColumn, string sortDirection, int pageNumber, int pageSize)
        {
            using IDbConnection conn = Connection;
            try
            {

                var __param = new DynamicParameters();


                DataTable dtChannel = new("ArrayIntType");
                dtChannel.Columns.Add("keyid");
                if (channelId != null)
                {
                    foreach (var channel in channelId)
                    {
                        dtChannel.Rows.Add(channel);
                    }
                }
                else
                {
                    dtChannel.Rows.Add(0);
                }

                DataTable dtBrand = new("ArrayIntType");
                dtBrand.Columns.Add("keyid");
                if (groupBrandId != null)
                {
                    foreach (var brand in groupBrandId)
                    {
                        dtBrand.Rows.Add(brand);
                    }
                }
                else
                {
                    dtBrand.Rows.Add(0);
                }

                DataTable dtAccount = new("ArrayIntType");
                dtAccount.Columns.Add("keyid");
                if (accountId != null)
                {
                    foreach (var item in accountId)
                    {
                        dtAccount.Rows.Add(item);
                    }
                }
                else
                {
                    dtAccount.Rows.Add(0);
                }

                DataTable dtSubAccount = new("ArrayIntType");
                dtSubAccount.Columns.Add("keyid");
                if (subAccountId != null)
                {
                    foreach (var item in subAccountId)
                    {
                        dtSubAccount.Rows.Add(item);
                    }
                }
                else
                {
                    dtSubAccount.Rows.Add(0);
                }

                DataTable dtSubChannel = new("ArrayIntType");
                dtSubChannel.Columns.Add("keyid");
                if (subChannelId != null)
                {
                    foreach (var item in subChannelId)
                    {
                        dtSubChannel.Rows.Add(item);
                    }
                }
                else
                {
                    dtSubChannel.Rows.Add(0);
                }

                DataTable dtCategory = new("ArrayIntType");
                dtCategory.Columns.Add("keyid");
                if (categoryId != null)
                {
                    foreach (var item in categoryId)
                    {
                        dtCategory.Rows.Add(item);
                    }
                }
                else
                {
                    dtCategory.Rows.Add(0);
                }

                DataTable dtSubCategory = new("ArrayIntType");
                dtSubCategory.Columns.Add("keyid");
                if (subcategoryId != null)
                {
                    foreach (var item in subcategoryId)
                    {
                        dtSubCategory.Rows.Add(item);
                    }
                }
                else
                {
                    dtSubCategory.Rows.Add(0);
                }

                DataTable dtsubActivityType = new("ArrayIntType");
                dtsubActivityType.Columns.Add("keyid");
                if (subActivityTypeId != null)
                {
                    foreach (var item in subActivityTypeId)
                    {
                        dtsubActivityType.Rows.Add(item);
                    }
                }
                else
                {
                    dtsubActivityType.Rows.Add(0);
                }

                DataTable dtActivity = new("ArrayIntType");
                dtActivity.Columns.Add("keyid");
                if (activityId != null)
                {
                    foreach (var item in activityId)
                    {
                        dtActivity.Rows.Add(item);
                    }
                }
                else
                {
                    dtActivity.Rows.Add(0);
                }

                DataTable dtSubActivity = new("ArrayIntType");
                dtSubActivity.Columns.Add("keyid");
                if (subActivityId != null)
                {
                    foreach (var item in subActivityId)
                    {
                        dtSubActivity.Rows.Add(item);
                    }
                }
                else
                {
                    dtSubActivity.Rows.Add(0);
                }

                DataTable dtDistributor = new("ArrayIntType");
                dtDistributor.Columns.Add("keyid");
                if (distributorId != null)
                {
                    foreach (var item in distributorId)
                    {
                        dtDistributor.Rows.Add(item);
                    }
                }
                else
                {
                    dtDistributor.Rows.Add(0);
                }

                __param.Add("@period", period);
                __param.Add("@profileId", profile);
                __param.Add("@categoryId", dtCategory.AsTableValuedParameter());
                __param.Add("@subcategoryId", dtSubCategory.AsTableValuedParameter());
                __param.Add("@subActivityTypeId", dtsubActivityType.AsTableValuedParameter());
                __param.Add("@activityId", dtActivity.AsTableValuedParameter());
                __param.Add("@subActivityId", dtSubActivity.AsTableValuedParameter());
                __param.Add("@channelId", dtChannel.AsTableValuedParameter());
                __param.Add("@subChannelId", dtSubChannel.AsTableValuedParameter());

                __param.Add("@accountId", dtAccount.AsTableValuedParameter());
                __param.Add("@subAccountId", dtSubAccount.AsTableValuedParameter());
                __param.Add("@distributorId", dtDistributor.AsTableValuedParameter());
                __param.Add("@groupBrandId", dtBrand.AsTableValuedParameter());

                __param.Add("@txtSearch", keyword);
                __param.Add("@sort", sortDirection);
                __param.Add("@order", sortColumn);
                __param.Add("@start", pageNumber);
                __param.Add("@length", pageSize);
                BudgetSSTTConsoleTemplate temp = new BudgetSSTTConsoleTemplate();
                var _res = await conn.QueryMultipleAsync("ip_ss_tt_tmp_list", __param, commandType: CommandType.StoredProcedure, commandTimeout: 180);
                temp = _res.Read<BudgetSSTTConsoleTemplate>().First();
                temp.Data = _res.Read<object>().ToList();
                temp.Activity = _res.Read<object>().ToList();
                temp.Account= _res.Read<object>().ToList();
                temp.Distributor= _res.Read<object>().ToList();
                temp.Brand= _res.Read<object>().ToList();
                

                return temp;
            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }
        public async Task<object> SetBudgetTTConsoleUploadRC(DataTable data, string profile, string email)
        {
            using IDbConnection conn = Connection;
            try
            {

                var __param = new DynamicParameters();

                __param.Add("@ssttrc", data.AsTableValuedParameter());
                __param.Add("@profileid", profile);
                __param.Add("@email", email);


                var __reader = await conn.QueryMultipleAsync("ip_ss_tt_upload_rc", __param, commandType: CommandType.StoredProcedure, commandTimeout: 180);

                var __res = new
                {
                    errMessage = __reader.Read().ToList(),
                    uploadStat = __reader.Read().ToList(),
                    promoList = __reader.Read().ToList()
                };
                return __res;
            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }

        public async Task<object> SetBudgetTTConsoleUploadDC(DataTable data, string profile, string email)
        {
            using IDbConnection conn = Connection;
            try
            {

                var __param = new DynamicParameters();

                __param.Add("@ssttdc", data.AsTableValuedParameter());
                __param.Add("@profileid", profile);
                __param.Add("@email", email);


                var __reader = await conn.QueryMultipleAsync("ip_ss_tt_upload_dc", __param, commandType: CommandType.StoredProcedure, commandTimeout: 180);
                var __res = new
                {
                    errMessage = __reader.Read().ToList(),
                    uploadStat = __reader.Read().ToList(),
                    promoList = __reader.Read().ToList()
                };
                return __res;
            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }

        public async Task<object> SetBudgetTTConsoleCreate(string period, int category, int subcategory, int channel, int subChannel,
            int account, int subAccount, int distributor, string distributorShortDesc, int subActivityType, int activity, int subActivity, int groupBrand, string budgetName,
            decimal tt,  string profile, string email)
        {

            try
            {
                using IDbConnection conn = Connection;
                DateTime utcTime = DateTime.UtcNow;
                var zone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
               
                var __ssValueParam = new
                {
                    period = period,
                    channelid = channel,
                    subchannelid = subChannel,
                    accountid = account,
                    subaccountid = subAccount,
                    groupbrandid = groupBrand,
                };

                decimal ssValue = 0;
                conn.Open();
                var __res = await conn.QueryAsync<object>("ip_ss_ssvalue_get", __ssValueParam, commandType: CommandType.StoredProcedure, commandTimeout: 180);
                var firstRow = __res.FirstOrDefault(); // Get the first row

                if (firstRow != null)
                {
                    var dictionary = (IDictionary<string, object>)firstRow; // Convert to dictionary
                    ssValue = Convert.ToDecimal(dictionary.Values.LastOrDefault()); // Get the last column value
                }

                conn.Close();
                conn.Open();
                string __query = "SELECT LongDesc FROM tbmst_activity where Id=" + activity;
                string activityDesc = conn.QueryAsync<string>(__query).Result.First();
                //no exception on Feb 6
                //if (activityDesc == "Trading Term - Consumer Promo" || activityDesc == "Trading Term - Promo Fund")
                //{
                //    subActivity = 0;
                //}
                conn.Close();

                var __param = new
                {
                    period = period,
                    categoryid = category,
                    subcategoryid = subcategory,
                    channelid = channel,
                    subchannelid = subChannel,
                    accountid = account,
                    subaccountid = subAccount,
                    distributorid = distributor,
                    distributorshortdesc = distributorShortDesc,
                    subactivitytypeid = subActivityType,
                    activityid = activity,
                    subactivityid = subActivity,
                    groupbrandid = groupBrand,
                    budgetname = budgetName,
                    tt = tt,
                    ssvalue = ssValue,
                    createdBy = profile,
                    createdEmail = email,
                    createdOn = TimeZoneInfo.ConvertTimeFromUtc(utcTime, zone),
                };

                conn.Open();
                try
                {
                    __query = @" INSERT INTO tbtrx_ss_tt 
                                 (period, categoryid, subcategoryid, channelid, subChannelid, accountid, subaccountid,distributorid, 
                                  distributorshortdesc,groupbrandid, subactivitytypeid, activityid, subactivityid, budgetname, tt, ssvalue,
                                  CreatedEmail, CreatedOn, CreatedBy)
                                  VALUES 
                                 (@period, @categoryid, @subcategoryid,@channelid, @subchannelid, @accountid, @subaccountid, @distributorid, 
                                  @distributorshortdesc, @groupbrandid, @subactivitytypeid, @activityid, @subactivityid, @budgetname, @tt, @ssvalue,
                                  @createdEmail, @createdOn, @createdBy) ";
                    return await conn.QueryAsync<object>(__query, __param);
                }
                catch (SqlException sqlEx)
                {
                    if (sqlEx.Number == 2601 || sqlEx.Number == 2627) // Duplicate key error numbers for SQL Server
                    {
                        throw new Exception("TT Consol data already exist");
                    }
                    else
                    {
                        throw;
                    }

                }
               
            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }



        public async Task<object> SetBudgetTTConsoleUpdate(int id, string period, int category, int subcategory, int channel, int subChannel,
            int account, int subAccount, int distributor, string distributorShortDesc, int subActivityType, int activity, int subActivity, int groupBrand, string budgetName,
            decimal tt, string profile, string email)
        {
            try
            {
                using IDbConnection conn = Connection;
                DateTime utcTime = DateTime.UtcNow;
                var zone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
                var __ssValueParam = new
                {
                    period = period,
                    channelid = channel,
                    subchannelid = subChannel,
                    accountid = account,
                    subaccountid = subAccount,
                    groupbrandid = groupBrand,
                };

                decimal ssValue = 0;
                var __res = await conn.QueryAsync<object>("ip_ss_ssvalue_get", __ssValueParam, commandType: CommandType.StoredProcedure, commandTimeout: 180);
                var firstRow = __res.FirstOrDefault(); // Get the first row

                if (firstRow != null)
                {
                    var dictionary = (IDictionary<string, object>)firstRow; // Convert to dictionary
                    ssValue = Convert.ToDecimal(dictionary.Values.LastOrDefault()); // Get the last column value
                }

                conn.Open();
                string __query = "SELECT LongDesc FROM tbmst_activity where Id=" + activity;
                string activityDesc = conn.QueryAsync<string>(__query).Result.First();
                // no exception on Feb 6 add new activity exception Nov 30 2024
                //if (activityDesc == "Trading Term - Consumer Promo" || activityDesc == "Trading Term - Promo Fund")
                //{
                //    subActivity = 0;
                //}
                conn.Close();
                conn.Open();
                var __param = new
                {
                    id = id,
                    period = period,
                    categoryid = category,
                    subcategoryid = subcategory,
                    channelid = channel,
                    subchannelid = subChannel,
                    accountid = account,
                    subaccountid = subAccount,
                    distributorid = distributor,
                    distributorshortdesc = distributorShortDesc,
                    subactivitytypeid = subActivityType,
                    activityid = activity,
                    subactivityid = subActivity,
                    groupbrandid = groupBrand,
                    budgetname = budgetName,
                    tt = tt,
                    ssvalue = ssValue,
                    createdBy = profile,
                    createdEmail = email,
                    createdOn = TimeZoneInfo.ConvertTimeFromUtc(utcTime, zone),
                };

                try
                {
                    __query = @" UPDATE tbtrx_ss_tt 
                              SET   period=@period, categoryid=@categoryid, subcategoryid=@subcategoryid, channelid=@channelid, subChannelid=@subChannelid, 
                                    accountid=@accountid, subaccountid=@subaccountid,distributorid=@distributorid, 
                                    distributorshortdesc=@distributorshortdesc, groupbrandid=@groupbrandid, 
                                    subactivityid = @subactivityid,
                                    subactivitytypeid=@subactivitytypeid, budgetname=@budgetname, tt=@tt, ssvalue=@ssvalue,
                                    modifiedEmail=@createdEmail, modifiedOn=@createdOn, modifiedBy=@createdBy
                            WHERE id=@id ";
                    var __res2 = await conn.QueryAsync<object>(__query, __param);
                }
                catch (SqlException sqlEx )
                {
                    if (sqlEx.Number == 2601 || sqlEx.Number == 2627) // Duplicate key error numbers for SQL Server
                    {
                        throw new Exception("TT Consol data already exist");
                    }
                    else
                    {
                        throw;
                    }

                }
                conn.Close();


                conn.Open();

                // set dataTable
                var __dtParam = new
                {
                    period = period,
                    category = category,
                    subcategory = subcategory,
                    channel = channel,
                    subchannel = subChannel,
                    account = account,
                    subaccount = subAccount,
                    distributor = distributor,
                    distributorshortdesc = distributorShortDesc,
                    groupbrand = groupBrand,
                    subactivitytype = subActivityType,
                    subactivity = subActivity,
                    activity = activity,
                    tt = tt,
                    budgetname = budgetName
                };

                // Add columns to DataTable
                // defined dummy datatable
                string[] headerSSCR = { "period", "channel", "groupbrand", "m1", "m2", "m3", "m4", "m5", "m6", "m7", "m8", "m9", "m10", "m11", "m12" };
                string[] headerSSVolume = { "period", "channel", "subchannel", "account", "subaccount", "region", "groupbrand", "m1", "m2", "m3", "m4", "m5", "m6", "m7", "m8", "m9", "m10", "m11", "m12" };

                DataTable dtSSCR = new DataTable();

                foreach (string header in headerSSCR)
                {
                    // Add each string in the array as a column name
                    dtSSCR.Columns.Add(header, typeof(string)); // Default to string type
                }
                DataTable dtSSVolume = new DataTable();

                foreach (string header in headerSSVolume)
                {
                    // Add each string in the array as a column name
                    dtSSVolume.Columns.Add(header, typeof(string)); // Default to string type
                }

                DataTable dtTT = Helper.ConvertObjectToDataTable(__dtParam);

                var __param3 = new DynamicParameters();
                if (category == 3) // RC
                {
                    string sql = @"declare @sstt SSTTTypeRC

				INSERT INTO  @sstt

				SELECT tt.period,  
				cat.LongDesc category, 
				scat.LongDesc subCategory, 
                c.LongDesc channel, sc.LongDesc subChannel, 
                ac.LongDesc account,  sac.LongDesc subAccount, 
                d.LongDesc distributor, tt.distributorShortDesc, 
				bg.LongDesc groupBrand,
                sactt.LongDesc subActivityType, 
                ISNULL(sact.LongDesc, '') subActivity, 
				act.LongDesc activity,            
				tt.tt/100, tt.budgetName
				
                FROM tbtrx_ss_tt tt
                INNER JOIN tbmst_distributor	d	ON d.Id = tt.distributorId
                INNER JOIN tbmst_channel		c	ON c.Id = tt.channelid
                INNER JOIN tbmst_brand_group	bg	ON bg.Id = tt.groupbrandid
                LEFT JOIN tbmst_subchannel		sc	ON sc.Id = tt.subchannelid
                LEFT JOIN tbmst_account		ac	ON ac.Id = tt.accountid
                LEFT JOIN tbmst_subaccount		sac	ON sac.Id = tt.subaccountid
				LEFT JOIN tbmst_subcategory		scat	ON scat.Id = tt.subcategoryid
				LEFT JOIN tbmst_activity		act	ON act.Id = tt.activityid
				LEFT JOIN tbmst_subactivity	sact	ON sact.Id = tt.subactivityid
				LEFT JOIN tbmst_category cat	ON cat.Id = tt.categoryid
				LEFT JOIN tbmst_subactivity_type	sactt	ON sactt.Id = tt.subactivitytypeid
                WHERE tt.id=" + id +


				" EXEC [dbo].[ip_ss_tt_upload_rc_promo_update] @sstt";

                  
                    var _res3 = await conn.QueryAsync<object>(sql,  commandTimeout: 180);

                    return _res3.ToList();
                } else
                {
                    string sql = @"declare @sstt SSTTTypeDC

				INSERT INTO  @sstt

				SELECT tt.period,  
				cat.LongDesc category, 
				scat.LongDesc subCategory, 
                c.LongDesc channel, 
                d.LongDesc distributor, tt.distributorShortDesc, 
				bg.LongDesc groupBrand,
                sactt.LongDesc subActivityType, 				
                ISNULL(sact.LongDesc, '') subActivity, 
                act.LongDesc activity, 
				tt.tt/100, tt.budgetName
				
                FROM tbtrx_ss_tt tt
                INNER JOIN tbmst_distributor	d	ON d.Id = tt.distributorId
                INNER JOIN tbmst_channel		c	ON c.Id = tt.channelid
                INNER JOIN tbmst_brand_group	bg	ON bg.Id = tt.groupbrandid
                LEFT JOIN tbmst_subchannel		sc	ON sc.Id = tt.subchannelid
                LEFT JOIN tbmst_account		ac	ON ac.Id = tt.accountid
                LEFT JOIN tbmst_subaccount		sac	ON sac.Id = tt.subaccountid
				LEFT JOIN tbmst_subcategory		scat	ON scat.Id = tt.subcategoryid
				LEFT JOIN tbmst_activity		act	ON act.Id = tt.activityid
				LEFT JOIN tbmst_subactivity	sact	ON sact.Id = tt.subactivityid
				LEFT JOIN tbmst_category cat	ON cat.Id = tt.categoryid
				LEFT JOIN tbmst_subactivity_type	sactt	ON sactt.Id = tt.subactivitytypeid             
                WHERE tt.id=" + id +


                " EXEC [dbo].[ip_ss_tt_upload_dc_promo_update] @sstt";

                    var _res3 = await conn.QueryAsync<object>(sql, commandTimeout: 180);

                    return _res3.ToList();
     
                }

            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }

        public async Task<object> GetBudgetTTConsoleByid(int id)
        {
            try
            {
                using (IDbConnection conn = Connection)
                {
                    var __query = @"SELECT 
                tt.Id, tt.period, tt.categoryId, cat.LongDesc category, 
				tt.subCategoryId, scat.LongDesc subCategory, 
                tt.channelId, c.LongDesc channel, tt.subChannelId, sc.LongDesc subChannel, 
                tt.accountId, ac.LongDesc account, tt.subAccountId, sac.LongDesc subAccount, 
                tt.distributorId, d.LongDesc distributor, tt.distributorShortDesc, 
				tt.groupBrandId, bg.LongDesc groupBrand,
                tt.subActivityTypeId, sactt.LongDesc subActivityType, 
				tt.activityId, act.LongDesc activity, 
                tt.subActivityId, sact.LongDesc subActivity, 
                tt.budgetName, CAST(tt.tt AS DECIMAL(10, 2)) tt, tt.ssValue, tt.budgetAmount, 
                IIF(tt.ModifiedOn IS NULL, tt.createdOn, tt.modifiedOn) lastUpdatedOn, 
                IIF(tt.ModifiedOn IS NULL, tt.createdBy, tt.modifiedBy) lastUpdatedBy, 
                IIF(tt.ModifiedOn IS NULL, tt.createdEmail, tt.modifiedEmail) lastUpdatedByEmail
                FROM tbtrx_ss_tt tt
                INNER JOIN tbmst_distributor	d	ON d.Id = tt.distributorId
                INNER JOIN tbmst_channel		c	ON c.Id = tt.channelid
                INNER JOIN tbmst_brand_group	bg	ON bg.Id = tt.groupbrandid
                LEFT JOIN tbmst_subchannel		sc	ON sc.Id = tt.subchannelid
                LEFT JOIN tbmst_account		ac	ON ac.Id = tt.accountid
                LEFT JOIN tbmst_subaccount		sac	ON sac.Id = tt.subaccountid
				LEFT JOIN tbmst_subcategory		scat	ON scat.Id = tt.subcategoryid
				LEFT JOIN tbmst_activity		act	ON act.Id = tt.activityid
				LEFT JOIN tbmst_subactivity	sact	ON sact.Id = tt.subactivityid
				LEFT JOIN tbmst_category cat	ON cat.Id = tt.categoryid
				LEFT JOIN tbmst_subactivity_type	sactt	ON sactt.Id = tt.subactivitytypeid
                WHERE tt.id=@id";
                    return await conn.QueryAsync<object>(__query, new { id = id });
                }
            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }

        public async Task<object> SetBudgetPSInputUpload(DataTable data, string profile, string email)
        {
            using IDbConnection conn = Connection;
            try
            {

                var __param = new DynamicParameters();

                __param.Add("@psvalue", data.AsTableValuedParameter());
                __param.Add("@profileid", profile);
                __param.Add("@email", email);


                var __reader = await conn.QueryMultipleAsync("ip_ps_value_upload", __param, commandType: CommandType.StoredProcedure, commandTimeout: 180);

                var __res = new
                {
                    errMessage = __reader.Read().ToList(),
                    uploadStat = __reader.Read().ToList(),
                    promoList = __reader.Read().ToList()
                };

                return __res;

            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }

        public async Task<object> GetBudgetPSInputLP(string period, int[] distributorid,  int[] groupBrandId,
          string keyword, string sortColumn, string sortDirection, int pageNumber, int pageSize)
        {
            using IDbConnection conn = Connection;
            try
            {

                var __param = new DynamicParameters();
                __param.Add("@period", period);

                DataTable dtDistributor = new("ArrayIntType");
                dtDistributor.Columns.Add("keyid");
                if (distributorid != null)
                {
                    foreach (var item in distributorid)
                    {
                        dtDistributor.Rows.Add(item);
                    }
                }
                else
                {
                    dtDistributor.Rows.Add(0);
                }

                DataTable dtBrand = new("ArrayIntType");
                dtBrand.Columns.Add("keyid");
                if (groupBrandId != null)
                {
                    foreach (var brand in groupBrandId)
                    {
                        dtBrand.Rows.Add(brand);
                    }
                }
                else
                {
                    dtBrand.Rows.Add(0);
                }

             
                __param.Add("@distributorid", dtDistributor.AsTableValuedParameter());
                __param.Add("@groupbrandid", dtBrand.AsTableValuedParameter());
               


                __param.Add("@txtSearch", keyword);
                __param.Add("@sort", sortDirection);
                __param.Add("@order", sortColumn);
                __param.Add("@start", pageNumber);
                __param.Add("@length", pageSize);
                BaseLP baseLP = new BaseLP();
                var _res = await conn.QueryMultipleAsync("ip_ps_value_list", __param, commandType: CommandType.StoredProcedure, commandTimeout: 180);
                baseLP = _res.Read<BaseLP>().First();
                baseLP.Data = _res.Read<object>().ToList();

                return baseLP;
            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }

        public async Task<object> GetBudgetPSInputTemplate(string period, int[] distributorid, int[] groupBrandId,
       string keyword, string sortColumn, string sortDirection, int pageNumber, int pageSize)
        {
            using IDbConnection conn = Connection;
            try
            {

                var __param = new DynamicParameters();
                __param.Add("@period", period);

                DataTable dtDistributor = new("ArrayIntType");
                dtDistributor.Columns.Add("keyid");
                if (distributorid != null)
                {
                    foreach (var item in distributorid)
                    {
                        dtDistributor.Rows.Add(item);
                    }
                }
                else
                {
                    dtDistributor.Rows.Add(0);
                }

                DataTable dtBrand = new("ArrayIntType");
                dtBrand.Columns.Add("keyid");
                if (groupBrandId != null)
                {
                    foreach (var brand in groupBrandId)
                    {
                        dtBrand.Rows.Add(brand);
                    }
                }
                else
                {
                    dtBrand.Rows.Add(0);
                }


                __param.Add("@distributorid", dtDistributor.AsTableValuedParameter());
                __param.Add("@groupbrandid", dtBrand.AsTableValuedParameter());



                __param.Add("@txtSearch", keyword);
                __param.Add("@sort", sortDirection);
                __param.Add("@order", sortColumn);
                __param.Add("@start", pageNumber);
                __param.Add("@length", pageSize);
                BudgetPSInputTemplate baseLP = new BudgetPSInputTemplate();
                var _res = await conn.QueryMultipleAsync("ip_ps_value_tmp_list", __param, commandType: CommandType.StoredProcedure, commandTimeout: 180);
                baseLP = _res.Read<BudgetPSInputTemplate>().First();
                baseLP.Data = _res.Read<object>().ToList();
                baseLP.Distributor = _res.Read<object>().ToList();

                return baseLP;
            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }
        public async Task<object> GetBudgetPSInputFilter()
        {
            try
            {
                using (IDbConnection conn = Connection)
                {
                    conn.Open();
                    var sql = @" SELECT id,  longDesc, shortDesc
                            FROM tbmst_distributor
                            WHERE  ISNULL(IsDeleted, 0) = 0 ";

                    sql += @"SELECT DISTINCT entityId, entityDesc entity, entityShortDesc, groupBrandId, groupBrandDesc groupBrand
                             FROM vw_sku_active 
                             ORDER BY entityDesc, groupBrandDesc ";

                    using (var __resp = await conn.QueryMultipleAsync(sql, commandTimeout: 180))
                    {
                        var result = new
                        {
                            distributor = __resp.Read<object>().ToList(),
                            grpBrand = __resp.Read<object>().ToList(),
                        };

                        return result;
                    }

                }
            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }
    }

public class BudgetSSVolumeTemplate 
    {
        public int TotalCount { get; set; }
        public int FilteredCount { get; set; }
        public List<object>? Data { get; set; }
        public List<object>? Region { get; set; }
    }
}

public class BudgetSSTTConsoleTemplate
{
    public int TotalCount { get; set; }
    public int FilteredCount { get; set; }
    public List<object>? Data { get; set; }
    public List<object>? Activity { get; set; }
    public List<object>? Account { get; set; }
    public List<object>? Distributor { get; set; }
    public List<object>? Brand { get; set; }

}

public class BudgetPSInputTemplate
{
    public int TotalCount { get; set; }
    public int FilteredCount { get; set; }
    public List<object>? Data { get; set; }
    public List<object>? Distributor { get; set; }
//    public List<object>? Brand { get; set; }

}