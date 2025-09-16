using Dapper;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;
using Repositories.Contracts;
using Repositories.Entities.Configuration;

namespace Repositories.Repos
{
    public class MajorChangesRepository : IMajorChangesRepository
    {
        readonly IConfiguration _config;
        public MajorChangesRepository(IConfiguration config)
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

        private async Task InsertHistory(string categoryShort)
        {
            using IDbConnection conn = Connection;
            string qry = "INSERT INTO tbhis_major_changes " +
                "(ParentId, CategoryId, SubCategory, Attachment, Entity, Distributor, GroupBrand, BudgetSources, PromoPlan, " +
                "Activity, SubActivity, StartPromo, EndPromo, " +
                "ActivityDesc, InitiatorNotes, IncrSales, Investment, " +
                "ROI, CR, Channel, SubChannel, Account, SubAccount, " +
                "Region, Brand, SKU, Mechanism, Action, ModifiedOn, ModifiedBy, ModifiedEmail  ) " +
                "SELECT " +
                "id, CategoryId, SubCategory, Attachment, Entity, Distributor, GroupBrand, BudgetSources, PromoPlan," +
                "Activity, SubActivity, StartPromo, EndPromo , " +
                "ActivityDesc, InitiatorNotes, IncrSales, Investment, " +
                "ROI, CR, Channel, SubChannel, Account, SubAccount, " +
                "Region, Brand, SKU, Mechanism, 'UPDATE', ModifiedOn, ModifiedBy, ModifiedEmail " +
                " FROM tbset_major_changes " +
                " WHERE CategoryId=(SELECT id FROM tbmst_category WHERE ShortDesc='" + categoryShort + "')";
            var __result = await conn.ExecuteAsync(qry);
        }

