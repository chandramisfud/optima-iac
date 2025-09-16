using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
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
        [HttpGet("api/promo/creation", Name = "promo_creation_get_lp")]
        public async Task<IActionResult> GetPromoCreationLandingPage([FromQuery] promoCreationLPParam param)
        {
            IActionResult result;
            try
            {
                string tokenHeader = Request.Headers["Authorization"]!;
                tokenHeader = tokenHeader.Replace("Bearer ", "");
                var __res = TokenManager.GetClaim(__TokenSecret!, tokenHeader);
                if (__res.ProfileID != null)
                {
                    var __val = await _repoPromoCreation.GetPromoCreationLandingPage(param.year!, param.entity, param.distributor,
                        param.category, __res.ProfileID,
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
                return Conflict(new Model.BaseResponse
                {
                    error = true,
                    code = 500,
                    message = __ex.Message
                });
            }
            return result;
        }

        /// <summary>
        /// Get Promo Creation Listing for Download
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpGet("api/promo/creation/download", Name = "promo_creation_get_download")]
        public async Task<IActionResult> GetPromoCreationDownload([FromQuery] promoCreationDownloadParam param)
        {
            IActionResult result;
            try
            {
                string tokenHeader = Request.Headers["Authorization"]!;
                tokenHeader = tokenHeader.Replace("Bearer ", "");
                var __res = TokenManager.GetClaim(__TokenSecret, tokenHeader);
                if (__res.ProfileID != null)
                {
                    var __val = await _repoPromoCreation.GetPromoCreationDownload(param.year!, param.entity,
                        param.distributor, param.category, __res.ProfileID);
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

        /// <summary>
        /// Get Promo Creation Source Planning
        /// </summary>
        /// <param name="period"></param>
        /// <param name="entity"></param>
        /// <param name="distributor"></param>

        /// <returns></returns>
        [HttpGet("api/promo/creation/sourceplanning", Name = "promo_creation_get_source_planning")]
        public async Task<IActionResult> GetPromoCreationSourcePlanning([FromQuery] string period, int entity, int distributor)
        {
            IActionResult result;
            try
            {
                string tokenHeader = Request.Headers["Authorization"]!;
                tokenHeader = tokenHeader.Replace("Bearer ", "");
                var __res = TokenManager.GetClaim(__TokenSecret, tokenHeader);
                if (__res.ProfileID != null)
                {
                    var __val = await _repoPromoCreation.GetPromoCreationSourcePlanning(period, entity, distributor, __res.ProfileID);
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

        /// <summary>
        /// Get Promo Creation Source Budget
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpGet("api/promo/creation/sourcebudget", Name = "promo_creation_get_source_budget")]
        public async Task<IActionResult> GetPromoCreationSourceBudget([FromQuery] SourceBudgetParam param)
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
                return Ok(new Model.BaseResponse
                {
                    error = true,
                    code = 500,
                    message = __ex.Message
                });
            }
            return result;
        }

        /// <summary>
        /// Get Attribute Promo Creation Channel for Add Promo Creation
        /// </summary>
        /// <returns></returns>
        [HttpGet("api/promo/creation/channel", Name = "promo_creation_channel")]
        public async Task<IActionResult> GetPromoCreationChannel()
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
                return Ok(new Model.BaseResponse
                {
                    error = true,
                    code = 500,
                    message = __ex.Message
                });
            }
        }

        /// <summary>
        /// Get Attribute Promo Creation Sub Channel for Add Promo Creation
        /// </summary>
        /// <param name="arrayChannel"></param>
        /// <returns></returns>
        [HttpGet("api/promo/creation/subchannel", Name = "promo_creation_subchannel")]
        public async Task<IActionResult> GetPromoCreationSubChannel([FromQuery] int[] arrayChannel)
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
                return Ok(new Model.BaseResponse
                {
                    error = true,
                    code = 500,
                    message = __ex.Message
                });
            }
        }

        /// <summary>
        /// Get Attribute Promo Creation Account for Add Promo Creation
        /// </summary>
        /// <param name="arraySubChannel"></param>
        /// <returns></returns>
        [HttpGet("api/promo/creation/account", Name = "promo_creation_account")]
        public async Task<IActionResult> GetPromoCreationAccount([FromQuery] int[] arraySubChannel)
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
                return Ok(new Model.BaseResponse
                {
                    error = true,
                    code = 500,
                    message = __ex.Message
                });
            }
        }

        /// <summary>
        /// Get Attribute Promo Creation Sub Account for Add Promo Creation
        /// </summary>
        /// <param name="arrayAccount"></param>
        /// <returns></returns>
        [HttpGet("api/promo/creation/subaccount", Name = "promo_creation_subaccount")]
        public async Task<IActionResult> GetPromoCreationSubAccount([FromQuery] int[] arrayAccount)
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
                return Ok(new Model.BaseResponse
                {
                    error = true,
                    code = 500,
                    message = __ex.Message
                });
            }
        }

        /// <summary>
        /// Get Attribute Promo Creation Channel for Edit Promo
        /// </summary>
        /// <param name="promoId"></param>
        /// <returns></returns>
        [HttpGet("api/promo/creation/channel/promoid", Name = "promo_creation_channel_promopid")]
        public async Task<IActionResult> GetPromoCreationChannelByPromoId([FromQuery] int promoId)
        {
            try
            {
                string tokenHeader = Request.Headers["Authorization"]!;
                tokenHeader = tokenHeader.Replace("Bearer ", "");
                var __res = TokenManager.GetClaim(__TokenSecret, tokenHeader);
                if (__res.ProfileID != null)
                {
                    var __val = await _repoPromoCreation.GetPromoCreationChannelByPromoId(promoId, __res.ProfileID);
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
                return Ok(new Model.BaseResponse
                {
                    error = true,
                    code = 500,
                    message = __ex.Message
                });
            }
        }

        /// <summary>
        /// Get Attribute Promo Creation Sub Channel for Edit Promo
        /// </summary>
        /// <param name="promoId"></param>
        /// <param name="arrayChannel"></param>
        /// <returns></returns>
        [HttpGet("api/promo/creation/subchannel/promoid", Name = "promo_creation_subchannel_promoid")]
        public async Task<IActionResult> GetPromoCreationSubChannelByPromoId([FromQuery] int promoId, [FromQuery] int[] arrayChannel)
        {
            try
            {
                string tokenHeader = Request.Headers["Authorization"]!;
                tokenHeader = tokenHeader.Replace("Bearer ", "");
                var __res = TokenManager.GetClaim(__TokenSecret, tokenHeader);
                if (__res.ProfileID != null)
                {
                    var __val = await _repoPromoCreation.GetPromoCreationSubChannelByPromoId(promoId, arrayChannel, __res.ProfileID);
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
                return Ok(new Model.BaseResponse
                {
                    error = true,
                    code = 500,
                    message = __ex.Message
                });
            }
        }

        /// <summary>
        /// Get Attribute Promo Creation Account for Edit Promo 
        /// </summary>
        /// <param name="promoId"></param>
        /// <param name="arraySubChannel"></param>
        /// <returns></returns>
        [HttpGet("api/promo/creation/account/promoid", Name = "promo_creation_account_promoid")]
        public async Task<IActionResult> GetPromoCreationAccountByPlanningId([FromQuery] int promoId, [FromQuery] int[] arraySubChannel)
        {
            try
            {
                string tokenHeader = Request.Headers["Authorization"]!;
                tokenHeader = tokenHeader.Replace("Bearer ", "");
                var __res = TokenManager.GetClaim(__TokenSecret, tokenHeader);
                if (__res.ProfileID != null)
                {
                    var __val = await _repoPromoCreation.GetPromoCreationAccountByPromoId(promoId, arraySubChannel, __res.ProfileID);
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
                return Ok(new Model.BaseResponse
                {
                    error = true,
                    code = 500,
                    message = __ex.Message
                });
            }
        }

        /// <summary>
        /// Get Attribute Promo Creation Sub Account for Edit Promo
        /// </summary>
        /// <param name="promoId"></param>
        /// <param name="arrayAccount"></param>
        /// <returns></returns>
        [HttpGet("api/promo/creation/subaccount/promoid", Name = "promo_creation_subaccount_promoid")]
        public async Task<IActionResult> GetPromoCreationSubAccountByPromoId([FromQuery] int promoId, [FromQuery] int[] arrayAccount)
        {
            try
            {
                string tokenHeader = Request.Headers["Authorization"]!;
                tokenHeader = tokenHeader.Replace("Bearer ", "");
                var __res = TokenManager.GetClaim(__TokenSecret, tokenHeader);
                if (__res.ProfileID != null)
                {
                    var __val = await _repoPromoCreation.GetPromoCreationSubAccountByPromoId(promoId, arrayAccount, __res.ProfileID);
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
                return Ok(new Model.BaseResponse
                {
                    error = true,
                    code = 500,
                    message = __ex.Message
                });

            }
        }

        /// <summary>
        /// Get Promo Creation SKP for Generate SKP Draft
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("api/promo/creation/skpdraft", Name = "promo_creation_get_skpdraf")]
        public async Task<IActionResult> GetPromoCreationSKPDraft([FromQuery] int id)
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
                return Ok(new Model.BaseResponse
                {
                    error = true,
                    code = 500,
                    message = __ex.Message
                });
            }
            return result;
        }

        /// <summary>
        /// Create Promo Creation
        /// </summary>
        /// <param name="promo"></param>
        /// <returns></returns>

        [HttpPost("api/promo/creation", Name = "promo_creation_store")]
        public async Task<IActionResult> PromoCreationCreate([FromBody] PromoParam promo)
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
                            mechanism = mechanism.mechanism,
                            notes = mechanism.notes,
                            productId = mechanism.productId,
                            product = mechanism.product,
                            brandId = mechanism.brandId,
                            brand = mechanism.brand
                        });
                    }

                    // Create List Attachment
                    List<Repositories.Entities.PromoAttachmentStore> __attachment = new();
                    foreach (var attachment in promo.promoAttachment!)
                    {
                        __attachment.Add(new Repositories.Entities.PromoAttachmentStore
                        {
                            DocLink = attachment.docLink,
                            FileName = attachment.fileName
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
                        promoAttachment = __attachment,
                        Mechanisms = __mechanism
                    };

                    var x = await _repoPromoCreation.PromoCreationCreate(promoCreationDto);

                    return Ok(new BaseResponse
                    {
                        code = 200,
                        error = false,
                        message = MessageService.SaveSuccess,
                        values = new
                        {
                            id = x.id,
                            refId = x.refId,
                            userid_approver = x.userid_approver,
                            username_approver = x.username_approver,
                            email_approver = x.email_approver,
                            userid_initiator = x.userid_initiator,
                            username_initiator = x.username_initiator,
                            email_initiator = x.email_initiator,
                            isFullyApproved = x.isFullyApproved
                        }
                    });
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
        }

        /// <summary>
        /// Create Promo Creation
        /// </summary>
        /// <param name="promo"></param>
        /// <returns></returns>

        [HttpPut("api/promo/creation", Name = "promo_creation_update")]
        public async Task<IActionResult> PromoCreationUpdate([FromBody] PromoParam promo)
        {
            DateTime utcTime = DateTime.UtcNow;
            var zone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
            if (!ModelState.IsValid) return BadRequest(ModelState);
            string tokenHeader = Request.Headers["Authorization"]!;
            tokenHeader = tokenHeader.Replace("Bearer ", "");

            var __res = TokenManager.GetClaim(__TokenSecret, tokenHeader);
            if (__res.UserEmail != null)
            {
                if (!ModelState.IsValid) return Conflict(ModelState);
                try
                {
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
                            mechanism = mechanism.mechanism,
                            notes = mechanism.notes,
                            productId = mechanism.productId,
                            product = mechanism.product,
                            brandId = mechanism.brandId,
                            brand = mechanism.brand
                        });
                    }

                    // Create List Attachment
                    List<Repositories.Entities.PromoAttachmentStore> __attachment = new();
                    foreach (var attachment in promo.promoAttachment!)
                    {
                        __attachment.Add(new Repositories.Entities.PromoAttachmentStore
                        {
                            DocLink = attachment.docLink,
                            FileName = attachment.fileName
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
                        promoAttachment = __attachment,
                        Mechanisms = __mechanism
                    };

                    var x = await _repoPromoCreation.PromoCreationUpdate(promoCreationDto);

                    return Ok(new BaseResponse
                    {
                        code = 200,
                        error = false,
                        message = MessageService.SaveSuccess,
                        values = new
                        {
                            id = x.id,
                            refId = x.refId,
                            userid_approver = x.userid_approver,
                            username_approver = x.username_approver,
                            email_approver = x.email_approver,
                            userid_initiator = x.userid_initiator,
                            username_initiator = x.username_initiator,
                            email_initiator = x.email_initiator,
                            isFullyApproved = x.isFullyApproved
                        }
                    });
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
            }
            else
            {
                return NotFound(new BaseResponse { error = true, code = 404, message = MessageService.EmailTokenFailed });
            }
        }

        /// <summary>
        /// Get data promo creation by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("api/promo/creation/id", Name = "promo_creation_getbyId")]
        public async Task<IActionResult> GetPromoCreationById(int id)
        {
            try
            {
                var __res = await _repoPromoCreation.GetPromoCreationById(id);
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
                return Ok(new Model.BaseResponse
                {
                    error = true,
                    code = 500,
                    message = __ex.Message
                });
            }
        }


        [HttpPost("api/promo/creation/attachment", Name = "promo_creation_attachment")]
        public async Task<IActionResult> SavePromoAttachment([FromBody] PromoAttachmentParam param)
        {
            string tokenHeader = Request.Headers["Authorization"]!;
            tokenHeader = tokenHeader.Replace("Bearer ", "");

            var __res = TokenManager.GetClaim(__TokenSecret, tokenHeader);
            if (__res.UserEmail != null)
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);
                var res = await _repoPromoCreation.PromoCreationAttachment(param.promoId, param.docLink!,
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

        [HttpDelete("api/promo/creation/attachment", Name = "promo_creation_attachment")]
        public async Task<IActionResult> DeletePromoAttachment([FromBody] PromoDeleteAttachmentParam param)
        {
            string tokenHeader = Request.Headers["Authorization"]!;
            tokenHeader = tokenHeader.Replace("Bearer ", "");

            var __res = TokenManager.GetClaim(__TokenSecret, tokenHeader);
            if (__res.UserEmail != null)
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);
                var res = await _repoPromoCreation.PromoDeleteAttachment(param.promoId, param.docLink!);
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
        /// <summary>
        /// Get Promo Exist Account = Sub Account
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpGet("api/promo/creation/exist", Name = "promo_creation_get_exist")]
        public async Task<IActionResult> GetPromoExist([FromQuery] PromoExistParam param)
        {
            IActionResult result;
            try
            {
                var __val = await _repoPromoCreation.GetPromoExist(param.period!, param.activityDesc!, param.arrayChannel!, param.arrayAccount!, param.startPromo!, param.endPromo!);
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
                return Ok(new Model.BaseResponse
                {
                    error = true,
                    code = 500,
                    message = __ex.Message
                });
            }
            return result;
        }
        /// <summary>
        /// Get Promo Exist For DC SN#125
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpGet("api/promo/creation/existdc", Name = "promo_creation_get_exist_dc")]
        public async Task<IActionResult> GetPromoExistDC([FromQuery] PromoExistDCParam param)
        {
            IActionResult result;
            try
            {
                var __val = await _repoPromoCreation.GetPromoExistDC(param.period!, param.distributorId, param.subActivityId,
                    param.subActivityTypeId, param.startPromo!, param.endPromo!);
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
                return Ok(new Model.BaseResponse
                {
                    error = true,
                    code = 500,
                    message = __ex.Message
                });
            }
            return result;
        }

        /// <summary>
        /// Get Late Promo days
        /// </summary>
        /// <returns></returns>
        [HttpGet("api/promo/latepromodays", Name = "promo_get_latepromodays")]
        public async Task<IActionResult> GetLatePromoDays()
        {
            IActionResult result;
            try
            {
                var __val = await _repoPromoCreation.GetLatePromoDays();
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
                return Ok(new Model.BaseResponse
                {
                    error = true,
                    code = 500,
                    message = __ex.Message
                });
            }
            return result;
        }

        /// <summary>
        /// Promo Creation Cancel REquest
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost("api/promo/creation/cancelrequest", Name = "promo_creation_cancelreq")]
        public async Task<IActionResult> PromoCancelRequest([FromBody] PromoCancelReqParam param)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);
                string tokenHeader = Request.Headers["Authorization"]!;
                tokenHeader = tokenHeader.Replace("Bearer ", "");

                var __res = TokenManager.GetClaim(__TokenSecret, tokenHeader);
                if (__res.UserEmail != null)
                {
                    if (!ModelState.IsValid) return Conflict(ModelState);

                    var x = await _repoPromoCreation.PromoCancelRequest(param.promoId, __res.ProfileID!, param.notes!, __res.UserEmail);

                    return Ok(new BaseResponse
                    {
                        code = 200,
                        error = false,
                        message = MessageService.SaveSuccess,
                        values = new
                        {
                            id = x.id,
                            refId = x.refId,
                            userid_approver = x.userid_approver,
                            email_approver = x.email_approver
                        }
                    });
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
        }

        /// <summary>
        /// Get Cancel Reason
        /// </summary>
        /// <returns></returns>
        [HttpGet("api/promo/cancelreason", Name = "promo_get_cancelreason")]
        public async Task<IActionResult> GetCancelReason()
        {
            IActionResult result;
            try
            {
                var __val = await _repoPromoCreation.GetCancelReason();
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
                return Conflict(new Model.BaseResponse { error = true, code = 500, message = __ex.Message });
            }
            return result;
        }

        /// <summary>
        /// Get group brand
        /// </summary>
        /// <returns></returns>
        [HttpGet("api/promo/groupbrand/entityid", Name = "promo_groupbrand_entityid")]
        public async Task<IActionResult> GetGroupBrandByEntity(int entityid)
        {
            try
            {
                var __val = await _repoPromoCreation.GetGroupBrandByEntity(entityid);
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
                return Conflict(new Model.BaseResponse
                {
                    error = true,
                    code = 500,
                    message = __ex.Message
                });
            }
        }

        /// <summary>
        /// Get brand-group by group id
        /// </summary>
        /// <returns></returns>
        [HttpGet("api/promo/brand/groupbrandid", Name = "promo_brand_groupbrandid")]
        public async Task<IActionResult> GetGroupBrand(int groupbrandid)
        {
            try
            {
                var __val = await _repoPromoCreation.GetBrandByGroupId(groupbrandid);
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
                return Conflict(new Model.BaseResponse
                {
                    error = true,
                    code = 500,
                    message = __ex.Message
                });
            }
        }
        /// <summary>
        /// Get SubCategory List by categoryId
        /// </summary>
        /// <returns></returns>
        [HttpGet("api/promo/subcategory/categoryid", Name = "promo_subcategory_categoryid")]
        public async Task<IActionResult> GetSubCategoryId(int CategoryId)
        {
            try
            {
                var __val = await _repoPromoCreation.GetSubCategoryId(CategoryId);
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
        /// <summary>
        /// Get Sub Category List base on CategoryId
        /// </summary>
        /// <returns></returns>
        [HttpGet("api/promo/subaccount/accountid", Name = "promo_subaccount_accountid")]
        public async Task<IActionResult> GetSubAccount([FromQuery] int[] AccountId)
        {
            try
            {
                var __val = await _repoPromoCreation.GetSubAccount(AccountId);
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
        /// <summary>
        /// Get Sub Channel List base on ChannelId
        /// </summary>
        /// <returns></returns>
        [HttpGet("api/promo/subchannel/channelid", Name = "promo_subchannel_channelId")]
        public async Task<IActionResult> GetSubChannel([FromQuery] int[] ChannelId)
        {
            try
            {
                var __val = await _repoPromoCreation.GetSubChannel(ChannelId);
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
        /// <summary>
        /// Get Account List base on subchannel Id
        /// </summary>
        /// <returns></returns>
        [HttpGet("api/promo/account/subchannelid", Name = "promo_account_subchannelid")]
        public async Task<IActionResult> GetAccount([FromQuery] int[] SubChannelId)
        {
            try
            {
                var __val = await _repoPromoCreation.GetAccount(SubChannelId);
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
        /// <summary>
        /// Get activity and subactivity List base on subcategory Id
        /// </summary>
        /// <returns></returns>
        [HttpGet("api/promo/activity/subactivity/subcategoryid", Name = "promo_account_subactivity_activity")]
        public async Task<IActionResult> GetActivityandSubActivityId([FromQuery] int subCategoryId)
        {
            try
            {
                var __val = await _repoPromoCreation.GetActivityandSubActivityId(subCategoryId);
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
        /// <summary>
        /// Get category list
        /// </summary>
        /// <returns></returns>
        [HttpGet("api/promo/category", Name = "promo_getcategory_for_promocreation")]
        public async Task<IActionResult> GetCategoryList()
        {
            try
            {
                var __val = await _repoPromoCreation.GetCategoryList();
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
        /// validate promo Mechanism 
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpGet("api/promo/mechanism/validate", Name = "get_promo_mechanism_validate")]
        public async Task<IActionResult> GetPromoMechanismValidate([FromQuery] ValidateMechanismParam param)
        {
            IActionResult result;
            try
            {
                    var __val = await _repoPromoCreation.GetPromoMechanismValidate(param.promoId, param.entityId, 
                        param.subCategoryId, param.activityId, param.subActivityId, param.skuId, param.channelId, 
                        param.startDate!, param.endDate!);
                    if (__val != null)
                    {
                    // Parse the JSON array
                 
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
                    return NotFound(new BaseResponse
                    {
                        error = true,
                        code = 404,
                        message = MessageService.GetDataFailed
                    });
                    }
            }
            catch (Exception __ex)
            {
                result = StatusCode(StatusCodes.Status500InternalServerError, new BaseResponse
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
