using Microsoft.Extensions.Configuration;
using Repositories.Contracts;
using System.Data.SqlClient;
using System.Data;
using Dapper;
using Repositories.Entities;
using System.Globalization;
using Repositories.Entities.Models;
using Repositories.Entities.Dtos;
using PromoApproval;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;
using static Repositories.Repos.budgetApprovalSummary;


namespace Repositories.Repos
{
    public class BudgetV2Repository : IBudgetV2Repository
    {
        readonly IConfiguration __config;
        readonly IToolsEmailRepository __emailRepo;

        public BudgetV2Repository(IConfiguration config, IToolsEmailRepository emailRepo)
        {
            __config = config;  
            __emailRepo = emailRepo;
        }
        public IDbConnection Connection
        {
            get
            {
                return new SqlConnection(__config.GetConnectionString("DefaultConnection"));
            }
        }

        private BudgetV2ApprovalRequestRespon convertToBudgetApprovalRespon (List<BudgetV2ApprovalRequestData> data,
         List<BudgetTableAttribute> attrib   )
        {
            BudgetV2ApprovalRequestRespon res = new BudgetV2ApprovalRequestRespon();
            if (data != null && data.Count>0)
            {
                BudgetTableAttribute atrHeader = attrib[0];
                res.header = new List<Header>();
                Header header = new Header(atrHeader);
                header.headerName = "";           
                res.header.Add(header);
                
                header = new Header(atrHeader);
                header.headerName = "Sari Husada Generasi Mahardika";
                res.header.Add(header);

                header = new Header(atrHeader);
                header.headerName = "Nutricia Indonesia Sejahtera";
                res.header.Add(header);

                header = new Header(atrHeader);
                header.headerName = "Nutricia Medical Nutrition";
                res.header.Add(header);

                BudgetTableAttribute atrSubHeader = attrib[1];
                res.subHeader = new List<SubHeader>();
                SubHeader subHeader = new SubHeader(atrSubHeader);
                subHeader.headerName = "Channel";
                subHeader.valueSubHeader = new List<tblHeader>();

                BudgetTableAttribute atrValueSubHeader = attrib[2];
                List<tblHeader> valueSubHeaders = new List<tblHeader>();

                tblHeader tblHeader = new tblHeader(atrValueSubHeader);
                tblHeader.headerName = "Count of Promo ID";
                valueSubHeaders.Add(tblHeader);

                tblHeader = new tblHeader(atrValueSubHeader);
                tblHeader.headerName = "Total Cost";
                valueSubHeaders.Add(tblHeader);

                subHeader.valueSubHeader = new List<tblHeader>();
                subHeader.valueSubHeader.AddRange(valueSubHeaders);
                subHeader.valueSubHeader.AddRange(valueSubHeaders);
                subHeader.valueSubHeader.AddRange(valueSubHeaders);

                res.subHeader.Add(subHeader);

                subHeader = new SubHeader(atrValueSubHeader);
                subHeader.headerName = "Total Count of Promo ID";      
             
                res.subHeader.Add(subHeader);
                subHeader = new SubHeader(atrValueSubHeader);
                subHeader.headerName = "Total Sum of Cost";
                res.subHeader.Add(subHeader);

                res.body = new List<Body>();
                BudgetTableAttribute atribBody = attrib[3];
                BudgetTableAttribute atribValue = attrib[4];
                BudgetTableAttribute atribfooter = attrib[5];
                foreach (var item in data.Take(data.Count-1))
                {
                    Body body = new Body(atribBody);
                    body.text = item.channel;
                    body.value = new List<tblValue>();
                    tblValue valueData = new tblValue(atribValue);

                    valueData.value = item.promoIdTotCountSGM;
                    body.value.Add(valueData);
                    valueData = new tblValue(atribValue);
                    valueData.value = item.totInvestmentSGM;
                    body.value.Add(valueData);
                    valueData = new tblValue(atribValue);
                    valueData.value = item.promoIdTotCountNIS;
                    body.value.Add(valueData);
                    valueData = new tblValue(atribValue);
                    valueData.value = item.totInvestmentNIS;
                    body.value.Add(valueData);
                    valueData = new tblValue(atribValue);
                    valueData.value = item.promoIdTotCountNMN;
                    body.value.Add(valueData);
                    valueData = new tblValue(atribValue);
                    valueData.value = item.totInvestmentNMN;
                    body.value.Add(valueData);
                    valueData = new tblValue(atribValue);
                    valueData.value = item.promoIdTotCount;
                    body.value.Add(valueData);
                    valueData = new tblValue(atribValue);
                    valueData.value = item.totInvestment;
                    body.value.Add(valueData);

                    res.body.Add(body);
                }
                // footer is last data
                var lastitem = data.Last();
                res.footer = new List<Body>();
                Body footer = new Body(atribBody);
                footer.text = lastitem.channel;
                footer.value = new List<tblValue>();

                tblValue footervalueData = new tblValue(atribValue);               
                footervalueData.value = lastitem.promoIdTotCountSGM;
                footer.value.Add(footervalueData);
                footervalueData = new tblValue(atribValue);
                footervalueData.value = lastitem.totInvestmentSGM;
                footer.value.Add(footervalueData);
                footervalueData = new tblValue(atribValue);
                footervalueData.value = lastitem.promoIdTotCountNIS;
                footer.value.Add(footervalueData);
                footervalueData = new tblValue(atribValue);
                footervalueData.value = lastitem.totInvestmentNIS;
                footer.value.Add(footervalueData);
                footervalueData = new tblValue(atribValue);
                footervalueData.value = lastitem.promoIdTotCountNMN;
                footer.value.Add(footervalueData);
                footervalueData = new tblValue(atribValue);
                footervalueData.value = lastitem.totInvestmentNMN;
                footer.value.Add(footervalueData);
                footervalueData = new tblValue(atribValue);
                footervalueData.value = lastitem.promoIdTotCount;
                footer.value.Add(footervalueData);
                footervalueData = new tblValue(atribValue);
                footervalueData.value = lastitem.totInvestment;
                footer.value.Add(footervalueData);
                res.footer.Add(footer);

            }
            return res;
        }
        public async Task<object> GetBudgetApprovalRequestReport(int period, DataTable channelId, int categoryId,
        DataTable groupBrand, DataTable approvalStatus)
        {
            try
            {
                using (IDbConnection conn = Connection)
                {
                   
                    var param = new DynamicParameters();

                    param.Add("@Period", period);
                    param.Add("@categoryId", categoryId);
                    param.Add("@channelId", channelId.AsTableValuedParameter());            
                    param.Add("@groupBrand", groupBrand.AsTableValuedParameter());
                    param.Add("@approvalStatus", approvalStatus.AsTableValuedParameter());
                    //param.Add("@subactivitytypeId", groupBrand.AsTableValuedParameter());
                    //param.Add("@5bio", limaBio ? 1 : 0);


                    conn.Open();
                    var __res = await conn.QueryMultipleAsync("ip_promo_budget_approval_rpt_list", param, commandType: CommandType.StoredProcedure, commandTimeout: 180);
                    BudgetReportData  res = new BudgetReportData();


                    res.budgetOver5Bio = __res.Read<object>().ToList();
                    var budgetBellow5BioAll = __res.Read<BudgetV2ApprovalRequestData>().ToList();
                    var budgetBellow5BioContractual = __res.Read<BudgetV2ApprovalRequestData>().ToList();
                    var budgetBellow5BioNonContractual = __res.Read<BudgetV2ApprovalRequestData>().ToList();
                    res.budgetPromoList = __res.Read<object>().ToList();
                    res.emailApproval = __res.Read<object>().ToList();
                    var budgetAttribute = __res.Read<BudgetTableAttribute>().ToList();
                    res.budgetBellow5BioAll = convertToBudgetApprovalRespon(budgetBellow5BioAll, budgetAttribute);
                    res.budgetBellow5BioContractual = convertToBudgetApprovalRespon(budgetBellow5BioContractual, budgetAttribute);
                    res.budgetBellow5BioNonContractual = convertToBudgetApprovalRespon(budgetBellow5BioNonContractual, budgetAttribute);
                    return res;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        public async Task<object> GetBudgetApprovalRequestDataForEmail(int period, int categoryId,
            DataTable channelId,  DataTable groupBrand, DataTable approvalStatus, DataTable month,
            int? is5Bio,
            string profileId, string userEmail)
        {
            try
            {
                using (IDbConnection conn = Connection)
                {
                    BudgetReportData res = new BudgetReportData();
                    var param = new DynamicParameters();

                    param.Add("@Period", period);
                    param.Add("@categoryId", categoryId);
                    param.Add("@channelId", channelId.AsTableValuedParameter());
                    param.Add("@groupBrand", groupBrand.AsTableValuedParameter());
                    param.Add("@approvalStatus", approvalStatus.AsTableValuedParameter());
                    param.Add("@month", month.AsTableValuedParameter());
                    param.Add("@5bio", is5Bio);
                    param.Add("@batchid", "");
             
                    string batchId = DateTime.Now.ToString("yyyyMMddHHmmss");
                    conn.Open();
                    var __res = await conn.QueryMultipleAsync("ip_promo_budget_approval_rpt_list", param, commandType: CommandType.StoredProcedure, commandTimeout: 180);
                    res.batchId = batchId;
                    res.budgetOver5Bio = __res.Read<object>().ToList();
                    var budgetBellow5BioAll = __res.Read<BudgetV2ApprovalRequestData>().ToList();
                    var budgetBellow5BioContractual = __res.Read<BudgetV2ApprovalRequestData>().ToList();
                    var budgetBellow5BioNonContractual = __res.Read<BudgetV2ApprovalRequestData>().ToList();
                    res.budgetPromoList = __res.Read<dynamic>().ToList();
                    res.emailApproval = __res.Read<object>().ToList();
                    var budgetAttribute = __res.Read<BudgetTableAttribute>().ToList();
                    res.emailApprovalSigned = __res.Read<object>().ToList();
                    res.nextApproval = __res.Read<object>().ToList();
                    List<periodRespon> _period = __res.Read<periodRespon>().ToList();
                    if (_period.Count > 0)
                    {
                        res.period = _period.First().period;
                        res.periodDesc = _period.First().periodDesc;
                    }

                    res.budgetBellow5BioAll = convertToBudgetApprovalRespon(budgetBellow5BioAll, budgetAttribute);
                    res.budgetBellow5BioContractual = convertToBudgetApprovalRespon(budgetBellow5BioContractual, budgetAttribute);
                    res.budgetBellow5BioNonContractual = convertToBudgetApprovalRespon(budgetBellow5BioNonContractual, budgetAttribute);
                   
                    // update data request with batchid
                    param = new DynamicParameters();

                    param.Add("@batchId", batchId);
                    param.Add("@profileId", profileId);
                    param.Add("@userEmail", userEmail);
                    conn.Close();
                    if (res.budgetPromoList.Count > 0)
                    {
                        conn.Open();
                        DataTable dtPromo = new DataTable();
                        dtPromo.Columns.Add("keyid");
                        foreach (var row in res.budgetPromoList)
                        {
                            //PropertyInfo prop = row.GetType().GetProperty("id");
                            dtPromo.Rows.Add(row.id);
                        }
                        
                        param.Add("@promoId", dtPromo.AsTableValuedParameter());


                        //conn.Open();
                        var __res2 = await conn.QueryAsync("ip_promo_budget_request_approval", param, commandType: CommandType.StoredProcedure, commandTimeout: 180);
                        conn.Close();
                    }
                    return res;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        public async Task<object> GetBudgetApprovalRequestDataForDownload(int period, int categoryId,
            DataTable channelId,    DataTable groupBrand, DataTable approvalStatus, DataTable month,  int? is5Bio)
        {
            try
            {
                using (IDbConnection conn = Connection)
                {
                    BudgetReportData res = new BudgetReportData();
                    var param = new DynamicParameters();                   

                    param.Add("@Period", period);
                    param.Add("@categoryId", categoryId);
                    param.Add("@channelId", channelId.AsTableValuedParameter());
                    param.Add("@groupBrand", groupBrand.AsTableValuedParameter());
                    param.Add("@approvalStatus", approvalStatus.AsTableValuedParameter());
                    param.Add("@month", month.AsTableValuedParameter());
                    param.Add("@5bio", is5Bio);


                    conn.Open();
                    var __res = await conn.QueryAsync("ip_promo_budget_approval_dl_list", param, commandType: CommandType.StoredProcedure, commandTimeout: 180);


                    return __res.ToList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }
        public async Task<object> GetBudgetApprovalRequestList(int period, DataTable channelId,
       DataTable groupBrand, DataTable approvalStatus, DataTable month, int? is5Bio, int categoryId, int start, int length, 
       string txtSearch, string sort, string order)
        {
            try
            {
                using (IDbConnection conn = Connection)
                {

                    var param = new DynamicParameters();

                    param.Add("@Period", period);
                    param.Add("@channelId", channelId.AsTableValuedParameter());
                    param.Add("@groupBrand", groupBrand.AsTableValuedParameter());
                    param.Add("@approvalStatus", approvalStatus.AsTableValuedParameter());             
                    param.Add("@month", month.AsTableValuedParameter());
                    param.Add("@5bio", is5Bio);
                    param.Add("@categoryId", categoryId);
                    param.Add("@start", start);
                    param.Add("@length", length);
                    param.Add("@order", order);
                    param.Add("@sort", sort);
                    param.Add("@txtSearch", txtSearch);
                 
                    conn.Open();
                    var __res = await conn.QueryMultipleAsync("ip_promo_budget_approval_list", param, commandType: CommandType.StoredProcedure, commandTimeout: 180);
                    var _budgetApproval = __res.Read<object>().ToList();
                    var _data = __res.Read<object>().ToList();
                    var _stat = __res.ReadSingle<BaseLPStats>();


                    return new
                    {
                        budgetApproval = _budgetApproval,
                        budgetApprovalDetail = new { 
                            data = _data,
                            totalCount = _stat.recordsTotal,
                            filteredCount = _stat.recordsFiltered
                            }
                    };
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }
        public async Task<object> GetBudgetApprovalRequestFilter()
        {
            try
            {
                var dateTimeFormat = CultureInfo.CurrentCulture.DateTimeFormat;
                List<object> listMonth = new();
                // Add the first three letters of each month name to the ListBox
                for (int i = 1; i <= 12; i++)
                {
                    string monthAbbr = dateTimeFormat.GetAbbreviatedMonthName(i);
                    listMonth.Add(new {
                        text =  monthAbbr,
                        value = i
                    }); ;
                }
                using (IDbConnection conn = Connection)
                {
                    conn.Open();
                    var sql = @" SELECT id, shortDesc, longDesc FROM tbmst_channel 
                                 WHERE ISNULL(IsDelete, 0) = 0; ";

                    sql += @"SELECT DISTINCT entityId, entityDesc entity, entityShortDesc, groupBrandId, groupBrandDesc groupBrand
                             FROM vw_sku_active 
                             ORDER BY entityDesc, groupBrandDesc ";

                    sql += @"SELECT StatusCode, statusDesc FROM tbmst_master_status
                             WHERE statusType = 'TPD'";                   
                    
                    sql += @"SELECT DISTINCT categoryId, categoryDesc 
                             FROM vw_activity_active ORDER BY categoryId";

                    using (var __resp = await conn.QueryMultipleAsync(sql, commandTimeout: 180))
                    {
                        var result = new
                        {
                            channel = __resp.Read<object>().ToList(),
                            grpBrand = __resp.Read<object>().ToList(),  
                            approvalStatus = __resp.Read<object>().ToList(),
                            category = __resp.Read<object>().ToList(),
                            months = listMonth,
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

        public async Task<object> GetBudgetDeploymentRequestFilter()
        {
            try
            {
                using (IDbConnection conn = Connection)
                {
                    conn.Open();
                    var sql = @" SELECT id, shortDesc, longDesc FROM tbmst_channel 
                                 WHERE ISNULL(IsDelete, 0) = 0; ";

                    sql += @"SELECT DISTINCT entityId, entityDesc entity, entityShortDesc, groupBrandId, groupBrandDesc groupBrand
                             FROM vw_sku_active 
                             ORDER BY entityDesc, groupBrandDesc ";

                    sql += @"SELECT id, longdesc subActivityType 
                                FROM tbmst_subactivity_type WHERE isnull(IsDeleted,0) = 0";

                    using (var __resp = await conn.QueryMultipleAsync(sql, commandTimeout: 180))
                    {
                        var result = new
                        {
                            channel = __resp.Read<object>().ToList(),
                            grpBrand = __resp.Read<object>().ToList(),
                            subactivityType = __resp.Read<object>().ToList(),
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

        public async Task<object> GetBudgetDeploymentRequestList(int period, DataTable channelId,
            DataTable groupBrand, DataTable subactivityTypeId)
        {
            try
            {
                using (IDbConnection conn = Connection)
                {
                    DataTable dt = new DataTable();
                    dt.Columns.Add("keyid");
                    dt.Rows.Add(0);
                  
                    var param = new DynamicParameters();

                    param.Add("@Period", period);
                    param.Add("@channelId", channelId.AsTableValuedParameter());
                    param.Add("@subactivityTypeId", subactivityTypeId.AsTableValuedParameter());
                    param.Add("@groupBrand", groupBrand.AsTableValuedParameter());
                    //param.Add("@5bio", null);


                    conn.Open();
                    var __res = await conn.QueryMultipleAsync("ip_promo_budget_deploy_list", param, commandType: CommandType.StoredProcedure, commandTimeout: 180);
                    return new
                    {
                        //budgetDeployement = __res.Read<object>().ToList(),
                        budgetDeploymentDetail = __res.Read<object>().ToList()
                    };
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }
        public async Task<object> GetBudgetDeploymentUpdateStatus(string profileId, string userEmail, string batchId)
        {
            try
            {
                using (IDbConnection conn = Connection)
                {
                    var param = new DynamicParameters();
                
                    param.Add("@profileid", profileId);
                    param.Add("@useremail", userEmail);
                    param.Add("@batchId", batchId);


                    conn.Open();
                    var __res = await conn.QueryMultipleAsync("ip_promo_budget_deploy_update_status", param, commandType: CommandType.StoredProcedure, commandTimeout: 180);
                    return new
                    {
                        promo = __res.Read<object>().ToList(),
                        promoApproval = __res.Read<object>().ToList()
                    };
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        public async Task<object> GetBudgetDeploymentRequest(DataTable promoId,
           string profileId, string userEmail, string batchId)
        {
            try
            {
                using (IDbConnection conn = Connection)
                {

                    var param = new DynamicParameters();

                    param.Add("@promoid", promoId.AsTableValuedParameter());
                    param.Add("@profileid", profileId);
                    param.Add("@useremail", userEmail);
                    param.Add("@batchid", batchId);


                    conn.Open();
                    var __res = await conn.QueryAsync("ip_promo_budget_request_deploy", param, commandType: CommandType.StoredProcedure, commandTimeout: 180);
                    return __res;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        
        public async void CekAndRunBudgetDeployment(string batchId, string userId, string email)
        {
            
            string GetApprovedPromoQuery = @"
                SELECT PromoId, B.Investment cost, approved, deployed
                FROM tbtrx_promo_budget_status A
                LEFT JOIN tbtrx_promo B ON B.Id = A.PromoId 
                WHERE BatchId = @BatchId";
            string GetSettingPromoDeployment = @"
                select setvalue from appsetting 
                where setname='webUrl' OR setname = 'budgetApprovalSummaryEmail'
                OR setname = 'budgetApprovalSummaryEmailCC' OR setname = 'budgetApprovalSummaryEmailBCC'
                order by Id";
            
            await Task.Run(async () =>
            {
                try
                {
                    BGLogger.WriteLog("START Checking on budget approved by " + userId );
                    using (IDbConnection conn = Connection)
                    {
                        conn.Open();
                        var _res2 = await conn.QueryAsync<string>(GetSettingPromoDeployment);
                        string[] settings = _res2.ToArray();
                        string webUrl = settings[0];
                        string[] lsEmailSummary = settings[1].Split(',');
                        string[] lsEmailSummaryCC = null;
                        if (settings[2] != null)
                        {
                            lsEmailSummaryCC = settings[2].Split(',');
                        }
                        string[] lsEmailSummaryBCC = null;
                        if (settings[3] != null)
                        {
                            lsEmailSummaryBCC = settings[3].Split(',');
                        }

                        BGLogger.WriteLog("Get Approved Promo, batch: " + batchId);
                        var promoCost = await conn.QueryAsync<promoCostApproved>(GetApprovedPromoQuery, new { BatchId = batchId });

                        //process approved not deployed yet
                        if (promoCost.Where(x => x.approved == 1 && x.deployed==0).Any())
                        {
                           

                            var param = new DynamicParameters();
                            DataTable dtPromoIds = new("ArrayIntType");
                            dtPromoIds.Columns.Add("keyid");

                            // select approved only
                            foreach (var item in promoCost.Where(x=>x.approved==1 && x.deployed == 0).ToList())
                            {
                                dtPromoIds.Rows.Add(item.promoId);
                            }

                            string deployBatchId = DateTime.Now.ToString("yyyyMMddHHmmss");
                            param.Add("@promoid", dtPromoIds.AsTableValuedParameter());
                            param.Add("@profileid", userId);
                            param.Add("@useremail", email);
                            param.Add("@batchid", deployBatchId);

                            conn.Close();
                            conn.Open();

                            BGLogger.WriteLog("REQUEST Deploy, batchdeploy " + deployBatchId);
                            var __res = await conn.QueryAsync("ip_promo_budget_request_deploy", param, commandType: CommandType.StoredProcedure, commandTimeout: 180);

                            param = new DynamicParameters();

                            param.Add("@profileid", userId);
                            param.Add("@useremail", email);
                            param.Add("@batchid", deployBatchId);
                            conn.Close();
                            conn.Open();

                            BGLogger.WriteLog("DEPLOY Update status, batchDeploy: " + deployBatchId);
                            var __res2 = await conn.QueryMultipleAsync("ip_promo_budget_deploy_update_status", param, commandType: CommandType.StoredProcedure, commandTimeout: 180);

                            var promo = __res2.Read<object>().ToList();
                            // Fully approve
                            var promoApproval = __res2.Read<promoApprover>().ToList();
                            
                            //data fully approve

                            int totFailed = 0;
                            decimal totFailedCost = 0;
                            int totSuccess = 0;
                            decimal totSuccessCost = 0;

                            if (promoApproval.Count > 0)
                            {
                                PostSummaryData data = new PostSummaryData();
                                // totRequest by batch
                                data.processName = "Budget mass approval request";
                                data.nourut = 1;
                                data.qty = promoCost.Where(x => x.cost <= _FiveBio).Count();
                                data.cost = promoCost.Where(x => x.cost <= _FiveBio).Sum(x => x.cost);
                                data.is5Bio = 0;
                                data.batchId = batchId;
                                await InsertBudgetSummary(data);
                                data.qty = promoCost.Where(x => x.cost > _FiveBio).Count();
                                data.cost = promoCost.Where(x => x.cost > _FiveBio).Sum(x => x.cost);
                                data.is5Bio = 1;
                                await InsertBudgetSummary(data);

                                int is5Bio = (promoApproval.First().cost <= _FiveBio) ? 0 : 1;
                                data = new PostSummaryData();
                                data.processName = "Fully approved budget mass approval";
                                data.nourut = 2;
                                data.qty = promoApproval.Count();
                                data.cost = promoApproval.Sum(x => x.cost);
                                data.is5Bio = is5Bio;
                                data.batchId = batchId;
                                await InsertBudgetSummary(data);

                                BGLogger.WriteLog("TOTAL Promo: " + data.qty + ", Cost:" + data.cost +
                                    ", batchDeploy: " + deployBatchId);

                                List<GetSummaryFailedData> failedData = new List<GetSummaryFailedData>();

                                foreach (var item in promoApproval)
                                {
                                    // catch any error so it not interrupt the process
                                    try
                                    {
                                        bool isSuccess = await generateBodyAndSendEmail(item.id, item.userIdApprover,
                                            item.emailApprover, webUrl);
                                        if (isSuccess)
                                        {
                                            totSuccess++;
                                            totSuccessCost += item.cost;
                                        }
                                        else
                                        {
                                            totFailed++;
                                            totFailedCost += item.cost;
                                            // insert failed promo to log
                                            failedData = await InsertFailedPromoSummary(batchId, item.id, item.refId, item.cost);

                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        BGLogger.WriteLog("generate Body And SendEmail Err: " + ex.Message);
                                    }
                                }

                                //data succes
                                data = new PostSummaryData();
                                data.processName = "Success send email to 1st approver";
                                data.nourut = 3;
                                data.qty = totSuccess;
                                data.cost = totSuccessCost;
                                data.batchId = batchId;
                                data.is5Bio = is5Bio;
                                await InsertBudgetSummary(data);

                                //data failed
                                data = new PostSummaryData();
                                data.processName = "Failed send email to 1st approver";
                                data.nourut = 4;
                                data.qty = totFailed;
                                data.cost = totFailedCost;
                                data.batchId = batchId;
                                data.is5Bio = is5Bio;
                                var dataSummary = await InsertBudgetSummary(data);

                                string htmlTemplatePath = "template/budget-approval-summary.html";
                                // Read the HTML template
                                string title = "Budget Mass Approval Summary";
                                string htmlContent = System.IO.File.ReadAllText(htmlTemplatePath);
                                htmlContent = htmlContent
                                    .Replace("{{Title}}", title)
                                    .Replace("{{TableSummary}}", budgetApprovalSummary.GenerateTableSummary(dataSummary, failedData));
                                EmailBody body = new EmailBody();
                                body.body = htmlContent;
                                body.subject = title;
                                body.email =  lsEmailSummary;
                                body.cc = lsEmailSummaryCC;
                                body.bcc = lsEmailSummaryBCC;

                                BGLogger.WriteLog("SEND Budget Mass Approval Summary email ");

                                await __emailRepo.SendEmail(body);
                            }
                        } else
                        {
                            BGLogger.WriteLog("No budget approved ");
                        }
                    }
                    BGLogger.WriteLog("END Checking on budget approved");
                }
                catch (Exception ex)
                {                    
                    BGLogger.WriteLog("Cek And Run Budget Deployment Err: " + ex.Message);
                }
            });
        }

        private async Task<bool> generateBodyAndSendEmail(int promoId, string profileID,  string email, string urlServer)
        {
            try
            {
                PromoDisplayList __res = new();
                string htmlContent = String.Empty;
                using (IDbConnection conn = Connection)
                {
                    var __query = new DynamicParameters();
                    __query.Add("@Id", promoId);
                    BGLogger.WriteLog("Get promo display : " + promoId + ", email:" + email);
                    var __obj = await conn.QueryMultipleAsync("[dbo].[ip_promo_select_display]", __query, commandType: CommandType.StoredProcedure, commandTimeout: 180);
                    {
                        __res.PromoHeader = __obj.Read<PromoDisplayData>().FirstOrDefault();
                        __res.Regions = __obj.Read<PromoRegionRes>().ToList();
                        __res.Channels = __obj.Read<PromoChannelRes>().ToList();
                        __res.SubChannels = __obj.Read<PromoSubChannelRes>().ToList();
                        __res.Accounts = __obj.Read<PromoAccountRes>().ToList();
                        __res.SubAccounts = __obj.Read<PromoSubAccountRes>().ToList();
                        __res.Brands = __obj.Read<PromoBrandRes>().ToList();
                        __res.Skus = __obj.Read<PromoProductRes>().ToList();
                        __res.Activities = __obj.Read<PromoActivityRes>().ToList();
                        __res.SubActivities = __obj.Read<PromoSubActivityRes>().ToList();
                        __res.Attachments = __obj.Read<PromoAttachment>().ToList();
                        __res.ListApprovalStatus = __obj.Read<ApprovalRes>().ToList();
                        __res.Mechanisms = __obj.Read<MechanismData>().ToList();
                        __res.Investments = __obj.Read<PromoReconInvestmentData>().ToList();
                        __res.GroupBrand = __obj.Read<object>().ToList();

                    }

                    if (__res.PromoHeader != null)
                    {
                        //read n render the html
                        // Path to the HTML template
                        string htmlTemplatePath = "template/email-approval.html";

                        // Read the HTML template
                        htmlContent = System.IO.File.ReadAllText(htmlTemplatePath);
                        string sku = String.Join(", ", __res.Skus?.Where(x => x.flag).Select(x => x.longDesc).ToList());
                        if (sku != null && sku.Length > 60) sku = sku.PadRight(60) + " ....";
                        string brand = String.Join(", ", __res.Brands?.Where(x => x.flag).Select(x => x.longDesc).ToList());
                        if (brand != null && brand.Length > 60) brand = brand.PadRight(60) + " ....";
                        //Min Buy 2 SGM GUM 1+ 900 discount 3%; Min Buy 5 SGM GUM 1+ 400 discount 3% 
                        string mechanism = String.Join(", ", __res.Mechanisms?.Select(x => x.Mechanism));
                        if (mechanism != null && mechanism.Length > 60) mechanism = mechanism.PadRight(60) + " ....";

                        approverParamKey key = new approverParamKey();
                        key.promoId = __res.PromoHeader.Id.ToString();
                        key.refId = __res.PromoHeader.RefId.ToString();
                        key.nameApprover = __res.PromoHeader.UserApprover1;
                        key.profileId = profileID;
                        key.sy = __res.PromoHeader.StartPromo.ToString("yyyy");


                        string encriptedParam = Encrypt(JsonConvert.SerializeObject(key));
                        string urlApprove = urlServer + "/promo/approval/email/approve?p=" + encriptedParam;
                        string urlSendback = urlServer + "/promo/approval/email/send-back?p=" + encriptedParam;
                        // Replace placeholders in the HTML with actual data
                        htmlContent = htmlContent
                            .Replace("{{refId}}", __res.PromoHeader.RefId?.ToString())
                            .Replace("{{modifReason}}", __res.PromoHeader.ModifReason)
                            //23 Dec 2024
                            .Replace("{{createOn}}", __res.PromoHeader.CreateOn.ToString("dd MMM yyyy"))
                            .Replace("{{createBy}}", __res.PromoHeader.CreateBy)
                            .Replace("{{promoPlanRefId}}", __res.PromoHeader.PromoPlanRefId)
                            .Replace("{{distributorName}}", __res.PromoHeader.DistributorName)
                            .Replace("{{channel}}", __res.Channels?.Where(x => x.flag)?.FirstOrDefault()?.longDesc)
                            .Replace("{{account}}", __res.PromoHeader.AccountDesc)
                            .Replace("{{subAccount}}", __res.SubAccounts.Where(x => x.flag)?.FirstOrDefault()?.longDesc)
                            .Replace("{{region}}", __res.PromoHeader.RegionDesc)
                            .Replace("{{brand}}", brand)
                            .Replace("{{SKU}}", sku)
                            .Replace("{{activity}}", __res.PromoHeader.ActivityDesc)
                            .Replace("{{subActivity}}", __res.PromoHeader.SubActivityLongDesc)
                            //01/01/2025 - 31/12/2025 
                            .Replace("{{startPromo}}", __res.PromoHeader.StartPromo.ToString("dd/MM/yyyy"))
                            .Replace("{{endPromo}}", __res.PromoHeader.EndPromo.ToString("dd/MM/yyyy"))
                            .Replace("{{allocation}}", "")
                            .Replace("{{mechanism}}", mechanism)
                            //107,326,696,342.00
                            .Replace("{{planNormalSales}}", __res.PromoHeader.PlanNormalSales.ToString("N2"))
                            .Replace("{{planIncrSales}}", __res.PromoHeader.PlanIncrSales.ToString("N2"))
                            .Replace("{{planTotSales}}", __res.PromoHeader.PlanTotSales.ToString("N2"))
                            .Replace("{{planInvestment}}", __res.PromoHeader.PlanInvestment.ToString("N2"))
                            .Replace("{{planCostRatio}}", __res.PromoHeader.PlanCostRatio.ToString("N2"))
                            .Replace("{{planRoi}}", __res.PromoHeader.PlanRoi.ToString("N2"))
                            .Replace("{{normalSales}}", __res.PromoHeader.NormalSales.ToString("N2"))
                            .Replace("{{incrSales}}", __res.PromoHeader.IncrSales.ToString("N2"))
                            .Replace("{{totSales}}", __res.PromoHeader.totSales.ToString("N2"))
                            .Replace("{{investment}}", __res.PromoHeader.Investment.ToString("N2"))
                            .Replace("{{costRatio}}", __res.PromoHeader.CostRatio.ToString("N2"))
                            .Replace("{{roi}}", __res.PromoHeader.Roi.ToString("N2"))
                            .Replace("{{lastStatus1}}", __res.PromoHeader.LastStatus1)
                            .Replace("{{lastStatus2}}", __res.PromoHeader.LastStatus2)
                            .Replace("{{lastStatus3}}", __res.PromoHeader.LastStatus3)
                            .Replace("{{lastStatus4}}", __res.PromoHeader.LastStatus4)
                            .Replace("{{lastStatus5}}", __res.PromoHeader.LastStatus5)
                            .Replace("{{userApprover1}}", __res.PromoHeader.UserApprover1)
                            .Replace("{{userApprover2}}", __res.PromoHeader.UserApprover2)
                            .Replace("{{userApprover3}}", __res.PromoHeader.UserApprover3)
                            .Replace("{{userApprover4}}", __res.PromoHeader.UserApprover4)
                            .Replace("{{userApprover5}}", __res.PromoHeader.UserApprover5)
                            .Replace("{{approvalNotes}}", __res.PromoHeader.ApprovalNotes)
                            .Replace("{{approvalLink}}", urlApprove)
                            .Replace("{{sendbackLink}}", urlSendback);

                        BGLogger.WriteLog("Generate attachment: " + promoId);

                        string rowAttachment = genAttachmentContent(__res.Attachments, urlServer);
                        htmlContent = htmlContent.Replace("{{attachment}}", rowAttachment);


                        //send email
                        EmailBody body = new EmailBody();
                        //email = "andrie.xva@gmail.com";
                        body.body = htmlContent;
                        body.subject = $"[APPROVAL NOTIF] Promo requires approval ({__res.PromoHeader.RefId?.ToString()})";
                        body.email = new string[] { email };


                        await __emailRepo.SendEmail(body);
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                BGLogger.WriteLog("generateBodyAndSendEmail Exception: " + ex.Message);
                // put log error
                return false;
            }

        }

        private async Task<List<GetSummaryData>> InsertBudgetSummary(PostSummaryData postData)
        {
            using (IDbConnection conn = Connection)
            {
                string qry = @"
                MERGE INTO tbtrx_promo_budget_status_summary_email AS target
                USING (SELECT @processName AS ProcessName, @batchId AS batchId) AS source
                ON target.processSummary = source.ProcessName and target.batchId = source.batchId
                WHEN MATCHED AND @is5Bio=1 THEN 
                    UPDATE SET Above5Bio = @cost, Above5BioQty = @qty
                WHEN NOT MATCHED THEN
                    INSERT (batchId, nourut, processSummary, Below5BioQty, Below5Bio, Above5BioQty, Above5Bio)
                    VALUES (@batchId, @nourut,
                        @processName, 
                        CASE WHEN @is5Bio=0 THEN @qty ELSE 0 END,  -- Below5BioQty
                        CASE WHEN @is5Bio=0 THEN @cost ELSE 0 END,  -- Below5BioValue
                        CASE WHEN @is5Bio=1 THEN @qty ELSE 0 END,  -- Above5BioQty
                        CASE WHEN @is5Bio=1 THEN @cost ELSE 0 END  -- Above5BioValue
                    );

                select * from tbtrx_promo_budget_status_summary_email
                WHERE batchId=@batchId
                order by nourut;
                ";

                var __res = await conn.QueryAsync<GetSummaryData>(qry, postData);

                return __res.ToList();
            }

         }

        private async Task<List<GetSummaryFailedData>> InsertFailedPromoSummary(string batchId, int promoId, 
            string promoRef, decimal cost)
        {
            using (IDbConnection conn = Connection)
            {
                string qry = @" 
                DECLARE @utcDate123 DATETIME
                SET @utcDate123 = dbo.getUTCDateConversion()

                INSERT INTO tbtrx_promo_budget_status_send_email_failed 
                (batchId, promoId, promoRefId, cost, actionOn) VALUES 
                (@batchid, @promoid, @promorefid, @cost, @utcDate123);

                SELECT promorefid, cost 
                FROM tbtrx_promo_budget_status_send_email_failed
                WHERE batchId=@batchid
                order by actionOn 
                ";

                var __res = await conn.QueryAsync<GetSummaryFailedData>(qry, new
                {
                    batchid = batchId, promoid =promoId, promorefid = promoRef, cost = cost
                });

                return __res.ToList();
            }

        }
        private string Encrypt(string plaintext, string key = "0123456789abcdef0123456789abcdef",
            string iv = "abcdef0123456789")
        {
            using (Aes aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(key);
                aes.IV = Encoding.UTF8.GetBytes(iv);
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;

                ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
                byte[] plaintextBytes = Encoding.UTF8.GetBytes(plaintext);
                byte[] encryptedBytes = encryptor.TransformFinalBlock(plaintextBytes, 0, plaintextBytes.Length);

                return ToUrlSafeBase64(Convert.ToBase64String(encryptedBytes));
            }
        }
        private string ToUrlSafeBase64(string base64)
        {
            return base64.Replace("+", "-").Replace("/", "_").TrimEnd('=');
        }
        private string genAttachmentContent(IList<PromoAttachment> attachs, string url)
        {
            string _res = string.Empty;
            string _temp = "  <tr>\r\n        <td>\r\n\t\t\t<a href=\"{{url}}/assets/media/promo/{{promoID}}/{{row}}/{{fileAttachment}}\" target=\"_blank\">\r\n\t\t\t\t{{fileAttachment}}\r\n\t\t\t</a>\r\n        </td>\r\n    </tr>      ";
            int i = 1;
            foreach (var attach in attachs)
            {
                _res += _temp.Replace("{{url}}", url)
                    .Replace("{{promoID}}", attach.PromoId.ToString())
                    .Replace("{{row}}", attach.DocLink)
                    .Replace("{{fileAttachment}}", attach.FileName);
                i++;
            }

            return _res;
        }
        private async Task<PromoDisplayList> GetPromoDisplayByIdForSendEmail(int id)
        {
            try
            {
                PromoDisplayList __res = new();
                using (IDbConnection conn = Connection)
                {
                    var __query = new DynamicParameters();
                    __query.Add("@Id", id);
                    var __obj = await conn.QueryMultipleAsync("[dbo].[ip_promo_select_display]", __query, commandType: CommandType.StoredProcedure, commandTimeout: 180);
                    {
                        __res.PromoHeader = __obj.Read<PromoDisplayData>().FirstOrDefault();
                        __res.Regions = __obj.Read<PromoRegionRes>().ToList();
                        __res.Channels = __obj.Read<PromoChannelRes>().ToList();
                        __res.SubChannels = __obj.Read<PromoSubChannelRes>().ToList();
                        __res.Accounts = __obj.Read<PromoAccountRes>().ToList();
                        __res.SubAccounts = __obj.Read<PromoSubAccountRes>().ToList();
                        __res.Brands = __obj.Read<PromoBrandRes>().ToList();
                        __res.Skus = __obj.Read<PromoProductRes>().ToList();
                        __res.Activities = __obj.Read<PromoActivityRes>().ToList();
                        __res.SubActivities = __obj.Read<PromoSubActivityRes>().ToList();
                        __res.Attachments = __obj.Read<PromoAttachment>().ToList();
                        __res.ListApprovalStatus = __obj.Read<ApprovalRes>().ToList();
                        __res.Mechanisms = __obj.Read<MechanismData>().ToList();
                        __res.Investments = __obj.Read<PromoReconInvestmentData>().ToList();
                        __res.GroupBrand = __obj.Read<object>().ToList();

                    }
                }
                return __res;
            }
            catch (System.Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }
        public async Task<object> SetBudgetApprovalRequestApprove(string batchId, string profileId, string userEmail)
        {           
            try
            {
                BudgetReportData res = new BudgetReportData();
                using (IDbConnection conn = Connection)
                {

                    var param = new DynamicParameters();

                    param.Add("@batchId", batchId);
                    param.Add("@profileId", profileId);
                    param.Add("@userEmail", userEmail);
                 
                    conn.Open();
                    var __res2 = await conn.QueryAsync("ip_promo_budget_approval", param, commandType: CommandType.StoredProcedure, commandTimeout: 180);
                    conn.Close();

                    conn.Open();
                    param = new DynamicParameters();
                    DataTable dt = new DataTable();
                    dt.Columns.Add("keyid");
                    dt.Rows.Add(0);
                    DataTable dtStatus = new DataTable();
                    dtStatus.Columns.Add("id");
                    dtStatus.Rows.Add(0);
                    param.Add("@batchId", batchId);
                    param.Add("@Period", "");
                    param.Add("@channelId", dt.AsTableValuedParameter());
                    param.Add("@groupBrand", dt.AsTableValuedParameter());
                    param.Add("@approvalStatus", dtStatus.AsTableValuedParameter());
                    var __res = await conn.QueryMultipleAsync("ip_promo_budget_approval_rpt_list", param, commandType: CommandType.StoredProcedure, commandTimeout: 180);
                    res.batchId = batchId;
                    res.budgetOver5Bio = __res.Read<object>().ToList();
                    var budgetBellow5BioAll = __res.Read<BudgetV2ApprovalRequestData>().ToList();
                    var budgetBellow5BioContractual = __res.Read<BudgetV2ApprovalRequestData>().ToList();
                    var budgetBellow5BioNonContractual = __res.Read<BudgetV2ApprovalRequestData>().ToList();
                    res.budgetPromoList = __res.Read<dynamic>().ToList();
                    res.emailApproval = __res.Read<object>().ToList();
                    var budgetAttribute = __res.Read<BudgetTableAttribute>().ToList();
                    res.emailApprovalSigned = __res.Read<object>().ToList();
                    res.nextApproval = __res.Read<object>().ToList();
                    List<periodRespon> _period = __res.Read<periodRespon>().ToList();
                    if (_period.Count > 0)
                    {
                        res.period = _period.First().period;
                        res.periodDesc = _period.First().periodDesc;
                    }

                    res.budgetBellow5BioAll = convertToBudgetApprovalRespon(budgetBellow5BioAll, budgetAttribute);
                    res.budgetBellow5BioContractual = convertToBudgetApprovalRespon(budgetBellow5BioContractual, budgetAttribute);
                    res.budgetBellow5BioNonContractual = convertToBudgetApprovalRespon(budgetBellow5BioNonContractual, budgetAttribute);
                    conn.Close();
                }
                return res;
            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }

        public async Task<object> SetBudgetApprovalRequestReject(string batchId, string profileId, string userEmail)
        {
            try
            {
                BudgetReportData res = new BudgetReportData();
                using (IDbConnection conn = Connection)
                {

                    var param = new DynamicParameters();

                    param.Add("@batchId", batchId);
                    param.Add("@profileId", profileId);
                    param.Add("@userEmail", userEmail);

                    conn.Open();
                    var __res2 = await conn.QueryAsync("ip_promo_budget_approval_reject", param, commandType: CommandType.StoredProcedure, commandTimeout: 180);
                    conn.Close();
                    return true;
                }
                return res;
            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }

        public async Task<object> GetBudgetApprovalByBatch(string batchId)
        {
            try
            {
                BudgetApprovalDataByBatch res = new BudgetApprovalDataByBatch();
                using (IDbConnection conn = Connection)
                {

                    var param = new DynamicParameters();

                    param.Add("@batchId", batchId);

                    var __res = await conn.QueryMultipleAsync("ip_promo_budget_approval_by_batch_list", param, commandType: CommandType.StoredProcedure, commandTimeout: 180);

                    res.budgetOver5Bio = __res.Read<object>().ToList();
                    res.budgetBellow5BioAll = __res.Read<object>().ToList();
                    res.budgetBellow5BioContractual = __res.Read<object>().ToList();
                    res.budgetBellow5BioNonContractual = __res.Read<object>().ToList();
                    res.emailApproval = __res.Read<object>().ToList();
                    res.period= __res.ReadSingleOrDefault<object>();
                    return res;
                }
            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }
    }
}
