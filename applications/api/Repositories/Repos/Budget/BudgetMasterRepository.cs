using Microsoft.Extensions.Configuration;
using Repositories.Contracts;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Repositories.Entities.Report;
using static Dapper.SqlMapper;
using System.Threading.Channels;
using Repositories.Entities.Models;
using Repositories.Entities;
using Entities;
using Repositories.Entities.BudgetAllocation;

namespace Repositories.Repos
{
    public partial class BudgetMasterRepository : IBudgetMasterRepository
    {
        readonly IConfiguration __config;
        public BudgetMasterRepository(IConfiguration config)
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

        public async Task<BaseLP> GetBudgetMasterLandingPage(string year, int entity, int distributor,
            string userid, string keyword, string sortColumn, string sortDirection, int pageNumber, int pageSize)
        {
            BaseLP? res = null;
            using (IDbConnection conn = Connection)
                try
                {
                    var __param = new DynamicParameters();
                    __param.Add("@periode", year);
                    __param.Add("@entity", entity);
                    __param.Add("@distributor", distributor);
                    __param.Add("@userid", userid);
                    __param.Add("@keyword", keyword);
                    __param.Add("@sortDirection", sortDirection);
                    __param.Add("@sortColumn", sortColumn);
                    __param.Add("@pageNum", pageNumber);
                    __param.Add("@pageSize", pageSize);
                    var __res = await conn.QueryMultipleAsync("ip_budgetmaster_list_p", __param, commandType: CommandType.StoredProcedure, commandTimeout: 180);
                    res = __res.ReadSingle<BaseLP>();
                    res.Data = __res.Read<BudgetMasterDto>().Cast<object>().ToList();
                }
                catch (Exception __ex)
                {
                    throw new Exception(__ex.Message);
                }
            return res;
        }


        public async Task<IEnumerable<BaseDropDownList>> GetAllEntity()
        {
            IEnumerable<BaseDropDownList> principals;

            using (IDbConnection conn = Connection)
            {

                var sql = "Select * from tbmst_principal where IsDeleted=0";

                principals = await conn.QueryAsync<BaseDropDownList>(sql);
            }
            return principals;
        }

        public async Task<IList<BaseDropDownList>> GetDistributorList(int budgetid, int[] arrayParent)
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
                var child = await conn.QueryAsync<BaseDropDownList>("ip_getattribute_byparent", __query, commandType: CommandType.StoredProcedure, commandTimeout: 180);
                return child.ToList();
            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }

