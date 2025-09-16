using Microsoft.AspNetCore.Mvc;
using V7.MessagingServices;
using V7.Services;
using V7.Model.Promo;
using Repositories.Entities;
using V7.Model;

namespace V7.Controllers.Promo
{
    public partial class PromoController : BaseController
    {
        /// <summary>
        /// Get Promo Sendback for LP
        /// </summary>
        /// <param name="year"></param>
        /// <param name="entity"></param>
        /// <param name="distributor"></param>
        /// <param name="budgetparent"></param>
        /// <param name="channel"></param>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        [HttpGet("api/promo/sendback", Name = "promo_sendback_LP")]
        public async Task<IActionResult> GetPromoSendbackLP([FromQuery] string year, int entity, int distributor,
            int budgetparent, int channel, int categoryId=0)
        {
            try
            {
                string tokenHeader = Request.Headers["Authorization"]!;
                tokenHeader = tokenHeader.Replace("Bearer ", "");
                var __res = TokenManager.GetClaim(__TokenSecret, tokenHeader);
                if (__res.ProfileID != null)
                {
                    if (!ModelState.IsValid) return BadRequest(ModelState);
                    var __acc = await _repoPromoSendback.GetPromoSendbackLP(year, entity, distributor, budgetparent,
                        channel, __res.ProfileID, categoryId);
                    if (__acc != null)
                    {
                        return Ok(new Model.BaseResponse
                        {
                            error = false,
                            code = 200,
                            values = __acc,
                            message = MessagingServices.MessageService.GetDataSuccess
                        });
                    }
                    else
                    {
                        return Ok(new Model.BaseResponse { code = 404, error = true, message = MessageService.GetDataFailed });
                    }
                }
                else
                {
                    return NotFound(new Model.BaseResponse
                    {
                        error = true,
                        code = 404,
                        message = MessageService.EmailTokenFailed
                    });
                }
            }
            catch (Exception __ex)
            {
                return Conflict(new Model.BaseResponse { code = 500, error = true, message = __ex.Message });
            }
        }

        /// <summary>
        /// Get Entity List
        /// </summary>
        /// <returns></returns>
        [HttpGet("api/promo/sendback/entity", Name = "promo_sendback_entity")]
        public async Task<IActionResult> GetPromoSendbackEntity()
        {
            try
            {
                var __val = await _repoPromoSendback.GetAllEntity();
                if (__val != null)
                {
                    return Ok(new Model.BaseResponse
                    {
                        code = 200,
                        error = false,
                        message = MessageService.GetDataSuccess,
                        values = __val
                    });
                }
                else
                {
                    return Conflict(new Model.BaseResponse { code = 404, error = true, message = MessageService.GetDataFailed });
                }

            }
            catch (Exception __ex)
            {
                return Conflict(new Model.BaseResponse { code = 500, error = true, message = __ex.Message });

            }
        }

        /// <summary>
        /// Get distributor list
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpGet("api/promo/sendback/distributor", Name = "promo_sendback_distributor")]
        public async Task<IActionResult> GetPromoSendbackDistributor([FromQuery] DistributorListParam param)
        {
            try
            {
                var __val = await _repoPromoSendback.GetDistributorList(param.budgetId, param.entityId!);
                if (__val != null)
                {
                    return Ok(new Model.BaseResponse
                    {
                        code = 200,
                        error = false,
                        message = MessageService.GetDataSuccess,
                        values = __val
                    });
                }
                else
                {
                    return Conflict(new Model.BaseResponse { code = 404, error = true, message = MessageService.GetDataFailed });
                }

            }
            catch (Exception __ex)
            {
                return Conflict(new Model.BaseResponse { code = 500, error = true, message = __ex.Message });

            }
        }

