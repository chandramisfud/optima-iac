using Microsoft.AspNetCore.Mvc;
//using Microsoft.Extensions.Caching.Memory;
//using Repositories.Entities.Models;
using V7.MessagingServices;
using V7.Model.Budget;
using V7.Services;
using V7.Model;
//using System.Runtime.Caching;
using Repositories.Entities;

namespace V7.Controllers.Budget
{
    /// <summary>
    /// Budget Handle Controller
    /// </summary>
    public partial class BudgetController : BaseController
    {
        private const string CacheKeyPrefix = "FilteredDataCacheKey_";
        private static bool _BAIsChanged;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpGet("api/budget/Allocation/distributor", Name = "budget_allocation_distributor")]
        public async Task<IActionResult> GetBudgetAllocationDistributor([FromQuery] DistributorListParam param)
        {
            try
            {
                //if (!ModelState.IsValid) return Conflict(ModelState);
                var __val = await _repoBudgetMaster.GetDistributorList(param.budgetId, param.entityId!);
                if (__val != null)
                {
                    return Ok(new BaseResponse
                    {
                        code = 200,
                        error = false,
                        message = MessageService.GetDataSuccess,
                        values = __val
                    });
                }
                else
                {
                    return Ok(new BaseResponse { code = 404, error = true, message = MessageService.GetDataFailed });
                }

            }
            catch (Exception __ex)
            {
                return Conflict(new BaseResponse { code = 500, error = true, message = __ex.Message });

            }
        }

        [HttpGet("api/budget/allocation/entity", Name = "budget_allocation_entity")]
        public async Task<IActionResult> GetBudgetAllocationEntity()
        {
            try
            {
                var __val = await _repoBudgetMaster.GetAllEntity();
                if (__val != null)
                {
                    return Ok(new BaseResponse
                    {
                        code = 200,
                        error = false,
                        message = MessageService.GetDataSuccess,
                        values = __val
                    });
                }
                else
                {
                    return Ok(new BaseResponse { code = 404, error = true, message = MessageService.GetDataFailed });
                }

            }
            catch (Exception __ex)
            {
                return Conflict(new BaseResponse { code = 500, error = true, message = __ex.Message });

            }
        }