        public async Task<List<MajorChangesResp>> Select()
        {
            List<MajorChangesResp>? res = null;
            try
            {
                using IDbConnection conn = Connection;
                string qry = @" SELECT * FROM tbset_major_changes mc " +
                    "LEFT JOIN tbmst_category cat on cat.id = mc.CategoryId " +
                    "WHERE cat.ShortDesc='RC'";
                var __result = await conn.QueryAsync<MajorChangesResp>(qry);
                res = __result.ToList();
            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
            return res;
        }

        public async Task<List<MajorChangesResp>> SelectDC()
        {
            List<MajorChangesResp>? res = null;
            try
            {
                using IDbConnection conn = Connection;
                string qry = " SELECT * FROM tbset_major_changes mc " +
                    "LEFT JOIN tbmst_category cat on cat.id = mc.CategoryId " +
                    "WHERE cat.ShortDesc='DC' ";
                var __result = await conn.QueryAsync<MajorChangesResp>(qry);
                res = __result.ToList();
            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
            return res;
        }


        public async Task<List<MajorChangesResp>> GetHistory(string year, string catShortDesc = "RC")
        {
            List<MajorChangesResp>? res = null;
            try
            {
                using IDbConnection conn = Connection;
                string qry = " SELECT * FROM tbhis_major_changes " +
                    "WHERE YEAR(ModifiedOn) = '" + year + "' AND " +
                    "CategoryId=(SELECT id FROM tbmst_category WHERE ShortDesc='" + catShortDesc + "') ";
                var __result = await conn.QueryAsync<MajorChangesResp>(qry);
                res = __result.ToList();
            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
            return res;
        }

        public async Task<bool> UpdateMajorChanges(MajorChangesReq changes)
        {
            DateTime utcTime = DateTime.UtcNow;
            var zone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
            bool res = false;
            try
            {
                string catShortDesc = "RC";
                using IDbConnection conn = Connection;
                string qry = "UPDATE tbset_major_changes " +
                    "SET " +
                    "SubCategory  = @SubCategory, " +
                    "Distributor = @Distributor, " +
                    "GroupBrand    = @GroupBrand, " +
                    "BudgetSources    = @BudgetSources, " +
                    "PromoPlan    = @PromoPlan, " +
                    "Activity = @Activity, " +
                    "SubActivity  = @SubActivity , " +
                    "StartPromo   = @StartPromo  , " +
                    "EndPromo   = @EndPromo  , " +
                    "ActivityDesc   = @ActivityDesc  , " +
                    "InitiatorNotes    = @InitiatorNotes   , " +
                    "IncrSales    = @IncrSales , " +
                    "Investment    = @Investment , " +
                    "ROI    = @ROI, " +
                    "CR    = @CR, " +
                    "Channel   = @Channel, " +
                    "SubChannel    = @SubChannel , " +
                    "Account    = @Account, " +
                    "SubAccount    = @SubAccount, " +
                    "Region    = @Region, " +
                    "Brand    = @Brand, " +
                    "SKU    = @SKU, " +
                    "Mechanism    = @Mechanism , " +
                    "Attachment    = @Attachment, " +
                    "ModifiedOn     = @ModifiedOn, " +
                    "ModifiedBy      = @userid, " +
                    "ModifiedEmail       = @useremail " +
                    "WHERE CategoryId=(SELECT id FROM tbmst_category WHERE ShortDesc='" + catShortDesc + "') ";
                var __result = await conn.ExecuteAsync(qry, new
                {
                    SubCategory = changes.SubCategory,
                    Distributor = changes.Distributor,
                    GroupBrand = changes.GroupBrand,
                    BudgetSources = changes.BudgetSources,
                    PromoPlan = changes.PromoPlan,
                    Activity = changes.Activity,
                    SubActivity = changes.SubActivity,
                    StartPromo = changes.StartPromo,
                    EndPromo = changes.EndPromo,
                    ActivityDesc = changes.ActivityDesc,
                    InitiatorNotes = changes.InitiatorNotes,
                    IncrSales = changes.IncrSales,
                    Investment = changes.Investment,
                    ROI = changes.ROI,
                    CR = changes.CR,
                    Channel = changes.Channel,
                    SubChannel = changes.SubChannel,
                    Account = changes.Account,
                    SubAccount = changes.SubAccount,
                    Region = changes.Region,
                    Brand = changes.Brand,
                    SKU = changes.SKU,
                    Mechanism = changes.Mechanism,
                    Attachment = changes.Attachment,
                    ModifiedOn = TimeZoneInfo.ConvertTimeFromUtc(utcTime, zone),
                    userid = changes.userid,
                    useremail = changes.useremail
                });
                res = __result > 0;
                if (res)
                {
                    await InsertHistory(catShortDesc);
                }
            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
            return res;
        }

        public async Task<bool> UpdateMajorChangesDC(MajorChangesReq changes)
        {
            DateTime utcTime = DateTime.UtcNow;
            var zone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
            bool res = false;
            try
            {
                string catShortDesc = "DC";

                using IDbConnection conn = Connection;
                string qry = "UPDATE tbset_major_changes " +
                    "SET " +
                    //                    "Year = @Year, " +
                    "SubCategory  = @SubCategory, " +
                    //                    "Entity = @Entity, " +
                    "Distributor = @Distributor, " +
                    "GroupBrand    = @GroupBrand, " +
                    "BudgetSources    = @BudgetSources, " +
                    "PromoPlan    = @PromoPlan, " +
                    "Activity = @Activity, " +
                    "SubActivity  = @SubActivity , " +
                    "StartPromo   = @StartPromo  , " +
                    "EndPromo   = @EndPromo  , " +
                    "ActivityDesc   = @ActivityDesc  , " +
                    "InitiatorNotes    = @InitiatorNotes   , " +
                    "IncrSales    = @IncrSales , " +
                    "Investment    = @Investment , " +
                    "ROI    = @ROI   , " +
                    "CR    = @CR, " +
                    "Channel   = @Channel, " +
                    "SubChannel    = @SubChannel , " +
                    "Account    = @Account   , " +
                    "SubAccount    = @SubAccount , " +
                    "Region    = @Region   , " +
                    "Brand    = @Brand , " +
                    "SKU    = @SKU   , " +
                    "Mechanism    = @Mechanism , " +
                    "Attachment    = @Attachment, " +
                    "ModifiedOn     = @ModifiedOn, " +
                    "ModifiedBy      = @userid, " +
                    "ModifiedEmail       = @useremail " +
                    "WHERE CategoryId=(SELECT id FROM tbmst_category WHERE ShortDesc='" + catShortDesc + "') ";
                var __result = await conn.ExecuteAsync(qry, new
                {
                    SubCategory = changes.SubCategory,
                    Distributor = changes.Distributor,
                    GroupBrand = changes.GroupBrand,
                    BudgetSources = changes.BudgetSources,
                    PromoPlan = changes.PromoPlan,
                    Activity = changes.Activity,
                    SubActivity = changes.SubActivity,
                    StartPromo = changes.StartPromo,
                    EndPromo = changes.EndPromo,
                    ActivityDesc = changes.ActivityDesc,
                    InitiatorNotes = changes.InitiatorNotes,
                    IncrSales = changes.IncrSales,
                    Investment = changes.Investment,
                    ROI = changes.ROI,
                    CR = changes.CR,
                    Channel = changes.Channel,
                    SubChannel = changes.SubChannel,
                    Account = changes.Account,
                    SubAccount = changes.SubAccount,
                    Region = changes.Region,
                    Brand = changes.Brand,
                    SKU = changes.SKU,
                    Mechanism = changes.Mechanism,
                    Attachment = changes.Attachment,
                    ModifiedOn = TimeZoneInfo.ConvertTimeFromUtc(utcTime, zone),
                    userid = changes.userid,
                    useremail = changes.useremail
                });
                res = __result > 0;
                if (res)
                {
                    await InsertHistory(catShortDesc);
                }
            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
            return res;
        }
    }
}
