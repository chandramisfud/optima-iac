using Microsoft.AspNetCore.Mvc;
using Repositories.Entities.Models.PromoApproval;
using V7.MessagingServices;
using V7.Model.Promo;
using V7.Model.PromoApproval;
using V7.Services;

namespace V7.Controllers.Promo
{
    public partial class PromoController : BaseController
    {
        /// <summary>
        /// Get Promo SKP Validation landing page
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpGet("api/promo/skpvalidation", Name = "promo_skpvalidation")]
        public async Task<IActionResult?> GetPromoSKPValidationLandingPage([FromQuery] PromoSKPValidationParam param)
        {
            IActionResult? result = null;
            try
            {
                string tokenHeader = Request.Headers["Authorization"]!;
                tokenHeader = tokenHeader.Replace("Bearer ", "");
                var __res = TokenManager.GetClaim(__TokenSecret, tokenHeader);

                if (__res.ProfileID != null)
                {
                    var __val = await _repoPromoSKPValidation.GetPromoListSKPFlagging(param.Period!, param.EntityId, param.DistributorId,
                        param.BudgetParentId, param.ChannelId, param.CancelStatus, param.StartFrom, param.StartTo, param.Status, __res.ProfileID);
                    if (__val != null)
                    {
                        if (__val.Count > 0)
                        {
                            if (param.SortDirection == Model.Report.SKPValidationSortDirection.asc)
                            {
                                switch (param.SortColumn)
                                {
                                    case PromoSKPValidationSortColumn.refId:
                                        __val = __val.OrderBy(x => x.RefId).ToList();
                                        break;
                                    case PromoSKPValidationSortColumn.createBy:
                                        __val = __val.OrderBy(x => x.CreateBy).ToList();
                                        break;
                                    case PromoSKPValidationSortColumn.activityDesc:
                                        __val = __val.OrderBy(x => x.ActivityDesc).ToList();
                                        break;
                                    case PromoSKPValidationSortColumn.channelDesc:
                                        __val = __val.OrderBy(x => x.channelDesc).ToList();
                                        break;
                                    case PromoSKPValidationSortColumn.subAccountDesc:
                                        __val = __val.OrderBy(x => x.subAccountDesc).ToList();
                                        break;
                                    case PromoSKPValidationSortColumn.createdName:
                                        __val = __val.OrderBy(x => x.createdName).ToList();
                                        break;
                                    case PromoSKPValidationSortColumn.skpstatus:
                                        __val = __val.OrderBy(x => x.skpstatus).ToList();
                                        break;
                                    case PromoSKPValidationSortColumn.skp_notes:
                                        __val = __val.OrderBy(x => x.skp_notes).ToList();
                                        break;
                                }
                            }
                            else
                            {
                                switch (param.SortColumn)
                                {
                                    case PromoSKPValidationSortColumn.refId:
                                        __val = __val.OrderByDescending(x => x.RefId).ToList();
                                        break;
                                    case PromoSKPValidationSortColumn.createBy:
                                        __val = __val.OrderByDescending(x => x.CreateBy).ToList();
                                        break;
                                    case PromoSKPValidationSortColumn.activityDesc:
                                        __val = __val.OrderByDescending(x => x.ActivityDesc).ToList();
                                        break;
                                    case PromoSKPValidationSortColumn.channelDesc:
                                        __val = __val.OrderByDescending(x => x.channelDesc).ToList();
                                        break;
                                    case PromoSKPValidationSortColumn.subAccountDesc:
                                        __val = __val.OrderByDescending(x => x.subAccountDesc).ToList();
                                        break;
                                    case PromoSKPValidationSortColumn.createdName:
                                        __val = __val.OrderByDescending(x => x.createdName).ToList();
                                        break;
                                    case PromoSKPValidationSortColumn.skpstatus:
                                        __val = __val.OrderByDescending(x => x.skpstatus).ToList();
                                        break;
                                    case PromoSKPValidationSortColumn.skp_notes:
                                        __val = __val.OrderByDescending(x => x.skp_notes).ToList();
                                        break;
                                }
                            }
                        }

                        result = Ok(new Model.BaseResponse
                        {
                            error = false,
                            code = 200,
                            values = __val,
                            message = MessageService.GetDataSuccess
                        });
                    }
                }
                else
                {
                    result = Conflict(new Model.BaseResponse { error = true, code = 404, message = MessageService.EmailTokenFailed });
                }
            }
            catch (Exception __ex)
            {
                result = Conflict(new Model.BaseResponse { error = true, code = 500, message = __ex.Message });
            }
            return result;
        }

        /// <summary>
        /// Get List Entity
        /// </summary>
        /// <returns></returns>
        [HttpGet("api/promo/skpvalidation/entity", Name = "promo_skpvalidation_get_entity_list")]
        public async Task<IActionResult> GetPromoEntitySKPValidation()
        {
            try
            {
                if (!ModelState.IsValid) return Conflict(ModelState);
                var __val = await _repoPromoSKPValidation.GetEntityList();
                return Ok(new Model.BaseResponse
                {
                    error = false,
                    code = 200,
                    message = MessageService.GetDataSuccess,
                    values = __val
                });
            }
            catch (Exception __ex)
            {
                return Conflict(new Model.BaseResponse { error = true, code = 500, message = __ex.Message });
            }
        }

