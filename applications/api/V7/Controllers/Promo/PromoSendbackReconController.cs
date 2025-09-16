using Microsoft.AspNetCore.Mvc;
using V7.MessagingServices;
using V7.Services;
using V7.Model.Promo;
using Repositories.Entities;

namespace V7.Controllers.Promo
{
    /// <summary>
    /// Promo sendback recon controller
    /// </summary>
    public partial class PromoController : BaseController
    {
        /// <summary>
        /// Get Promo sendback recon LP
        /// </summary>
        /// <param name="year"></param>
        /// <param name="entity"></param>
        /// <param name="distributor"></param>
        /// <param name="budgetparent"></param>
        /// <param name="channel"></param>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        [HttpGet("api/promo/sendbackrecon", Name = "promo_sendbackrecon_LP")]
        public async Task<IActionResult> GetPromoSendbackReconLP([FromQuery] string year, int entity, int distributor,
            int budgetparent, int channel, int categoryId = 0)
        {
            try
            {
                string tokenHeader = Request.Headers["Authorization"]!;
                tokenHeader = tokenHeader.Replace("Bearer ", "");
                var __res = TokenManager.GetClaim(__TokenSecret, tokenHeader);
                if (__res.ProfileID != null)
                {
                    if (!ModelState.IsValid) return BadRequest(ModelState);
                    var __acc = await _repoPromoSendback.GetPromoSendbackReconLP(year, entity, distributor, budgetparent,
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
        [HttpGet("api/promo/sendbackrecon/entity", Name = "promo_sendbackrecon_entity")]
        public async Task<IActionResult> GetPromoSendbackReconEntity()
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
        [HttpGet("api/promo/sendbackrecon/distributor", Name = "promo_sendbackrecon_distributor")]
        public async Task<IActionResult> GetPromoSendbackReconDistributor([FromQuery] DistributorListParam param)
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
        /// get promo sendback recon by ID, ex:3300                                                                                                                                                                       
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("api/promo/sendbackrecon/id", Name = "promo_sendbackrecon_by_id")]
        public async Task<IActionResult> GetPromoSendbackReconById(int id)
        {
            try
            {
                var __acc = await _repoPromoSendback.GetPromoSendbackReconById(id, "");
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
        /// Save Promo Recon Sendback
        /// </summary>
        /// <param name="promo"></param>
        /// <returns></returns>
        [HttpPost("api/promo/sendbackrecon", Name = "promo_sendback_recon")]
        public async Task<IActionResult> PromoSendbackReconUpdate([FromBody] PromoReconUpdateParam promo)
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
                    Repositories.Entities.PromoReconV4TypeDto __promo = new()
                    {
                        PromoId = promo.PromoHeader!.promoId,
                        PromoPlanId = promo.PromoHeader.promoPlanId,
                        AllocationId = promo.PromoHeader.allocationId,
                        AllocationRefId = promo.PromoHeader.allocationRefId!,
                        BudgetMasterId = promo.PromoHeader.budgetMasterId,
                        CategoryShortDesc = promo.PromoHeader.categoryShortDesc!,
                        PrincipalShortDesc = promo.PromoHeader.principalShortDesc!,
                        CategoryId = promo.PromoHeader.categoryId,
                        SubCategoryId = promo.PromoHeader.subCategoryId,
                        ActivityId = promo.PromoHeader.activityId,
                        SubActivityId = promo.PromoHeader.subActivityId,
                        ActivityDesc = promo.PromoHeader.activityDesc!,
                        StartPromo = promo.PromoHeader.startPromo,
                        EndPromo = promo.PromoHeader.endPromo,
                        Mechanisme1 = promo.PromoHeader.mechanisme1!,
                        Mechanisme2 = promo.PromoHeader.mechanisme2!,
                        Mechanisme3 = promo.PromoHeader.mechanisme3!,
                        Mechanisme4 = promo.PromoHeader.mechanisme4!,
                        Investment = promo.PromoHeader.investment,
                        NormalSales = promo.PromoHeader.normalSales,
                        IncrSales = promo.PromoHeader.incrSales,
                        Roi = promo.PromoHeader.roi,
                        CostRatio = promo.PromoHeader.costRatio,
                        StatusApproval = promo.PromoHeader.statusApproval!,
                        Notes = promo.PromoHeader.notes!,
                        TsCoding = promo.PromoHeader.tsCoding!,
                        initiator_notes = promo.PromoHeader.initiator_notes!,
                        ModifReason = promo.PromoHeader.modifReason!,
                        CreateOn = TimeZoneInfo.ConvertTimeFromUtc(utcTime, zone),
                        CreateBy = __res.ProfileID!,
                        CreatedEmail = __res.UserEmail
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

                    PromoReconCreationDto promoReconDto = new()
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
                        Reconciled = promo.Reconciled,
                        ReconciledUpd = promo.ReconciledUpd
                    };

                    var x = await _repoPromoRecon.PromoReconUpdate(promoReconDto);

                    var _value = new Model.Promo.PromoReconResponse
                    {
                        email_approver = x.email_approver,
                        email_initiator = x.email_initiator,
                        Id = x.id,
                        IsFullyApproved = x.isFullyApproved,
                        RefId = x.refId,
                        userid_approver = x.userid_approver,
                        userid_initiator = x.userid_initiator,
                        username_approver = x.username_approver,
                        username_initiator = x.username_initiator,
                        major_changes = x.major_changes
                    };
                    return Ok(new Model.BaseResponse
                    {
                        code = 200,
                        error = false,
                        message = MessageService.SaveSuccess,
                        values = _value
                    });
                }
                else
                {
                    return NotFound(new Model.BaseResponse { error = true, code = 404, message = MessageService.EmailTokenFailed });
                }
            }
            catch (Exception __ex)
            {
                return Ok(new Model.BaseResponse
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
        [HttpGet("api/promo/sendbackrecon/category", Name = "promo_getcategory_for_promosendbackrecon")]
        public async Task<IActionResult> GetCategoryListforPromoReconSendBack()
        {
            try
            {
                var __val = await _repoPromoSendback.GetCategoryListforPromoReconSendBack();
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
        /// Get Promo Sendback Recon Source Budget
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpGet("api/promo/sendbackrecon/sourcebudget", Name = "promo_sendbackrecon_get_source_budget")]
        public async Task<IActionResult> GetPromoSendbackReconSourceBudget([FromQuery] SourceBudgetParam param)
        {
            IActionResult result;
            try
            {
                string tokenHeader = Request.Headers["Authorization"]!;
                tokenHeader = tokenHeader.Replace("Bearer ", "");
                var __res = TokenManager.GetClaim(__TokenSecret, tokenHeader);
                if (__res.ProfileID != null)
                {

                    var __val = await _repoPromoCreation.GetPromoCreationSourceBudget(
                    param.year!, param.entityId, param.distributorId, param.subCategoryId, param.activityId, param.subActivityId,
                    param.arrayRegion!, param.arrayChannel!, param.arraySubChannel!, param.arrayAccount!, param.arraySubAccount!, param.arrayBrand!, param.arraySKU!, __res.ProfileID
                    );
                    if (__val != null)
                    {
                        result = Ok(new Model.BaseResponse
                        {
                            error = false,
                            code = 200,
                            values = __val,
                            message = MessageService.GetDataSuccess
                        });
                    }
                    else
                    {
                        return NotFound(new Model.BaseResponse { error = true, code = 404, message = MessageService.GetDataFailed });
                    }
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
                    error = true,
                    code = 500,
                    message = __ex.Message
                });
            }
            return result;
        }
    }
}
