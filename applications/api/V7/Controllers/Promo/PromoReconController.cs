using Microsoft.AspNetCore.Mvc;
using Repositories.Entities;
using Repositories.Entities.Models;
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
        /// Get Promo Creation Listing with pagination
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpGet("api/promo/recon", Name = "promo_recon_lp")]
        public async Task<IActionResult> GetPromoReconLandingPage([FromQuery] promoReconLPParam param)
        {
            IActionResult result;
            try
            {
                string tokenHeader = Request.Headers["Authorization"]!;
                tokenHeader = tokenHeader.Replace("Bearer ", "");
                var __res = TokenManager.GetClaim(__TokenSecret, tokenHeader);
                if (__res.ProfileID != null)
                {
                    var __val = await _repoPromoRecon.GetPromoReconLandingPage(param.year!, param.entity, param.distributor, __res.ProfileID,
                        param.categoryId, param.budgetParent, param.channel, param.promoStart, param.promoEnd,
                        String.IsNullOrEmpty(param.Search) ? "" : param.Search, param.PageNumber, param.PageSize);
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
        /// Get Promo Creation Listing for Download
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpGet("api/promo/recon/download", Name = "promo_recon_download")]
        public async Task<IActionResult> GetPromoReconDownload([FromQuery] promoReconDownloadParam param)
        {
            IActionResult result;
            try
            {
                string tokenHeader = Request.Headers["Authorization"]!;
                tokenHeader = tokenHeader.Replace("Bearer ", "");
                var __res = TokenManager.GetClaim(__TokenSecret, tokenHeader);
                if (__res.ProfileID != null)
                {
                    var __val = await _repoPromoRecon.GetPromoReconDownload(param.year!, param.promoStart, param.promoEnd, __res.ProfileID,
                        param.categoryId);
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
        /// Get Attribute Promo Creation Channel for Add Promo Creation
        /// </summary>
        /// <returns></returns>
        [HttpGet("api/promo/recon/channel", Name = "promo_recon_channel")]
        public async Task<IActionResult> GetPromoReconChannel()
        {
            try
            {
                string tokenHeader = Request.Headers["Authorization"]!;
                tokenHeader = tokenHeader.Replace("Bearer ", "");
                var __res = TokenManager.GetClaim(__TokenSecret, tokenHeader);
                if (__res.ProfileID != null)
                {
                    var __val = await _repoPromoCreation.GetPromoCreationChannel(__res.ProfileID);
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
        /// Get Attribute Promo Creation Sub Channel for Add Promo Creation
        /// </summary>
        /// <param name="arrayChannel"></param>
        /// <returns></returns>
        [HttpGet("api/promo/recon/subchannel", Name = "promo_recon_subchannel")]
        public async Task<IActionResult> GetPromoReconSubChannel([FromQuery] int[] arrayChannel)
        {
            try
            {
                string tokenHeader = Request.Headers["Authorization"]!;
                tokenHeader = tokenHeader.Replace("Bearer ", "");
                var __res = TokenManager.GetClaim(__TokenSecret, tokenHeader);
                if (__res.ProfileID != null)
                {
                    var __val = await _repoPromoCreation.GetPromoCreationSubChannel(arrayChannel, __res.ProfileID);
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
        /// Get Attribute Promo Creation Account for Add Promo Creation
        /// </summary>
        /// <param name="arraySubChannel"></param>
        /// <returns></returns>
        [HttpGet("api/promo/recon/account", Name = "promo_recon_account")]
        public async Task<IActionResult> GetPromoReconAccount([FromQuery] int[] arraySubChannel)
        {
            try
            {
                string tokenHeader = Request.Headers["Authorization"]!;
                tokenHeader = tokenHeader.Replace("Bearer ", "");
                var __res = TokenManager.GetClaim(__TokenSecret, tokenHeader);
                if (__res.ProfileID != null)
                {
                    var __val = await _repoPromoCreation.GetPromoCreationAccount(arraySubChannel, __res.ProfileID);
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
        /// Get Attribute Promo Creation Sub Account for Add Promo Creation
        /// </summary>
        /// <param name="arrayAccount"></param>
        /// <returns></returns>
        [HttpGet("api/promo/recon/subaccount", Name = "promo_recon_subaccount")]
        public async Task<IActionResult> GetPromoreconSubAccount([FromQuery] int[] arrayAccount)
        {
            try
            {
                string tokenHeader = Request.Headers["Authorization"]!;
                tokenHeader = tokenHeader.Replace("Bearer ", "");
                var __res = TokenManager.GetClaim(__TokenSecret, tokenHeader);
                if (__res.ProfileID != null)
                {
                    var __val = await _repoPromoCreation.GetPromoCreationSubAccount(arrayAccount, __res.ProfileID);
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
        /// Get Attribute Promo Creation Channel for Edit Promo
        /// </summary>
        /// <param name="promoId"></param>
        /// <returns></returns>
        [HttpGet("api/promo/recon/channel/promoid", Name = "promo_recon_channel_promoid")]
        public async Task<IActionResult> GetPromoreconChannelByPromoId([FromQuery] int promoId)
        {
            try
            {
                string tokenHeader = Request.Headers["Authorization"]!;
                tokenHeader = tokenHeader.Replace("Bearer ", "");
                var __res = TokenManager.GetClaim(__TokenSecret, tokenHeader);
                if (__res.ProfileID != null)
                {
                    var __val = await _repoPromoRecon.GetPromoReconChannelByPromoId(promoId, __res.ProfileID);
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
        /// Get Attribute Promo Creation Sub Channel for Edit Promo
        /// </summary>
        /// <param name="promoId"></param>
        /// <param name="arrayChannel"></param>
        /// <returns></returns>
        [HttpGet("api/promo/recon/subchannel/promoid", Name = "promo_recon_subchannel_promoid")]
        public async Task<IActionResult> GetPromoReconSubChannelByPromoId([FromQuery] int promoId, [FromQuery] int[] arrayChannel)
        {
            try
            {
                string tokenHeader = Request.Headers["Authorization"]!;
                tokenHeader = tokenHeader.Replace("Bearer ", "");
                var __res = TokenManager.GetClaim(__TokenSecret, tokenHeader);
                if (__res.ProfileID != null)
                {
                    var __val = await _repoPromoRecon.GetPromoReconSubChannelByPromoId(promoId, arrayChannel, __res.ProfileID);
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
        /// Get Attribute Promo Creation Account for Edit Promo 
        /// </summary>
        /// <param name="promoId"></param>
        /// <param name="arraySubChannel"></param>
        /// <returns></returns>
        [HttpGet("api/promo/recon/account/promoid", Name = "promo_recon_account_promoid")]
        public async Task<IActionResult> GetPromoReconAccountByPlanningId([FromQuery] int promoId, [FromQuery] int[] arraySubChannel)
        {
            try
            {
                string tokenHeader = Request.Headers["Authorization"]!;
                tokenHeader = tokenHeader.Replace("Bearer ", "");
                var __res = TokenManager.GetClaim(__TokenSecret, tokenHeader);
                if (__res.ProfileID != null)
                {
                    var __val = await _repoPromoRecon.GetPromoReconAccountByPromoId(promoId, arraySubChannel, __res.ProfileID);
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
        /// Get Attribute Promo Creation Sub Account for Edit Promo
        /// </summary>
        /// <param name="promoId"></param>
        /// <param name="arrayAccount"></param>
        /// <returns></returns>
        [HttpGet("api/promo/recon/subaccount/promoid", Name = "promo_recon_subaccount_promoid")]
        public async Task<IActionResult> GetPromoReconSubAccountByPromoId([FromQuery] int promoId, [FromQuery] int[] arrayAccount)
        {
            try
            {
                string tokenHeader = Request.Headers["Authorization"]!;
                tokenHeader = tokenHeader.Replace("Bearer ", "");
                var __res = TokenManager.GetClaim(__TokenSecret, tokenHeader);
                if (__res.ProfileID != null)
                {
                    var __val = await _repoPromoRecon.GetPromoReconSubAccountByPromoId(promoId, arrayAccount, __res.ProfileID);
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
        /// Get Promo Creation SKP for Generate SKP Draft
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("api/promo/recon/skpdraft", Name = "promo_recon_get_skpdraf")]
        public async Task<IActionResult> GetPromoReconSKPDraft([FromQuery] int id)
        {
            IActionResult result;
            try
            {
                string tokenHeader = Request.Headers["Authorization"]!;
                tokenHeader = tokenHeader.Replace("Bearer ", "");
                var __res = TokenManager.GetClaim(__TokenSecret, tokenHeader);
                if (__res.ProfileID != null)
                {
                    var __val = await _repoPromoCreation.GetPromoCreationSKPDraft(id);
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
        /// Update Promo Recon
        /// </summary>
        /// <param name="promo"></param>
        /// <returns></returns>

        [HttpPut("api/promo/recon", Name = "promo_recon_update")]
        public async Task<IActionResult> PromoReconUpdate([FromBody] PromoReconUpdateParam promo)
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
                        AllocationRefId = promo.PromoHeader.allocationRefId,
                        BudgetMasterId = promo.PromoHeader.budgetMasterId,
                        CategoryShortDesc = promo.PromoHeader.categoryShortDesc,
                        PrincipalShortDesc = promo.PromoHeader.principalShortDesc,
                        CategoryId = promo.PromoHeader.categoryId,
                        SubCategoryId = promo.PromoHeader.subCategoryId,
                        ActivityId = promo.PromoHeader.activityId,
                        SubActivityId = promo.PromoHeader.subActivityId,
                        ActivityDesc = promo.PromoHeader.activityDesc,
                        StartPromo = promo.PromoHeader.startPromo,
                        EndPromo = promo.PromoHeader.endPromo,
                        Mechanisme1 = promo.PromoHeader.mechanisme1,
                        Mechanisme2 = promo.PromoHeader.mechanisme2,
                        Mechanisme3 = promo.PromoHeader.mechanisme3,
                        Mechanisme4 = promo.PromoHeader.mechanisme4,
                        Investment = promo.PromoHeader.investment,
                        NormalSales = promo.PromoHeader.normalSales,
                        IncrSales = promo.PromoHeader.incrSales,
                        Roi = promo.PromoHeader.roi,
                        CostRatio = promo.PromoHeader.costRatio,
                        StatusApproval = promo.PromoHeader.statusApproval,
                        Notes = promo.PromoHeader.notes,
                        TsCoding = promo.PromoHeader.tsCoding,
                        initiator_notes = promo.PromoHeader.initiator_notes,
                        ModifReason = promo.PromoHeader.modifReason,
                        CreateOn = TimeZoneInfo.ConvertTimeFromUtc(utcTime, zone),
                        CreateBy = __res.ProfileID,
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
                            mechanism = mechanism.mechanism,
                            notes = mechanism.notes,
                            productId = mechanism.productId,
                            product = mechanism.product,
                            brandId = mechanism.brandId,
                            brand = mechanism.brand
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
                    return Ok(new BaseResponse
                    {
                        code = 200,
                        error = false,
                        message = MessageService.SaveSuccess,
                        values = _value
                    });
                }
                else
                {
                    return NotFound(new BaseResponse { error = true, code = 404, message = MessageService.EmailTokenFailed });
                }
            }
            catch (Exception __ex)
            {
                return Conflict(new BaseResponse { error = true, code = 500, message = __ex.Message });
            }
        }

        /// <summary>
        /// Get data promo creation by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("api/promo/recon/id", Name = "promo_recon_byId")]
        public async Task<IActionResult> GetPromoReconnById(int id)
        {
            try
            {
                var __res = await _repoPromoRecon.GetPromoReconById(id);
                if (__res == null)
                {
                    return Ok(
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


        [HttpPost("api/promo/recon/attachment", Name = "promo_recon_attachment")]
        public async Task<IActionResult> SavePromoReconAttachment([FromBody] PromoAttachmentParam param)
        {
            try
            {
                string tokenHeader = Request.Headers["Authorization"]!;
                tokenHeader = tokenHeader.Replace("Bearer ", "");

                var __res = TokenManager.GetClaim(__TokenSecret, tokenHeader);
                if (__res.UserEmail != null)
                {
                    if (!ModelState.IsValid) return BadRequest(ModelState);
                    var res = await _repoPromoRecon.PromoReconAttachment(param.promoId, param.docLink!,
                        param.fileName!, __res.ProfileID!);
                    if (res)
                        return Ok(new Model.BaseResponse { code = 200, error = false, message = MessageService.SaveSuccess });
                    else
                        return Ok(new Model.BaseResponse { code = 200, error = true, message = MessageService.SaveFailed });
                }
                else
                {
                    return NotFound(new BaseResponse { error = true, code = 404, message = MessageService.EmailTokenFailed });
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

        [HttpDelete("api/promo/recon/attachment", Name = "promo_recon_attachment")]
        public async Task<IActionResult> DeletePromoReconAttachment([FromBody] PromoDeleteAttachmentParam param)
        {
            try
            {
                string tokenHeader = Request.Headers["Authorization"]!;
                tokenHeader = tokenHeader.Replace("Bearer ", "");

                var __res = TokenManager.GetClaim(__TokenSecret, tokenHeader);
                if (__res.UserEmail != null)
                {
                    if (!ModelState.IsValid) return BadRequest(ModelState);
                    var res = await _repoPromoRecon.PromoReconDeleteAttachment(param.promoId, param.docLink!);
                    if (res)
                        return Ok(new Model.BaseResponse { code = 200, error = false, message = MessageService.DeleteSucceed });
                    else
                        return Ok(new Model.BaseResponse { code = 200, error = true, message = MessageService.DeleteFailed });
                }
                else
                {
                    return NotFound(new BaseResponse { error = true, code = 404, message = MessageService.EmailTokenFailed });
                }

            }
            catch (System.Exception __ex)
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
        [HttpGet("api/promo/recon/category", Name = "promo_recon_getcategory")]
        public async Task<IActionResult> GetCategoryListforPromoRecon()
        {
            try
            {
                var __val = await _repoPromoRecon.GetCategoryListforPromoRecon();
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
        /// Get mapping promo recon - subactivityId list, isDeleted = all semua data muncul, jika isDeleted = 0 maka hanya data yang active
        /// </summary>
        /// <param name="activityId"></param>
        /// <param name="isDeleted"></param>
        /// <returns></returns>
        [HttpGet("api/promo/recon/mapping/subactivity/activityid/join-result", Name = "promo_recon_get_mapping_subactivity_activityId")]
        public async Task<IActionResult> GetPromoReconSubActivitybyActivityId([FromQuery] int activityId, string isDeleted)
        {
            try
            {
                var __val = await _repoPromoRecon.GetPromoReconSubActivitybyActivityId(activityId, isDeleted);
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
        /// Get mapping promo recon - subactivityId list
        /// </summary>
        /// <param name="activityId"></param>
        /// <returns></returns>
        [HttpGet("api/promo/recon/mapping/subactivity/activityid", Name = "promosubactivity_activity")]
        public async Task<IActionResult> GetPromoSubactivity([FromQuery] string activityId)
        {
            try
            {
                var __val = await _repoPromoRecon.GetPromoSubactivity(activityId);
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
        /// Get Promo Recon Source Budget
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpGet("api/promo/recon/sourcebudget", Name = "promo_recon_get_source_budget")]
        public async Task<IActionResult> GetPromoReconSourceBudget([FromQuery] SourceBudgetParam param)
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