        public async Task<IList<BaseDropDownList>> GetCategoryList()
        {
            using IDbConnection conn = Connection;
            try
            {
                var __query = new DynamicParameters();

                __query.Add("@attribute", "category");
                __query.Add("@budgetid", "0");
                conn.Open();
                var child = await conn.QueryAsync<BaseDropDownList>("ip_getattribute_byparent", __query, commandType: CommandType.StoredProcedure, commandTimeout: 180);
                return child.ToList();
            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }

        public async Task<BudgetMasterDto> BudgetMasterById(int Id)
        {
            using IDbConnection conn = Connection;
            var sql = @"SELECT a.*,a.ShortDesc BudgetMasterShortDesc, a.LongDesc BudgetMasterLongDesc,  b.ShortDesc PrincipalShortDesc,b.LongDesc PrincipalLongDesc,
                            c.ShortDesc DistributorShortDesc,c.LongDesc DistributorLongDesc,d.ShortDesc CategoryShortDesc,
                            d.LongDesc CategoryLongDesc FROM tbtrx_budgetmaster a
                            INNER JOIN tbmst_principal b on a.PrincipalId=b.Id
                            INNER JOIN tbmst_distributor c on a.DistributorId=c.Id
                            INNER JOIN tbmst_category d on a.CategoryId=d.Id WHERE a.Id = @Id";
            var result = await conn.QueryAsync<BudgetMasterDto>(sql, new { id = Id });
            return result.FirstOrDefault()!;
        }

        public async Task<BudgetMasterDto> BudgetAllocationSourceById(int Id, string budgetType)
        {
            using IDbConnection conn = Connection;
            if (budgetType == "BAL")
            {
                var sql = @"SELECT a.*,a.ShortDesc BudgetMasterShortDesc, a.LongDesc BudgetMasterLongDesc,  b.ShortDesc PrincipalShortDesc,b.LongDesc PrincipalLongDesc,
                            c.ShortDesc DistributorShortDesc,c.LongDesc DistributorLongDesc,d.ShortDesc CategoryShortDesc,
                            d.LongDesc CategoryLongDesc FROM tbtrx_budgetmaster a
                            INNER JOIN tbmst_principal b on a.PrincipalId=b.Id
                            INNER JOIN tbmst_distributor c on a.DistributorId=c.Id
                            INNER JOIN tbmst_category d on a.CategoryId=d.Id WHERE a.Id = @Id";
                var result = await conn.QueryAsync<BudgetMasterDto>(sql, new { id = Id });
                return result.FirstOrDefault()!;
            }
            else
            {
                return new BudgetMasterDto();
            }
        }

        public async Task<ErrorMessageDto> CreateBudgetAllocation(BudgetAllocationStoreDto budgetallocation)
        {

            DataTable __ba = Tools.CastToDataTable(new BudgetHeaderStore(), null!);
            __ba.Rows.Add
            (
                0,
                budgetallocation.BudgetHeader!.Periode
                , budgetallocation.BudgetHeader.BudgetType
                , budgetallocation.BudgetHeader.DistributorId
                , budgetallocation.BudgetHeader.OwnerId
                , budgetallocation.BudgetHeader.FromOwnerId
                , budgetallocation.BudgetHeader.BudgetMasterId
                , budgetallocation.BudgetHeader.BudgetSourceId
                , budgetallocation.BudgetHeader.SalesAmount
                , budgetallocation.BudgetHeader.BudgetAmount
                , budgetallocation.BudgetHeader.LongDesc
                , budgetallocation.BudgetHeader.ShortDesc
                , budgetallocation.BudgetHeader.UserId
            );

            DataTable __child = Tools.CastToDataTable(new BudgetAttributeStore(), null!);
            DataTable __childstring = Tools.CastToDataTable(new BudgetStringAttributeStore(), null!);
            DataTable __detail = Tools.CastToDataTable(new BudgetAllocationDetailStoreDto(), null!);
            DataTable __reg = __child.Clone();
            foreach (BudgetAttributeStore v in budgetallocation.Regions!)
                __reg.Rows.Add(v.Id);

            DataTable __chan = __child.Clone();
            foreach (BudgetAttributeStore v in budgetallocation.Channels!)
                __chan.Rows.Add(v.Id);

            DataTable __subchan = __child.Clone();
            foreach (BudgetAttributeStore v in budgetallocation.SubChannels!)
                __subchan.Rows.Add(v.Id);

            DataTable __acc = __child.Clone();
            foreach (BudgetAttributeStore v in budgetallocation.Accounts!)
                __acc.Rows.Add(v.Id);

            DataTable __subacc = __child.Clone();
            foreach (BudgetAttributeStore v in budgetallocation.SubAccounts!)
                __subacc.Rows.Add(v.Id);

            DataTable __useracc = __childstring.Clone();
            foreach (BudgetStringAttributeStore v in budgetallocation.UserAccess!)
                __useracc.Rows.Add(v.Id);

            DataTable __brand = __child.Clone();
            foreach (BudgetAttributeStore v in budgetallocation.Brands!)
                __brand.Rows.Add(v.Id);

            DataTable __product = __child.Clone();
            foreach (BudgetAttributeStore v in budgetallocation.Products!)
                __product.Rows.Add(v.Id);

            foreach (BudgetAllocationDetailStoreDto v in budgetallocation.BudgetDetail!)
                __detail.Rows.Add(
                    v.AllocationId,
                    v.LineIndex,
                    v.SubcategoryId,
                    v.ActivityId,
                    v.SubactivityId,
                    v.BudgetAmount,
                    v.LongDesc
                );

            try
            {
                using IDbConnection conn = Connection;
                var __query = new DynamicParameters();
                __query.Add("@BUDGETALLOC", __ba.AsTableValuedParameter());
                __query.Add("@REGIONS", __reg.AsTableValuedParameter());
                __query.Add("@CHANNELS", __chan.AsTableValuedParameter());
                __query.Add("@SUBCHANNELS", __subchan.AsTableValuedParameter());
                __query.Add("@ACCOUNTS", __acc.AsTableValuedParameter());
                __query.Add("@SUBACCOUNTS", __subacc.AsTableValuedParameter());
                __query.Add("@USERACCESS", __useracc.AsTableValuedParameter());
                __query.Add("@BRANDS", __brand.AsTableValuedParameter());
                __query.Add("@PRODUCTS", __product.AsTableValuedParameter());
                __query.Add("@BUDGETDETAIL", __detail.AsTableValuedParameter());

                conn.Open();
                var result = await conn.QueryAsync<ErrorMessageDto>("[dbo].[ip_budgetallocation_create]", __query, commandType: CommandType.StoredProcedure, commandTimeout: 180);
                return result.FirstOrDefault()!;
            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }

        public async Task<ErrorMessageDto> UpdateBudgetAllocation(BudgetAllocationUpdateDto budgetallocation)
        {

            DataTable __ba = Tools.CastToDataTable(new BudgetHeaderStore(), null!);
            __ba.Rows.Add
            (
                budgetallocation.BudgetHeader!.id
                , budgetallocation.BudgetHeader.Periode
                , budgetallocation.BudgetHeader.BudgetType
                , budgetallocation.BudgetHeader.DistributorId
                , budgetallocation.BudgetHeader.OwnerId
                , budgetallocation.BudgetHeader.FromOwnerId
                , budgetallocation.BudgetHeader.BudgetMasterId
                , budgetallocation.BudgetHeader.BudgetSourceId
                , budgetallocation.BudgetHeader.SalesAmount
                , budgetallocation.BudgetHeader.BudgetAmount
                , budgetallocation.BudgetHeader.LongDesc
                , budgetallocation.BudgetHeader.ShortDesc
                , budgetallocation.BudgetHeader.UserId
            );

            DataTable __child = Tools.CastToDataTable(new BudgetAttributeStore(), null!);
            DataTable __childstring = Tools.CastToDataTable(new BudgetStringAttributeStore(), null!);
            DataTable __detail = Tools.CastToDataTable(new BudgetAllocationDetailStoreDto(), null!);
            DataTable __reg = __child.Clone();
            foreach (BudgetAttributeStore v in budgetallocation.Regions!)
                __reg.Rows.Add(v.Id);

            DataTable __chan = __child.Clone();
            foreach (BudgetAttributeStore v in budgetallocation.Channels!)
                __chan.Rows.Add(v.Id);

            DataTable __subchan = __child.Clone();
            foreach (BudgetAttributeStore v in budgetallocation.SubChannels!)
                __subchan.Rows.Add(v.Id);

            DataTable __acc = __child.Clone();
            foreach (BudgetAttributeStore v in budgetallocation.Accounts!)
                __acc.Rows.Add(v.Id);

            DataTable __subacc = __child.Clone();
            foreach (BudgetAttributeStore v in budgetallocation.SubAccounts!)
                __subacc.Rows.Add(v.Id);

            DataTable __useracc = __childstring.Clone();
            foreach (BudgetStringAttributeStore v in budgetallocation.UserAccess!)
                __useracc.Rows.Add(v.Id);

            DataTable __brand = __child.Clone();
            foreach (BudgetAttributeStore v in budgetallocation.Brands!)
                __brand.Rows.Add(v.Id);

            DataTable __product = __child.Clone();
            foreach (BudgetAttributeStore v in budgetallocation.Products!)
                __product.Rows.Add(v.Id);

            foreach (BudgetAllocationDetailStoreDto v in budgetallocation.BudgetDetail!)
                __detail.Rows.Add(
                    v.AllocationId,
                    v.LineIndex,
                    v.SubcategoryId,
                    v.ActivityId,
                    v.SubactivityId,
                    v.BudgetAmount,
                    v.LongDesc
                );
            try
            {
                using IDbConnection conn = Connection;
                var __query = new DynamicParameters();
                __query.Add("@BUDGETALLOC", __ba.AsTableValuedParameter());
                __query.Add("@REGIONS", __reg.AsTableValuedParameter());
                __query.Add("@CHANNELS", __chan.AsTableValuedParameter());
                __query.Add("@SUBCHANNELS", __subchan.AsTableValuedParameter());
                __query.Add("@ACCOUNTS", __acc.AsTableValuedParameter());
                __query.Add("@SUBACCOUNTS", __subacc.AsTableValuedParameter());
                __query.Add("@USERACCESS", __useracc.AsTableValuedParameter());
                __query.Add("@BRANDS", __brand.AsTableValuedParameter());
                __query.Add("@PRODUCTS", __product.AsTableValuedParameter());
                __query.Add("@BUDGETDETAIL", __detail.AsTableValuedParameter());

                conn.Open();
                var result = await conn.QueryAsync<ErrorMessageDto>("[dbo].[ip_budgetallocation_update]", __query, commandType: CommandType.StoredProcedure, commandTimeout: 180);
                return result.FirstOrDefault()!;
            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }
        public async Task<bool> CreateBudgetMaster(BudgetMasterSaveDto budgetMaster)
        {
            DateTime utcTime = DateTime.UtcNow;
            var zone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
            bool res = false;
            try
            {
                using IDbConnection conn = Connection;
                conn.Open();
                var sql = @"

                INSERT INTO [dbo].[tbtrx_budgetmaster]
                (

                          [Periode]
                          ,[DistributorId]
                          ,[PrincipalId]
                          ,[OwnerId]
                          ,[StartDate]
                          ,[EndDate]
                          ,[BudgetAmount]
                          ,[IsAllocated]
                          ,[LongDesc]
                          ,[ShortDesc]
                          ,[IsActive]
                          ,[CreateOn]
                          ,[CreateBy]
                          ,[CategoryId]
                )                
                VALUES
                (
   
                          @Periode
                          ,@DistributorId
                          ,@PrincipalId
                          ,@OwnerId
                          ,@StartDate
                          ,@EndDate
                          ,@BudgetAmount
                          ,@IsAllocated
                          ,@LongDesc
                          ,@ShortDesc
                          ,@IsActive
                          ,@CreateOn
                          ,@CreateBy
                          ,@CategoryId
                )
                ";
                var __res = await conn.ExecuteAsync(sql, new
                {
                    categoryid = budgetMaster.CategoryId,
                    periode = budgetMaster.Periode,
                    distributorid = budgetMaster.DistributorId,
                    principalid = budgetMaster.PrincipalId,
                    ownerid = budgetMaster.OwnerId,
                    startdate = TimeZoneInfo.ConvertTimeFromUtc(utcTime, zone),
                    enddate = new DateTime(Convert.ToInt16(budgetMaster.Periode), 12, 31),
                    budgetamount = budgetMaster.BudgetAmount,
                    isallocated = 0,
                    longdesc = budgetMaster.LongDesc,
                    shortdesc = "",
                    isactive = 1,
                    CreateOn = TimeZoneInfo.ConvertTimeFromUtc(utcTime, zone),
                    createby = budgetMaster.CreateBy
                });
                res = __res > 0;
            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
            return res;
        }

        public async Task<bool> UpdateBudgetMaster(BudgetMasterSaveDto budgetMaster)
        {
            bool res = false;
            try
            {
                using IDbConnection conn = Connection;
                conn.Open();
                var sql = @"UPDATE [dbo].[tbtrx_budgetmaster]
                    SET         
                          Periode=@Periode
                          ,DistributorId=@DistributorId
                          ,PrincipalId=@PrincipalId
                          ,BudgetAmount=@BudgetAmount
                          ,IsAllocated=@IsAllocated
                          ,LongDesc=@LongDesc
                          ,ShortDesc=@ShortDesc
                          ,ModifiedOn=@ModifiedOn
                          ,ModifiedBy=@ModifiedBy
                          ,CategoryId=@CategoryId
                    WHERE
                          Id=@Id";
                var __res = await conn.ExecuteAsync(sql, budgetMaster);
                res = __res > 0;
            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
            return res;
        }

        public async Task<bool> DeleteBudgetMaster(BudgetMasterDeleteDto budgetMasterDeleteDto)
        {
            DateTime utcTime = DateTime.UtcNow;
            var zone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
            bool res = false;
            try
            {
                using IDbConnection conn = Connection;
                conn.Open();
                var sql = @"UPDATE [dbo].[tbtrx_budgetmaster]
                        SET
                        IsDelete=1
                        ,DeleteBy=@DeleteBy
                        ,DeleteOn=@DeleteOn
                        WHERE
                        Id=@Id";
                var __res = await conn.ExecuteAsync(sql, new
                {
                    id = budgetMasterDeleteDto.Id,
                    deleteby = budgetMasterDeleteDto.DeleteBy,
                    deleteon = TimeZoneInfo.ConvertTimeFromUtc(utcTime, zone)

                });
                res = __res > 0;
            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
            return res;
        }

        #region budgetAssignment
        public async Task<IList<BudgetAssignmentView>> GetBudgetAssignmentLP(string year, int entity, int distributor, int BudgetParent, int channel, string userid)
        {
            using IDbConnection conn = Connection;

            var __param = new DynamicParameters();
            __param.Add("@periode", year);
            __param.Add("@entity", entity);
            __param.Add("@distributor", distributor);
            __param.Add("@budgetparent", BudgetParent);
            __param.Add("@channel", channel);
            __param.Add("@userid", userid);

            var result = await conn.QueryAsync<BudgetAssignmentView>("ip_budgetassignment_list", __param, commandType: CommandType.StoredProcedure, commandTimeout: 180);
            return result.ToList();
        }
        #endregion
    }
}
