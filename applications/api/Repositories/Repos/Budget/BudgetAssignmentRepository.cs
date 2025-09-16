using Dapper;
using Entities;
using Repositories.Contracts;
using Repositories.Entities;

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
        public async Task<Entities.BudgetAllocation.BudgetAllocationDto> GetByPrimaryId(int id)
        {
            List<Entities.BudgetAllocation.BudgetAllocationDto> __budget = new();
            using (IDbConnection conn = Connection)
            {
                var __query = new DynamicParameters();
                __query.Add("@Id", id);
                __query.Add("@LongDesc", string.Empty);
                // __budget=await conn.QueryAsync<BudgetAllocationDto>("[dbo].[ip_budgetallocation_select]",__query,commandType:CommandType.StoredProcedure);
                using var __re = await conn.QueryMultipleAsync("[dbo].[ip_budgetallocation_select_new]", __query, commandType: CommandType.StoredProcedure, commandTimeout:180);
                Entities.BudgetAllocation.BudgetAllocationDto __result = new()
                {
                    BudgetAssignment = __re.Read<Entities.BudgetAllocation.BudgetAssignmentDetailDto>().FirstOrDefault()!,
                    BudgetParent = __re.Read<Entities.BudgetAllocation.BudgetBalanceDto>().FirstOrDefault()!,
                    BudgetAllocation = __re.Read<Entities.BudgetAllocation.BudgetAllocationView>().FirstOrDefault()!,
                    Regions = new List<Entities.BudgetAllocation.Region>()
                };
                foreach (RegionRes r in __re.Read<RegionRes>())
                    __result.Regions.Add(new Entities.BudgetAllocation.Region()
                    {
                        flag = r.flag,
                        Id = r.RegionId,
                        LongDesc = r.RegionName,
                    });
                __result.Channels = new List<Entities.BudgetAllocation.Channel>();
                foreach (ChannelRes c in __re.Read<ChannelRes>())
                    __result.Channels.Add(new Entities.BudgetAllocation.Channel()
                    {
                        flag = c.flag,
                        Id = c.ChannelId,
                        LongDesc = c.ChannelName,
                    });
                __result.SubChannels = new List<Entities.BudgetAllocation.SubChannel>();
                foreach (SubChannelRes sc in __re.Read<SubChannelRes>())
                    __result.SubChannels.Add(new Entities.BudgetAllocation.SubChannel()
                    {
                        flag = sc.flag,
                        Id = sc.SubChannelId,
                        LongDesc = sc.SubChannelName,
                    });
                __result.Accounts = new List<Entities.BudgetAllocation.Account>();
                foreach (AccountRes a in __re.Read<AccountRes>())
                    __result.Accounts.Add(new Entities.BudgetAllocation.Account()
                    {
                        flag = a.flag,
                        Id = a.AccountId,
                        LongDesc = a.AccountName,
                    });
                __result.SubAccounts = new List<Entities.BudgetAllocation.SubAccount>();
                foreach (SubAccountRes sa in __re.Read<SubAccountRes>())
                    __result.SubAccounts.Add(new Entities.BudgetAllocation.SubAccount()
                    {
                        flag = sa.flag,
                        Id = sa.SubAccountId,
                        LongDesc = sa.SubAccountName,
                    });

                __result.Brand = new List<Entities.BudgetAllocation.Brand>();
                foreach (BrandRes br in __re.Read<BrandRes>())
                    __result.Brand.Add(new Entities.BudgetAllocation.Brand()
                    {
                        flag = br.flag,
                        Id = br.BrandId,
                        LongDesc = br.BrandName,
                    });

                __result.Product = new List<Entities.BudgetAllocation.Product>();
                foreach (ProductRes pr in __re.Read<ProductRes>())
                    __result.Product.Add(new Entities.BudgetAllocation.Product()
                    {
                        flag = pr.flag,
                        Id = pr.ProductId,
                        LongDesc = pr.ProductName,
                    });

                __result.SubCategory = new List<Entities.BudgetAllocation.SubCategory>();
                foreach (SubCategoryRes sc in __re.Read<SubCategoryRes>())
                    __result.SubCategory.Add(new Entities.BudgetAllocation.SubCategory()
                    {
                        flag = sc.flag,
                        Id = sc.SubCategoryId,
                        LongDesc = sc.SubCategoryName,
                    });

                __result.Activity = new List<Entities.BudgetAllocation.Activity>();
                foreach (ActivityRes act in __re.Read<ActivityRes>())
                    __result.Activity.Add(new Entities.BudgetAllocation.Activity()
                    {
                        flag = act.flag,
                        Id = act.ActivityId,
                        LongDesc = act.ActivityName,
                    });

                __result.SubActivity = new List<Entities.BudgetAllocation.SubActivity>();
                foreach (SubActivityRes sact in __re.Read<SubActivityRes>())
                    __result.SubActivity.Add(new Entities.BudgetAllocation.SubActivity()
                    {
                        flag = sact.flag,
                        Id = sact.SubActivityId,
                        LongDesc = sact.SubActivityName,
                    });



                __result.UserAccess = new List<Entities.BudgetAllocation.BudgetAllocationChildsTypeDto>();
                foreach (UserAccessRes uac in __re.Read<UserAccessRes>())
                    __result.UserAccess.Add(new Entities.BudgetAllocation.BudgetAllocationChildsTypeDto()
                    {
                        flag = uac.flag,
                        Id = uac.UserAccessId,
                    });

                __result.UserAssign = new List<Entities.BudgetAllocation.BudgetAllocationChildsTypeDto>();
                foreach (UserAssignRes uas in __re.Read<UserAssignRes>())
                    __result.UserAssign.Add(new Entities.BudgetAllocation.BudgetAllocationChildsTypeDto()
                    {
                        flag = uas.flag,
                        Id = uas.UserAssignId,
                    });
                __result.UserPromo = new List<Entities.BudgetAllocation.BudgetAllocationChildsTypeDto>();
                foreach (UserPromoRes up in __re.Read<UserPromoRes>())
                    __result.UserPromo.Add(new Entities.BudgetAllocation.BudgetAllocationChildsTypeDto()
                    {
                        flag = up.flag,
                        Id = up.UserPromoId,
                    });

                __result.BudgetDetail = new List<Entities.BudgetAllocation.BudgetAllocationDetailDto>();
                foreach (Entities.BudgetAllocation.BudgetAllocationDetailDto item in __re.Read<Entities.BudgetAllocation.BudgetAllocationDetailDto>())
                    __result.BudgetDetail.Add(new Entities.BudgetAllocation.BudgetAllocationDetailDto()
                    {
                        AllocationId = item.AllocationId,
                        LineIndex = item.LineIndex,
                        subcategory = item.subcategory,
                        subcategorydesc = item.subcategorydesc,
                        activity = item.activity,
                        activitydesc = item.activitydesc,
                        subactivity = item.subactivity,
                        subactivitydesc = item.subactivitydesc,
                        BudgetAmount = item.BudgetAmount,
                        LongDesc = item.LongDesc
                    });
                __budget.Add(__result);
            }
            return __budget.FirstOrDefault()!;
        }

        public async Task<IList<Entities.BudgetAllocation.BudgetAllocationView>> GetBudgetAllocationByConditions(string year, int entity, int distributor, int BudgetParent, int channel, string userid)
        {
            using IDbConnection conn = Connection;

            var __param = new DynamicParameters();
            __param.Add("@periode", year);
            __param.Add("@entity", entity);
            __param.Add("@distributor", distributor);
            __param.Add("@budgetparent", BudgetParent);
            __param.Add("@channel", channel);
            __param.Add("@userid", userid);

            var result = await conn.QueryAsync<Entities.BudgetAllocation.BudgetAllocationView>("ip_budgetallocation_list", __param, commandType: CommandType.StoredProcedure, commandTimeout:180);
            return result.ToList();
            //return result;

        }

        public async Task<ErrorMessageDto> CreateBudgetAssignment(BudgetAssignmentStoreDto budgetAssignment)
        {

            DataTable __detail = Tools.CastToDataTable(new BudgetAssignmentDetailCreate(), null!);
            foreach (BudgetAssignmentDetailCreate v in budgetAssignment.AssignmentDetail!)
                __detail.Rows.Add(
                    v.Id,
                    v.RefId,
                    v.AssignmentId,
                    v.OwnId,
                    v.Desc,
                    v.BudgetAmount

                );
            using IDbConnection conn = Connection;

            var __query = new DynamicParameters();
            __query.Add("@id", budgetAssignment.Id);
            __query.Add("@refid", budgetAssignment.RefId);
            __query.Add("@budgetid", budgetAssignment.BudgetId);
            __query.Add("@frownid", budgetAssignment.FrownId);
            __query.Add("@allocationid", budgetAssignment.AllocationId);
            __query.Add("@budgetamount", budgetAssignment.BudgetAmount);
            __query.Add("@userid", budgetAssignment.UserId);
            __query.Add("@budgetdetail", __detail.AsTableValuedParameter());

            conn.Open();
            var result = await conn.QueryAsync<ErrorMessageDto>("[dbo].[ip_budgetassignment_create]", __query, commandType: CommandType.StoredProcedure, commandTimeout:180);

            return result.FirstOrDefault()!;
        }

        public async Task<ErrorMessageDto> UpdateBudgetAssignment(BudgetAssignmentUpdateDto budgetAssignment)
        {
            try
            {
                DataTable __detail = Tools.CastToDataTable(new BudgetAssignmentDetailUpdateDto(), null!);
                foreach (BudgetAssignmentDetailUpdateDto v in budgetAssignment.AssignmentDetail!)
                    __detail.Rows.Add(
                        v.Id,
                        v.ownid,
                        v.Desc,
                        v.BudgetAmount

                    );
                using IDbConnection conn = Connection;

                var __query = new DynamicParameters();
                __query.Add("@assignmentid", budgetAssignment.AssignmentId);
                __query.Add("@userid", budgetAssignment.UserId);
                __query.Add("@budgetdetail", __detail.AsTableValuedParameter());

                conn.Open();
                var result = await conn.QueryAsync<ErrorMessageDto>("[dbo].[ip_budgetassignment_update]", __query, commandType: CommandType.StoredProcedure, commandTimeout:180);

                return result.FirstOrDefault()!;
            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }

        public async Task<BudgetAssignmentDto> GetAssignmentById(int Id)
        {
            List<BudgetAssignmentDto> __budget = new();


            using (IDbConnection conn = Connection)
            {

                var __param = new DynamicParameters();
                __param.Add("@id", Id);


                using var __re = await conn.QueryMultipleAsync("ip_get_assignment", __param, commandType: CommandType.StoredProcedure, commandTimeout:180);
                BudgetAssignmentDto __result = new();
                __result = __re.Read<BudgetAssignmentDto>().FirstOrDefault()!;
                __result.AssignmentId = new List<BudgetAssignmentDetail>();
                __result.OwnerId = new List<string>()!;

                foreach (string r in __re.Read<string>())
                    __result.OwnerId.Add(r);

                foreach (BudgetAssignmentDetail r in __re.Read<BudgetAssignmentDetail>())
                    __result.AssignmentId.Add(new BudgetAssignmentDetail()
                    {
                        Id = r.Id,
                        RefId = r.RefId,
                        OwnId = r.OwnId,
                        OwnName = r.OwnName,
                        Desc = r.Desc,
                        IsAllocated = r.IsAllocated,
                        BudgetAmount = r.BudgetAmount,
                        IsActive = r.IsActive,
                        IsLocked = r.IsLocked,
                        CreateOn = r.CreateOn,
                        CreateBy = r.CreateBy,
                        ModifiedOn = r.ModifiedOn,
                        ModifiedBy = r.ModifiedBy,
                        IsDelete = r.IsDelete,
                        DeleteOn = r.DeleteOn,
                        DeleteBy = r.DeleteBy
                    });

                __budget.Add(__result);

            }
            return __budget.First();
        }
    }
}