        /// <summary>
        /// get promo sendback by ID                                                                                                                                                                        
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("api/promo/sendback/id", Name = "promo_sendback_by_id")]
        public async Task<IActionResult> GetPromoSendbackById(int id)
        {
            try
            {
                var __acc = await _repoPromoSendback.GetPromoSendbackById(id);
                if (__acc != null)
                {
                    return Ok(new Model.BaseResponse
                    {
                        error = false,
                        code = 200,
                        values = __acc,
                        message = MessageService.GetDataSuccess
                    });
                }
                else
                {
                    return Conflict(new Model.BaseResponse { code = 404, error = true, message = MessageService.GetDataFailed });
                }
            }
            catch (Exception __ex)
            {
                return Conflict(new Model.BaseResponse { code = 500, error = true, message = __ex.Message });
            }
        }

        /// <summary>
        /// Get list activity for promo sendback
        /// </summary>
        /// <param name="budgetid"></param>
        /// <param name="arrayParent"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        [HttpGet("api/promo/sendback/activity", Name = "promo_sendback_activity")]
        public async Task<IActionResult> GetActivityByCategory(int budgetid, int[] arrayParent, string status)
        {
            try
            {
                var activity = await _repoPromoSendback.GetAttributeByParent(budgetid, arrayParent, "activity", status);
                if (activity != null)
                {
                    return Ok(new Model.BaseResponse
                    {
                        error = false,
                        code = 200,
                        values = activity,
                        message = MessagingServices.MessageService.GetDataSuccess
                    });
                }
                else
                {
                    return Conflict(new Model.BaseResponse { code = 404, error = true, message = MessageService.GetDataFailed });
                }
            }
            catch (System.Exception __ex)
            {
                return Conflict(new Model.BaseResponse { code = 500, error = true, message = __ex.Message });
            }
        }

        /// <summary>
        /// Get list subactivity for promo sendback
        ///  </summary>
        /// <param name="budgetid"></param>
        /// <param name="arrayParent"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        [HttpGet("api/promo/sendback/subactivity", Name = "promo_sendback_subactivity")]
        public async Task<IActionResult> GetSubActivityByCategory(int budgetid, int[] arrayParent, string status)
        {
            try
            {
                var subactivity = await _repoPromoSendback.GetAttributeByParent(budgetid, arrayParent, "subactivity", status);
                if (subactivity != null)
                {
                    return Ok(new Model.BaseResponse
                    {
                        error = false,
                        code = 200,
                        values = subactivity,
                        message = MessagingServices.MessageService.GetDataSuccess
                    });
                }
                else
                {
                    return Conflict(new Model.BaseResponse { code = 404, error = true, message = MessageService.GetDataFailed });
                }
            }
            catch (System.Exception __ex)
            {
                return Conflict(new Model.BaseResponse { code = 500, error = true, message = __ex.Message });
            }
        }

        /// <summary>
        /// Get list channel by promo
        /// </summary>
        /// <param name="promoId"></param>
        /// <param name="arrayParent"></param>
        /// <returns></returns>
        [HttpGet("api/promo/sendback/channel", Name = "promo_sendback_channel")]
        public async Task<IActionResult> GetChannelByPromo(int promoId, int[] arrayParent)
        {
            try
            {
                string tokenHeader = Request.Headers["Authorization"]!;
                tokenHeader = tokenHeader.Replace("Bearer ", "");
                var __res = TokenManager.GetClaim(__TokenSecret, tokenHeader);
                if (__res.ProfileID != null)
                {
                    var activity = await _repoPromoSendback.GetAttributeByPromo(__res.ProfileID, arrayParent, "channel", promoId);
                    if (activity != null)
                    {
                        return Ok(new Model.BaseResponse
                        {
                            error = false,
                            code = 200,
                            values = activity,
                            message = MessagingServices.MessageService.GetDataSuccess
                        });
                    }
                    else
                    {
                        return Conflict(new Model.BaseResponse { code = 404, error = true, message = MessageService.GetDataFailed });
                    }
                }
                else
                {
                    return NotFound(new Model.BaseResponse
                    {
                        error = true,
                        code = 404,
                        message = MessageService.EmailTokenFailed
                    });
                }
            }
            catch (System.Exception __ex)
            {
                return Conflict(new Model.BaseResponse { code = 500, error = true, message = __ex.Message });
            }
        }

        /// <summary>
        /// Get list channel by promo
        /// </summary>
        /// <param name="promoId"></param>
        /// <param name="arrayParent"></param>
        /// <returns></returns>
        [HttpGet("api/promo/sendback/subchannel", Name = "promo_sendback_subchannel")]
        public async Task<IActionResult> GetSubChannelByPromo(int promoId, int[] arrayParent)
        {
            try
            {
                string tokenHeader = Request.Headers["Authorization"]!;
                tokenHeader = tokenHeader.Replace("Bearer ", "");
                var __res = TokenManager.GetClaim(__TokenSecret, tokenHeader);
                if (__res.ProfileID != null)
                {
                    var activity = await _repoPromoSendback.GetAttributeByPromo(__res.ProfileID, arrayParent, "subchannel", promoId);
                    if (activity != null)
                    {
                        return Ok(new Model.BaseResponse
                        {
                            error = false,
                            code = 200,
                            values = activity,
                            message = MessagingServices.MessageService.GetDataSuccess
                        });
                    }
                    else
                    {
                        return Conflict(new Model.BaseResponse { code = 404, error = true, message = MessageService.GetDataFailed });
                    }
                }
                else
                {
                    return NotFound(new Model.BaseResponse
                    {
                        error = true,
                        code = 404,
                        message = MessageService.EmailTokenFailed
                    });
                }
            }
            catch (System.Exception __ex)
            {
                return Conflict(new Model.BaseResponse { code = 500, error = true, message = __ex.Message });
            }
        }

        /// <summary>
        /// Get list account by promo
        /// </summary>
        /// <param name="promoId"></param>
        /// <param name="arrayParent"></param>
        /// <returns></returns>
        [HttpGet("api/promo/sendback/account", Name = "promo_sendback_account")]
        public async Task<IActionResult> GetAccountByPromo(int promoId, int[] arrayParent)
        {
            try
            {
                string tokenHeader = Request.Headers["Authorization"]!;
                tokenHeader = tokenHeader.Replace("Bearer ", "");
                var __res = TokenManager.GetClaim(__TokenSecret, tokenHeader);
                if (__res.ProfileID != null)
                {
                    var activity = await _repoPromoSendback.GetAttributeByPromo(__res.ProfileID, arrayParent, "account", promoId);
                    if (activity != null)
                    {
                        return Ok(new Model.BaseResponse
                        {
                            error = false,
                            code = 200,
                            values = activity,
                            message = MessagingServices.MessageService.GetDataSuccess
                        });
                    }
                    else
                    {
                        return Conflict(new Model.BaseResponse { code = 404, error = true, message = MessageService.GetDataFailed });
                    }
                }
                else
                {
                    return NotFound(new Model.BaseResponse
                    {
                        error = true,
                        code = 404,
                        message = MessageService.EmailTokenFailed
                    });
                }
            }
            catch (System.Exception __ex)
            {
                return Conflict(new Model.BaseResponse { code = 500, error = true, message = __ex.Message });
            }
        }

        /// <summary>
        /// Get list sub account by promo
        /// </summary>
        /// <param name="promoId"></param>
        /// <param name="arrayParent"></param>
        /// <returns></returns>
        [HttpGet("api/promo/sendback/subaccount", Name = "promo_sendback_subaccount")]
        public async Task<IActionResult> GetSubAccountByPromo(int promoId, int[] arrayParent)
        {
            try
            {
                string tokenHeader = Request.Headers["Authorization"]!;
                tokenHeader = tokenHeader.Replace("Bearer ", "");
                var __res = TokenManager.GetClaim(__TokenSecret, tokenHeader);
                if (__res.ProfileID != null)
                {
                    var activity = await _repoPromoSendback.GetAttributeByPromo(__res.ProfileID, arrayParent, "subaccount", promoId);
                    if (activity != null)
                    {
                        return Ok(new Model.BaseResponse
                        {
                            error = false,
                            code = 200,
                            values = activity,
                            message = MessagingServices.MessageService.GetDataSuccess
                        });
                    }
                    else
                    {
                        return Conflict(new Model.BaseResponse { code = 404, error = true, message = MessageService.GetDataFailed });
                    }
                }
                else
                {
                    return NotFound(new Model.BaseResponse
                    {
                        error = true,
                        code = 404,
                        message = MessageService.EmailTokenFailed
                    });
                }
            }
            catch (System.Exception __ex)
            {
                return Conflict(new Model.BaseResponse { code = 500, error = true, message = __ex.Message });
            }
        }

        /// <summary>
        /// Update promo sendback
        /// </summary>
        /// <param name="promo"></param>
        /// <returns></returns>
        [HttpPost("api/promo/sendback", Name = "promo_sendback")]
        public async Task<IActionResult> PromoSendback([FromBody] PromoParam promo)
        {
             DateTime utcTime = DateTime.UtcNow;
            var zone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);
                string tokenHeader = Request.Headers["Authorization"]!;
                tokenHeader = tokenHeader.Replace("Bearer ", "");

                var __res = TokenManager.GetClaim(__TokenSecret, tokenHeader);
                if (__res.UserEmail != null)
                {
                    if (!ModelState.IsValid) return Conflict(ModelState);

                    // Create PromoHeader
                    Repositories.Entities.Promo __promo = new()
                    {
                        promoId = promo.PromoHeader!.promoId,
                        promoPlanId = promo.PromoHeader.promoPlanId,
                        allocationId = promo.PromoHeader.allocationId,
                        allocationRefId = promo.PromoHeader.allocationRefId,
                        budgetMasterId = promo.PromoHeader.budgetMasterId,
                        categoryShortDesc = promo.PromoHeader.categoryShortDesc,
                        principalShortDesc = promo.PromoHeader.principalShortDesc,
                        categoryId = promo.PromoHeader.categoryId,
                        subCategoryId = promo.PromoHeader.subCategoryId,
                        activityId = promo.PromoHeader.activityId,
                        subActivityId = promo.PromoHeader.subActivityId,
                        activityDesc = promo.PromoHeader.activityDesc,
                        startPromo = promo.PromoHeader.startPromo,
                        endPromo = promo.PromoHeader.endPromo,
                        mechanisme1 = promo.PromoHeader.mechanisme1,
                        mechanisme2 = promo.PromoHeader.mechanisme2,
                        mechanisme3 = promo.PromoHeader.mechanisme3,
                        mechanisme4 = promo.PromoHeader.mechanisme4,
                        investment = promo.PromoHeader.investment,
                        normalSales = promo.PromoHeader.normalSales,
                        incrSales = promo.PromoHeader.incrSales,
                        roi = promo.PromoHeader.roi,
                        costRatio = promo.PromoHeader.costRatio,
                        statusApproval = promo.PromoHeader.statusApproval,
                        notes = promo.PromoHeader.notes,
                        tsCoding = promo.PromoHeader.tsCoding,
                        initiator_notes = promo.PromoHeader.initiator_notes,
                        modifReason = promo.PromoHeader.modifReason,
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
                            mechanism = mechanism.mechanism!,
                            notes = mechanism.notes!,
                            productId = mechanism.productId,
                            product = mechanism.product!,
                            brandId = mechanism.brandId,
                            brand = mechanism.brand!
                        });
                    }
                    List<Repositories.Entities.PromoAttachmentStore> __attachment = new();
                    foreach (var item in promo.promoAttachment!)
                    {
                        __attachment.Add(new Repositories.Entities.PromoAttachmentStore
                        {
                            DocLink = item.docLink,
                            FileName = item.fileName
                        });
                    }

                    PromoCreationDto promoCreationDto = new()
                    {
                        PromoHeader = __promo,
                        Regions = __region,
                        Channels = __channel,
                        SubChannels = __subchannel,
                        Accounts = __account,
                        SubAccounts = __subaccount,
                        Brands = __brand,
                        Skus = __sku,
                        Mechanisms = __mechanism,
                        promoAttachment = __attachment
                    };

                    // SP: ip_promo_v4_insert
                    var x = await _repoPromoSendback.PromoSendback(promoCreationDto);

                    return Ok(new Model.BaseResponse
                    {
                        code = 200,
                        error = false,
                        message = MessageService.SaveSuccess,
                        values = new PromoSendbackResponse
                        {
                            id = x.id,
                            refId = x.refId,
                            username_approver = x.username_approver,
                            email_approver = x.email_approver,
                            userid_approver = x.userid_approver,
                            IsFullyApproved = x.isFullyApproved
                        }
                    });
                }
                else
                {
                    return NotFound(new Model.BaseResponse { error = true, code = 404, message = MessageService.EmailTokenFailed });
                }
            }
            catch (Exception __ex)
            {
                return Conflict(new Model.BaseResponse
                {
                    code = 500,
                    error = true,
                    message = __ex.Message
                });
            }
        }
        /// <summary>
        /// Get category list
        /// </summary>
        /// <returns></returns>
        [HttpGet("api/promo/sendback/category", Name = "promo_getcategory_for_promosendback")]
        public async Task<IActionResult> GetCategoryListforPromoSendBack()
        {
            try
            {
                var __val = await _repoPromoSendback.GetCategoryListforPromoSendBack();
                if (__val != null)
                {
                    return Ok(new Model.BaseResponse
                    {
                        code = 200,
                        error = false,
                        message = MessageService.GetDataSuccess,
                        values = __val
                    });
                }
                else
                {
                    return Ok(new Model.BaseResponse { code = 404, error = true, message = MessageService.GetDataFailed });
                }
            }
            catch (Exception __ex)
            {
                return Conflict(new Model.BaseResponse { code = 500, error = true, message = __ex.Message });
            }
        }
         /// <summary>
        /// Get Promo Sendback Source Budget
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpGet("api/promo/sendback/sourcebudget", Name = "promo_sendback_get_source_budget")]
        public async Task<IActionResult> GetPromoSendbackSourceBudget([FromQuery] SourceBudgetParam param)
        {
            IActionResult result;
            try
            {
                string tokenHeader = Request.Headers["Authorization"]!;
                tokenHeader = tokenHeader.Replace("Bearer ", "");
                var __res = TokenManager.GetClaim(__TokenSecret, tokenHeader);
                if (__res.ProfileID != null)
                {

                    var __val = await _repoPromoSendback.GetPromoSendbackSourceBudget(
                    param.year!, param.entityId, param.distributorId, param.subCategoryId, param.activityId, param.subActivityId,
                    param.arrayRegion!, param.arrayChannel!, param.arraySubChannel!, param.arrayAccount!, param.arraySubAccount!, param.arrayBrand!, param.arraySKU!, __res.ProfileID
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
                else
                {
                    return NotFound(new BaseResponse { error = true, code = 404, message = MessageService.EmailTokenFailed });
                }
            }
            catch (Exception __ex)
            {
                return Ok(new Model.BaseResponse
                {
                    error = true,
                    code = 500,
                    message = __ex.Message
                });
            }
            return result;
        }
    }
}
