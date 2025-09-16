using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using Dapper;
using Microsoft.Extensions.Configuration;
using Repositories.Contracts;
using Repositories.Entities.Models;

namespace Repositories.Repos
{
    public class MatrixPromoRepository : IMatrixPromoRepository
    {
        readonly IConfiguration __config;
        public MatrixPromoRepository(IConfiguration config)
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
        private DataTable _castToDataTable<T>(T __type, List<T> __contents)
        {
            DataTable datas = new();
            try
            {
                PropertyInfo[] __columns = typeof(T).GetProperties();
                foreach (PropertyInfo v in __columns)
                {
                    if (v.GetCustomAttribute(typeof(System.ComponentModel.DisplayNameAttribute)) is not System.ComponentModel.DisplayNameAttribute __dispName)
                        datas.Columns.Add(v.Name, v.PropertyType);
                    else
                        datas.Columns.Add(__dispName.DisplayName, v.PropertyType);
                }
                if (__contents != null)
                {
                    foreach (var r in __contents)
                    {
                        DataRow __row = datas.NewRow();
                        foreach (PropertyInfo v in __columns)
                        {
                            if (v.GetCustomAttribute(typeof(System.ComponentModel.DisplayNameAttribute)) is not System.ComponentModel.DisplayNameAttribute __dispName)
                                __row[v.Name] = v.GetValue(r);
                            else
                                __row[__dispName.DisplayName] = v.GetValue(r);
                        }
                        datas.Rows.Add(__row);
                    }
                }
            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
            return datas;
        }
        public async Task<object> CreateMatrixPromoAproval(MatrixPromoApprovalInsert body)
        {
            try
            {
                DataTable __matrixapprover = _castToDataTable(new MatrixApproverDetail(), null!);
                foreach (MatrixApproverDetail v in body.matrixApprover!)
                    __matrixapprover.Rows.Add(v.SeqApproval, v.Approver);
                using IDbConnection conn = Connection;

                var __param = new DynamicParameters();
                //EP1 2024 #143 
                // __param.Add("@periode", body.periode);
                __param.Add("@entityid", body.entityid);
                __param.Add("@distributorid", body.distributorid);
                __param.Add("@subactivitytypeid", body.subactivitytypeid);
                __param.Add("@channelid", body.channelid);
                __param.Add("@subchannelid", body.subchannelid);
                __param.Add("@initiator", body.initiator);
                __param.Add("@mininvestment", body.mininvestment);
                __param.Add("@maxinvestment", body.maxinvestment);
                __param.Add("@userid", body.userid);
                __param.Add("@useremail", body.useremail);
                __param.Add("@categoryId", body.categoryId);
                __param.Add("@matrix", __matrixapprover.AsTableValuedParameter());

                object result = await conn.QueryAsync<object>("ip_matrix_approval_insert", __param, 
                    commandType: CommandType.StoredProcedure, commandTimeout:180);
                return result;
            }
            catch (System.Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }

        public async Task<IList<ChannelforMatrixPromo>> GetChannelforMatrixPromo()
        {
            using IDbConnection conn = Connection;
            try
            {
                var __query = @"SELECT Id, LongDesc FROM tbmst_channel WHERE ISNULL(IsDelete,0)=0 ";
                var __res = await conn.QueryAsync<ChannelforMatrixPromo>(__query);
                return __res.ToList();
            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }

        public async Task<IList<DistributorforMatrixPromo>> GetDistributorforMatrixPromo(int PrincipalId)
        {
            using IDbConnection conn = Connection;
            try
            {
                var __query = @"SELECT DistributorId, td.LongDesc  FROM tbset_principal_distributor a LEFT JOIN 
                                    tbmst_distributor td 
                                    on DistributorId = td.Id  
                                    WHERE ISNULL(a.IsDeleted,0)=0  
                                    AND ISNULL(td.IsDeleted,0)=0 AND a.PrincipalId = @PrincipalId ";
                var __res = await conn.QueryAsync<DistributorforMatrixPromo>(__query, new { PrincipalId = PrincipalId });
                return __res.ToList();
            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }

        public async Task<IList<EntityforMatrixPromo>> GetEntityForMatrixPromo()
        {
            using IDbConnection conn = Connection;
            try
            {
                var __query = @"SELECT Id, LongDesc FROM tbmst_principal WHERE ISNULL(IsDeleted,0)=0 ";
                var __res = await conn.QueryAsync<EntityforMatrixPromo>(__query);
                return __res.ToList();
            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }

        public async Task<IList<InitiatorforMatrixPromo>> GetInitiatorforMatrixPromo()
        {
            using IDbConnection conn = Connection;
            try
            {
                var __query = @"SELECT Id FROM tbset_user WHERE ISNULL(IsDeleted,0)=0 ";
                var __res = await conn.QueryAsync<InitiatorforMatrixPromo>(__query);
                return __res.ToList();
            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }
        public async Task<IList<SubActivityTypeforMatrixPromo>> GetSubActivityTypeforMatrixPromo()
        {
            using IDbConnection conn = Connection;
            try
            {
                var __query = @"SELECT Id, LongDesc FROM tbmst_subactivity_type WHERE ISNULL(IsDeleted,0)=0 ";
                var __res = await conn.QueryAsync<SubActivityTypeforMatrixPromo>(__query);
                return __res.ToList();
            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }

        public async Task<IList<SubChannelforMatrixPromo>> GetSubChannelforMatrixPromo(int ChannelId)
        {
            using IDbConnection conn = Connection;
            try
            {
                var __query = @"select ts.id, ts.LongDesc 
                                    from tbmst_subchannel ts 
                                    inner join tbmst_channel tc on ts.ChannelId = tc.Id and isnull(tc.IsDelete, 0) = 0
                                    where isnull(ts.IsDelete, 0) = 0 and  tc.id = @ChannelId";
                var __res = await conn.QueryAsync<SubChannelforMatrixPromo>(__query, new { ChannelId = ChannelId });
                return __res.ToList();
            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }

        public async Task<IList<MatrixPromoModel>> GetMatrixPromoAproval(MatrixPromoApprovalBodyReq body)
        {
            try
            {
                using IDbConnection conn = Connection;
                var __param = new DynamicParameters();
                //EP1 2024 #143 
                //__param.Add("@periode", body.periode);
                __param.Add("@entity", body.entity);
                __param.Add("@distributor", body.distributor);
                __param.Add("@userid", body.userid);

                var result = await conn.QueryAsync<MatrixPromoModel>("ip_matrix_approval_lp", __param, commandType: CommandType.StoredProcedure, commandTimeout:180);
                return result.ToList();
            }
            catch (System.Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }
        public async Task<object> GetMatrixPromoAprovalHistory(int category, int entity, int distributor, string userid,
            int start, int length, string txtSearch, string order, string sort)
        {
            try
            {
                using IDbConnection conn = Connection;
                var __param = new DynamicParameters();
                //EP1 2024 #143 
                //__param.Add("@periode", body.periode);
                __param.Add("@category", category);
                __param.Add("@entity",entity);
                __param.Add("@distributor", distributor);
                __param.Add("@userid", userid);
                __param.Add("@start", start);
                __param.Add("@length", length);
                __param.Add("@txtSearch", txtSearch);
                __param.Add("@order", order);
                __param.Add("@sort", sort);



                BaseLP baseLP = new BaseLP();
                var __result = conn.QueryMultiple("ip_matrix_approval_history_list", __param, commandType: CommandType.StoredProcedure, commandTimeout: 180);

                baseLP = __result.Read<BaseLP>().First();
                baseLP.Data = __result.Read<object>().ToList();
                return baseLP;
            }
            catch (System.Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }

        public async Task<GetMatrixPromoAprovalbyIdResult> GetMatrixPromoAprovalbyId(GetMatrixPromoAprovalbyIdBody body)
        {
            try
            {
                using IDbConnection conn = Connection;
                var __param = new DynamicParameters();
                __param.Add("@id", body.id);
                var __obj = await conn.QueryMultipleAsync("ip_matrix_approval_getbyid", __param, commandType: CommandType.StoredProcedure, commandTimeout:180);
                var __headerDtos = __obj.Read<MatrixPromoModel>().FirstOrDefault();
                var __detailMatrixDtos = __obj.Read<MatrixPromoApproverDetail>();

                GetMatrixPromoAprovalbyIdResult __res = new()
                {
                    header = __headerDtos,
                    detailMatrix = __detailMatrixDtos.ToList()
                };
                return __res;
            }
            catch (System.Exception __ex)
            {
                throw new Exception(__ex.Message);
            }

        }
        public async Task<object> UpdateMatrixPromoAproval(MatrixPromoApprovalUpdate body)
        {
            try
            {
                DataTable __matrixapprover = _castToDataTable(new MatrixApproverDetail(), null!);
                foreach (MatrixApproverDetail v in body.matrixApprover!)
                    __matrixapprover.Rows.Add(v.SeqApproval, v.Approver);
                using IDbConnection conn = Connection;

                var __param = new DynamicParameters();
                __param.Add("@id", body.id);
                //EP1 2024 #143 
                // __param.Add("@periode", body.periode);
                __param.Add("@entityid", body.entityid);
                __param.Add("@distributorid", body.distributorid);
                __param.Add("@subactivitytypeid", body.subactivitytypeid);
                __param.Add("@channelid", body.channelid);
                __param.Add("@subchannelid", body.subchannelid);
                __param.Add("@initiator", body.initiator);
                __param.Add("@mininvestment", body.mininvestment);
                __param.Add("@maxinvestment", body.maxinvestment);
                __param.Add("@userid", body.userid);
                __param.Add("@useremail", body.useremail);
                __param.Add("@categoryId", body.categoryId);
                __param.Add("@matrix", __matrixapprover.AsTableValuedParameter());

                object result = await conn.QueryAsync<object>("ip_matrix_approval_update", __param, 
                    commandType: CommandType.StoredProcedure, commandTimeout:180);
                return result;

            }
            catch (System.Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }

        public async Task<IList<CategoryforMatrixPromo>> GetCategoryforMatrixPromo()
        {
            using IDbConnection conn = Connection;
            try
            {
                var __query = @"SELECT Id, LongDesc FROM tbmst_category WHERE ISNULL(IsDeleted,0)=0 ";
                var __res = await conn.QueryAsync<CategoryforMatrixPromo>(__query);
                return __res.ToList();
            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }

        public async Task<IList<object>> GetSubActivityTypebyCategoryId(int categoryId)
        {
            using IDbConnection conn = Connection;
            var __param = new DynamicParameters();
            __param.Add("@categoryId", categoryId);

            var __res = await conn.QueryAsync<object>("ip_getsubactivitytype_bycategory", __param, commandType: CommandType.StoredProcedure, commandTimeout:180);

            return __res.ToList();

        }
    }
}