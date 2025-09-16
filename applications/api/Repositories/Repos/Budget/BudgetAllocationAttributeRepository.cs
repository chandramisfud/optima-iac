using Dapper;
using Repositories.Contracts;
using Repositories.Entities.BudgetAllocation;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Repos
{
    public partial class BudgetMasterRepository : IBudgetMasterRepository
    {
        public async Task<IEnumerable<Region>> GetAllRegion()
        {

            IEnumerable<Region> region;

            using (IDbConnection conn = Connection)
            {     	        
                var __query = new DynamicParameters();
                __query.Add("@ID", string.Empty);
                __query.Add("@NAME", string.Empty);
                region = await conn.QueryAsync<Region>("[dbo].[ip_select_region]", __query, commandType: CommandType.StoredProcedure, commandTimeout:180);
            }
            return region;
        }

        public async Task<IEnumerable<Channel>> GetAllChannel()
        {
            IEnumerable<Channel> __channel = null!;
            using (IDbConnection conn = Connection)
            {
                conn.Open();
                var sql = @"SELECT * FROM tbmst_channel WHERE ISNULL(IsDelete, 0) = 0";
                __channel = await conn.QueryAsync<Channel>(sql);
            }
            return __channel;
        }

        public async Task<IEnumerable<SubChannel>> GetAllSubChannel(int channelId)
        {
            IEnumerable<SubChannel> __subchannel;
            using (IDbConnection conn = Connection)
            {
                var sql = "";
                if (channelId == 0)
                {
                    sql = @"SELECT A.[Id]
                                ,A.[ChannelId]
                                ,A.[LongDesc]
                                ,A.[ShortDesc]
                                ,A.[CreateBy]
                                ,A.[RefId]
                                ,b.[LongDesc] ChannelLongDesc
                                FROM tbmst_subchannel AS A LEFT JOIN
                                tbmst_channel AS B ON A.ChannelId = B.Id 
                                WHERE ISNULL(A.IsDelete, 0) = 0";
                } else
                {
                    sql = @"SELECT A.[Id]
                                ,A.[ChannelId]
                                ,A.[LongDesc]
                                ,A.[ShortDesc]
                                ,A.[CreateBy]
                                ,A.[RefId]
                                ,b.[LongDesc] ChannelLongDesc
                                FROM tbmst_subchannel AS A LEFT JOIN
                                tbmst_channel AS B ON A.ChannelId = B.Id 
                                WHERE ISNULL(A.IsDelete, 0) = 0 AND ChannelId=" + channelId;
                }
                __subchannel = await conn.QueryAsync<SubChannel>(sql);
            }
            return __subchannel;
        }

        public async Task<IEnumerable<Account>> GetAllAccount(int subChannelId)
        {
            IEnumerable<Account> __account;
            using (IDbConnection conn = Connection)
            {
                var sql = "";
                if (subChannelId == 0)
                {
                    sql = @"SELECT A.[Id]
                                        ,A.[SubChannelId]
                                        ,A.[LongDesc]
                                        ,A.[ShortDesc]
                                        ,A.[IsActive]
                                        ,A.[CreateOn]
                                        ,A.[CreateBy]
                                        ,A.[RefId]
                                        ,B.[LongDesc] SubChannelLongDesc
                                    FROM [dbo].[tbmst_account] 
                                    as A left join tbmst_subchannel AS B on 
                                    A.SubChannelId = b.Id 
                                    WHERE ISNULL(A.IsDelete, 0) = 0";
                } else
                {
                    sql = @"SELECT A.[Id]
                                        ,A.[SubChannelId]
                                        ,A.[LongDesc]
                                        ,A.[ShortDesc]
                                        ,A.[IsActive]
                                        ,A.[CreateOn]
                                        ,A.[CreateBy]
                                        ,A.[RefId]
                                        ,B.[LongDesc] SubChannelLongDesc
                                    FROM [dbo].[tbmst_account] 
                                    as A left join tbmst_subchannel AS B on 
                                    A.SubChannelId = b.Id 
                                    WHERE ISNULL(A.IsDelete, 0) = 0 AND SubChannelId=" + subChannelId;
                }
                __account = await conn.QueryAsync<Account>(sql);
            }
            return __account;
        }

        public async Task<IEnumerable<ViewSubAccount>> GetAllSubAccount(int accountId)
        {
            IEnumerable<ViewSubAccount> __subacc;
            using (IDbConnection conn = Connection)
            {
                var sql = "";
                if (accountId == 0)
                {
                    sql = @"select SubAccountId as id, 
                                ChannelRefID, SubChannelRefID, AccountRefID, AccountID,
                                SubAccountRefID,  ChannelDesc, SubChannelDesc, 
                                AccountDesc, SubAccountDesc as longDesc
                                from vw_account";
                }
                else
                {
                    sql = @"select SubAccountId as id, 
                                ChannelRefID, SubChannelRefID, AccountRefID, AccountID,
                                SubAccountRefID as refId,  ChannelDesc, SubChannelDesc, 
                                AccountDesc, SubAccountDesc as longDesc
                                from vw_account
                                WHERE AccountId=" + accountId;
                }
                __subacc = await conn.QueryAsync<ViewSubAccount>(sql, commandTimeout: 280);
            }
            return __subacc;
        }

        public async Task<List<UserAllDto>> GetAllActiveUser()
        {

            using IDbConnection conn = Connection;
            var sql = @"SELECT a.*, case when isdeleted=0 then 'Active' else 'Inactive' end as statusname, 
            case when isdeleted=0 then 'Active' else 'Deleted' end as statussearch, 
            b.usergroupname, c.levelname FROM tbset_user a 
            left join tbset_usergroup b on a.usergroupid = b.usergroupid
            left join tbset_userlevel c on a.userlevel = c.userlevel
            WHERE ISNULL(isdeleted, 0)=0";
            var result = await conn.QueryAsync<UserAllDto>(sql);
            return result.ToList();
        }

        public async Task<IList<MasterIdDesc>> GetBrandAttributeByParent(int budgetid, int[] arrayParent)
        {
            using IDbConnection conn = Connection;

            DataTable __parent = new("ArrayIntType");
            __parent.Columns.Add("keyid");
            foreach (int v in arrayParent)
                __parent.Rows.Add(v);

            var __query = new DynamicParameters();

            __query.Add("@budgetid", budgetid);
            __query.Add("@attribute", "brand");
            __query.Add("@parent", __parent.AsTableValuedParameter());


            conn.Open();
            var child = await conn.QueryAsync<MasterIdDesc>("[dbo].[ip_getattribute_byparent]", __query, commandType: CommandType.StoredProcedure, commandTimeout:180);
            return child.ToList();
        }

        public async Task<IList<MasterIdDesc>> GetSubCategoryAttributeByParent(int budgetid, int[] arrayParent)
        {
            using IDbConnection conn = Connection;

            DataTable __parent = new("ArrayIntType");
            __parent.Columns.Add("keyid");
            foreach (int v in arrayParent)
                __parent.Rows.Add(v);

            var __query = new DynamicParameters();

            __query.Add("@budgetid", budgetid);
            __query.Add("@attribute", "subcategory");
            __query.Add("@parent", __parent.AsTableValuedParameter());


            conn.Open();
            var child = await conn.QueryAsync<MasterIdDesc>("[dbo].[ip_getattribute_byparent]", __query, commandType: CommandType.StoredProcedure, commandTimeout:180);
            return child.ToList();
        }

        public async Task<IList<MasterIdDesc>> GetActivityAttributeByParent(int budgetid, int[] arrayParent)
        {
            using IDbConnection conn = Connection;

            DataTable __parent = new("ArrayIntType");
            __parent.Columns.Add("keyid");
            foreach (int v in arrayParent)
                __parent.Rows.Add(v);

            var __query = new DynamicParameters();

            __query.Add("@budgetid", budgetid);
            __query.Add("@attribute", "activity");
            __query.Add("@parent", __parent.AsTableValuedParameter());


            conn.Open();
            var child = await conn.QueryAsync<MasterIdDesc>("[dbo].[ip_getattribute_byparent]", __query, commandType: CommandType.StoredProcedure, commandTimeout:180);
            return child.ToList();
        }

        public async Task<IList<MasterIdDesc>> GetSubActivityAttributeByParent(int budgetid, int[] arrayParent)
        {
            using IDbConnection conn = Connection;

            DataTable __parent = new("ArrayIntType");
            __parent.Columns.Add("keyid");
            foreach (int v in arrayParent)
                __parent.Rows.Add(v);

            var __query = new DynamicParameters();

            __query.Add("@budgetid", budgetid);
            __query.Add("@attribute", "subactivity");
            __query.Add("@parent", __parent.AsTableValuedParameter());


            conn.Open();
            var child = await conn.QueryAsync<MasterIdDesc>("[dbo].[ip_getattribute_byparent]", __query, commandType: CommandType.StoredProcedure, commandTimeout:180);
            return child.ToList();
        }
        public async Task<IEnumerable<Product>> GetAllProductByBrandId(int brandId)
        {
            IEnumerable<Product> product;
            using (IDbConnection conn = Connection)
            {
                var sql = "";
                if (brandId == 0)
                {
                    sql = @"SELECT  A.[Id]
                            ,A.[PrincipalId]
                            ,A.[BrandId]
                            ,A.[LongDesc]
                            ,A.[ShortDesc]
                            ,A.[CreateBy]
                            ,A.[RefId]
							,B.[ShortDesc] PrincipalShortDesc
                            ,B.[LongDesc] PrincipalLongDesc
							,C.[ShortDesc] BrandShortDesc
                            ,C.[LongDesc] BrandLongDesc
                        FROM  [dbo].tbmst_product AS A LEFT JOIN 
                            [dbo].tbmst_principal AS B ON A.PrincipalId = B.Id LEFT JOIN
                            [dbo].tbmst_brand as c ON A.BrandId = C.Id 
                        WHERE ISNULL(A.IsDeleted, 0) = 0";
                } else
                {
                    sql = @"SELECT  A.[Id]
                            ,A.[PrincipalId]
                            ,A.[BrandId]
                            ,A.[LongDesc]
                            ,A.[ShortDesc]
                            ,A.[CreateBy]
                            ,A.[RefId]
							,B.[ShortDesc] PrincipalShortDesc
                            ,B.[LongDesc] PrincipalLongDesc
							,C.[ShortDesc] BrandShortDesc
                            ,C.[LongDesc] BrandLongDesc
                        FROM  [dbo].tbmst_product AS A LEFT JOIN 
                            [dbo].tbmst_principal AS B ON A.PrincipalId = B.Id LEFT JOIN
                            [dbo].tbmst_brand as c ON A.BrandId = C.Id 
                        WHERE ISNULL(A.IsDeleted, 0) = 0 AND A.BrandId= " + brandId;
                }
                product = await conn.QueryAsync<Product>(sql);
            }
            return product;
        }
    }
}
