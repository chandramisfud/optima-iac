using System.Data;
using System.Data.SqlClient;
using Dapper;
using Microsoft.Extensions.Configuration;
using Repositories.Contracts;

namespace Repositories.Repos
{
    public class PromoCalculatorRepo : IPromoCalculatorRepo
    {
        readonly IConfiguration __config;
        public PromoCalculatorRepo(IConfiguration config)
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
        public async Task<object> GetPromoCalculatorLP(int mainActivityId, int channelId)
        {
            try
            {
                using (IDbConnection conn = Connection)
                {
                    var sql = @"SELECT A.id, B.longDesc mainActivity,  A.mainActivityId, A.channelId, C.LongDesc channelLongDesc,
                    IIF(baseline = 0, 'Disabled', IIF(baseline = 1, 'Enabled', IIF(baseline = 2, 'Auto', 'Unknown'))) AS baseline, 
                    IIF(totalSales = 0, 'Disabled', IIF(totalSales = 1, 'Enabled', IIF(totalSales = 2, 'Auto', 'Unknown'))) AS totalSales, 
                    IIF(uplift = 0, 'Disabled', IIF(uplift = 1, 'Enabled', IIF(uplift = 2, 'Auto', 'Unknown'))) AS uplift, 
                    IIF(salesContribution = 0, 'Disabled', IIF(salesContribution = 1, 'Enabled', IIF(salesContribution = 2, 'Auto', 'Unknown'))) AS salesContribution,
                    IIF(storesCoverage = 0, 'Disabled', IIF(storesCoverage = 1, 'Enabled', IIF(storesCoverage = 2, 'Auto', 'Unknown'))) AS storesCoverage,
                    IIF(redemptionRate = 0, 'Disabled', IIF(redemptionRate = 1, 'Enabled', IIF(redemptionRate = 2, 'Auto', 'Unknown'))) AS redemptionRate,
                    IIF(cr = 0, 'Disabled', IIF(cr = 1, 'Enabled', IIF(cr = 2, 'Auto', 'Unknown'))) AS cr,
	                IIF(cost = 0, 'Disabled', IIF(cost = 1, 'Enabled', IIF(cost = 2, 'Auto', 'Unknown'))) AS cost,
					-- recon part
					IIF(baselineRecon = 0, 'Disabled', IIF(baselineRecon = 1, 'Enabled', IIF(baselineRecon = 2, 'Auto', 'Unknown'))) AS baselineRecon, 
                    IIF(totalSalesRecon = 0, 'Disabled', IIF(totalSalesRecon = 1, 'Enabled', IIF(totalSalesRecon = 2, 'Auto', 'Unknown'))) AS totalSalesRecon, 
                    IIF(upliftRecon = 0, 'Disabled', IIF(upliftRecon = 1, 'Enabled', IIF(upliftRecon = 2, 'Auto', 'Unknown'))) AS upliftRecon, 
                    IIF(salesContributionRecon = 0, 'Disabled', IIF(salesContributionRecon = 1, 'Enabled', IIF(salesContributionRecon = 2, 'Auto', 'Unknown'))) AS salesContributionRecon,
                    IIF(storesCoverageRecon = 0, 'Disabled', IIF(storesCoverageRecon = 1, 'Enabled', IIF(storesCoverageRecon = 2, 'Auto', 'Unknown'))) AS storesCoverageRecon,
                    IIF(redemptionRateRecon = 0, 'Disabled', IIF(redemptionRateRecon = 1, 'Enabled', IIF(redemptionRateRecon = 2, 'Auto', 'Unknown'))) AS redemptionRateRecon,
                    IIF(crRecon = 0, 'Disabled', IIF(crRecon = 1, 'Enabled', IIF(crRecon = 2, 'Auto', 'Unknown'))) AS crRecon,
	                IIF(costRecon = 0, 'Disabled', IIF(costRecon = 1, 'Enabled', IIF(costRecon = 2, 'Auto', 'Unknown'))) AS costRecon
                      FROM tbset_config_promo_calculator A
                      LEFT JOIN tbmst_main_activity B ON B.id = A.mainActivityId
                      LEFT JOIN tbmst_channel c on C.Id = A.channelId
                      WHERE 1=1
                      ";
                    if (mainActivityId > 0) sql += " AND A.mainActivityId=" + mainActivityId;
                    if (channelId > 0) sql += " AND A.channelId=" + channelId;
                    var result = await conn.QueryAsync<object>(sql);
                    return result.ToList();
                }
            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }

        public async Task<object> GetPromoCalculatorFilter()
        {
            try
            {
                using (IDbConnection conn = Connection)
                {
                    conn.Open();
                    var sql = @"
                    SELECT id mainActivityId, longDesc mainActivityDesc FROM tbmst_main_activity

                    SELECT id channelId, LongDesc channelLongDesc 
                    FROM tbmst_channel
                    WHERE isdelete=0
                      ";
                    using (var __resp = await conn.QueryMultipleAsync(sql, commandTimeout: 180))
                    {
                        var result = new
                        {
                            mainActivity = __resp.Read<object>().ToList(),
                            channel = __resp.Read<object>().ToList(),
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
        public async Task<object> GetPromoCalculatorChannel()
        {
            try
            {
                using (IDbConnection conn = Connection)
                {
                    conn.Open();
                    var sql = @"
                    SELECT id channelId, LongDesc channelLongDesc FROM tbmst_channel
                        WHERE isdelete=0";
                    var __resp = await conn.QueryAsync<object>(sql, commandTimeout: 180);

                    return __resp;

                }

            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }

        // Create new Calculator
        public async Task<bool> SetPromoCalculatorSave(string mainActivity, int channel,
            int baseline, int totalSales, int uplift,
            int salesContribution, int storesCoverage, int redemptionRate, int cr, int cost,
            int baselineRecon, int totalSalesRecon, int upliftRecon,
            int salesContributionRecon, int storesCoverageRecon, int redemptionRateRecon, int crRecon, int costRecon,
            int[] subActivity, string createdBy, string createdEmail)
        {
            bool res = false;
            try
            {
                DateTime utcTime = DateTime.UtcNow;
                var zone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
                using (IDbConnection conn = Connection)
                {
                    conn.Open(); // Open the connection

                    using (var transaction = conn.BeginTransaction())
                    {
                        try
                        {
                            int? masterId = 0;
                            // select main activity first on case create with multi channel
                            var sql = "SELECT id FROM tbmst_main_activity" +
                                " WHERE longDesc = @mainActivity";
                            masterId = await conn.ExecuteScalarAsync<int?>(sql,
                                new { mainActivity = mainActivity }, transaction);

                            if (!masterId.HasValue)
                            {
                                // Insert into Master table 
                                sql = @"INSERT INTO tbmst_main_activity(longDesc, createdOn, createdBy, createdByEmail)
                                VALUES(@longDesc, @createdOn, @createdBy, @createdByEmail);
                                SELECT CAST(SCOPE_IDENTITY() as int);";
                                masterId = await conn.ExecuteScalarAsync<int>(sql, new
                                {
                                    longDesc = mainActivity,
                                    createdBy = createdBy,
                                    createdByEmail = createdEmail,
                                    createdOn = TimeZoneInfo.ConvertTimeFromUtc(utcTime, zone),
                                }, transaction);
                            } else // cek if masteractivity and channel already exist
                            {
                                sql = @" SELECT A.id
                                    FROM tbset_config_promo_calculator A
                                    WHERE A.mainActivityId = @mainActivity and A.channelId=@channel";
                                int configId = await conn.ExecuteScalarAsync<int>(sql,
                                           new { mainActivity = masterId, channel = channel }, transaction);
                                if (configId > 0)
                                {
                                    throw new Exception("Main Activity and channel(" + channel + ") exist");
                                }
                            }

                            if (masterId > 0)
                            {
                                // Insert into Master table 
                                sql = @"INSERT INTO tbset_config_promo_calculator
                                (mainActivityId, channelId,
                                baseline, totalSales, uplift, salesContribution, storesCoverage, 
                                redemptionRate, cr, cost,
                                baselineRecon, totalSalesRecon, upliftRecon, salesContributionRecon, storesCoverageRecon, 
                                redemptionRateRecon, crRecon, costRecon,
                                createdOn, createdBy, createdByEmail)
                                VALUES(@mainActivity, @channel,
                                @baseline, @totalSales, @uplift, @sales, @stores, @redemptionRate,@cr, @cost, 
                                @baselineRecon, @totalSalesRecon, @upliftRecon, @salesRecon, @storesRecon, 
                                @redemptionRateRecon, @crRecon, @costRecon, 
                                @createdOn, @createdBy, @createdByEmail);";
                                var calcId = await conn.ExecuteScalarAsync<int>(sql, new
                                {
                                    mainActivity = masterId,
                                    channel = channel,
                                    baseline = baseline,
                                    totalSales = totalSales,
                                    uplift = uplift,
                                    sales = salesContribution,
                                    stores = storesCoverage,
                                    redemptionRate = redemptionRate,
                                    cr = cr,
                                    cost = cost,

                                    baselineRecon = baselineRecon,
                                    totalSalesRecon = totalSalesRecon,
                                    upliftRecon = upliftRecon,
                                    salesRecon = salesContributionRecon,
                                    storesRecon = storesCoverageRecon,
                                    redemptionRateRecon = redemptionRateRecon,
                                    crRecon = crRecon,
                                    costRecon = costRecon,

                                    createdBy = createdBy,
                                    createdByEmail = createdEmail,
                                    createdOn = TimeZoneInfo.ConvertTimeFromUtc(utcTime, zone),
                                }, transaction);

                            }
                            if (subActivity.Length > 0 && masterId > 0)
                            {
                                //DELETE existing
                                var delSQL = "DELETE FROM tbmst_main_activity_dtl " +
                                    " WHERE mainactivityid = @MasterId";

                                await conn.ExecuteAsync(delSQL, new { MasterId = masterId }, transaction);

                                foreach (var item in subActivity)
                                {
                                    if (item > 0)
                                    {
                                        // Insert into Detail
                                        var detailSql = "INSERT INTO tbmst_main_activity_dtl(mainactivityid, subactivityid) " +
                                            "VALUES (@mainActivity, @subactivity)";
                                        await conn.ExecuteAsync(detailSql, new
                                        {
                                            mainActivity = masterId,
                                            subactivity = item
                                        }, transaction);
                                    }
                                }
                            }
                            // If everything succeeds, commit the transaction
                            transaction.Commit();

                            res = true;
                        }
                        catch (Exception ex)
                        {
                            // If there is an error, rollback the transaction
                            transaction.Rollback();
                            throw new Exception("Rollback: " + ex.Message);
                        }
                    }
                }
            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
            return res;
        }

        public async Task<bool> SetPromoCalculatorUpdate(int id, string mainActivity, int channel,
           int baseline, int totalSales, int uplift,
           int salesContribution, int storesCoverage, int redemptionRate, int cr, int cost,
           int baselineRecon, int totalSalesRecon, int upliftRecon,
           int salesContributionRecon, int storesCoverageRecon, int redemptionRateRecon, int crRecon, int costRecon,
           int[] subActivity, string createdBy, string createdEmail)
        {
            bool res = false;
            try
            {
                DateTime utcTime = DateTime.UtcNow;
                var zone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
                using (IDbConnection conn = Connection)
                {
                    conn.Open(); // Open the connection

                    //using (var transaction = conn.BeginTransaction())
                    //{

                    // Insert into Master table 
                    var sql = @"UPDATE tbmst_main_activity
                                SET longDesc=@longDesc, modifiedOn=@createdOn, modifiedBy=@createdBy, 
                                modifiedByEmail=@createdByEmail
                                WHERE id = " + id;
                    var _res = await conn.ExecuteScalarAsync<int>(sql, new
                    {
                        longDesc = mainActivity,
                        createdBy = createdBy,
                        createdByEmail = createdEmail,
                        createdOn = TimeZoneInfo.ConvertTimeFromUtc(utcTime, zone),
                    });

                    // Insert into Master table 
                    sql = @"UPDATE tbset_config_promo_calculator
                                 SET channelId=@channel, baseline = @baseline, totalSales=@totalSales, uplift=@uplift, salesContribution=@sales, 
                                storesCoverage=@stores, redemptionRate=@redemptionRate, cr=@cr, cost=@cost,

                                baselineRecon = @baselineRecon, totalSalesRecon=@totalSalesRecon, upliftRecon=@upliftRecon, 
                                salesContributionRecon=@salesRecon, 
                                storesCoverageRecon=@storesRecon, redemptionRateRecon=@redemptionRateRecon, 
                                crRecon=@crRecon, costRecon=@costRecon,

                                modifiedOn=@createdOn, modifiedBy=@createdBy, modifiedByEmail=@createdByEmail 
                                WHERE mainactivityid=@id AND channelId=@channel";
                    var rowAffected = await conn.ExecuteAsync(sql, new
                    {
                        id = id,
                        channel = channel,
                        baseline = baseline,
                        totalSales = totalSales,
                        uplift = uplift,
                        sales = salesContribution,
                        stores = storesCoverage,
                        redemptionRate = redemptionRate,
                        cr = cr,
                        cost = cost,
                        baselineRecon = baselineRecon,
                        totalSalesRecon = totalSalesRecon,
                        upliftRecon = upliftRecon,
                        salesRecon = salesContributionRecon,
                        storesRecon = storesCoverageRecon,
                        redemptionRateRecon = redemptionRateRecon,
                        crRecon = crRecon,
                        costRecon = costRecon,

                        createdBy = createdBy,
                        createdByEmail = createdEmail,
                        createdOn = TimeZoneInfo.ConvertTimeFromUtc(utcTime, zone),
                    });

                    if (rowAffected == 0) // if no row updated then insert
                    {
                        // Insert into Master table 
                        sql = @"INSERT INTO tbset_config_promo_calculator
                                (mainActivityId, channelId,
                                baseline, totalSales, uplift, salesContribution, storesCoverage, 
                                redemptionRate, cr, cost,
                                baselineRecon, totalSalesRecon, upliftRecon, salesContributionRecon, storesCoverageRecon, 
                                redemptionRateRecon, crRecon, costRecon,
                                createdOn, createdBy, createdByEmail)
                                VALUES(@mainActivity, @channel,
                                @baseline, @totalSales, @uplift, @sales, @stores, @redemptionRate,@cr, @cost, 
                                @baselineRecon, @totalSalesRecon, @upliftRecon, @salesRecon, @storesRecon, 
                                @redemptionRateRecon, @crRecon, @costRecon, 
                                @createdOn, @createdBy, @createdByEmail);";
                        var calcId = await conn.ExecuteScalarAsync<int>(sql, new
                        {
                            mainActivity = id,
                            channel = channel,
                            baseline = baseline,
                            totalSales = totalSales,
                            uplift = uplift,
                            sales = salesContribution,
                            stores = storesCoverage,
                            redemptionRate = redemptionRate,
                            cr = cr,
                            cost = cost,

                            baselineRecon = baselineRecon,
                            totalSalesRecon = totalSalesRecon,
                            upliftRecon = upliftRecon,
                            salesRecon = salesContributionRecon,
                            storesRecon = storesCoverageRecon,
                            redemptionRateRecon = redemptionRateRecon,
                            crRecon = crRecon,
                            costRecon = costRecon,

                            createdBy = createdBy,
                            createdByEmail = createdEmail,
                            createdOn = TimeZoneInfo.ConvertTimeFromUtc(utcTime, zone),
                        });
                    }

                    //using (var transaction = conn.BeginTransaction())
                    //{
                    //    try
                    //    {

                    //DELETE existing
                    var delSQL = "DELETE FROM tbmst_main_activity_dtl " +
                            "WHERE mainactivityid=" + id;
                    await conn.ExecuteAsync(delSQL);

                    foreach (var item in subActivity)
                    {
                        if (item > 0)
                        {
                            // Insert into Detail
                            var detailSql = "INSERT INTO tbmst_main_activity_dtl(mainactivityid, subactivityid) " +
                                "VALUES (@mainActivity, @subactivity)";
                            await conn.ExecuteAsync(detailSql, new
                            {
                                mainActivity = id,
                                subactivity = item
                            });
                        }
                    }
                    // If everything succeeds, commit the transaction
                    // transaction.Commit();

                    //}
                    //catch (Exception ex)
                    //{
                    //    // If there is an error, rollback the transaction
                    //    transaction.Rollback();

                    //    res = false;
                    //}
                    res = true;
                }


            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
            return res;
        }
        public async Task<object> GetPromoCalculatorById(int mainActivityId, int channelId)
        {
            try
            {
                mPromoCalculatorById __res = new mPromoCalculatorById();
                using (IDbConnection conn = Connection)
                {
                    conn.Open();
                    var sql = @"
                        SELECT 					
                        A.id, A.mainActivityId, 
						B.longDesc mainActivityDesc, 

                        A.channelId, ch.LongDesc channelLongDesc,
                        A.baseline, A.totalSales, A.uplift, 
                        A.salesContribution, A.storesCoverage, A.redemptionRate, A.cr, A.cost, 
						A.baselineRecon, A.totalSalesRecon, A.upliftRecon, 
                        A.salesContributionRecon, A.storesCoverageRecon, A.redemptionRateRecon, A.crRecon, A.costRecon
                        FROM tbset_config_promo_calculator A
                        LEFT JOIN tbmst_channel ch on ch.Id = A.channelId
                        LEFT JOIN tbmst_main_activity B ON B.id = A.mainActivityId
                        WHERE A.mainActivityId = @mainActivity and A.channelId=@channel

                        SELECT 					
                        A.channelId, ch.LongDesc channelLongDesc,
                        A.baseline, A.totalSales, A.uplift, 
                        A.salesContribution, A.storesCoverage, A.redemptionRate, A.cr, A.cost, 
						A.baselineRecon, A.totalSalesRecon, A.upliftRecon, 
                        A.salesContributionRecon, A.storesCoverageRecon, A.redemptionRateRecon, A.crRecon, A.costRecon
                        FROM tbset_config_promo_calculator A
                        LEFT JOIN tbmst_channel ch on ch.Id = A.channelId
                        LEFT JOIN tbmst_main_activity B ON B.id = A.mainActivityId
                        WHERE A.mainActivityId = @mainActivity

                        SELECT  C.subActivityId, D.subActivityDesc subActivity,
		                        D.CategoryDesc category, SubCategoryDesc subCategory, ActivityDesc activity
                        from tbmst_main_activity_dtl C
                        LEFT JOIN tbmst_main_activity B ON B.id = C.mainActivityId
                        LEFT JOIN vw_activity_active D ON D.SubActivityId = C.subactivityid
                        WHERE mainactivityid=@mainActivity
                        ";

                    var __resp = await conn.QueryMultipleAsync(sql,
                        new { mainActivity = mainActivityId, channel = channelId });

                    __res = __resp.ReadSingle<mPromoCalculatorById>();
                    __res.channelList = __resp.Read<Channellist>().ToArray();
                    __res.subActivityList = __resp.Read<Subactivitylist>().ToArray();

                    return __res;
                }
            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }
        public async Task<object> GetPromoCalculatorFilterAndSubActivityCoverage()
        {
            try
            {
                using (IDbConnection conn = Connection)
                {
                    conn.Open();
                    var sql = @"
                            SELECT DISTINCT categoryid, categorydesc FROM vw_activity_active 
                            ORDER BY CategoryDesc;

	                        SELECT DISTINCT categoryid, categorydesc, subcategoryid, subcategorydesc 
                            FROM vw_activity_active ORDER BY CategoryDesc, SubCategoryDesc;

	                        SELECT DISTINCT CategoryId, CategoryDesc, SubCategoryId, SubCategoryDesc, 
                            ActivityId, ActivityDesc FROM vw_activity_active 
                            ORDER BY CategoryDesc, SubCategoryDesc, ActivityDesc;
                        ";

                    sql += @"SELECT VA.subActivityId
	                      ,VA.SubActivityDesc subActivity
	                      ,VA.categoryId 
	                      ,VA.CategoryDesc category
	                      ,VA.subCategoryId
	                      ,VA.SubCategoryDesc subCategory
	                      ,VA.activityId 
	                      ,VA.ActivityDesc activity
                          ,MA.[longDesc] mainActivity
                          ,IIF(mainactivityid is NULL, 1, 0) available
                      FROM vw_activity_active VA
                      LEFT JOIN tbmst_main_activity_dtl MAD on MAD.subactivityid = VA.SubActivityId
                      LEFT JOIN tbmst_main_activity MA on MA.id = MAD.mainactivityid
                      WHERE 1=1 ";

                    using (var __resp = await conn.QueryMultipleAsync(sql, commandTimeout: 180))
                    {
                        var result = new
                        {
                            category = __resp.Read<object>().ToList(),
                            subCategory = __resp.Read<object>().ToList(),
                            activity = __resp.Read<object>().ToList(),
                            subActivity = __resp.Read<object>().ToList(),
                        };

                        return result;
                    }

                    //string paramList = string.Join(", ", category);
                    //if (category.Length > 0)
                    //{
                    //    sql += " AND CategoryId IN (" + paramList + ") ";
                    //}

                    //paramList = string.Join(", ", subCategory);
                    //if (subCategory.Length > 0)
                    //{
                    //    sql += " AND subCategoryId IN (" + paramList + ") ";
                    //}
                    //paramList = string.Join(", ", activity);
                    //if (activity.Length > 0)
                    //{
                    //    sql += " AND ActivityId IN (" + paramList + ") ";
                    //}
                    //var result = await conn.QueryAsync<object>(sql);
                }
            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }

    }


    public class mPromoCalculatorById
    {
        public int mainActivityId { get; set; }
        public string mainActivityDesc { get; set; }
        public int channelId { get; set; }
        public string channelLongDesc { get; set; }
        public int baseline { get; set; }
        public int uplift { get; set; }
        public int totalSales { get; set; }
        public int salesContribution { get; set; }
        public int storesCoverage { get; set; }
        public int redemptionRate { get; set; }
        public int cr { get; set; }
        public int cost { get; set; }
        public int baselineRecon { get; set; }
        public int upliftRecon { get; set; }
        public int totalSalesRecon { get; set; }
        public int salesContributionRecon { get; set; }
        public int storesCoverageRecon { get; set; }
        public int redemptionRateRecon { get; set; }
        public int crRecon { get; set; }
        public int costRecon { get; set; }
        public Subactivitylist[] subActivityList { get; set; }
        public Channellist[] channelList { get; set; }
    }

    public class Subactivitylist
    {
        public int subActivityId { get; set; }
        public string subActivity { get; set; }
        public string activity { get; set; }
        public string subCategory { get; set; }
        public string category { get; set; }
    }

    public class Channellist
    {
        public int channelId { get; set; }
        public string channelLongDesc { get; set; }
        public int baseline { get; set; }
        public int uplift { get; set; }
        public int totalSales { get; set; }
        public int salesContribution { get; set; }
        public int storesCoverage { get; set; }
        public int redemptionRate { get; set; }
        public int cr { get; set; }
        public int cost { get; set; }
        public int baselineRecon { get; set; }
        public int upliftRecon { get; set; }
        public int totalSalesRecon { get; set; }
        public int salesContributionRecon { get; set; }
        public int storesCoverageRecon { get; set; }
        public int redemptionRateRecon { get; set; }
        public int crRecon { get; set; }
        public int costRecon { get; set; }
    }

}