        /// <summary>
        /// Get List Distributor
        /// </summary>
        /// <param name="budgetId"></param>
        /// <param name="parentId"></param>
        /// <returns></returns>
        [HttpGet("api/promo/skpvalidation/distributor", Name = "promo_skpvalidation_get_distributor_list")]
        public async Task<IActionResult> GetDistributorSKPValidation([FromQuery] int budgetId, int[] parentId)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);

                var __val = await _repoPromoSKPValidation.GetDistributorList(budgetId, parentId);
                return Ok(new Model.BaseResponse
                {
                    error = false,
                    code = 200,
                    message = MessageService.GetDataSuccess,
                    values = __val
                });
            }
            catch (Exception __ex)
            {
                return Conflict(new Model.BaseResponse { error = true, code = 500, message = __ex.Message });
            }
        }

        /// <summary>
        /// Get List Channel
        /// </summary>
        /// <returns></returns>
        [HttpGet("api/promo/skpvalidation/channel", Name = "promo_skpvalidation_get_channel_list")]
        public async Task<IActionResult> GetChannelSKPValidation()
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);

                string tokenHeader = Request.Headers["Authorization"]!;
                tokenHeader = tokenHeader.Replace("Bearer ", "");
                var __res = TokenManager.GetClaim(__TokenSecret, tokenHeader);

                List<int> arrayParent = new();

                if (__res.ProfileID != null)
                {
                    var __val = await _repoPromoSKPValidation.GetChannelList(__res.ProfileID, arrayParent.ToArray());
                    return Ok(new Model.BaseResponse
                    {
                        error = false,
                        code = 200,
                        message = MessageService.GetDataSuccess,
                        values = __val
                    });
                }
                else
                {
                    return Conflict(new Model.BaseResponse { error = true, code = 404, message = MessageService.EmailTokenFailed });
                }
            }
            catch (Exception __ex)
            {
                return Conflict(new Model.BaseResponse { error = true, code = 500, message = __ex.Message });

            }
        }

        /// <summary>
        /// Get Promo SKPValidation by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("api/promo/skpvalidation/id", Name = "promo_skpvalidation_get_by_id")]
        public async Task<IActionResult> GetPromoWithSKP([FromQuery] int id)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);
                var __acc = await _repoPromoSKPValidation.GetPromoWithSKP(id, "");
                if (__acc != null)
                {
                    return Ok(new Model.BaseResponse { error = false, code = 200, values = __acc, message = MessageService.GetDataSuccess });
                }
                else
                {
                    return Conflict(new Model.BaseResponse { error = true, code = 404, message = MessageService.GetDataFailed });
                }
            }
            catch (Exception __ex)
            {
                return Conflict(new Model.BaseResponse { error = true, code = 500, message = __ex.Message });
            }
        }

        /// <summary>
        /// Saving SKPValidation
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPut("api/promo/skpvalidation/", Name = "promo_skpvalidation_update")]
        public async Task<IActionResult> ApprovalPromoWithSKPFlagging([FromBody] PromoApprovalWithSKPParam param)
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
                    if (!ModelState.IsValid) return BadRequest(ModelState);
                    PromoSKP paramDto = new()
                    {
                        promoid = param.promoId,
                        notes = param.notes,
                        approvaldate = TimeZoneInfo.ConvertTimeFromUtc(utcTime, zone),

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

                    var x = await _repoPromoSKPValidation.UpdateApprovalPromoWithSKPFlagging(paramDto);

                    return Ok(new Model.BaseResponse { error = false, code = 200, values = x });
                }
                else
                {
                    return Conflict(new Model.BaseResponse { error = true, code = 404, message = MessageService.EmailTokenFailed });
                }
            }
            catch (Exception __ex)
            {
                return Conflict(new Model.BaseResponse { error = true, code = 500, message = __ex.Message });
            }
        }


        [HttpGet("api/promo/skpvalidation/download", Name = "promo_skpvalidation_download")]
        public async Task<IActionResult> GetSKPValidationDownload([FromQuery] SKPValidationDownloadParam param)
        {
            try
            {
                string tokenHeader = Request.Headers["Authorization"]!;
                tokenHeader = tokenHeader.Replace("Bearer ", "");
                var __res = TokenManager.GetClaim(__TokenSecret, tokenHeader);
                if (__res.ProfileID != null)
                {
                    if (!ModelState.IsValid) return Conflict(ModelState);
                    var __val = await _repoPromoSKPValidation.GetSKPValidationDownload(param.Period!, param.EntityId, param.DistributorId,
                        param.BudgetParentId, param.ChannelId, __res.ProfileID,
                        param.CancelStatus, param.StartFrom!, param.StartTo!, param.SubmissionParam, param.Status,
                        param.Search!, param.SortColumn.ToString(), param.SortDirection.ToString(), param.PageNumber, param.PageSize);
                    if (__val != null)
                    {
                        return Ok(new Model.BaseResponse
                        {
                            error = false,
                            code = 200,
                            values = __val,
                            message = MessageService.GetDataSuccess
                        });
                    }
                    else
                    {
                        return Ok(new Model.BaseResponse { error = true, code = 404, message = MessageService.GetDataFailed });
                    }
                }
                else
                {
                    return Conflict(new Model.BaseResponse { error = true, code = 404, message = MessageService.EmailTokenFailed });
                }
            }
            catch (Exception __ex)
            {
                return Conflict(new Model.BaseResponse { error = true, code = 500, message = __ex.Message });
            }
        }
    }
}
