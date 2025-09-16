using Microsoft.AspNetCore.Mvc;
using Repositories.Entities.Models.PromoApproval;
using V7.MessagingServices;
using V7.Model.PromoApproval;
using V7.Model.Promo;
using V7.Services;
using Microsoft.AspNetCore.Authorization;
namespace V7.Controllers.Promo
{
    public partial class PromoController : BaseController
    {
        /// <summary>
        /// Get Promo Approval for landing page
        /// </summary>
        /// <param name="year"></param>
        /// <param name="category"></param>
        /// <param name="entity"></param>
        /// <param name="distributor"></param>
        /// <param name="budgetparent"></param>
        /// <param name="channel"></param>
        /// <returns></returns>
        [HttpGet("api/promo/approval", Name = "promo_approval")]
        public async Task<IActionResult> GetPromoApprovalLP([FromQuery] string year, int category, int entity, int distributor,
            int budgetparent, int channel)
        {
            try
            {
                string tokenHeader = Request.Headers["Authorization"]!;
                tokenHeader = tokenHeader.Replace("Bearer ", "");
                var __res = TokenManager.GetClaim(__TokenSecret, tokenHeader);
                if (__res.ProfileID != null)
                {
                    if (!ModelState.IsValid) return BadRequest(ModelState);
                    var __acc = await _repoPromoApproval.GetPromoApprovalLP(year, category, entity, distributor, budgetparent, channel, __res.ProfileID);
                    if (__acc == null)
                    {
                        return NotFound(new Model.BaseResponse { error = true, code = 404, message = MessageService.DataNotFound });
                    }
                    return Ok(new Model.BaseResponse { error = false, code = 200, values = __acc, message = MessageService.GetDataSuccess });
                }
                else
                {
                    return NotFound(new Model.BaseResponse { error = true, code = 404, message = MessageService.EmailTokenFailed });
                }
            }
            catch (Exception __ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Model.BaseResponse { error = true, code = 500, message = __ex.Message });
            }

        }