        [HttpGet("api/budget/allocation", Name = "budget_allocation")]
        public async Task<IActionResult> GetBudgetAllocationWithPagination([FromQuery] budgetAllocationLPParam param)
        {
            try
            {
                string tokenHeader = Request.Headers["Authorization"]!;
                tokenHeader = tokenHeader.Replace("Bearer ", "");
                var __res = TokenManager.GetClaim(__TokenSecret, tokenHeader);
                if (__res.ProfileID != null)
                {
                    // Membuat kunci cache unik berdasarkan kombinasi filter, nomor halaman, dan ukuran halaman
                    // + user profile - August 09 23
                    string cacheKey = $"{CacheKeyPrefix}{param.year}_{param.entity}_{param.distributor}_{param.budgetParent}_{param.channel}_{__res.ProfileID}";

                    // Cek apakah data telah dipaging dan difilter sebelumnya dan tersimpan di cache
                    BaseLP respon = new();
                    if (false) //always reload, filter and paging get done in UI
                    {

                    }
                    else
                    {
                        // Data tidak ada di cache, lakukan pengambilan data dari sumbernya (misalnya, dari database)
                        var data = await _repoBudgetMaster.GetBudgetAllocationLandingPage(param.year!, param.entity, param.distributor, param.budgetParent,
                        param.channel, __res.ProfileID);
                        var filteredData = data;
                        if (!String.IsNullOrEmpty(param.Search))
                        {
                            param.Search = param.Search.ToLower();
                            filteredData = filteredData.Where(x => x.periode.ToString().Contains(param.Search) ||
                            x.refId!.Contains(param.Search) || x.longDesc!.ToLower().Contains(param.Search) ||
                            x.budgetType!.Contains(param.Search) || x.budgetAmount.ToString().Contains(param.Search) ||
                            x.distributor!.ToLower().Contains(param.Search) || x.entity!.ToLower().Contains(param.Search)
                            ).ToList();
                        }
                        respon.TotalCount = data.Count;
                        // save data to cache for 5minutes
                        _BAIsChanged = false;

                        // sort order
                        switch (param.SortColumn)
                        {
                            case sortBudgetAllocationField.longDesc:
                                if (param.SortDirection.ToString() == "asc")
                                {
                                    filteredData = filteredData.OrderBy(x => x.longDesc).ToList();
                                }
                                else
                                {
                                    filteredData = filteredData.OrderByDescending(x => x.longDesc).ToList();
                                }
                                break;
                            case sortBudgetAllocationField.distributor:
                                if (param.SortDirection.ToString() == "asc")
                                {
                                    filteredData = filteredData.OrderBy(x => x.distributor).ToList();
                                }
                                else
                                {
                                    filteredData = filteredData.OrderByDescending(x => x.distributor).ToList();
                                }
                                break;
                            case sortBudgetAllocationField.refId:
                                if (param.SortDirection.ToString() == "asc")
                                {
                                    filteredData = filteredData.OrderBy(x => x.refId).ToList();
                                }
                                else
                                {
                                    filteredData = filteredData.OrderByDescending(x => x.refId).ToList();
                                }
                                break;
                            case sortBudgetAllocationField.budgetAmount:
                                if (param.SortDirection.ToString() == "asc")
                                {
                                    filteredData = filteredData.OrderBy(x => x.budgetAmount).ToList();
                                }
                                else
                                {
                                    filteredData = filteredData.OrderByDescending(x => x.budgetAmount).ToList();
                                }
                                break;
                            default:
                                break;

                        }

                        // Lakukan paging dan kembalikan data yang sesuai dengan permintaan
                        if (param.PageSize >= 0)
                        {
                            var pagedData = Helper.GetPagedData(filteredData.Cast<object>().ToList(), param.PageNumber, param.PageSize);
                            respon.Data = pagedData;
                        }
                        else
                        {
                            respon.Data = filteredData.Cast<object>().ToList();
                        }

                        respon.FilteredCount = filteredData.Count;
                    }

                    if (respon != null)
                    {
                        return Ok(new BaseResponse
                        {
                            code = 200,
                            error = false,
                            message = MessageService.GetDataSuccess,
                            values = respon
                        });
                    }
                    else
                    {
                        return Ok(new BaseResponse { code = 404, error = true, message = MessageService.GetDataFailed });
                    }
                }
                else
                {
                    return NotFound(new BaseResponse { error = true, code = 404, message = MessageService.EmailTokenFailed });
                }
            }
            catch (Exception __ex)
            {
                return Conflict(new BaseResponse { code = 500, error = true, message = __ex.Message });

            }
        }

