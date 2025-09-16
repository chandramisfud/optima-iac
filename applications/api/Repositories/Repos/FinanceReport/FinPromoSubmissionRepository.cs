using System.Data;
using System.Data.SqlClient;
using Dapper;
using Microsoft.Extensions.Configuration;
using Repositories.Contracts;
using Repositories.Entities.Models;

namespace Repositories.Repos
{
    public class FinPromoSubmissionRepository : IFinPromoSubmissionReportRepo
    {
        readonly IConfiguration __config;
        public FinPromoSubmissionRepository(IConfiguration config)
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

        public async Task<IList<FinPromoSubmissionChannelList>> GetChannelList(string userid, int[] arrayParent)
        {
            try
            {
                using IDbConnection conn = Connection;
                DataTable __parent = new("ArrayIntType");
                __parent.Columns.Add("keyid");
                foreach (int v in arrayParent)
                    __parent.Rows.Add(v);

                var __query = new DynamicParameters();

                __query.Add("@userid", userid);
                __query.Add("@attribute", "channel");
                __query.Add("@parent", __parent.AsTableValuedParameter());


                conn.Open();
                var child = await conn.QueryAsync<FinPromoSubmissionChannelList>("ip_getattribute_bymapping", __query, commandType: CommandType.StoredProcedure, commandTimeout:180);
                return child.ToList();
            }
            catch (System.Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }

        public async Task<IList<FinPromoSubmissionDistributorList>> GetDistributorList(int budgetid, int[] arrayParent)
        {
            try
            {
                using IDbConnection conn = Connection;
                DataTable __parent = new("ArrayIntType");
                __parent.Columns.Add("keyid");
                foreach (int v in arrayParent)
                    __parent.Rows.Add(v);

                var __query = new DynamicParameters();

                __query.Add("@budgetid", budgetid);
                __query.Add("@attribute", "distributor");
                __query.Add("@parent", __parent.AsTableValuedParameter());


                conn.Open();
                var child = await conn.QueryAsync<FinPromoSubmissionDistributorList>("ip_getattribute_byparent", __query, commandType: CommandType.StoredProcedure, commandTimeout:180);
                return child.ToList();
            }
            catch (System.Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }

        public async Task<IList<FinPromoSubmissionEntityList>> GetEntityList()
        {
            try
            {
                using IDbConnection conn = Connection;
                var __query = @"SELECT Id, ShortDesc, LongDesc FROM tbmst_principal WHERE ISNULL(IsDeleted,0)=0 ";
                var __res = await conn.QueryAsync<FinPromoSubmissionEntityList>(__query);
                return __res.ToList();
            }
            catch (System.Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }

        public async Task<IList<ConfigLatePromoCreation>> GetFinLatePromoSubmission()
        {
            try
            {
                using IDbConnection conn = Connection;
                var result = await conn.QueryAsync<ConfigLatePromoCreation>("ip_conf_latepromocreation", commandType: CommandType.StoredProcedure, commandTimeout:180);
                return result.AsList();
            }
            catch (System.Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }
        public async Task<PromoSubmissionLandingPage> GetFinPromoSubmissionLandingPage(
            string period,
            int entityId,
            int distributorId,
            int channelId,
            string profileId,
            string search,
            string sortColumn,
            string sortDirection = "ASC",
            int pageNum = 0,
            int dataDisplayed = 10
            )
        {
            try
            {
                PromoSubmissionLandingPage res = new();
                using (IDbConnection conn = Connection)
                {
                    var startData = pageNum * dataDisplayed;
                    search = (search) ?? "";

                    var __param = new DynamicParameters();
                    __param.Add("@periode", period);
                    __param.Add("@entity", entityId);
                    __param.Add("@distributor", distributorId);
                    __param.Add("@channel", channelId);
                    __param.Add("@userid", profileId);
                    __param.Add("@start", startData);
                    __param.Add("@length", dataDisplayed);
                    __param.Add("@filter", "");
                    __param.Add("@txtSearch", search);

                    var __object = await conn.QueryMultipleAsync("ip_promo_submission_list_p", __param, commandType: CommandType.StoredProcedure, commandTimeout:180);
                    var data = __object.Read<PromoSubmissionData>().ToList();

                    var count = __object.ReadSingle<FinPromoSubmissionRecord>();

                    res.totalCount = count.recordsTotal;
                    res.filteredCount = count.recordsFiltered;

                    res.Data = data;
                }
                return res;
            }
            catch (System.Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }

        public async Task<IList<PromoSubmissonExceptionList>> GetPromoSubmissonExceptionList(string idx)
        {
            try
            {
                using IDbConnection conn = Connection;
                var __param = new DynamicParameters();
                __param.Add("@idx", idx);
                var result = await conn.QueryAsync<PromoSubmissonExceptionList>("ip_promo_submission_exception_list", __param, commandType: CommandType.StoredProcedure, commandTimeout:180);
                return result.ToList();
            }
            catch (System.Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }

        public async Task<IEnumerable<PromoSubmissionUserGroup>> GetUserGroupList()
        {
            try
            {
                IEnumerable<PromoSubmissionUserGroup> res;
                using (IDbConnection conn = Connection)
                {
                    var __query = @"SELECT a.*, b.name as groupmenupermissionname FROM tbset_usergroup a left join tbset_config_dropdown b on a.groupmenupermission = b.id";
                    res = await conn.QueryAsync<PromoSubmissionUserGroup>(__query);
                }
                return res;
            }
            catch (System.Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }

        public async Task<List<PromoSubmissionUser>> GetUserList(string usergroupid, int userlevel, int isdeleted)
        {
            try
            {
                using IDbConnection conn = Connection;
                var sqlWhere = "";
                var sqlselect = @"SELECT a.*, case when isdeleted=0 then 'Active' else 'Inactive' end as statusname, case when isdeleted=0 then 'Active' else 'Deleted' end as statussearch, b.usergroupname, c.levelname FROM tbset_user a 
                            left join tbset_usergroup b on a.usergroupid = b.usergroupid
                            left join tbset_userlevel c on a.userlevel = c.userlevel";
                if (usergroupid != "all")
                {
                    sqlWhere = " where b.usergroupid=@usergroupid";
                }
                else
                {
                    sqlWhere = "";
                }

                if (userlevel != 0)
                {
                    if (sqlWhere == "")
                    {
                        sqlWhere = " where c.userlevel=@userlevel";
                    }
                    else
                    {
                        sqlWhere += " and c.userlevel=@userlevel";
                    }
                }
                if (isdeleted != 0)
                {
                    if (isdeleted == 2)
                    {
                        isdeleted = 0;
                    }
                    if (sqlWhere == "")
                    {
                        sqlWhere = " where isdeleted=@isdeleted";
                    }
                    else
                    {
                        sqlWhere += " and isdeleted=@isdeleted";
                    }
                }
                var sql = sqlselect + sqlWhere;
                var result = await conn.QueryAsync<PromoSubmissionUser>(sql, new { UserGroupId = usergroupid, UserLevel = userlevel, IsDeleted = isdeleted });
                return result.ToList();
            }
            catch (System.Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }

        public async Task<PromoSubmissionList> PromoSubmissionEmail(string period, int entity, int distributor, string userid)
        {
            try
            {
                using IDbConnection conn = Connection;
                var __param = new DynamicParameters();
                __param.Add("@period", period);
                __param.Add("@entity", entity);
                __param.Add("@distributor", distributor);
                __param.Add("@userid", userid);

                var result = await conn.QueryMultipleAsync("ip_promo_submission_email", __param, commandType: CommandType.StoredProcedure, commandTimeout:180);
                var PromoSubmissionList1 = result.Read<PromoSubmissionList1>();
                var PromoSubmissionList2 = result.Read<PromoSubmissionList2>();

                PromoSubmissionList __res = new()
                {
                    submissionlist1 = PromoSubmissionList1.ToList(),
                    submissionlist2 = PromoSubmissionList2.ToList()

                };
                return __res;
            }
            catch (System.Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }

        public async Task PromoSubmissionExceptionClear(string idx)
        {
            try
            {
                using IDbConnection conn = Connection;
                var __param = new DynamicParameters();
                __param.Add("@idx", idx);
                var result = await conn.ExecuteAsync("ip_promo_submission_exception_clear", __param, commandType: CommandType.StoredProcedure, commandTimeout:180);
            }
            catch (System.Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }

        public async Task PromoSubmissionExceptionUpload(DataTable promo, string idx)
        {
            try
            {
                using IDbConnection conn = Connection;
                if (conn.State == ConnectionState.Closed)
                    conn.Open();

                var result = await conn.QueryAsync<DNUpload>("ip_promo_submission_exception_upload",
                new
                {
                    idx = idx,
                    promo = promo.AsTableValuedParameter("PromoSubmissionException")
                }
                   , commandType: CommandType.StoredProcedure, commandTimeout:180);
            }
            catch (System.Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }
    }
}