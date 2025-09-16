using System.Data;
using System.Data.SqlClient;
using Dapper;
using Microsoft.Extensions.Configuration;
using Repositories.Contracts;
using Repositories.Entities.Models;
using Repositories.Entities.Report;

namespace Repositories.Repos
{
    public class DNListingPromoDistributorRepo : IDNListingPromoDistributorRepo
    {
        readonly IConfiguration __config;
        public DNListingPromoDistributorRepo(IConfiguration config)
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

        public Task<UserProfileDataById> GetById(string id)
        {
            try
            {
                using IDbConnection conn = Connection;
                var userDictionary = new Dictionary<string, UserProfileDataById>();
                string sql = @"SELECT A.[id]
                            ,A.[username]
                            ,A.[email]
                            ,A.[password]
                            ,A.[usergroupid]
                            ,A.[userlevel]
                            ,A.[isLogin]
                            ,A.[lastLogin]
                            ,A.[department]
                            ,A.[jobtitle]
                            ,A.[contactinfo]
                            ,B.[DistributorId]
                            ,A.[registered]
                            ,A.[code]
                            ,A.[password_change]
                            ,A.[token]
                            ,A.[token_date]
                            ,A.[userinput]
                            ,A.[dateinput]
                            ,A.[useredit]
                            ,A.[dateedit]
                            ,A.[isdeleted]
                            ,A.[deletedby]
                            ,A.[deletedon]
                FROM tbset_user AS A LEFT JOIN 
                tbset_user_distributor AS B ON A.id = B.UserId 
                WHERE A.id =@id";
                var list = conn.Query<UserProfileDataById, ListDistributor, UserProfileDataById>(
                sql, (user, distributor) =>
                {

                    if (!userDictionary.TryGetValue(user.id!, out UserProfileDataById? userEntry))
                    {
                        userEntry = user;
                        userEntry.distributorlist = new List<ListDistributor>();
                        userDictionary.Add(userEntry.id!, userEntry);
                    }

                    userEntry.distributorlist!.Add(distributor);
                    return userEntry;
                },
                    new { Id = id },
                    splitOn: "DistributorId")
                    .Distinct()
                    .FirstOrDefault();

                return Task.FromResult(list)!;
            }
            catch (System.Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }

        public async Task<IList<ListingPromoReportingChannelList>> GetChannelList(string userid, int[] arrayParent)
        {
            using IDbConnection conn = Connection;
            try
            {
                DataTable __parent = new("ArrayIntType");
                __parent.Columns.Add("keyid");
                foreach (int v in arrayParent)
                    __parent.Rows.Add(v);

                var __query = new DynamicParameters();

                __query.Add("@userid", userid);
                __query.Add("@attribute", "channel");
                __query.Add("@parent", __parent.AsTableValuedParameter());


                conn.Open();
                var child = await conn.QueryAsync<ListingPromoReportingChannelList>("ip_getattribute_bymapping", __query, commandType: CommandType.StoredProcedure, commandTimeout:180);
                return child.ToList();
            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }

        public async Task<IList<ListingPromoReportingDistributorList>> GetDistributorList(int budgetid, int[] arrayParent)
        {
            using IDbConnection conn = Connection;
            try
            {
                DataTable __parent = new("ArrayIntType");
                __parent.Columns.Add("keyid");
                foreach (int v in arrayParent)
                    __parent.Rows.Add(v);

                var __query = new DynamicParameters();

                __query.Add("@budgetid", budgetid);
                __query.Add("@attribute", "distributor");
                __query.Add("@parent", __parent.AsTableValuedParameter());

                conn.Open();
                var child = await conn.QueryAsync<ListingPromoReportingDistributorList>("ip_getattribute_byparent", __query, commandType: CommandType.StoredProcedure, commandTimeout:180);
                return child.ToList();
            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }

        public async Task<IList<ListingPromoReportingEntityList>> GetEntityList()
        {
            using IDbConnection conn = Connection;
            try
            {
                var __query = @"SELECT Id, ShortDesc, LongDesc FROM tbmst_principal WHERE ISNULL(IsDeleted,0)=0 ";
                var __res = await conn.QueryAsync<ListingPromoReportingEntityList>(__query);
                return __res.ToList();
            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }

        public async Task<ListingPromoReportingLandingPage> GetListingPromoReportingLandingPage(string period, int categoryId, int entityId, int distributorId, int budgetParentId, int channelId, string profileId, string createFrom, string createTo, string startFrom, string startTo, int submissionParam, string keyword, string sortColumn, string sortDirection = "ASC", int pageNum = 0, int dataDisplayed = 10)
        {
            ListingPromoReportingLandingPage res = new();
            using (IDbConnection conn = Connection)
                try
                {
                    var startData = pageNum * dataDisplayed;
                    keyword = (keyword) ?? "";

                    var __param = new DynamicParameters();
                    __param.Add("@periode", period);
                    __param.Add("@category", categoryId);
                    __param.Add("@entity", entityId);
                    __param.Add("@distributor", distributorId);
                    __param.Add("@budgetparent", budgetParentId);
                    __param.Add("@channel", channelId);
                    __param.Add("@userid", profileId);
                    __param.Add("@create_from", createFrom);
                    __param.Add("@create_to", createTo);
                    __param.Add("@start_from", startFrom);
                    __param.Add("@start_to", startTo);
                    __param.Add("@submission_param", submissionParam);
                    __param.Add("@start", startData);
                    __param.Add("@length", dataDisplayed);
                    __param.Add("@filter", "");
                    __param.Add("@txtSearch", keyword);

                    var __object = await conn.QueryMultipleAsync("ip_promo_list_reporting_p", __param, commandType: CommandType.StoredProcedure, commandTimeout:180);
                    var data = __object.Read<ListingPromoReportingData>().ToList();
                    var count = __object.ReadSingle<ListingPromoReportingRecordCount>();

                    res.totalCount = count.recordsTotal;
                    res.filteredCount = count.recordsFiltered;

                    res.Data = data;
                }
                catch (Exception __ex)
                {
                    throw new Exception(__ex.Message);
                }
            return res;
        }
        public async Task<IList<object>> GetCategoryDropdownList()
        {
            using IDbConnection conn = Connection;
            try
            {
                var __query = @"SELECT id, shortDesc, longDesc FROM tbmst_category WHERE ISNULL(IsDeleted,0)=0 ";
                var __res = await conn.QueryAsync<object>(__query);
                return __res.ToList();
            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }

    }
}