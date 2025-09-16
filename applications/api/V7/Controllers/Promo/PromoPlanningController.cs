using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using OfficeOpenXml.Configuration;
using Org.BouncyCastle.Utilities;
using Repositories.Contracts;
using Repositories.Entities;
using Repositories.Entities.BudgetAllocation;
using Repositories.Entities.Models;
using System.Collections.Immutable;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Security.Principal;
using System.Threading.Channels;
using System.Xml.Linq;
using V7.MessagingServices;
using V7.Model.Promo;
using V7.Services;

namespace V7.Controllers.Promo
{
    /// <summary>
    /// Document Completeness handler
    /// </summary>
    public partial class PromoController : BaseController
    {
        /// <summary>
        /// Get Promo Planning Listing with pagination
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpGet("api/promo/planning", Name = "promo_planning_get_lp")]
        public async Task<IActionResult> GetPromoPlanningLandingPage([FromQuery] promoPlanningLPParam param)
        {
            IActionResult result;
            try
            {
                string tokenHeader = Request.Headers["Authorization"]!;
                tokenHeader = tokenHeader.Replace("Bearer ", "");
                var __res = TokenManager.GetClaim(__TokenSecret, tokenHeader);
                if (__res.ProfileID != null)
                {
                    var __val = await _repoPromoPlanning.GetPromoPlanningLandingPage(param.year!, param.entity, param.distributor, param.createFrom!, param.createTo!, param.startFrom!, param.startTo!, __res.ProfileID,
                        String.IsNullOrEmpty(param.Search) ? "" : param.Search, param.SortColumn.ToString(), param.SortDirection.ToString(), param.PageNumber, param.PageSize);
                    if (__val != null)
                    {
                        result = Ok(new BaseResponse
                        {
                            error = false,
                            code = 200,
                            values = __val,
                            message = MessageService.GetDataSuccess
                        });
                    }
                    else
                    {
                        return NotFound(new BaseResponse { error = true, code = 404, message = MessageService.GetDataFailed });
                    }
                }
                else
                {
                    return NotFound(new BaseResponse { error = true, code = 404, message = MessageService.EmailTokenFailed });
                }
            }
            catch (Exception __ex)
            {
                result = StatusCode(StatusCodes.Status500InternalServerError, new BaseResponse { error = true, code = 500, message = __ex.Message });
            }
            return result;
        }