        /// <summary>
        /// Get Entity List
        /// </summary>
        /// <returns></returns>
        [HttpGet("api/promo/approval/entity", Name = "promo_approval_entity")]
        public async Task<IActionResult> GetPromoApprovalEntity()
        {
            try
            {
                var __val = await _repoPromoApproval.GetAllEntity();
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
        /// Get Distributor List
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpGet("api/promo/approval/distributor", Name = "promo_approval_distributor")]
        public async Task<IActionResult> GetBudgetApprovalDistributor([FromQuery] DistributorListParam param)
        {
            try
            {
                var __val = await _repoPromoApproval.GetDistributorList(param.budgetId, param.entityId!);
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
        /// Get PRomo Approval by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("api/promo/approval/id", Name = "get_promo_approval_by_id")]
        public async Task<IActionResult> GetPromoApprovalById(int id)
        {
            try
            {
                var __val = await _repoPromoApproval.GetPromoByPrimaryId(id);
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
        /// Promo Approval approve
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost("api/promo/approval/approve", Name = "get_promo_approval_approve")]
        public async Task<IActionResult> PromoApprovalApprove([FromBody] PromoApprovalWithSKPParam param)
        {
            DateTime utcTime = DateTime.UtcNow;
            var zone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
            try
            {
                string tokenHeader = Request.Headers["Authorization"]!;
                tokenHeader = tokenHeader.Replace("Bearer ", "");
                var __res = TokenManager.GetClaim(__TokenSecret, tokenHeader);
                if (__res.ProfileID != null)
                {
                    string approveCode = "TP2";
                    string email = __res.UserEmail;
                    PromoSKP paramDto = new()
                    {
                        promoid = param.promoId,
                        notes = param.notes,
                        statuscode = approveCode + __res.ProfileID,
                        approvaldate = TimeZoneInfo.ConvertTimeFromUtc(utcTime, zone),
                        useremail = email,

                        PromoSKPHeader = new SKPValidationDto
                        {
                            PromoId = param.promoId,
                            skpstatus = param.PromoSKPHeader!.skpstatus,
                            skp_notes = param.PromoSKPHeader.skp_notes,

                            ActivityDescBy = __res.ProfileID,
                            BrandBy = __res.ProfileID,
                            BrandDraftBy = __res.ProfileID,
                            ActivityDescDraftBy = __res.ProfileID,
                            ChannelBy = __res.ProfileID,
                            ChannelDraftBy = __res.ProfileID,
                            DistributorBy = __res.ProfileID,
                            DistributorDraftBy = __res.ProfileID,
                            EntityBy = __res.ProfileID,
                            EntityDraftBy = __res.ProfileID,
                            InvestmentDraftBy = __res.ProfileID,
                            InvestmentMatchBy = __res.ProfileID,
                            MechanismDraftBy = __res.ProfileID,
                            MechanismMatchBy = __res.ProfileID,
                            PeriodDraftBy = __res.ProfileID,
                            PeriodMatchBy = __res.ProfileID,
                            SKPDraftAvailBfrAct60By = __res.ProfileID,
                            SKPDraftAvailBy = __res.ProfileID,
                            SKPSign7By = __res.ProfileID,
                            StoreNameBy = __res.ProfileID,
                            StoreNameDraftBy = __res.ProfileID,

                            ActivityDescOn = TimeZoneInfo.ConvertTimeFromUtc(utcTime, zone),
                            BrandOn = TimeZoneInfo.ConvertTimeFromUtc(utcTime, zone),
                            BrandDraftOn = TimeZoneInfo.ConvertTimeFromUtc(utcTime, zone),
                            ActivityDescDraftOn = TimeZoneInfo.ConvertTimeFromUtc(utcTime, zone),
                            ChannelOn = TimeZoneInfo.ConvertTimeFromUtc(utcTime, zone),
                            ChannelDraftOn = TimeZoneInfo.ConvertTimeFromUtc(utcTime, zone),
                            DistributorOn = TimeZoneInfo.ConvertTimeFromUtc(utcTime, zone),
                            DistributorDraftOn = TimeZoneInfo.ConvertTimeFromUtc(utcTime, zone),
                            EntityOn = TimeZoneInfo.ConvertTimeFromUtc(utcTime, zone),
                            EntityDraftOn = TimeZoneInfo.ConvertTimeFromUtc(utcTime, zone),
                            InvestmentDraftOn = TimeZoneInfo.ConvertTimeFromUtc(utcTime, zone),
                            InvestmentMatchOn = TimeZoneInfo.ConvertTimeFromUtc(utcTime, zone),
                            MechanismDraftOn = TimeZoneInfo.ConvertTimeFromUtc(utcTime, zone),
                            MechanismMatchOn = TimeZoneInfo.ConvertTimeFromUtc(utcTime, zone),
                            PeriodDraftOn = TimeZoneInfo.ConvertTimeFromUtc(utcTime, zone),
                            PeriodMatchOn = TimeZoneInfo.ConvertTimeFromUtc(utcTime, zone),
                            SKPDraftAvailBfrAct60On = TimeZoneInfo.ConvertTimeFromUtc(utcTime, zone),
                            SKPDraftAvailOn = TimeZoneInfo.ConvertTimeFromUtc(utcTime, zone),
                            SKPSign7On = TimeZoneInfo.ConvertTimeFromUtc(utcTime, zone),
                            StoreNameOn = TimeZoneInfo.ConvertTimeFromUtc(utcTime, zone),
                            StoreNameDraftOn = TimeZoneInfo.ConvertTimeFromUtc(utcTime, zone),

                            ActivityDesc = param.PromoSKPHeader.ActivityDesc,
                            Brand = param.PromoSKPHeader.Brand,
                            BrandDraft = param.PromoSKPHeader.BrandDraft,
                            ActivityDescDraft = param.PromoSKPHeader.ActivityDescDraft,
                            Channel = param.PromoSKPHeader.Channel,
                            ChannelDraft = param.PromoSKPHeader.ChannelDraft,
                            Distributor = param.PromoSKPHeader.Distributor,
                            DistributorDraft = param.PromoSKPHeader.DistributorDraft,
                            Entity = param.PromoSKPHeader.Entity,
                            EntityDraft = param.PromoSKPHeader.EntityDraft,
                            InvestmentDraft = param.PromoSKPHeader.InvestmentDraft,
                            InvestmentMatch = param.PromoSKPHeader.InvestmentMatch,
                            MechanismDraft = param.PromoSKPHeader.MechanismDraft,
                            MechanismMatch = param.PromoSKPHeader.MechanismMatch,
                            PeriodDraft = param.PromoSKPHeader.PeriodDraft,
                            PeriodMatch = param.PromoSKPHeader.PeriodMatch,
                            SKPDraftAvailBfrAct60 = param.PromoSKPHeader.SKPDraftAvailBfrAct60,
                            SKPDraftAvail = param.PromoSKPHeader.SKPDraftAvail,
                            SKPSign7 = param.PromoSKPHeader.SKPSign7,
                            StoreName = param.PromoSKPHeader.StoreName,
                            StoreNameDraft = param.PromoSKPHeader.StoreNameDraft
                        }
                    };

                    var __val = await _repoPromoApproval.ApprovalPromoWithSKP(paramDto);
                    if (__val != null)
                    {
                        return Ok(new Model.BaseResponse
                        {
                            code = 200,
                            error = false,
                            message = MessageService.SaveSuccess,
                            values = __val
                        });
                    }
                    else
                    {
                        return Ok(new Model.BaseResponse { code = 404, error = true, message = MessageService.SaveFailed });
                    }
                }
                else
                {
                    return NotFound(new Model.BaseResponse { error = true, code = 404, message = MessageService.EmailTokenFailed });
                }
            }
            catch (Exception __ex)
            {
                return Conflict(new Model.BaseResponse { code = 500, error = true, message = __ex.Message });

            }
        }

        /// <summary>
        /// Promo Approval Sendback
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost("api/promo/approval/sendback", Name = "get_promo_approval_sendback")]
        public async Task<IActionResult> PromoApprovalSendback([FromBody] PromoApprovalWithSKPParam param)
        {
            DateTime utcTime = DateTime.UtcNow;
            var zone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
            try
            {
                string tokenHeader = Request.Headers["Authorization"]!;
                tokenHeader = tokenHeader.Replace("Bearer ", "");
                var __res = TokenManager.GetClaim(__TokenSecret, tokenHeader);
                if (__res.ProfileID != null)
                {
                    string approveCode = "TP3";
                    string email = __res.UserEmail;
                    PromoSKP paramDto = new()
                    {
                        promoid = param.promoId,
                        notes = param.notes,
                        statuscode = approveCode + __res.ProfileID,
                        approvaldate = TimeZoneInfo.ConvertTimeFromUtc(utcTime, zone),
                        useremail = email,

                        PromoSKPHeader = new SKPValidationDto
                        {
                            PromoId = param.promoId,
                            skpstatus = param.PromoSKPHeader!.skpstatus,
                            skp_notes = param.PromoSKPHeader.skp_notes,

                            ActivityDescBy = __res.ProfileID,
                            BrandBy = __res.ProfileID,
                            BrandDraftBy = __res.ProfileID,
                            ActivityDescDraftBy = __res.ProfileID,
                            ChannelBy = __res.ProfileID,
                            ChannelDraftBy = __res.ProfileID,
                            DistributorBy = __res.ProfileID,
                            DistributorDraftBy = __res.ProfileID,
                            EntityBy = __res.ProfileID,
                            EntityDraftBy = __res.ProfileID,
                            InvestmentDraftBy = __res.ProfileID,
                            InvestmentMatchBy = __res.ProfileID,
                            MechanismDraftBy = __res.ProfileID,
                            MechanismMatchBy = __res.ProfileID,
                            PeriodDraftBy = __res.ProfileID,
                            PeriodMatchBy = __res.ProfileID,
                            SKPDraftAvailBfrAct60By = __res.ProfileID,
                            SKPDraftAvailBy = __res.ProfileID,
                            SKPSign7By = __res.ProfileID,
                            StoreNameBy = __res.ProfileID,
                            StoreNameDraftBy = __res.ProfileID,

                            ActivityDescOn = TimeZoneInfo.ConvertTimeFromUtc(utcTime, zone),
                            BrandOn = TimeZoneInfo.ConvertTimeFromUtc(utcTime, zone),
                            BrandDraftOn = TimeZoneInfo.ConvertTimeFromUtc(utcTime, zone),
                            ActivityDescDraftOn = TimeZoneInfo.ConvertTimeFromUtc(utcTime, zone),
                            ChannelOn = TimeZoneInfo.ConvertTimeFromUtc(utcTime, zone),
                            ChannelDraftOn = TimeZoneInfo.ConvertTimeFromUtc(utcTime, zone),
                            DistributorOn = TimeZoneInfo.ConvertTimeFromUtc(utcTime, zone),
                            DistributorDraftOn = TimeZoneInfo.ConvertTimeFromUtc(utcTime, zone),
                            EntityOn = TimeZoneInfo.ConvertTimeFromUtc(utcTime, zone),
                            EntityDraftOn = TimeZoneInfo.ConvertTimeFromUtc(utcTime, zone),
                            InvestmentDraftOn = TimeZoneInfo.ConvertTimeFromUtc(utcTime, zone),
                            InvestmentMatchOn = TimeZoneInfo.ConvertTimeFromUtc(utcTime, zone),
                            MechanismDraftOn = TimeZoneInfo.ConvertTimeFromUtc(utcTime, zone),
                            MechanismMatchOn = TimeZoneInfo.ConvertTimeFromUtc(utcTime, zone),
                            PeriodDraftOn = TimeZoneInfo.ConvertTimeFromUtc(utcTime, zone),
                            PeriodMatchOn = TimeZoneInfo.ConvertTimeFromUtc(utcTime, zone),
                            SKPDraftAvailBfrAct60On = TimeZoneInfo.ConvertTimeFromUtc(utcTime, zone),
                            SKPDraftAvailOn = TimeZoneInfo.ConvertTimeFromUtc(utcTime, zone),
                            SKPSign7On = TimeZoneInfo.ConvertTimeFromUtc(utcTime, zone),
                            StoreNameOn = TimeZoneInfo.ConvertTimeFromUtc(utcTime, zone),
                            StoreNameDraftOn = TimeZoneInfo.ConvertTimeFromUtc(utcTime, zone),

                            ActivityDesc = param.PromoSKPHeader.ActivityDesc,
                            Brand = param.PromoSKPHeader.Brand,
                            BrandDraft = param.PromoSKPHeader.BrandDraft,
                            ActivityDescDraft = param.PromoSKPHeader.ActivityDescDraft,
                            Channel = param.PromoSKPHeader.Channel,
                            ChannelDraft = param.PromoSKPHeader.ChannelDraft,
                            Distributor = param.PromoSKPHeader.Distributor,
                            DistributorDraft = param.PromoSKPHeader.DistributorDraft,
                            Entity = param.PromoSKPHeader.Entity,
                            EntityDraft = param.PromoSKPHeader.EntityDraft,
                            InvestmentDraft = param.PromoSKPHeader.InvestmentDraft,
                            InvestmentMatch = param.PromoSKPHeader.InvestmentMatch,
                            MechanismDraft = param.PromoSKPHeader.MechanismDraft,
                            MechanismMatch = param.PromoSKPHeader.MechanismMatch,
                            PeriodDraft = param.PromoSKPHeader.PeriodDraft,
                            PeriodMatch = param.PromoSKPHeader.PeriodMatch,
                            SKPDraftAvailBfrAct60 = param.PromoSKPHeader.SKPDraftAvailBfrAct60,
                            SKPDraftAvail = param.PromoSKPHeader.SKPDraftAvail,
                            SKPSign7 = param.PromoSKPHeader.SKPSign7,
                            StoreName = param.PromoSKPHeader.StoreName,
                            StoreNameDraft = param.PromoSKPHeader.StoreNameDraft
                        }
                    };



                    var __val = await _repoPromoApproval.ApprovalPromoWithSKP(paramDto);
                    if (__val != null)
                    {
                        return Ok(new Model.BaseResponse
                        {
                            code = 200,
                            error = false,
                            message = MessageService.SaveSuccess,
                            values = __val
                        });
                    }
                    else
                    {
                        return Ok(new Model.BaseResponse { code = 404, error = true, message = MessageService.SaveFailed });
                    }
                }
                else
                {
                    return NotFound(new Model.BaseResponse { error = true, code = 404, message = MessageService.EmailTokenFailed });
                }
            }
            catch (Exception __ex)
            {
                return Conflict(new Model.BaseResponse { code = 500, error = true, message = __ex.Message });
            }
        }

        /// <summary>
        /// Promo Approval approve by email (no token) 
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost("api/promo/approvalbyemail/approve", Name = "get_promo_approval_byEmail_approve")]
        public async Task<IActionResult> PromoApprovalByEmailApprove([FromBody] PromoApprovalByEmailWithSKPParam param)
        {
            DateTime utcTime = DateTime.UtcNow;
            var zone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
            try
            {
                string approveCode = "TP2";
                string email = "";
                PromoSKP paramDto = new()
                {
                    promoid = param.promoId,
                    notes = param.notes,
                    statuscode = approveCode + param.profileId,
                    approvaldate = TimeZoneInfo.ConvertTimeFromUtc(utcTime, zone),
                    useremail = email,

                    PromoSKPHeader = new SKPValidationDto
                    {
                        PromoId = param.promoId,
                        skpstatus = param.PromoSKPHeader!.skpstatus,
                        skp_notes = param.PromoSKPHeader.skp_notes,

                        ActivityDescBy = param.profileId,
                        BrandBy = param.profileId,
                        BrandDraftBy = param.profileId,
                        ActivityDescDraftBy = param.profileId,
                        ChannelBy = param.profileId,
                        ChannelDraftBy = param.profileId,
                        DistributorBy = param.profileId,
                        DistributorDraftBy = param.profileId,
                        EntityBy = param.profileId,
                        EntityDraftBy = param.profileId,
                        InvestmentDraftBy = param.profileId,
                        InvestmentMatchBy = param.profileId,
                        MechanismDraftBy = param.profileId,
                        MechanismMatchBy = param.profileId,
                        PeriodDraftBy = param.profileId,
                        PeriodMatchBy = param.profileId,
                        SKPDraftAvailBfrAct60By = param.profileId,
                        SKPDraftAvailBy = param.profileId,
                        SKPSign7By = param.profileId,
                        StoreNameBy = param.profileId,
                        StoreNameDraftBy = param.profileId,

                        ActivityDescOn = TimeZoneInfo.ConvertTimeFromUtc(utcTime, zone),
                        BrandOn = TimeZoneInfo.ConvertTimeFromUtc(utcTime, zone),
                        BrandDraftOn = TimeZoneInfo.ConvertTimeFromUtc(utcTime, zone),
                        ActivityDescDraftOn = TimeZoneInfo.ConvertTimeFromUtc(utcTime, zone),
                        ChannelOn = TimeZoneInfo.ConvertTimeFromUtc(utcTime, zone),
                        ChannelDraftOn = TimeZoneInfo.ConvertTimeFromUtc(utcTime, zone),
                        DistributorOn = TimeZoneInfo.ConvertTimeFromUtc(utcTime, zone),
                        DistributorDraftOn = TimeZoneInfo.ConvertTimeFromUtc(utcTime, zone),
                        EntityOn = TimeZoneInfo.ConvertTimeFromUtc(utcTime, zone),
                        EntityDraftOn = TimeZoneInfo.ConvertTimeFromUtc(utcTime, zone),
                        InvestmentDraftOn = TimeZoneInfo.ConvertTimeFromUtc(utcTime, zone),
                        InvestmentMatchOn = TimeZoneInfo.ConvertTimeFromUtc(utcTime, zone),
                        MechanismDraftOn = TimeZoneInfo.ConvertTimeFromUtc(utcTime, zone),
                        MechanismMatchOn = TimeZoneInfo.ConvertTimeFromUtc(utcTime, zone),
                        PeriodDraftOn = TimeZoneInfo.ConvertTimeFromUtc(utcTime, zone),
                        PeriodMatchOn = TimeZoneInfo.ConvertTimeFromUtc(utcTime, zone),
                        SKPDraftAvailBfrAct60On = TimeZoneInfo.ConvertTimeFromUtc(utcTime, zone),
                        SKPDraftAvailOn = TimeZoneInfo.ConvertTimeFromUtc(utcTime, zone),
                        SKPSign7On = TimeZoneInfo.ConvertTimeFromUtc(utcTime, zone),
                        StoreNameOn = TimeZoneInfo.ConvertTimeFromUtc(utcTime, zone),
                        StoreNameDraftOn = TimeZoneInfo.ConvertTimeFromUtc(utcTime, zone),

                        ActivityDesc = param.PromoSKPHeader.ActivityDesc,
                        Brand = param.PromoSKPHeader.Brand,
                        BrandDraft = param.PromoSKPHeader.BrandDraft,
                        ActivityDescDraft = param.PromoSKPHeader.ActivityDescDraft,
                        Channel = param.PromoSKPHeader.Channel,
                        ChannelDraft = param.PromoSKPHeader.ChannelDraft,
                        Distributor = param.PromoSKPHeader.Distributor,
                        DistributorDraft = param.PromoSKPHeader.DistributorDraft,
                        Entity = param.PromoSKPHeader.Entity,
                        EntityDraft = param.PromoSKPHeader.EntityDraft,
                        InvestmentDraft = param.PromoSKPHeader.InvestmentDraft,
                        InvestmentMatch = param.PromoSKPHeader.InvestmentMatch,
                        MechanismDraft = param.PromoSKPHeader.MechanismDraft,
                        MechanismMatch = param.PromoSKPHeader.MechanismMatch,
                        PeriodDraft = param.PromoSKPHeader.PeriodDraft,
                        PeriodMatch = param.PromoSKPHeader.PeriodMatch,
                        SKPDraftAvailBfrAct60 = param.PromoSKPHeader.SKPDraftAvailBfrAct60,
                        SKPDraftAvail = param.PromoSKPHeader.SKPDraftAvail,
                        SKPSign7 = param.PromoSKPHeader.SKPSign7,
                        StoreName = param.PromoSKPHeader.StoreName,
                        StoreNameDraft = param.PromoSKPHeader.StoreNameDraft
                    }
                };
                var __val = await _repoPromoApproval.ApprovalPromoWithSKP(paramDto);
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
                    return Ok(new Model.BaseResponse { code = 404, error = true, message = MessageService.SaveFailed });
                }
            }
            catch (Exception __ex)
            {
                return Conflict(new Model.BaseResponse { code = 500, error = true, message = __ex.Message });
            }
        }

        /// <summary>
        /// Promo Approval Sendback by email (no token)
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost("api/promo/approvalbyemail/sendback", Name = "get_promo_approval_byEmail_sendback")]
        public async Task<IActionResult> PromoApprovalByEmailSendback([FromBody] PromoApprovalByEmailWithSKPParam param)
        {
            DateTime utcTime = DateTime.UtcNow;
            var zone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
            try
            {
                string approveCode = "TP3";
                string email = "";
                PromoSKP paramDto = new()
                {
                    promoid = param.promoId,
                    notes = param.notes,
                    statuscode = approveCode + param.profileId,
                    approvaldate = TimeZoneInfo.ConvertTimeFromUtc(utcTime, zone),
                    useremail = email,
                    PromoSKPHeader = new SKPValidationDto
                    {
                        PromoId = param.promoId,
                        skpstatus = param.PromoSKPHeader!.skpstatus,
                        skp_notes = param.PromoSKPHeader.skp_notes,

                        ActivityDescBy = param.profileId,
                        BrandBy = param.profileId,
                        BrandDraftBy = param.profileId,
                        ActivityDescDraftBy = param.profileId,
                        ChannelBy = param.profileId,
                        ChannelDraftBy = param.profileId,
                        DistributorBy = param.profileId,
                        DistributorDraftBy = param.profileId,
                        EntityBy = param.profileId,
                        EntityDraftBy = param.profileId,
                        InvestmentDraftBy = param.profileId,
                        InvestmentMatchBy = param.profileId,
                        MechanismDraftBy = param.profileId,
                        MechanismMatchBy = param.profileId,
                        PeriodDraftBy = param.profileId,
                        PeriodMatchBy = param.profileId,
                        SKPDraftAvailBfrAct60By = param.profileId,
                        SKPDraftAvailBy = param.profileId,
                        SKPSign7By = param.profileId,
                        StoreNameBy = param.profileId,
                        StoreNameDraftBy = param.profileId,

                        ActivityDescOn = TimeZoneInfo.ConvertTimeFromUtc(utcTime, zone),
                        BrandOn = TimeZoneInfo.ConvertTimeFromUtc(utcTime, zone),
                        BrandDraftOn = TimeZoneInfo.ConvertTimeFromUtc(utcTime, zone),
                        ActivityDescDraftOn = TimeZoneInfo.ConvertTimeFromUtc(utcTime, zone),
                        ChannelOn = TimeZoneInfo.ConvertTimeFromUtc(utcTime, zone),
                        ChannelDraftOn = TimeZoneInfo.ConvertTimeFromUtc(utcTime, zone),
                        DistributorOn = TimeZoneInfo.ConvertTimeFromUtc(utcTime, zone),
                        DistributorDraftOn = TimeZoneInfo.ConvertTimeFromUtc(utcTime, zone),
                        EntityOn = TimeZoneInfo.ConvertTimeFromUtc(utcTime, zone),
                        EntityDraftOn = TimeZoneInfo.ConvertTimeFromUtc(utcTime, zone),
                        InvestmentDraftOn = TimeZoneInfo.ConvertTimeFromUtc(utcTime, zone),
                        InvestmentMatchOn = TimeZoneInfo.ConvertTimeFromUtc(utcTime, zone),
                        MechanismDraftOn = TimeZoneInfo.ConvertTimeFromUtc(utcTime, zone),
                        MechanismMatchOn = TimeZoneInfo.ConvertTimeFromUtc(utcTime, zone),
                        PeriodDraftOn = TimeZoneInfo.ConvertTimeFromUtc(utcTime, zone),
                        PeriodMatchOn = TimeZoneInfo.ConvertTimeFromUtc(utcTime, zone),
                        SKPDraftAvailBfrAct60On = TimeZoneInfo.ConvertTimeFromUtc(utcTime, zone),
                        SKPDraftAvailOn = TimeZoneInfo.ConvertTimeFromUtc(utcTime, zone),
                        SKPSign7On = TimeZoneInfo.ConvertTimeFromUtc(utcTime, zone),
                        StoreNameOn = TimeZoneInfo.ConvertTimeFromUtc(utcTime, zone),
                        StoreNameDraftOn = TimeZoneInfo.ConvertTimeFromUtc(utcTime, zone),

                        ActivityDesc = param.PromoSKPHeader.ActivityDesc,
                        Brand = param.PromoSKPHeader.Brand,
                        BrandDraft = param.PromoSKPHeader.BrandDraft,
                        ActivityDescDraft = param.PromoSKPHeader.ActivityDescDraft,
                        Channel = param.PromoSKPHeader.Channel,
                        ChannelDraft = param.PromoSKPHeader.ChannelDraft,
                        Distributor = param.PromoSKPHeader.Distributor,
                        DistributorDraft = param.PromoSKPHeader.DistributorDraft,
                        Entity = param.PromoSKPHeader.Entity,
                        EntityDraft = param.PromoSKPHeader.EntityDraft,
                        InvestmentDraft = param.PromoSKPHeader.InvestmentDraft,
                        InvestmentMatch = param.PromoSKPHeader.InvestmentMatch,
                        MechanismDraft = param.PromoSKPHeader.MechanismDraft,
                        MechanismMatch = param.PromoSKPHeader.MechanismMatch,
                        PeriodDraft = param.PromoSKPHeader.PeriodDraft,
                        PeriodMatch = param.PromoSKPHeader.PeriodMatch,
                        SKPDraftAvailBfrAct60 = param.PromoSKPHeader.SKPDraftAvailBfrAct60,
                        SKPDraftAvail = param.PromoSKPHeader.SKPDraftAvail,
                        SKPSign7 = param.PromoSKPHeader.SKPSign7,
                        StoreName = param.PromoSKPHeader.StoreName,
                        StoreNameDraft = param.PromoSKPHeader.StoreNameDraft
                    }
                };

                var __val = await _repoPromoApproval.ApprovalPromoWithSKP(paramDto);
                if (__val != null)
                {
                    return Ok(new Model.BaseResponse
                    {
                        code = 200,
                        error = false,
                        message = MessageService.SaveSuccess,
                        values = __val
                    });
                }
                else
                {
                    return Ok(new Model.BaseResponse { code = 404, error = true, message = MessageService.SaveFailed });
                }
            }
            catch (Exception __ex)
            {
                return Conflict(new Model.BaseResponse { code = 500, error = true, message = __ex.Message });
            }
        }
    }
}
