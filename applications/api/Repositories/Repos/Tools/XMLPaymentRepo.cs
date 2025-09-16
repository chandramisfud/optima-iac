using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using Dapper;
using Microsoft.Extensions.Configuration;
using Repositories.Contracts;
using Repositories.Entities.Models;

namespace Repositories.Repos
{
    public class XMLPaymentRepo : IXMLPaymentRepository
    {
        readonly IConfiguration _config;
        public XMLPaymentRepo(IConfiguration config)
        {
            _config = config;
        }

        public IDbConnection Connection
        {
            get
            {
                return new SqlConnection(_config.GetConnectionString("DefaultConnection"));
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
        public async Task<IList<XMLGenerate>> GetXMLGenerate(EntityParam body)
        {
            try
            {
                DataTable __id = new("AttributeType");
                __id.Columns.Add("AttributeType");
                foreach (int v in body.id!)
                    __id.Rows.Add(v);
                using IDbConnection conn = Connection;
                var __param = new DynamicParameters();
                __param.Add("@entity", body.entity);
                __param.Add("@distributor", __id.AsTableValuedParameter());
                var result = await conn.QueryAsync<XMLGenerate>("ip_xml_generate", __param, commandType: CommandType.StoredProcedure, commandTimeout:180);
                return result.AsList();
            }
            catch (System.Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }
        public async Task<IList<XMLGenerateErrorMessage>> GenerateXMLFlagging(XmlFlaggingBody body)
        {
            try
            {
                DataTable __ponumber = _castToDataTable(new PoNumber(), null!);
                using IDbConnection conn = Connection;
                if (conn.State == ConnectionState.Closed)
                    conn.Open();

                foreach (PoNumber v in body.PoNumber!)
                    __ponumber.Rows.Add(v.PONumber, v.OriginalId, v.entityId);

                var result = await conn.QueryAsync<XMLGenerateErrorMessage>("ip_xml_generate_flagging",
               new
               {
                   UserId = body.userid,
                   UserEmail = body.useremail,
                   PoNumber = __ponumber.AsTableValuedParameter()
               }
                   , commandType: CommandType.StoredProcedure, commandTimeout:180);

                return result.ToList();
            }
            catch (System.Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }
        public async Task<IList<XmlGenerateAccrual>> GetXmlGenerateAccrual(XmlGenerateAccrualBody body)
        {
            try
            {
                using IDbConnection conn = Connection;
                var __param = new DynamicParameters();
                __param.Add("@entity", body.entity);
                __param.Add("@userid", body.userid);
                var result = await conn.QueryAsync<XmlGenerateAccrual>("[dbo].[ip_xml_generate_accrual]", __param, commandType: CommandType.StoredProcedure, commandTimeout:180);
                return result.AsList();
            }
            catch (System.Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }
        public async Task<IList<XMLGenerate>> GetXMLGenerateNonBatch(EntityParam body)
        {
            try
            {
                using IDbConnection conn = Connection;
                var __param = new DynamicParameters();
                __param.Add("@entity", body.entity);
                var result = await conn.QueryAsync<XMLGenerate>("ip_xml_generate_non_batching", __param, commandType: CommandType.StoredProcedure, commandTimeout:180);
                return result.AsList();
            }
            catch (System.Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }
        public async Task<IList<XMLGenerateNonBatchDitributorsPayment>> GetXMLGenerateNonBatchDistributorPayment(EntityParam body)
        {
            try
            {
                using IDbConnection conn = Connection;
                var __param = new DynamicParameters();
                __param.Add("@entity", body.entity);
                var result = await conn.QueryAsync<XMLGenerateNonBatchDitributorsPayment>("ip_xml_generate_non_batching_dist_payment", __param, commandType: CommandType.StoredProcedure, commandTimeout:180);
                return result.AsList();
            }
            catch (System.Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }
        public async Task<IList<XmlGenerateAccrualById>> GenerateXMLAccrualbyId(int id)
        {
            try
            {
                using IDbConnection conn = Connection;
                var __param = new DynamicParameters();
                __param.Add("@id", id);
                var result = await conn.QueryAsync<XmlGenerateAccrualById>("ip_xml_generate_accrual_byid", __param, commandType: CommandType.StoredProcedure, commandTimeout:180);
                return result.ToList();
            }
            catch (System.Exception __ex)
            {
                throw new Exception(__ex.Message);
            }

        }
        public async Task<IList<XMLFlaggingList>> GetXMLFlaggingList(int entityId, string userProfileId, string generateOn)
        {
            try
            {
                using IDbConnection conn = Connection;
                var __param = new DynamicParameters();
                __param.Add("@entityid", entityId);
                __param.Add("@userid", userProfileId);
                __param.Add("@generateon", generateOn);

                var result = await conn.QueryAsync<XMLFlaggingList>("ip_xml_generate_flagging_list", __param, commandType: CommandType.StoredProcedure, commandTimeout:180);
                return result.AsList();
            }
            catch (System.Exception __ex)
            {
                throw new Exception(__ex.Message);
            }

        }
        public async Task XMLFlaggingUpdate(XMLFlaggingUpdateBody body)
        {
            try
            {
                using IDbConnection conn = Connection;
                DataTable __id = new("AttributeType");
                __id.Columns.Add("AttributeType");
                foreach (XMLGenerateNMNbody v in body.UpdateId!)
                    __id.Rows.Add(v.id);

                var __param = new DynamicParameters();
                __param.Add("@userid", body.userid);
                __param.Add("@useremail", body.useremail);
                __param.Add("@ponumber", __id.AsTableValuedParameter());
                var result = await conn.ExecuteAsync("dbo.ip_xml_generate_flagging_update", __param, commandType: CommandType.StoredProcedure, commandTimeout:180);
            }
            catch (System.Exception __ex)
            {
                throw new Exception(__ex.Message);
            }

        }
        public async Task<List<XMLUploadListDto>> GetXMLUploadList(XMLUploadListBody body)
        {
            try
            {
                using IDbConnection conn = Connection;
                var __param = new DynamicParameters();
                __param.Add("@uploadtype", body.uploadtype);
                var result = await conn.QueryAsync<XMLUploadListDto>("ip_xml_upload_list", __param, commandType: CommandType.StoredProcedure, commandTimeout:180);
                return result.AsList();
            }
            catch (System.Exception __ex)
            {
                throw new Exception(__ex.Message);
            }

        }
        public async Task XMLUpload(XMLUploadBody body)
        {
            try
            {
                using IDbConnection conn = Connection;
                var __param = new DynamicParameters();
                __param.Add("@userid", body.userid);
                __param.Add("@useremail", body.useremail);
                __param.Add("@filename", body.filename);
                __param.Add("@uploadtype", body.uploadtype);

                var result = await conn.ExecuteAsync("ip_xml_upload", __param, commandType: CommandType.StoredProcedure, commandTimeout:180);
            }
            catch (System.Exception __ex)
            {
                throw new Exception(__ex.Message);
            }

        }
        public async Task<IList<XMLFlaggingList>> GetXMLFlaggingListHistory(XMLFlaggingHistoryBody body)
        {
            try
            {
                using IDbConnection conn = Connection;
                var __param = new DynamicParameters();
                __param.Add("@ponumber", body.ponumber);
                var result = await conn.QueryAsync<XMLFlaggingList>("ip_xml_generate_flagging_hist", __param, commandType: CommandType.StoredProcedure, commandTimeout:180);
                return result.AsList();
            }
            catch (System.Exception __ex)
            {
                throw new Exception(__ex.Message);
            }

        }
        public async Task<IList<XMLGenerateNMN>> GetXMLGenerateNMN(XMLGenerateNMNbody bodyreq)
        {
            try
            {
                using IDbConnection conn = Connection;
                var __param = new DynamicParameters();
                __param.Add("@id", bodyreq.id);
                var result = await conn.QueryAsync<XMLGenerateNMN>("ip_xml_generate_accrual_byid_nmn", __param, commandType: CommandType.StoredProcedure, commandTimeout:180);
                return result.AsList();
            }
            catch (System.Exception __ex)
            {
                throw new Exception(__ex.Message);
            }

        }
        public async Task<IList<XMLGenerateBatchNameList>> GetXMLBatchName(XMLGenerateBatchNameBodyReq body)
        {
            try
            {
                using IDbConnection conn = Connection;
                DataTable __id = new("AttributeType");
                __id.Columns.Add("AttributeType");
                foreach (int v in body.id!)
                    __id.Rows.Add(v);

                var __param = new DynamicParameters();
                __param.Add("@entitylist", body.entitylist);
                __param.Add("@userid", body.userid);
                __param.Add("@distributorlist", __id.AsTableValuedParameter());
                var result = await conn.QueryAsync<XMLGenerateBatchNameList>("dbo.ip_xml_generate_batchname", __param, commandType: CommandType.StoredProcedure, commandTimeout:180);
                return result.AsList();
            }
            catch (System.Exception __ex)
            {
                throw new Exception(__ex.Message);
            }

        }

        public async Task<XMLGeneratePaymentBatch> GenerateXMLPaymentBatch(XMLGeneratePaymentBatchBody body)
        {
            try
            {
                DataTable __id = new("AttributeType");
                __id.Columns.Add("AttributeType");
                foreach (int v in body.id!)
                    __id.Rows.Add(v);
                using IDbConnection conn = Connection;
                var __query = new DynamicParameters();
                __query.Add("@entity", body.entity);
                __query.Add("@batching", body.batching);
                __query.Add("@distributor", __id.AsTableValuedParameter());

                var result = await conn.QueryMultipleAsync("ip_xml_generate_payment_batch", __query, commandType: CommandType.StoredProcedure, commandTimeout:180);
                var xmlgeneratedtos = result.Read<XMLGenerate>();
                var distributorpayments = result.Read<DistributorPayment>();
                var paymentbatchs = result.Read<PaymentBatch>();

                XMLGeneratePaymentBatch __res = new()
                {
                    xmlgenerate = xmlgeneratedtos.ToList(),
                    distributorpayment = distributorpayments.ToList(),
                    paymentbacth = paymentbatchs.ToList()
                };

                return __res;
            }
            catch (System.Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }
        public async Task<IList<DistributorEntityXMLGenerate>> GetDistributorbyEntity(int PrincipalId)
        {
            using IDbConnection conn = Connection;
            try
            {
                var __query = @"SELECT DistributorId, td.LongDesc  FROM tbset_principal_distributor a LEFT JOIN 
                                    tbmst_distributor td 
                                    on DistributorId = td.Id  
                                    WHERE ISNULL(a.IsDeleted,0)=0  
                                    AND ISNULL(td.IsDeleted,0)=0 AND a.PrincipalId = @PrincipalId ";
                var __res = await conn.QueryAsync<DistributorEntityXMLGenerate>(__query, new { PrincipalId = PrincipalId });
                return __res.AsList();
            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }
        public async Task<IList<EntityforXMLGenerate>> GetEntityforXMLGenerate()
        {
            using IDbConnection conn = Connection;
            try
            {
                var __query = @"SELECT Id, ShortDesc, LongDesc FROM tbmst_principal WHERE ISNULL(IsDeleted,0)=0 ";
                var __res = await conn.QueryAsync<EntityforXMLGenerate>(__query);
                return __res.ToList();
            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }

        public async Task<List<UserProfileXMLGenerate>> GetUserList(
            string usergroupid = "all",
            int userlevel = 0,
            int isdeleted = 2
            )
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
                var result = await conn.QueryAsync<UserProfileXMLGenerate>(sql, new { UserGroupId = usergroupid, UserLevel = userlevel, IsDeleted = isdeleted });
                return result.ToList();
            }
            catch (System.Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }
        // promo/accrualreportheader
        public async Task<IList<PromoAccrualReportHeader>> GetPromoAccrualHeader(string periode, int entityId, string closingDate)
        {
            try
            {
                using IDbConnection conn = Connection;
                var __param = new DynamicParameters();
                __param.Add("@periode", periode);
                __param.Add("@entity", entityId);
                __param.Add("@closingdt", closingDate);

                var result = await conn.QueryAsync<PromoAccrualReportHeader>("ip_promo_accrual_report_hdr", __param, commandType: CommandType.StoredProcedure, commandTimeout:180);
                return result.ToList();
            }
            catch (System.Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }
    }
}