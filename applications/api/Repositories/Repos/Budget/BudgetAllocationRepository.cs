using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Repositories.Entities.BudgetAllocation;
using Repositories.Contracts;
using Repositories.Entities.Models;
using System.Security.Principal;
using Entities;

namespace Repositories.Repos
{
    public partial class BudgetMasterRepository : IBudgetMasterRepository
    {

        public async Task<List<BudgetAllocationView>> GetBudgetAllocationLandingPage(string year, int entity, int distributor,
            int BudgetParent, int channel, string userid)
        {
            using IDbConnection conn = Connection;
            var __param = new DynamicParameters();
            __param.Add("@periode", year);
            __param.Add("@entity", entity);
            __param.Add("@distributor", distributor);
            __param.Add("@budgetparent", BudgetParent);
            __param.Add("@channel", channel);
            __param.Add("@userid", userid);

            var result = await conn.QueryAsync<BudgetAllocationView>("ip_budgetallocation_list", __param, commandType: CommandType.StoredProcedure, commandTimeout:180);
            return result.ToList();

        }

        public async Task<List<BudgetAllocatedSource>> GetAllocatedBudgetSource(string year, int entity, int distributor, 
            string userid, string budgetType)
        {
            List<BudgetAllocatedSource>? res = new();
            try
            {
                using IDbConnection conn = Connection;

                var __param = new DynamicParameters();
                __param.Add("@periode", year);
                __param.Add("@entity", entity);
                __param.Add("@distributor", distributor);
                __param.Add("@userid", userid);

                if (budgetType == "BAL")
                {
                    string qry = @"SELECT  
		                    A.Id  
                            ,A.RefId
		                    ,A.Periode		                    
		                    ,A.BudgetAmount
		                    ,A.OwnerId
		                    ,A.LongDesc [Desc] 
	                    FROM  
		                    tbtrx_budgetmaster A LEFT OUTER JOIN
		                    tbmst_category E ON A.CategoryId = E.Id LEFT OUTER JOIN
		                    tbmst_distributor B ON A.DistributorId = B.Id LEFT OUTER JOIN
		                    tbmst_principal C ON A.PrincipalId = C.Id LEFT OUTER JOIN
		                    tbset_user D ON A.OwnerId = D.id
	                    WHERE 
		                    A.Periode=@periode AND ISNULL(A.IsDelete,0)=0
		                    AND IIF(@entity=0,'',A.PrincipalId) =IIF(@entity=0,'',@entity)
		                    AND IIF(@distributor=0,'',A.DistributorId) =IIF(@distributor=0,'',@distributor)
		                    AND IIF(@userid='0','0',A.CreateBy) =IIF(@userid='0','0',@userid)
		                    AND ISNULL(A.IsAllocated,0)=0
	                    ORDER BY A.Id";
                    var qryParam = new
                    {
                        periode = year,
                        entity = entity,
                        distributor = distributor,
                        userid = userid,
                    };
                    var result = await conn.QueryAsync<BudgetAllocatedSource>(qry, qryParam);
                    res = result.ToList();
                }
                else
                {
                    // from ip_assignment_detail_select
                    string qry = @"SELECT a.Id,d.RefId, a.[Desc],a.BudgetAmount,a.OwnId,d.Periode,
                            c.Id BudgetSourceId,a.IsAllocated
	                        FROM tbtrx_budgetassignment_detail a
	                        INNER JOIN tbtrx_budgetassignment b on a.AssignmentId=b.Id
	                        INNER JOIN tbtrx_allocation c on c.Id=b.AllocationId
	                        INNER JOIN tbtrx_budgetmaster d on d.Id=c.BudgetMasterId 
	                        WHERE OwnId=@userid and a.IsAllocated=0 AND ISNULL(d.IsDelete,0)=0
	                        and d.Periode=@periode
	                        and IIF(@entity=0,0,d.PrincipalId)=IIF(@entity=0,0,@entity)
	                        and IIF(@distributor=0,0,d.DistributorId)=IIF(@distributor=0,0,@distributor)
	                        AND isnull(a.IsAllocated,0)=0";

                    var qryParam = new
                    {
                        periode = year,
                        entity = entity,
                        distributor = distributor,
                        userid = userid,
                    };
                    var result = await conn.QueryAsync<BudgetAllocatedSource>(qry, qryParam);
                    res = result.ToList();
                }
                return res;
            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
        }

        public async Task<object> GetAllocatedBudgetSourceById(int id, string budgetType)
        {
            object res = new();

            try
            {
                using IDbConnection conn = Connection;

                if (budgetType == "BAL")
                {
                    var sql = @"SELECT a.*,b.ShortDesc PrincipalShortDesc,b.LongDesc PrincipalLongDesc,
                            c.ShortDesc DistributorShortDesc,c.LongDesc DistributorLongDesc,d.ShortDesc CategoryShortDesc,
                            d.LongDesc CategoryLongDesc FROM tbtrx_budgetmaster a
                            INNER JOIN tbmst_principal b on a.PrincipalId=b.Id
                            INNER JOIN tbmst_distributor c on a.DistributorId=c.Id
                            INNER JOIN tbmst_category d on a.CategoryId=d.Id WHERE a.Id = @Id";
                    var result = await conn.QueryAsync<BudgetMasterView>(sql, new { id = id });
                    return result.FirstOrDefault()!;
                }
                else
                {  //BTR
                    List<BudgetAllocationSourceById> lsBA = new();
                    var __query = new DynamicParameters();
                    __query.Add("@Id", id);
                    __query.Add("@LongDesc", string.Empty);
                    using var __re = await conn.QueryMultipleAsync("[dbo].[ip_get_assignmentdetail]", __query, commandType: CommandType.StoredProcedure, commandTimeout:180);
                    BudgetAllocationSourceById __result = new()
                    {
                        BudgetAssignment = __re.Read<BudgetAssignmentDetailDto>().FirstOrDefault(),
                        BudgetAllocation = __re.Read<BudgetAllocationView>().FirstOrDefault(),
                        Regions = new List<Region>()
                    };
                    foreach (RegionRes r in __re.Read<RegionRes>())
                        __result.Regions.Add(new Region()
                        {
                            flag = r.flag,
                            Id = r.RegionId,
                            LongDesc = r.RegionName,
                        });
                    __result.Channels = new List<Channel>();
                    foreach (ChannelRes c in __re.Read<ChannelRes>())
                        __result.Channels.Add(new Channel()
                        {
                            flag = c.flag,
                            Id = c.ChannelId,
                            LongDesc = c.ChannelName,
                        });
                    __result.SubChannels = new List<SubChannel>();
                    foreach (SubChannelRes sc in __re.Read<SubChannelRes>())
                        __result.SubChannels.Add(new SubChannel()
                        {
                            flag = sc.flag,
                            Id = sc.SubChannelId,
                            LongDesc = sc.SubChannelName,
                            ChannelId = sc.ChannelId
                        });
                    __result.Accounts = new List<Account>();
                    foreach (AccountRes a in __re.Read<AccountRes>())
                        __result.Accounts.Add(new Account()
                        {

                            Id = a.AccountId,
                            LongDesc = a.AccountName,
                            SubChannelId = a.SubChannelId,
                        });
                    __result.SubAccounts = new List<SubAccount>();
                    foreach (SubAccountRes sa in __re.Read<SubAccountRes>())
                        __result.SubAccounts.Add(new SubAccount()
                        {
                            Id = sa.SubAccountId,
                            LongDesc = sa.SubAccountName,
                            AccountId = sa.AccountId,
                        });

                    __result.Brand = new List<Brand>();
                    foreach (BrandRes br in __re.Read<BrandRes>())
                        __result.Brand.Add(new Brand()
                        {
                            flag = br.flag,
                            Id = br.BrandId,
                            LongDesc = br.BrandName,
                        });

                    __result.Product = new List<Product>();
                    foreach (ProductRes pr in __re.Read<ProductRes>())
                        __result.Product.Add(new Product()
                        {
                            flag = pr.flag,
                            Id = pr.ProductId,
                            LongDesc = pr.ProductName,
                            BrandId = pr.BrandId,
                        });

                    __result.SubCategory = new List<SubCategory>();
                    foreach (SubCategoryRes sc in __re.Read<SubCategoryRes>())
                        __result.SubCategory.Add(new SubCategory()
                        {
                            Id = sc.SubCategoryId,
                            LongDesc = sc.SubCategoryName,
                        });

                    __result.Activity = new List<Activity>();
                    foreach (ActivityRes act in __re.Read<ActivityRes>())
                        __result.Activity.Add(new Activity()
                        {
                            Id = act.ActivityId,
                            LongDesc = act.ActivityName,
                        });

                    __result.SubActivity = new List<SubActivity>();
                    foreach (SubActivityRes sact in __re.Read<SubActivityRes>())
                        __result.SubActivity.Add(new SubActivity()
                        {
                            Id = sact.SubActivityId,
                            LongDesc = sact.SubActivityName,
                        });



                    __result.UserAccess = new List<BudgetAllocationChildsTypeDto>();
                    foreach (UserAccessRes uac in __re.Read<UserAccessRes>())
                        __result.UserAccess.Add(new BudgetAllocationChildsTypeDto()
                        {
                            Id = uac.UserAccessId,
                        });

                    __result.UserAssign = new List<BudgetAllocationChildsTypeDto>();
                    foreach (UserAssignRes uas in __re.Read<UserAssignRes>())
                        __result.UserAssign.Add(new BudgetAllocationChildsTypeDto()
                        {
                            Id = uas.UserAssignId,
                        });
                    __result.UserPromo = new List<BudgetAllocationChildsTypeDto>();
                    foreach (UserPromoRes up in __re.Read<UserPromoRes>())
                        __result.UserPromo.Add(new BudgetAllocationChildsTypeDto()
                        {
                            Id = up.UserPromoId,
                        });

                    __result.BudgetDetail = new List<BudgetAllocationDetailDto>();
                    foreach (BudgetAllocationDetailDto item in __re.Read<BudgetAllocationDetailDto>())
                        __result.BudgetDetail.Add(new BudgetAllocationDetailDto()
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
                    return __result;
                }
            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }            
        }

        public async Task<BudgetAllocationDto> GetBudgetAllcationByPrimaryId(int id)
        {
            List<BudgetAllocationDto> __budget = new();
            using (IDbConnection conn = Connection)
            {
                var __query = new DynamicParameters();
                __query.Add("@Id", id);
                __query.Add("@LongDesc", string.Empty);
                using var __re = await conn.QueryMultipleAsync("[dbo].[ip_budgetallocation_select_new]", __query, commandType: CommandType.StoredProcedure, commandTimeout:180);
                BudgetAllocationDto __result = new()
                {
                    BudgetAssignment = __re.Read<BudgetAssignmentDetailDto>().FirstOrDefault(),
                    BudgetParent = __re.Read<BudgetBalanceDto>().FirstOrDefault(),
                    BudgetAllocation = __re.Read<BudgetAllocationView>().FirstOrDefault(),
                    Regions = new List<Region>()
                };
                foreach (RegionRes r in __re.Read<RegionRes>())
                    __result.Regions.Add(new Region()
                    {
                        flag = r.flag,
                        Id = r.RegionId,
                        LongDesc = r.RegionName,
                    });
                __result.Channels = new List<Channel>();
                foreach (ChannelRes c in __re.Read<ChannelRes>())
                    __result.Channels.Add(new Channel()
                    {
                        flag = c.flag,
                        Id = c.ChannelId,
                        LongDesc = c.ChannelName,
                    });
                __result.SubChannels = new List<SubChannel>();
                foreach (SubChannelRes sc in __re.Read<SubChannelRes>())
                    __result.SubChannels.Add(new SubChannel()
                    {
                        flag = sc.flag,
                        Id = sc.SubChannelId,
                        LongDesc = sc.SubChannelName,
                        ChannelId = sc.ChannelId
                    });
                __result.Accounts = new List<Account>();
                foreach (AccountRes a in __re.Read<AccountRes>())
                    __result.Accounts.Add(new Account()
                    {
                        flag = a.flag,
                        Id = a.AccountId,
                        LongDesc = a.AccountName,
                        SubChannelId = a.SubChannelId,
                    });
                __result.SubAccounts = new List<SubAccount>();
                foreach (SubAccountRes sa in __re.Read<SubAccountRes>())
                    __result.SubAccounts.Add(new SubAccount()
                    {
                        flag = sa.flag,
                        Id = sa.SubAccountId,
                        LongDesc = sa.SubAccountName,
                        AccountId = sa.AccountId,
                    });

                __result.Brand = new List<Brand>();
                foreach (BrandRes br in __re.Read<BrandRes>())
                    __result.Brand.Add(new Brand()
                    {
                        flag = br.flag,
                        Id = br.BrandId,
                        LongDesc = br.BrandName,
                    });

                __result.Product = new List<Product>();
                foreach (ProductRes pr in __re.Read<ProductRes>())
                    __result.Product.Add(new Product()
                    {
                        flag = pr.flag,
                        Id = pr.ProductId,
                        LongDesc = pr.ProductName,
                        BrandId = pr.BrandId
                    });

                __result.SubCategory = new List<SubCategory>();
                foreach (SubCategoryRes sc in __re.Read<SubCategoryRes>())
                    __result.SubCategory.Add(new SubCategory()
                    {
                        Id = sc.SubCategoryId,
                        LongDesc = sc.SubCategoryName,
                    });

                __result.Activity = new List<Activity>();
                foreach (ActivityRes act in __re.Read<ActivityRes>())
                    __result.Activity.Add(new Activity()
                    {
                        Id = act.ActivityId,
                        LongDesc = act.ActivityName,
                    });

                __result.SubActivity = new List<SubActivity>();
                foreach (SubActivityRes sact in __re.Read<SubActivityRes>())
                    __result.SubActivity.Add(new SubActivity()
                    {
                        Id = sact.SubActivityId,
                        LongDesc = sact.SubActivityName,
                    });



                __result.UserAccess = new List<BudgetAllocationChildsTypeDto>();
                // select userList that have access to budget Allocation
                var lsUser = __re.Read<UserAccessRes>();
                foreach (UserAccessRes uac in lsUser)
                    __result.UserAccess.Add(new BudgetAllocationChildsTypeDto()
                    {
                        Id = uac.UserAccessId,
                        flag = uac.flag,
                    });

                __result.UserAssign = new List<BudgetAllocationChildsTypeDto>();
                foreach (UserAssignRes uas in __re.Read<UserAssignRes>())
                    __result.UserAssign.Add(new BudgetAllocationChildsTypeDto()
                    {
                        Id = uas.UserAssignId,
                    });
                __result.UserPromo = new List<BudgetAllocationChildsTypeDto>();
                foreach (UserPromoRes up in __re.Read<UserPromoRes>())
                    __result.UserPromo.Add(new BudgetAllocationChildsTypeDto()
                    {
                        Id = up.UserPromoId,
                    });

                __result.BudgetDetail = new List<BudgetAllocationDetailDto>();
                foreach (BudgetAllocationDetailDto item in __re.Read<BudgetAllocationDetailDto>())
                    __result.BudgetDetail.Add(new BudgetAllocationDetailDto()
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
    }
}