        /// <summary>
        /// Get Budget Source
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpGet("api/budget/allocation/source", Name = "budget_allocation_source")]
        public async Task<IActionResult> GetAllocatationBudgetBySource([FromQuery] BudgetAllocationSourceParam param)
        {
            try
            {
                string tokenHeader = Request.Headers["Authorization"]!;
                tokenHeader = tokenHeader.Replace("Bearer ", "");
                var __res = TokenManager.GetClaim(__TokenSecret, tokenHeader);
                if (__res.ProfileID != null)
                {
                    BaseLP respon = new();

                    var result = await _repoBudgetMaster.GetAllocatedBudgetSource(param.year!, param.entity, param.distributor,
                        __res.ProfileID.ToString(), param.budgetType.ToString());


                    if (result != null)
                    {
                        var filtered = result;

                        if (!String.IsNullOrEmpty(param.Search))
                        {
                            filtered = result.Where(x => x.refId!.Contains(param.Search) ||
                            x.budgetAmount.ToString().Contains(param.Search) || x.Desc!.Contains(param.Search)
                            ).ToList();
                        }
                        // ordered before paging
                        filtered = filtered.OrderBy(x => x.id).ToList();

                        if (param.PageSize >= 0)
                        {
                            var pagedData = Helper.GetPagedData(filtered.Cast<object>().ToList(), param.PageNumber, param.PageSize);
                            respon.Data = pagedData;
                        }
                        else
                        {
                            respon.Data = filtered.Cast<object>().ToList();
                        }
                        respon.TotalCount = result.Count;
                        respon.FilteredCount = filtered.Count;
                        return Ok(new BaseResponse { error = false, code = 200, values = respon, message = MessageService.GetDataSuccess });
                    }
                    else
                    {
                        return Ok(new BaseResponse { code = 404, error = true, message = MessageService.GetDataFailed });
                    }
                }
                else
                {
                    return NotFound(new BaseResponse { error = true, code = 404, message = MessageService.EmailTokenFailed });
                }
            }
            catch (System.Exception __ex)
            {
                return NotFound(new BaseResponse { error = true, code = 500, message = __ex.Message });
            }
        }

        /// <summary>
        /// Get Budget Allocation Source by Id
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpGet("api/budget/allocation/source/id", Name = "budgetAllocation_source")]
        public async Task<IActionResult> GetBudgetAlocationSourceById([FromQuery] BudgetAllocationSourceByIDParam param)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var result = await _repoBudgetMaster.GetAllocatedBudgetSourceById(param.id, param.budgetType.ToString());
                if (result == null)
                {
                    return Ok(new BaseResponse
                    {
                        error = false,
                        code = 404,
                        message = MessageService.DataNotFound
                    }
                    );
                }
                return Ok(new BaseResponse
                {
                    error = false,
                    code = 200,
                    values = result,
                    message = MessageService.GetDataSuccess
                });
            }
            catch (System.Exception __ex)
            {
                return NotFound(new BaseResponse { error = true, code = 500, message = __ex.Message });
            }
        }

        [HttpPost("api/budget/allocation", Name = "budgetallocation_create")]
        public async Task<IActionResult> CreateBudgetAllocation([FromBody] BudgetAllocationCreateParam param)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            try
            {
                string tokenHeader = Request.Headers["Authorization"]!;
                tokenHeader = tokenHeader.Replace("Bearer ", "");
                var __res = TokenManager.GetClaim(__TokenSecret, tokenHeader);
                if (__res.ProfileID != null)
                {

                    BudgetAllocationStoreDto budgetAllocation = new();
                    budgetAllocation = new BudgetAllocationStoreDto
                    {
                        BudgetHeader = new BudgetHeaderStore
                        {
                            BudgetAmount = param.budgetHeader!.budgetAmount,
                            BudgetMasterId = param.budgetHeader.budgetMasterId,
                            BudgetSourceId = param.budgetHeader.budgetSourceId,
                            BudgetType = param.budgetHeader.budgetType,
                            DistributorId = param.budgetHeader.distributorId,
                            FromOwnerId = param.budgetHeader.fromOwnerId,

                            LongDesc = param.budgetHeader.longDesc,
                            Periode = param.budgetHeader.periode,
                            SalesAmount = param.budgetHeader.salesAmount,
                            ShortDesc = param.budgetHeader.shortDesc,
                            OwnerId = param.budgetHeader.ownerId,
                            UserId = __res.ProfileID
                        },
                        BudgetDetail = new List<BudgetAllocationDetailStoreDto>()
                    };

                    budgetAllocation.BudgetDetail.Add(new BudgetAllocationDetailStoreDto
                    {
                        AllocationId = param.budgetDetail!.allocationId,
                        ActivityId = param.budgetDetail.activityId,
                        BudgetAmount = param.budgetDetail.budgetAmount,
                        LineIndex = param.budgetDetail.lineIndex,
                        SubactivityId = param.budgetDetail.subActivityId,
                        SubcategoryId = param.budgetDetail.subCategoryId,
                        LongDesc = param.budgetDetail.longDesc
                    });

                    budgetAllocation.Brands = new List<BudgetAttributeStore>();
                    foreach (var item in param.brands!)
                    {
                        budgetAllocation.Brands.Add(new BudgetAttributeStore { Id = item.id });
                    }

                    budgetAllocation.Accounts = new List<BudgetAttributeStore>();
                    foreach (var item in param.accounts!)
                    {
                        budgetAllocation.Accounts.Add(new BudgetAttributeStore { Id = item.id });
                    }

                    budgetAllocation.SubAccounts = new List<BudgetAttributeStore>();
                    foreach (var item in param.subAccounts!)
                    {
                        budgetAllocation.SubAccounts.Add(new BudgetAttributeStore { Id = item.id });
                    }

                    budgetAllocation.UserAccess = new List<BudgetStringAttributeStore>();
                    foreach (var item in param.userAccess!)
                    {
                        budgetAllocation.UserAccess.Add(new BudgetStringAttributeStore { Id = item.id!.ToString() });
                    }

                    budgetAllocation.Channels = new List<BudgetAttributeStore>();
                    foreach (var item in param.channels!)
                    {
                        budgetAllocation.Channels.Add(new BudgetAttributeStore { Id = item.id });
                    }

                    budgetAllocation.SubChannels = new List<BudgetAttributeStore>();
                    foreach (var item in param.subChannels!)
                    {
                        budgetAllocation.SubChannels.Add(new BudgetAttributeStore { Id = item.id });
                    }

                    budgetAllocation.Regions = new List<BudgetAttributeStore>();
                    foreach (var item in param.regions!)
                    {
                        budgetAllocation.Regions.Add(new BudgetAttributeStore { Id = item.id });
                    }

                    budgetAllocation.Products = new List<BudgetAttributeStore>();
                    foreach (var item in param.skus!)
                    {
                        budgetAllocation.Products.Add(new BudgetAttributeStore { Id = item.id });
                    }

                    ErrorMessageDto __respo = await _repoBudgetMaster.CreateBudgetAllocation(budgetAllocation);
                    _BAIsChanged = true;

                    return Ok(new BaseResponse
                    {
                        code = 200,
                        values = __respo,
                        message = MessagingServices.MessageService.SaveSuccess
                    });
                }
                else
                {
                    return NotFound(new BaseResponse
                    {
                        error = true,
                        code = 404,
                        message = MessageService.EmailTokenFailed
                    });
                }
            }
            catch (Exception __ex)
            {
                return Conflict(new BaseResponse { code = 500, error = true, message = __ex.Message });
            }
        }


        [HttpPut("api/budget/allocation", Name = "budget_allocation_update")]
        public async Task<IActionResult> UpdateBudgetAllocation([FromBody] BudgetAllocationUpdateParam param)
        {

            if (!ModelState.IsValid) return BadRequest(ModelState);
            try
            {
                string tokenHeader = Request.Headers["Authorization"]!;
                tokenHeader = tokenHeader.Replace("Bearer ", "");
                var __res = TokenManager.GetClaim(__TokenSecret, tokenHeader);
                if (__res.ProfileID != null)
                {

                    BudgetAllocationUpdateDto budgetAllocation = new()
                    {
                        BudgetHeader = new BudgetHeaderStore
                        {
                            BudgetAmount = param.budgetHeader!.budgetAmount,
                            BudgetMasterId = param.budgetHeader.budgetMasterId,
                            BudgetSourceId = param.budgetHeader.budgetSourceId,
                            BudgetType = param.budgetHeader.budgetType,
                            DistributorId = param.budgetHeader.distributorId,
                            FromOwnerId = param.budgetHeader.fromOwnerId,
                            id = param.budgetHeader.id,
                            LongDesc = param.budgetHeader.longDesc,
                            Periode = param.budgetHeader.periode,
                            SalesAmount = param.budgetHeader.salesAmount,
                            ShortDesc = param.budgetHeader.shortDesc,
                            OwnerId = param.budgetHeader.ownerId,

                            UserId = __res.ProfileID
                        },
                        BudgetDetail = new List<BudgetAllocationDetailStoreDto>()
                    };

                    budgetAllocation.BudgetDetail.Add(new BudgetAllocationDetailStoreDto
                    {
                        AllocationId = param.budgetDetail!.allocationId,
                        ActivityId = param.budgetDetail.activityId,
                        BudgetAmount = param.budgetDetail.budgetAmount,
                        LineIndex = param.budgetDetail.lineIndex,
                        SubactivityId = param.budgetDetail.subActivityId,
                        SubcategoryId = param.budgetDetail.subCategoryId,
                        LongDesc = param.budgetDetail.longDesc
                    });

                    budgetAllocation.Brands = new List<BudgetAttributeStore>();
                    foreach (var item in param.brands!)
                    {
                        budgetAllocation.Brands.Add(new BudgetAttributeStore { Id = item.id });
                    }

                    budgetAllocation.Accounts = new List<BudgetAttributeStore>();
                    foreach (var item in param.accounts!)
                    {
                        budgetAllocation.Accounts.Add(new BudgetAttributeStore { Id = item.id });
                    }

                    budgetAllocation.SubAccounts = new List<BudgetAttributeStore>();
                    foreach (var item in param.subAccounts!)
                    {
                        budgetAllocation.SubAccounts.Add(new BudgetAttributeStore { Id = item.id });
                    }

                    budgetAllocation.UserAccess = new List<BudgetStringAttributeStore>();
                    foreach (var item in param.userAccess!)
                    {
                        budgetAllocation.UserAccess.Add(new BudgetStringAttributeStore { Id = item.id!.ToString() });
                    }

                    budgetAllocation.Channels = new List<BudgetAttributeStore>();
                    foreach (var item in param.channels!)
                    {
                        budgetAllocation.Channels.Add(new BudgetAttributeStore { Id = item.id });
                    }

                    budgetAllocation.SubChannels = new List<BudgetAttributeStore>();
                    foreach (var item in param.subChannels!)
                    {
                        budgetAllocation.SubChannels.Add(new BudgetAttributeStore { Id = item.id });
                    }

                    budgetAllocation.Regions = new List<BudgetAttributeStore>();
                    foreach (var item in param.regions!)
                    {
                        budgetAllocation.Regions.Add(new BudgetAttributeStore { Id = item.id });
                    }

                    budgetAllocation.Products = new List<BudgetAttributeStore>();
                    foreach (var item in param.skus!)
                    {
                        budgetAllocation.Products.Add(new BudgetAttributeStore { Id = item.id });
                    }

                    ErrorMessageDto __respo = await _repoBudgetMaster.UpdateBudgetAllocation(budgetAllocation);
                    _BAIsChanged = true;

                    return Ok(new BaseResponse
                    {
                        code = 200,
                        values = __respo,
                        message = MessagingServices.MessageService.SaveSuccess
                    });
                }
                else
                {
                    return NotFound(new BaseResponse
                    {
                        error = true,
                        code = 404,
                        message = MessageService.EmailTokenFailed
                    });
                }
            }
            catch (Exception __ex)
            {
                return Conflict(new BaseResponse { code = 500, error = true, message = __ex.Message });
            }
        }

        /// <summary>
        /// Get data by id budget allocation
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("api/budget/allocation/id", Name = "budgetAllocation_byId")]
        public async Task<IActionResult> GetBudgetAlocationById([FromQuery] int id)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var result = await _repoBudgetMaster.GetBudgetAllcationByPrimaryId(id);
                if (result == null)
                {
                    return Ok(new BaseResponse
                    {
                        error = false,
                        code = 404,
                        message = MessageService.GetDataFailed
                    }
                    );
                }
                return Ok(new BaseResponse
                {
                    error = false,
                    code = 200,
                    values = result,
                    message = MessageService.GetDataSuccess
                });
            }
            catch (System.Exception __ex)
            {
                return Conflict(new BaseResponse { code = 500, error = true, message = __ex.Message });
            }
        }
    }
}