        /// <summary>
        /// Get Distributor
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpGet("api/promo/distributor", Name = "promo_planning_distributor")]
        public async Task<IActionResult> GetPromoPlanningDistributor([FromQuery] DistributorListParam param)
        {
            try
            {
                var __val = await _repoPromoPlanning.GetDistributorList(param.budgetId, param.entityId!);
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
                    return Conflict(new BaseResponse { code = 404, error = true, message = MessageService.GetDataFailed });
                }
            }
            catch (Exception __ex)
            {
                return Conflict(new BaseResponse { code = 500, error = true, message = __ex.Message });
            }
        }

        /// <summary>
        /// Get Entity
        /// </summary>
        /// <returns></returns>
        [HttpGet("api/promo/entity", Name = "promo_planning_entity")]
        public async Task<IActionResult> GetBudgetHistoryEntity()
        {
            try
            {
                var __val = await _repoPromoPlanning.GetAllEntity();
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
                    return Conflict(new BaseResponse { code = 404, error = true, message = MessageService.GetDataFailed });
                }

            }
            catch (Exception __ex)
            {
                return Conflict(new BaseResponse { code = 500, error = true, message = __ex.Message });
            }
        }

        /// <summary>
        /// Create Promo Planning
        /// </summary>
        /// <param name="promoPlanning"></param>
        /// <returns></returns>

        [HttpPost("api/promo/planning", Name = "promop_lanning_store")]
        public async Task<IActionResult> PromoPlanningCreate([FromBody] PromoPlanningParam promoPlanning)
        {
            DateTime utcTime = DateTime.UtcNow;
            var zone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
            if (!ModelState.IsValid) return BadRequest(ModelState);
            string tokenHeader = Request.Headers["Authorization"]!;
            tokenHeader = tokenHeader.Replace("Bearer ", "");

            var __res = TokenManager.GetClaim(__TokenSecret, tokenHeader);
            try
            {
                if (__res.UserEmail != null)
                {
                    if (!ModelState.IsValid) return Conflict(ModelState);

                    // Create PromoPlanningHeader
                    Repositories.Entities.PromoPlanning __promoplanning = new()
                    {
                        promoPlanId = promoPlanning.PromoPlanningHeader!.promoPlanId,
                        periode = promoPlanning.PromoPlanningHeader.periode,
                        distributorId = promoPlanning.PromoPlanningHeader.distributorId,
                        entityId = promoPlanning.PromoPlanningHeader.entityId,
                        categoryShortDesc = promoPlanning.PromoPlanningHeader.categoryShortDesc,
                        principalShortDesc = promoPlanning.PromoPlanningHeader.principalShortDesc,
                        categoryId = promoPlanning.PromoPlanningHeader.categoryId,
                        subCategoryId = promoPlanning.PromoPlanningHeader.subCategoryId,
                        activityId = promoPlanning.PromoPlanningHeader.activityId,
                        subActivityId = promoPlanning.PromoPlanningHeader.subActivityId,
                        activityDesc = promoPlanning.PromoPlanningHeader.activityDesc,
                        startPromo = promoPlanning.PromoPlanningHeader.startPromo,
                        endPromo = promoPlanning.PromoPlanningHeader.endPromo,
                        mechanisme1 = promoPlanning.PromoPlanningHeader.mechanisme1,
                        mechanisme2 = promoPlanning.PromoPlanningHeader.mechanisme2,
                        mechanisme3 = promoPlanning.PromoPlanningHeader.mechanisme3,
                        mechanisme4 = promoPlanning.PromoPlanningHeader.mechanisme4,
                        investment = promoPlanning.PromoPlanningHeader.investment,
                        normalSales = promoPlanning.PromoPlanningHeader.normalSales,
                        incrSales = promoPlanning.PromoPlanningHeader.incrSales,
                        roi = promoPlanning.PromoPlanningHeader.roi,
                        costRatio = promoPlanning.PromoPlanningHeader.costRatio,
                        notes = promoPlanning.PromoPlanningHeader.notes,
                        initiator_notes = promoPlanning.PromoPlanningHeader.initiator_notes,
                        modifReason = promoPlanning.PromoPlanningHeader.modifReason,
                        createOn = TimeZoneInfo.ConvertTimeFromUtc(utcTime, zone),
                        createBy = __res.ProfileID,
                        createdEmail = __res.UserEmail
                    };

                    // Create List Region
                    List<Repositories.Entities.Region> __region = new();
                    foreach (var region in promoPlanning.Regions!)
                    {
                        __region.Add(new Repositories.Entities.Region { id = region.id });
                    }
                    // Create List Channel
                    List<Repositories.Entities.Channel> __channel = new();
                    foreach (var channel in promoPlanning.Channels!)
                    {
                        __channel.Add(new Repositories.Entities.Channel { id = channel.id });
                    }
                    // Create List SubChannel
                    List<Repositories.Entities.SubChannel> __subchannel = new();
                    foreach (var subchannel in promoPlanning.SubChannels!)
                    {
                        __subchannel.Add(new Repositories.Entities.SubChannel { id = subchannel.id });
                    }
                    // Create List Account
                    List<Repositories.Entities.Account> __account = new();
                    foreach (var account in promoPlanning.Accounts!)
                    {
                        __account.Add(new Repositories.Entities.Account { id = account.id });
                    }
                    // Create List SubAccount
                    List<Repositories.Entities.SubAccount> __subaccount = new();
                    foreach (var subaccount in promoPlanning.SubAccounts!)
                    {
                        __subaccount.Add(new Repositories.Entities.SubAccount { id = subaccount.id });
                    }
                    // Create List Brand
                    List<Repositories.Entities.Brand> __brand = new();
                    foreach (var brand in promoPlanning.Brands!)
                    {
                        __brand.Add(new Repositories.Entities.Brand { id = brand.id });
                    }
                    // Create List SKU
                    List<Repositories.Entities.Product> __sku = new();
                    foreach (var sku in promoPlanning.Skus!)
                    {
                        __sku.Add(new Repositories.Entities.Product { id = sku.id });
                    }
                    // Create List Mechanism
                    List<Repositories.Entities.MechanismType> __mechanism = new();
                    foreach (var mechanism in promoPlanning.Mechanisms!)
                    {
                        __mechanism.Add(new Repositories.Entities.MechanismType
                        {
                            id = mechanism.id,
                            mechanism = mechanism.mechanism,
                            notes = mechanism.notes,
                            productId = mechanism.productId,
                            product = mechanism.product,
                            brandId = mechanism.brandId,
                            brand = mechanism.brand
                        });
                    }

                    PromoPlanningDto promoPlanDto = new()
                    {
                        PromoPlanningHeader = __promoplanning,
                        Regions = __region,
                        Channels = __channel,
                        SubChannels = __subchannel,
                        Accounts = __account,
                        SubAccounts = __subaccount,
                        Brands = __brand,
                        Skus = __sku,
                        Mechanisms = __mechanism
                    };

                    var x = await _repoPromoPlanning.PromoPlanningCreate(promoPlanDto);

                    return Ok(new BaseResponse
                    {
                        code = 200,
                        error = false,
                        message = MessageService.SaveSuccess,
                        values = new
                        {
                            PromoPlanId = x.id,
                            RefId = x.refId
                        }
                    });
                }
                else
                {
                    return NotFound(new BaseResponse { error = true, code = 404, message = MessageService.EmailTokenFailed });
                }
            }
            catch (System.Exception __ex)
            {
                return Conflict(new BaseResponse { code = 500, error = true, message = __ex.Message });
            }
        }

        /// <summary>
        /// Update Promo Planning
        /// </summary>
        /// <param name="promo"></param>
        /// <returns></returns>

        [HttpPut("api/promo/planning", Name = "promop_lanning_update")]
        public async Task<IActionResult> PromoPlanningUpdate([FromBody] PromoPlanningParam promo)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            string tokenHeader = Request.Headers["Authorization"]!;
            tokenHeader = tokenHeader.Replace("Bearer ", "");

            try
            {
                DateTime utcTime = DateTime.UtcNow;
                var zone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
                var __res = TokenManager.GetClaim(__TokenSecret, tokenHeader);
                if (__res.UserEmail != null)
                {
                    if (!ModelState.IsValid) return Conflict(ModelState);

                    // Create PromoPlanningHeader
                    Repositories.Entities.PromoPlanning __promoplanning = new()
                    {
                        promoPlanId = promo.PromoPlanningHeader!.promoPlanId,
                        periode = promo.PromoPlanningHeader.periode,
                        distributorId = promo.PromoPlanningHeader.distributorId,
                        entityId = promo.PromoPlanningHeader.entityId,
                        categoryShortDesc = promo.PromoPlanningHeader.categoryShortDesc,
                        principalShortDesc = promo.PromoPlanningHeader.principalShortDesc,
                        categoryId = promo.PromoPlanningHeader.categoryId,
                        subCategoryId = promo.PromoPlanningHeader.subCategoryId,
                        activityId = promo.PromoPlanningHeader.activityId,
                        subActivityId = promo.PromoPlanningHeader.subActivityId,
                        activityDesc = promo.PromoPlanningHeader.activityDesc,
                        startPromo = promo.PromoPlanningHeader.startPromo,
                        endPromo = promo.PromoPlanningHeader.endPromo,
                        mechanisme1 = promo.PromoPlanningHeader.mechanisme1,
                        mechanisme2 = promo.PromoPlanningHeader.mechanisme2,
                        mechanisme3 = promo.PromoPlanningHeader.mechanisme3,
                        mechanisme4 = promo.PromoPlanningHeader.mechanisme4,
                        investment = promo.PromoPlanningHeader.investment,
                        normalSales = promo.PromoPlanningHeader.normalSales,
                        incrSales = promo.PromoPlanningHeader.incrSales,
                        roi = promo.PromoPlanningHeader.roi,
                        costRatio = promo.PromoPlanningHeader.costRatio,
                        notes = promo.PromoPlanningHeader.notes,
                        initiator_notes = promo.PromoPlanningHeader.initiator_notes,
                        modifReason = promo.PromoPlanningHeader.modifReason,
                        createOn = TimeZoneInfo.ConvertTimeFromUtc(utcTime, zone),
                        createBy = __res.ProfileID,
                        createdEmail = __res.UserEmail
                    };

                    // Create List Region
                    List<Repositories.Entities.Region> __region = new();
                    foreach (var region in promo.Regions!)
                    {
                        __region.Add(new Repositories.Entities.Region { id = region.id });
                    }
                    // Create List Channel
                    List<Repositories.Entities.Channel> __channel = new();
                    foreach (var channel in promo.Channels!)
                    {
                        __channel.Add(new Repositories.Entities.Channel { id = channel.id });
                    }
                    // Create List SubChannel
                    List<Repositories.Entities.SubChannel> __subchannel = new();
                    foreach (var subchannel in promo.SubChannels!)
                    {
                        __subchannel.Add(new Repositories.Entities.SubChannel { id = subchannel.id });
                    }
                    // Create List Account
                    List<Repositories.Entities.Account> __account = new();
                    foreach (var account in promo.Accounts!)
                    {
                        __account.Add(new Repositories.Entities.Account { id = account.id });
                    }
                    // Create List SubAccount
                    List<Repositories.Entities.SubAccount> __subaccount = new();
                    foreach (var subaccount in promo.SubAccounts!)
                    {
                        __subaccount.Add(new Repositories.Entities.SubAccount { id = subaccount.id });
                    }
                    // Create List Brand
                    List<Repositories.Entities.Brand> __brand = new();
                    foreach (var brand in promo.Brands!)
                    {
                        __brand.Add(new Repositories.Entities.Brand { id = brand.id });
                    }
                    // Create List SKU
                    List<Repositories.Entities.Product> __sku = new();
                    foreach (var sku in promo.Skus!)
                    {
                        __sku.Add(new Repositories.Entities.Product { id = sku.id });
                    }
                    // Create List Mechanism
                    List<Repositories.Entities.MechanismType> __mechanism = new();
                    foreach (var mechanism in promo.Mechanisms!)
                    {
                        __mechanism.Add(new Repositories.Entities.MechanismType
                        {
                            id = mechanism.id,
                            mechanism = mechanism.mechanism,
                            notes = mechanism.notes,
                            productId = mechanism.productId,
                            product = mechanism.product,
                            brandId = mechanism.brandId,
                            brand = mechanism.brand
                        });
                    }

                    PromoPlanningDto promoPlanDto = new()
                    {
                        PromoPlanningHeader = __promoplanning,
                        Regions = __region,
                        Channels = __channel,
                        SubChannels = __subchannel,
                        Accounts = __account,
                        SubAccounts = __subaccount,
                        Brands = __brand,
                        Skus = __sku,
                        Mechanisms = __mechanism
                    };

                    var x = await _repoPromoPlanning.PromoPlanningUpdate(promoPlanDto);
                    return Ok(new BaseResponse
                    {
                        code = 200,
                        error = false,
                        message = MessageService.SaveSuccess,
                        values = new
                        {
                            PromoPlanId = x.id,
                            RefId = x.refId
                        }
                    });

                }
                else
                {
                    return NotFound(new BaseResponse { error = true, code = 404, message = MessageService.EmailTokenFailed });
                }
            }
            catch (System.Exception __ex)
            {
                return Conflict(new BaseResponse { code = 500, error = true, message = __ex.Message });
            }
        }

        /// <summary>
        /// Get Promo Planning Listing for Download
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpGet("api/promo/planning/download", Name = "promo_planning_get_download")]
        public async Task<IActionResult> GetPromoPlanningDownload([FromQuery] promoPlanningDownloadParam param)
        {
            IActionResult result;
            try
            {
                string tokenHeader = Request.Headers["Authorization"]!;
                tokenHeader = tokenHeader.Replace("Bearer ", "");
                var __res = TokenManager.GetClaim(__TokenSecret, tokenHeader);
                if (__res.ProfileID != null)
                {
                    var __val = await _repoPromoPlanning.GetPromoPlanningDownload(param.year!, param.entity, param.distributor, param.createFrom!, param.createTo!, param.startFrom!, param.startTo!, __res.ProfileID);
                    if (__val != null)
                    {
                        result = Ok(new BaseResponse
                        {
                            error = false,
                            code = 200,
                            values = __val,
                            message = MessageService.GetDataSuccess
                        });
                    }
                    else
                    {
                        return NotFound(new BaseResponse { error = true, code = 404, message = MessageService.GetDataFailed });
                    }
                }
                else
                {
                    return NotFound(new BaseResponse { error = true, code = 404, message = MessageService.EmailTokenFailed });
                }
            }
            catch (Exception __ex)
            {
                result = StatusCode(StatusCodes.Status500InternalServerError, new BaseResponse { error = true, code = 500, message = __ex.Message });
            }
            return result;
        }

        /// <summary>
        /// Get data promo planning by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("api/promo/planning/id", Name = "promo_planning_getbyId")]
        public async Task<IActionResult> GetPromoPlanningById(int id)
        {
            try
            {
                var __res = await _repoPromoPlanning.GetPromoPlanningById(id);
                if (__res == null)
                {
                    return NotFound(
                        new BaseResponse
                        {
                            error = true,
                            code = 404,
                            message = MessagingServices.MessageService.GetDataFailed
                        }
                    );
                }
                else
                {
                    return Ok(new BaseResponse
                    {
                        error = false,
                        code = 200,
                        values = __res,
                        message = MessagingServices.MessageService.GetDataSuccess
                    });
                }
            }
            catch (Exception __ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new BaseResponse
                {
                    error = true,
                    code = 500,
                    message = __ex.Message
                });
            }
        }

        /// <summary>
        /// Get Attribute Promo
        /// &lt;br /&gt;
        /// isDeleted = All (All Data) ; 0 (data active) ; 1 (data inactive/isdeleted)
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpGet("api/promo/attribute", Name = "promo_attribute")]
        public async Task<IActionResult> GetPromoAttribute([FromQuery] AttributeParam param)
        {
            try
            {

                //if (!ModelState.IsValid) return Conflict(ModelState);
                var __val = await _repoPromoPlanning.GetAttributeByParent(param.budgetId, param.attribute!, param.arrayParent!, param.isDeleted!);
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
                    return Conflict(new BaseResponse { code = 404, error = true, message = MessageService.GetDataFailed });
                }

            }
            catch (Exception __ex)
            {
                return Conflict(new BaseResponse { code = 500, error = true, message = __ex.Message });

            }
        }

        /// <summary>
        /// Get Sub Category Active for Promo after cut off
        /// </summary>
        /// <returns></returns>
        [HttpGet("api/promo/subcategory", Name = "promo_subcategory")]
        public async Task<IActionResult> GetSubCategory()
        {
            try
            {
                var __val = await _repoPromoPlanning.GetSubCategory();
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
                    return Conflict(new BaseResponse { code = 404, error = true, message = MessageService.GetDataFailed });
                }

            }
            catch (Exception __ex)
            {
                return Conflict(new BaseResponse { code = 500, error = true, message = __ex.Message });

            }
        }

        /// <summary>
        /// Get Promo Mechanism
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpGet("api/promo/mechanism", Name = "promo_get_mechanism")]
        public async Task<IActionResult> GetPromoMechanism([FromQuery] MechanismSourceParam param)
        {
            IActionResult result;
            try
            {
                string tokenHeader = Request.Headers["Authorization"]!;
                tokenHeader = tokenHeader.Replace("Bearer ", "");
                var __res = TokenManager.GetClaim(__TokenSecret, tokenHeader);
                if (__res.ProfileID != null)
                {
                    var __val = await _repoPromoPlanning.GetPromoMechanism(param.entityId, param.subCategoryId, param.activityId, param.subActivityId, param.skuId, param.channelId, param.startDate!, param.endDate!);
                    if (__val != null)
                    {
                        result = Ok(new BaseResponse
                        {
                            error = false,
                            code = 200,
                            values = __val,
                            message = MessageService.GetDataSuccess
                        });
                    }
                    else
                    {
                        return NotFound(new BaseResponse { error = true, code = 404, message = MessageService.GetDataFailed });
                    }
                }
                else
                {
                    return NotFound(new BaseResponse { error = true, code = 404, message = MessageService.EmailTokenFailed });
                }
            }
            catch (Exception __ex)
            {
                result = StatusCode(StatusCodes.Status500InternalServerError, new BaseResponse { error = true, code = 500, message = __ex.Message });
            }
            return result;
        }

        /// <summary>
        /// Get Promo Attribute Region
        /// </summary>
        /// <returns></returns>
        [HttpGet("api/promo/attribute/region", Name = "promo_planning_attribute_region")]
        public async Task<IActionResult> GetPromoAttributeRegion()
        {
            try
            {
                var __val = await _repoPromoPlanning.GetPromoAttributeRegion();
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
                    return Conflict(new BaseResponse { code = 404, error = true, message = MessageService.GetDataFailed });
                }

            }
            catch (Exception __ex)
            {
                return Conflict(new BaseResponse { code = 500, error = true, message = __ex.Message });

            }
        }

        /// <summary>
        /// Get Attribute Promo Planning Channel for Add or Duplicate Promo Planning
        /// </summary>
        /// <returns></returns>
        [HttpGet("api/promo/planning/channel", Name = "promo_planning_channel")]
        public async Task<IActionResult> GetPromoPlanningChannel()
        {
            try
            {
                string tokenHeader = Request.Headers["Authorization"]!;
                tokenHeader = tokenHeader.Replace("Bearer ", "");
                var __res = TokenManager.GetClaim(__TokenSecret, tokenHeader);
                if (__res.ProfileID != null)
                {
                    var __val = await _repoPromoPlanning.GetPromoPlanningChannel(__res.ProfileID);
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
                        return NotFound(new BaseResponse { error = true, code = 404, message = MessageService.GetDataFailed });
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
        /// Get Attribute Promo Planning Channel for Edit Promo Planning
        /// </summary>
        /// <param name="promoPlanningId"></param>
        /// <returns></returns>
        [HttpGet("api/promo/planning/channel/promoplanningid", Name = "promo_planning_channel_promoplanningid")]
        public async Task<IActionResult> GetPromoPlanningChannelByPlanningId([FromQuery] int promoPlanningId)
        {
            try
            {
                string tokenHeader = Request.Headers["Authorization"]!;
                tokenHeader = tokenHeader.Replace("Bearer ", "");
                var __res = TokenManager.GetClaim(__TokenSecret, tokenHeader);
                if (__res.ProfileID != null)
                {
                    var __val = await _repoPromoPlanning.GetPromoPlanningChannelByPlanningId(promoPlanningId, __res.ProfileID);
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
                        return NotFound(new BaseResponse { error = true, code = 404, message = MessageService.GetDataFailed });
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
        /// Get Attribute Promo Planning Sub Channel for Add or Duplicate Promo Planning
        /// </summary>
        /// <param name="arrayChannel"></param>
        /// <returns></returns>
        [HttpGet("api/promo/planning/subchannel", Name = "promo_planning_subchannel")]
        public async Task<IActionResult> GetPromoPlanningSubChannel([FromQuery] int[] arrayChannel)
        {
            try
            {
                string tokenHeader = Request.Headers["Authorization"]!;
                tokenHeader = tokenHeader.Replace("Bearer ", "");
                var __res = TokenManager.GetClaim(__TokenSecret, tokenHeader);
                if (__res.ProfileID != null)
                {
                    var __val = await _repoPromoPlanning.GetPromoPlanningSubChannel(arrayChannel, __res.ProfileID);
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
                        return NotFound(new BaseResponse { error = true, code = 404, message = MessageService.GetDataFailed });
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
        /// Get Attribute Promo Planning Sub Channel for Edit Promo Planning
        /// </summary>
        /// <param name="promoPlanningId"></param>
        /// <param name="arrayChannel"></param>
        /// <returns></returns>
        [HttpGet("api/promo/planning/subchannel/promoplanningid", Name = "promo_planning_subchannel_promoplanningid")]
        public async Task<IActionResult> GetPromoPlanningSubChannelByPlanningId([FromQuery] int promoPlanningId, [FromQuery] int[] arrayChannel)
        {
            try
            {
                string tokenHeader = Request.Headers["Authorization"]!;
                tokenHeader = tokenHeader.Replace("Bearer ", "");
                var __res = TokenManager.GetClaim(__TokenSecret, tokenHeader);
                if (__res.ProfileID != null)
                {
                    var __val = await _repoPromoPlanning.GetPromoPlanningSubChannelByPlanningId(promoPlanningId, arrayChannel, __res.ProfileID);
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
                        return NotFound(new BaseResponse { error = true, code = 404, message = MessageService.GetDataFailed });
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
        /// Get Attribute Promo Planning Account for Add or Duplicate Promo Planning
        /// </summary>
        /// <param name="arraySubChannel"></param>
        /// <returns></returns>
        [HttpGet("api/promo/planning/account", Name = "promo_planning_account")]
        public async Task<IActionResult> GetPromoPlanningAccount([FromQuery] int[] arraySubChannel)
        {
            try
            {
                string tokenHeader = Request.Headers["Authorization"]!;
                tokenHeader = tokenHeader.Replace("Bearer ", "");
                var __res = TokenManager.GetClaim(__TokenSecret, tokenHeader);
                if (__res.ProfileID != null)
                {
                    var __val = await _repoPromoPlanning.GetPromoPlanningAccount(arraySubChannel, __res.ProfileID);
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
                        return NotFound(new BaseResponse { error = true, code = 404, message = MessageService.GetDataFailed });
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
        /// Get Attribute Promo Planning Account for Edit Promo Planning
        /// </summary>
        /// <param name="promoPlanningId"></param>
        /// <param name="arraySubChannel"></param>
        /// <returns></returns>
        [HttpGet("api/promo/planning/account/promoplanningid", Name = "promo_planning_account_promoplanningid")]
        public async Task<IActionResult> GetPromoPlanningAccountByPlanningId([FromQuery] int promoPlanningId, [FromQuery] int[] arraySubChannel)
        {
            try
            {
                string tokenHeader = Request.Headers["Authorization"]!;
                tokenHeader = tokenHeader.Replace("Bearer ", "");
                var __res = TokenManager.GetClaim(__TokenSecret, tokenHeader);
                if (__res.ProfileID != null)
                {
                    var __val = await _repoPromoPlanning.GetPromoPlanningAccountByPlanningId(promoPlanningId, arraySubChannel, __res.ProfileID);
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
                        return NotFound(new BaseResponse { error = true, code = 404, message = MessageService.GetDataFailed });
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
        /// Get Attribute Promo Planning Sub Account for Add or Duplicate Promo Planning
        /// </summary>
        /// <param name="arrayAccount"></param>
        /// <returns></returns>
        [HttpGet("api/promo/planning/subaccount", Name = "promo_planning_subaccount")]
        public async Task<IActionResult> GetPromoPlanningSubAccount([FromQuery] int[] arrayAccount)
        {
            try
            {
                string tokenHeader = Request.Headers["Authorization"]!;
                tokenHeader = tokenHeader.Replace("Bearer ", "");
                var __res = TokenManager.GetClaim(__TokenSecret, tokenHeader);
                if (__res.ProfileID != null)
                {
                    var __val = await _repoPromoPlanning.GetPromoPlanningSubAccount(arrayAccount, __res.ProfileID);
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
                        return NotFound(new BaseResponse { error = true, code = 404, message = MessageService.GetDataFailed });
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
        /// Get Attribute Promo Planning Sub Account for Edit Promo Planning
        /// </summary>
        /// <param name="promoPlanningId"></param>
        /// <param name="arrayAccount"></param>
        /// <returns></returns>
        [HttpGet("api/promo/planning/subaccount/promoplanningid", Name = "promo_planning_subaccount_promoplanningid")]
        public async Task<IActionResult> GetPromoPlanningSubAccountByPlanningId([FromQuery] int promoPlanningId, [FromQuery] int[] arrayAccount)
        {
            try
            {
                string tokenHeader = Request.Headers["Authorization"]!;
                tokenHeader = tokenHeader.Replace("Bearer ", "");
                var __res = TokenManager.GetClaim(__TokenSecret, tokenHeader);
                if (__res.ProfileID != null)
                {
                    var __val = await _repoPromoPlanning.GetPromoPlanningSubAccountByPlanningId(promoPlanningId, arrayAccount, __res.ProfileID);
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
                        return NotFound(new BaseResponse { error = true, code = 404, message = MessageService.GetDataFailed });
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
        /// Get Promo Baseline => promoType : 1 -> planning/promo ; 2 -> promo recon
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost("api/promo/baseline", Name = "promo_get_baseline")]
        public async Task<IActionResult> GetBaselineSales([FromBody] BaselineParam param)
        {
            IActionResult result;
            try
            {
                var __val = await _repoPromoPlanning.GetBaselineSales(
                    param.promoId, param.period, param.dateCreation!, param.typePromo, param.subCategoryId, param.subActivityId, param.distributorId, param.startPromo!, param.endPromo!,
                    param.arrayRegion!, param.arrayChannel!, param.arraySubChannel!, param.arrayAccount!, param.arraySubAccount!, param.arrayBrand!, param.arraySKU!
                    );
                if (__val != null)
                {
                    result = Ok(new BaseResponse
                    {
                        error = false,
                        code = 200,
                        values = __val,
                        message = MessageService.GetDataSuccess
                    });
                }
                else
                {
                    return NotFound(new BaseResponse { error = true, code = 404, message = MessageService.GetDataFailed });
                }
            }
            catch (Exception __ex)
            {
                result = StatusCode(StatusCodes.Status500InternalServerError, new BaseResponse { error = true, code = 500, message = __ex.Message });
            }
            return result;
        }

        /// <summary>
        /// Get Promo Configuretion ROI and Cost Ratio
        /// </summary>
        /// <param name="subActivityId"></param>
        /// <returns></returns>
        [HttpGet("api/promo/config-roi-cr", Name = "promo_get_config_roicr")]
        public async Task<IActionResult> GetPromoConfigROICR([FromQuery] int subActivityId)
        {
            IActionResult result;
            try
            {
                var __val = await _repoPromoPlanning.GetPromoConfigROICR(subActivityId);
                if (__val != null)
                {
                    result = Ok(new BaseResponse
                    {
                        error = false,
                        code = 200,
                        values = __val,
                        message = MessageService.GetDataSuccess
                    });
                }
                else
                {
                    return NotFound(new BaseResponse { error = true, code = 404, message = MessageService.GetDataFailed });
                }
            }
            catch (Exception __ex)
            {
                result = StatusCode(StatusCodes.Status500InternalServerError, new BaseResponse { error = true, code = 500, message = __ex.Message });
            }
            return result;
        }

        /// <summary>
        /// Get Promo Planning Exist Account = Sub Account
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpGet("api/promo/planning/exist", Name = "promo_planning_get_exist")]
        public async Task<IActionResult> GetPromoPlanningExist([FromQuery] PromoPlanningExistParam param)
        {
            IActionResult result;
            try
            {
                var __val = await _repoPromoPlanning.GetPromoPlanningExist(param.period!, param.activityDesc!, param.arrayChannel!, param.arrayAccount!, param.startPromo!, param.endPromo!);
                if (__val != null)
                {
                    result = Ok(new BaseResponse
                    {
                        error = false,
                        code = 200,
                        values = __val,
                        message = MessageService.GetDataSuccess
                    });
                }
                else
                {
                    return NotFound(new BaseResponse { error = true, code = 404, message = MessageService.GetDataFailed });
                }
            }
            catch (Exception __ex)
            {
                result = StatusCode(StatusCodes.Status500InternalServerError, new BaseResponse { error = true, code = 500, message = __ex.Message });
            }
            return result;
        }

        /// <summary>
        /// Get Promo Baseline => promoType : 1 -> planning/promo ; 2 -> promo recon
        /// </summary>
        /// <param name="subActivityId"></param>
        /// <returns></returns>
        [HttpGet("api/promo/investmenttype", Name = "promo_get_investmenttype")]
        public async Task<IActionResult> GetPromoPlanningInvestmentType([FromQuery] int subActivityId)
        {
            IActionResult result;
            try
            {
                var __val = await _repoPromoPlanning.GetPromoPlanningInvestmentType(subActivityId);
                if (__val != null)
                {
                    result = Ok(new BaseResponse
                    {
                        error = false,
                        code = 200,
                        values = __val,
                        message = MessageService.GetDataSuccess
                    });
                }
                else
                {
                    return NotFound(new BaseResponse { error = true, code = 404, message = MessageService.GetDataFailed });
                }
            }
            catch (Exception __ex)
            {
                result = StatusCode(StatusCodes.Status500InternalServerError, new BaseResponse { error = true, code = 500, message = __ex.Message });
            }
            return result;
        }

        /// <summary>
        /// Promo Planning cancel
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost("api/promo/planning/cancel", Name = "promo_planning_cancel")]
        public async Task<IActionResult> PromoPlanningCancel([FromBody] PromoPlanningCancelParam param)
        {
            IActionResult result;
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);
                string tokenHeader = Request.Headers["Authorization"]!;
                tokenHeader = tokenHeader.Replace("Bearer ", "");

                var __res = TokenManager.GetClaim(__TokenSecret, tokenHeader);
                if (__res.UserEmail != null)
                {

                    PromoPlanningCancelDto promoPlanCancelDto = new()
                    {
                        promoPlanningId = param.promoPlanningId,
                        reason = param.reason,
                        profileId = __res.ProfileID
                    };

                    var x = await _repoPromoPlanning.PromoPlanningCancel(promoPlanCancelDto);

                    result = Ok(new BaseResponse
                    {
                        error = false,
                        code = 200,
                        values = x,
                        message = MessageService.GetDataSuccess
                    });
                }
                else
                {
                    return NotFound(new BaseResponse { error = true, code = 404, message = MessageService.EmailTokenFailed });
                }

            }
            catch (Exception __ex)
            {
                result = StatusCode(StatusCodes.Status500InternalServerError, new BaseResponse { error = true, code = 500, message = __ex.Message });
            }
            return result;
        }
        /// <summary>
        /// Get Planning Approval by Param, Old API = "api/promoplanning/byconditions/{year}/{entity}/{distributor}/{userid}/{create_from}/{create_to}/{start_from}/{start_to}"
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpGet("api/promo/planningapproval/promoplanning", Name = "promo_planning_planningapproval_promoplanning")]
        public async Task<IActionResult> GetPromoPlanningByConditions([FromQuery] PromoPlanningViewbyConditionsParam param)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);
                var __val = await _repoPromoPlanning.GetPromoPlanningByConditions
                    (
                        param.periode!,
                        param.entityId,
                        param.distributorId,
                        param.create_from!,
                        param.create_to!,
                        param.start_from!,
                        param.start_to!,
                        "0"
                    );
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
                    return NotFound(new BaseResponse { error = true, code = 404, message = MessageService.GetDataFailed });
                }
            }
            catch (Exception __ex)
            {
                return Conflict(new BaseResponse { code = 500, error = true, message = __ex.Message });
            }
        }
        /// <summary>
        /// Promo planning approval upload, Old API = "api/promoplan/approval"
        /// </summary>
        /// <param name="formFile"></param>
        /// <returns></returns>
        [HttpPost("api/promo/planningapproval", Name = "post_promo_planningapproval_upload_xls")]
        public async Task<IActionResult> DNUploadUpdateFP(IFormFile formFile)
        {
            IActionResult result;
            try
            {
                if (!Path.GetExtension(formFile.FileName).Equals(".xlsx", StringComparison.OrdinalIgnoreCase))
                {
                    return UnprocessableEntity(new { status_code = "422", message = "un supported extension" });
                }
                using var stream = new MemoryStream();
                await formFile.CopyToAsync(stream);
                using var package = new ExcelPackage(stream);
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                ExcelWorksheet ws = package.Workbook.Worksheets[0];

                var rowCount = ws.Dimension.Rows;
                DataTable tbl = new("PromoPlanningAprovalType");
                tbl.Columns.Add("PromoPlanRefId", typeof(string));
                tbl.Columns.Add("TSCode", typeof(string));

                for (int row = 2; row <= rowCount; row++)
                {
                    tbl.Rows.Add(
                        ws.Cells[row, 1].Value.ToString()!.Trim()
                        , ws.Cells[row, 2].Value ?? string.Empty
                    );
                }
                string tokenHeader = Request.Headers["Authorization"]!;
                tokenHeader = tokenHeader.Replace("Bearer ", "");
                var __res = TokenManager.GetClaim(__TokenSecret, tokenHeader);
                if (__res.ProfileID != null)
                {
                    promoPlanningUploadParam __bodytoken = new()
                    {
                        userId = __res.ProfileID,
                    };
                    PlanningApprovalResult? __val = await _repoPromoPlanning.PromoPlannningApproval(tbl, __bodytoken.userId);
                    return Ok(new
                    {
                        error = false,
                        code = 200,
                        message = MessageService.UploadSuccess,
                        values = __val
                    });
                }
                else
                {
                    return Conflict(new BaseResponse { error = true, code = 404, message = MessageService.UploadFailed });
                }
            }
            catch (Exception __ex)
            {
                result = StatusCode(StatusCodes.Status403Forbidden, 
                    new BaseResponse { error = true, code = 403, message = __ex.Message });
            }
            return result;
        }

        /// <summary>
        /// Get Promo Tobe Created with pagination
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <param name="search"></param>
        /// <returns></returns>
        [HttpGet("api/promo/tobecreated", Name = "promo_planning_get_promotobecreated")]
        public async Task<IActionResult> GetPromoTobeCreated([FromQuery] int pageNumber, [FromQuery] int pageSize, [FromQuery] string search)
        {
            IActionResult result;
            try
            {
                string tokenHeader = Request.Headers["Authorization"]!;
                tokenHeader = tokenHeader.Replace("Bearer ", "");
                var __res = TokenManager.GetClaim(__TokenSecret, tokenHeader);
                if (__res.ProfileID != null)
                {
                    var __val = await _repoPromoPlanning.GetPromoTobeCreated(__res.ProfileID, pageNumber, pageSize, String.IsNullOrEmpty(search) ? "" : search);
                    if (__val != null)
                    {
                        result = Ok(new BaseResponse
                        {
                            error = false,
                            code = 200,
                            values = __val,
                            message = MessageService.GetDataSuccess
                        });
                    }
                    else
                    {
                        return NotFound(new BaseResponse { error = true, code = 404, message = MessageService.GetDataFailed });
                    }
                }
                else
                {
                    return NotFound(new BaseResponse { error = true, code = 404, message = MessageService.EmailTokenFailed });
                }
            }
            catch (Exception __ex)
            {
                result = StatusCode(StatusCodes.Status500InternalServerError, new BaseResponse { error = true, code = 500, message = __ex.Message });
            }
            return result;
        }
    }
}
