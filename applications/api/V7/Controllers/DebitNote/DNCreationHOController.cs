using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Repositories.Entities.Models;
using V7.MessagingServices;
using V7.Model.DebitNote;
using V7.Services;

namespace V7.Controllers.DebitNote
{
    public partial class DebitNoteController : BaseController
    {
        // debetnote/getbyId
        /// <summary>
        /// Get DN HO by Id, Old API = "debetnote/getbyId"
        /// </summary>
        /// <returns></returns>
        [HttpGet("api/dn/creation/ho/id", Name = "debetnote_ho_list_getbyid")]
        public async Task<IActionResult> GetDNHObyId([FromQuery] int id)
        {
            IActionResult result;
            List<string> EmptyList = new();
            try
            {
                if (!ModelState.IsValid) return Conflict(ModelState);
                var __val = await __repoDNCreationHO.GetDNHO(id);
                if (__val == null)
                {
                    return Ok(new BaseResponse { code = 204, error = true, message = MessageService.DataNotFound, values = EmptyList });
                }
                else
                {
                    return Ok(new BaseResponse
                    {
                        code = 200,
                        error = false,
                        message = MessageService.GetDataSuccess,
                        values = __val
                    });
                }
            }
            catch (Exception __ex)
            {
                result = StatusCode(StatusCodes.Status403Forbidden, new BaseResponse { error = true, code = 403, message = __ex.Message });
            }
            return result;
        }
        // debetnote/cancel
        /// <summary>
        /// DN Cancel HO, Old API = "debetnote/cancel"
        /// </summary>
        /// <returns></returns>
        [HttpPost("api/dn/creation/ho/cancel", Name = "dn_ho_cancel")]
        public async Task<IActionResult> CancelDNHO([FromBody] DNCancelBody body)
        {

            try
            {
                if (!ModelState.IsValid) return Conflict(ModelState);
                // get token from request header
                string tokenHeader = Request.Headers["Authorization"]!;
                tokenHeader = tokenHeader.Replace("Bearer ", "");
                // get user data from token
                var __res = TokenManager.GetClaim(__TokenSecret, tokenHeader);
                if (__res.UserEmail != null)
                {
                    DNCancelBody __bodytoken = new()
                    {
                        dnid = body.dnid,
                        reason = body.reason,
                        userid = __res.ProfileID
                    };
                    var __val = await __repoDNCreationHO.CancelDN(__bodytoken);
                    return Ok(new BaseResponse { code = 200, error = false, message = "DN with Refid = " + __val.RefId + " successfully cancel", values = __val });
                }
                else
                {
                    return Conflict(new BaseResponse { code = 404, error = true, message = MessageService.EmailTokenFailed });
                }
            }
            catch (Exception __ex)
            {
                return Conflict(new BaseResponse { code = 500, error = true, message = __ex.Message });

            }
        }
        // master/getAttributeByUser
        /// <summary>
        /// Get attribute by user, Old API = "master/getAttributeByUser"
        /// </summary>
        /// <returns></returns>
        [HttpGet("api/dn/creation/ho/attribute/user", Name = "all_ho_attribute_byuser")]
        public async Task<IActionResult> GetAttributeByUserDNHO()
        {
            try
            {
                string tokenHeader = Request.Headers["Authorization"]!;
                tokenHeader = tokenHeader.Replace("Bearer ", "");
                var __res = TokenManager.GetClaim(__TokenSecret, tokenHeader);

                if (__res.ProfileID != null)
                {
                    DNAttributebyUserParam __bodytoken = new()
                    {
                        userid = __res.ProfileID
                    };
                    var __val = await __repoDNCreationHO.GetAttributeByUser(__bodytoken.userid);
                    return Ok(new BaseResponse
                    {
                        error = false,
                        code = 200,
                        values = __val,
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
                return Conflict(new BaseResponse
                {
                    code = 500,
                    error = true,
                    message = __ex.Message
                });
            }
        }
        // master/getDistEntityByUser/
        /// <summary>
        /// Get attribute by user, Old API = "master/getDistEntityByUser/"
        /// </summary>
        /// <returns></returns>
        [HttpGet("api/dn/creation/ho/distributor-entity/user", Name = "all_ho_distributor-entity_byuser")]
        public async Task<IActionResult> GetDistributorEntityByUserIdDNHO()
        {
            try
            {
                string tokenHeader = Request.Headers["Authorization"]!;
                tokenHeader = tokenHeader.Replace("Bearer ", "");
                var __res = TokenManager.GetClaim(__TokenSecret, tokenHeader);

                if (__res.ProfileID != null)
                {
                    DNAttributebyUserParam __bodytoken = new()
                    {
                        userid = __res.ProfileID
                    };
                    var __val = await __repoDNCreationHO.GetDistributorEntityByUserId(__bodytoken.userid);
                    return Ok(new BaseResponse
                    {
                        error = false,
                        code = 200,
                        values = __val,
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
                return Conflict(new BaseResponse
                {
                    code = 500,
                    error = true,
                    message = __ex.Message
                });
            }
        }
        // sellingpoint/getByUser
        /// <summary>
        /// Get selling point by user, Old API = "sellingpoint/getByUser"
        /// </summary>
        /// <returns></returns>
        [HttpGet("api/dn/creation/ho/sellingpoint/user", Name = "all_ho_sellingpoint_byuser")]
        public async Task<IActionResult> GetSellingPointByUserDNHO()
        {
            try
            {
                string tokenHeader = Request.Headers["Authorization"]!;
                tokenHeader = tokenHeader.Replace("Bearer ", "");
                var __res = TokenManager.GetClaim(__TokenSecret, tokenHeader);

                if (__res.ProfileID != null)
                {
                    DNAttributebyUserParam __bodytoken = new()
                    {
                        userid = __res.ProfileID
                    };
                    var __val = await __repoDNCreationHO.GetSellingPointByUser(__bodytoken.userid);
                    return Ok(new BaseResponse
                    {
                        error = false,
                        code = 200,
                        values = __val,
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
                return Conflict(new BaseResponse
                {
                    code = 500,
                    error = true,
                    message = __ex.Message
                });
            }
        }
        /// <summary>
        /// Get DN HO Report
        /// </summary>
        /// <returns></returns>
        [HttpGet("api/dn/creation/ho/report", Name = "debitnote_ho_report")]
        public async Task<IActionResult> GetDNHOReport([FromQuery] DebitNoteReportParam param)
        {
            try
            {
                string tokenHeader = Request.Headers["Authorization"]!;
                tokenHeader = tokenHeader.Replace("Bearer ", "");
                var __res = TokenManager.GetClaim(__TokenSecret, tokenHeader);

                if (__res.ProfileID != null)
                {
                    var __val = await __repoDNCreationHO.GetDNReport(param.year!,
                     param.entity,
                     param.distributor,
                     param.channel, param.account,
                      __res.ProfileID);
                    return Ok(new BaseResponse
                    {
                        error = false,
                        code = 200,
                        values = __val,
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
                return Conflict(new BaseResponse
                {
                    code = 500,
                    error = true,
                    message = __ex.Message
                });

            }
        }
        // debetnote/store
        /// <summary>
        /// Create DN, Old API = "debetnote/store"
        /// </summary>
        /// <returns></returns>
        [HttpPost("api/dn/creation/ho", Name = "dn_ho_storevlidate")]
        public async Task<IActionResult> CreateDNHO([FromBody] DNCreationParam body)
        {
            IActionResult result;
            if (!ModelState.IsValid) return BadRequest(ModelState);
            try
            {
                string tokenHeader = Request.Headers["Authorization"]!;
                tokenHeader = tokenHeader.Replace("Bearer ", "");
                var __res = TokenManager.GetClaim(__TokenSecret, tokenHeader);
                if (__res.ProfileID != null)
                {
                    DNCreationParam __bodytoken = new()
                    {
                        IsDNPromo = body.IsDNPromo,
                        Id = body.Id,
                        Periode = body.Periode,
                        EntityId = body.EntityId,
                        DistributorId = body.DistributorId,
                        ActivityDesc = body.ActivityDesc,
                        AccountId = body.AccountId,
                        PromoId = body.PromoId,
                        IntDocNo = body.IntDocNo,
                        MemDocNo = body.MemDocNo,
                        DPP = body.DPP,
                        DNAmount = body.DNAmount,
                        FeeDesc = body.FeeDesc,
                        FeePct = body.FeePct,
                        FeeAmount = body.FeeAmount,
                        PPNPct = body.PPNPct,
                        DeductionDate = body.DeductionDate,
                        UserId = __res.ProfileID,
                        TaxLevel = body.TaxLevel,
                        WHTType = body.WHTType,
                        sellpoint = new List<DNSellpoint>()
                    };
                    foreach (var item in body.sellpoint!)
                    {
                        __bodytoken.sellpoint.Add(new DNSellpoint
                        {
                            flag = item.flag,
                            sellpoint = item.sellpoint,
                            LongDesc = item.LongDesc
                        });
                    }
                    __bodytoken.dnattachment = new List<DNAttachment>();
                    foreach (var item in body.dnattachment!)
                    {
                        __bodytoken.dnattachment.Add(new DNAttachment
                        {
                            DocLink = item.DocLink,
                            FileName = item.FileName
                        });
                    }
                    __bodytoken.pphPct = body.pphPct;
                    __bodytoken.pphAmt = body.pphAmt;
                    __bodytoken.statusPPH = body.statusPPH;
                    __bodytoken.FPNumber = body.FPNumber;
                    __bodytoken.FPDate = body.FPDate;
                    __bodytoken.statusPPN = body.statusPPN;

                    var __resValidation = await __repoDNCreationHO.GetDebetnoteStoreValidate(__bodytoken);
                    if (__resValidation.errorcode == 2)
                    {
                        var __resDN = await __repoDNCreationHO.CreateDN(__bodytoken);
                        return Ok(new BaseResponse
                        {
                            code = 200,
                            error = false,
                            message = MessageService.SaveSuccess,
                            values = new
                            {
                                Id = __resDN.Id,
                                refId = __resDN.RefId,
                                approverUserid = __resDN.approver_userid,
                                approverUserName = __resDN.approver_username,
                                approverEmail = __resDN.approver_email
                            }
                        });
                    }
                    else
                    {
                        return Ok(new BaseResponse
                        {
                            code = 666,
                            error = true,
                            message = __resValidation.messageout,
                            values = new
                            {
                                Id = __resValidation.id,
                                distributorId = __resValidation.DistributorId,
                                distributor = __resValidation.distributor,
                                entity = __resValidation.entity,
                                activityDesc = __resValidation.ActivityDesc,
                                dpp = __resValidation.DPP,
                                intdocno = __resValidation.IntDocNo,
                                refId = __resValidation.refid
                            }
                        });
                    }
                }
                else
                {
                    result = StatusCode(StatusCodes.Status403Forbidden, new BaseResponse { error = true, code = 403, message = MessageService.EmailTokenFailed });
                }
            }
            catch (Exception __ex)
            {
                return Ok(new BaseResponse { error = true, code = 500, message = __ex.Message });
            }
            return result;
        }
        // debetnote/update        
        /// <summary>
        /// Update DN, Old API = "debetnote/update"
        /// </summary>
        /// <returns></returns>
        [HttpPut("api/dn/creation/ho", Name = "dn_ho_updatevalidate")]
        public async Task<IActionResult> UpdateDNHO([FromBody] DNCreationParam body)
        {
            IActionResult result;
            if (!ModelState.IsValid) return BadRequest(ModelState);
            try
            {
                string tokenHeader = Request.Headers["Authorization"]!;
                tokenHeader = tokenHeader.Replace("Bearer ", "");
                var __res = TokenManager.GetClaim(__TokenSecret, tokenHeader);
                if (__res.ProfileID != null)
                {
                    DNCreationParam __bodytoken = new()
                    {
                        IsDNPromo = body.IsDNPromo,
                        Id = body.Id,
                        Periode = body.Periode,
                        EntityId = body.EntityId,
                        DistributorId = body.DistributorId,
                        ActivityDesc = body.ActivityDesc,
                        AccountId = body.AccountId,
                        PromoId = body.PromoId,
                        IntDocNo = body.IntDocNo,
                        MemDocNo = body.MemDocNo,
                        DPP = body.DPP,
                        DNAmount = body.DNAmount,
                        FeeDesc = body.FeeDesc,
                        FeePct = body.FeePct,
                        FeeAmount = body.FeeAmount,
                        PPNPct = body.PPNPct,
                        DeductionDate = body.DeductionDate,
                        UserId = __res.ProfileID,
                        TaxLevel = body.TaxLevel,
                        WHTType = body.WHTType,
                        sellpoint = new List<DNSellpoint>()
                    };
                    foreach (var item in body.sellpoint!)
                    {
                        __bodytoken.sellpoint.Add(new DNSellpoint
                        {
                            flag = item.flag,
                            sellpoint = item.sellpoint,
                            LongDesc = item.LongDesc
                        });
                    }
                    __bodytoken.dnattachment = new List<DNAttachment>();
                    foreach (var item in body.dnattachment!)
                    {
                        __bodytoken.dnattachment.Add(new DNAttachment
                        {
                            DocLink = item.DocLink,
                            FileName = item.FileName
                        });
                    }
                    __bodytoken.pphPct = body.pphPct;
                    __bodytoken.pphAmt = body.pphAmt;
                    __bodytoken.statusPPH = body.statusPPH;
                    __bodytoken.FPNumber = body.FPNumber;
                    __bodytoken.FPDate = body.FPDate;
                    __bodytoken.statusPPN = body.statusPPN;

                    var __resValidation = await __repoDNCreationHO.GetDebetnoteUpdateValidate(__bodytoken);
                    if (__resValidation.errorcode == 2)
                    {
                        var __resDN = await __repoDNCreationHO.UpdateDN(__bodytoken);
                        return Ok(new BaseResponse
                        {
                            code = 200,
                            error = false,
                            message = MessageService.SaveSuccess,
                            values = new
                            {
                                Id = body.Id,
                                refId = __resDN.RefId,
                                approverUserid = __resDN.approver_userid,
                                approverUserName = __resDN.approver_username,
                                approverEmail = __resDN.approver_email
                            }
                        });
                    }
                    else
                    {
                        return Ok(new BaseResponse
                        {
                            code = 666,
                            error = true,
                            message = __resValidation.messageout,
                            values = new
                            {
                                Id = __resValidation.id,
                                distributorId = __resValidation.DistributorId,
                                distributor = __resValidation.distributor,
                                entity = __resValidation.entity,
                                activityDesc = __resValidation.ActivityDesc,
                                dpp = __resValidation.DPP,
                                intdocno = __resValidation.IntDocNo,
                                refId = __resValidation.refid
                            }
                        });
                    }
                }
                else
                {
                    result = StatusCode(StatusCodes.Status403Forbidden, new BaseResponse { error = true, code = 403, message = MessageService.EmailTokenFailed });
                }
            }
            catch (Exception __ex)
            {
                return Ok(new BaseResponse { error = true, code = 500, message = __ex.Message });
            }
            return result;
        }
        /// <summary>
        /// DN post attachment, Old API = "dnattachment/store"
        /// </summary>
        /// <returns></returns>
        [HttpPost("api/dn/creation/ho/dnattachment", Name = "dnho_attachstore")]
        public async Task<IActionResult> CreateDNHOAttachment([FromBody] DNAttachmentBody body)
        {
            try
            {
                if (!ModelState.IsValid) return Conflict(ModelState);
                string tokenHeader = Request.Headers["Authorization"]!;
                tokenHeader = tokenHeader.Replace("Bearer ", "");
                var __res = TokenManager.GetClaim(__TokenSecret, tokenHeader);
                if (__res.ProfileID != null)
                {
                    DNAttachmentBody __bodytoken = new()
                    {
                        DNId = body.DNId,
                        DocLink = body.DocLink,
                        FileName = body.FileName,
                        CreateBy = __res.ProfileID
                    };
                    var __val = await __repoDNCreationHO.CreateDNAttachment(__bodytoken);
                    return Ok(new BaseResponse { code = 200, error = false, message = "Store data " + __val.RefId + "and " + __val.Id + " success", values = __val });
                }
                else
                {
                    return NotFound(new BaseResponse { error = true, code = 404, message = MessageService.EmailTokenFailed });
                }
            }
            catch (System.Exception __ex)
            {
                return Conflict(new BaseResponse { code = 400, error = true, message = __ex.Message });
            }
        }
        // dnattachment/delete
        /// <summary>
        /// DN delete attachment , Old API = "dnattachment/delete"
        /// </summary>
        /// <returns></returns>
        [HttpDelete("api/dn/creation/ho/dnattachment", Name = "dn_ho_attach_delete")]
        public async Task<IActionResult> DeleteDNHOAttachment([FromBody] DNAttachmentBody body)
        {
            try
            {
                if (!ModelState.IsValid) return Conflict(ModelState);
                // get token from request headers
                await __repoDNCreationHO.DeleteDNAttachment(body);
                return Ok(
                    new BaseResponse
                    {
                        code = 200,
                        error = false,
                        message = MessageService.DeleteSucceed,
                        values = null
                    });
            }
            catch (Exception __ex)
            {
                return Conflict(new BaseResponse { code = 400, error = true, message = __ex.Message });
            }
        }
        // debetnote/print/        
        /// <summary>
        /// DN Print", Old API = "debetnote/print/"
        /// </summary>
        /// <returns></returns>
        [HttpGet("api/dn/creation/ho/print", Name = "debetnote_ho_print")]
        public async Task<IActionResult> DNHOPrint([FromQuery] int id)
        {
            try
            {
                if (!ModelState.IsValid) return Conflict(ModelState);
                var __val = await __repoDNCreationHO.DNPrint(id);
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
                    return NotFound(new BaseResponse { code = 404, error = true, message = MessageService.DataNotFound });
                }
            }
            catch (Exception __ex)
            {
                return Conflict(new BaseResponse { code = 500, error = true, message = __ex.Message });
            }
        }
        /// <summary>
        /// Get List Entity for DN Creation HO
        /// </summary>
        /// <returns></returns>
        [HttpGet("api/dn/creation/ho/entity", Name = "dn_creation_ho_entity_list")]
        public async Task<IActionResult> GetEntityDNCreationHO()
        {
            try
            {
                if (!ModelState.IsValid) return Conflict(ModelState);
                var __val = await __repoDNCreationHO.GetEntityList();
                if (__val != null)
                {
                    return Ok(new BaseResponse
                    {
                        error = false,
                        code = 200,
                        message = MessageService.GetDataSuccess,
                        values = __val
                    });
                }
                else
                {
                    return NotFound(new BaseResponse { error = true, code = 404, message = MessageService.EmailTokenFailed });
                }
            }
            catch (Exception __ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new BaseResponse { error = true, code = 500, message = __ex.Message });

            }
        }
        /// <summary>
        /// Get List Sub Account for DN Creation HO
        /// </summary>
        /// <returns></returns>
        [HttpGet("api/dn/creation/ho/subaccount", Name = "dn_creation_ho_subaccount_list")]
        public async Task<IActionResult> GetSubAccountDNCreationHO()
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);

                string tokenHeader = Request.Headers["Authorization"]!;
                tokenHeader = tokenHeader.Replace("Bearer ", "");
                var __res = TokenManager.GetClaim(__TokenSecret, tokenHeader);
                if (__res.ProfileID != null)
                {
                    var __val = await __repoDNCreationHO.GetSubAccountList(__res.ProfileID);
                    return Ok(new BaseResponse
                    {
                        error = false,
                        code = 200,
                        message = MessageService.GetDataSuccess,
                        values = __val
                    });
                }
                else
                {
                    return NotFound(new BaseResponse { error = true, code = 404, message = MessageService.EmailTokenFailed });
                }
            }
            catch (Exception __ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new BaseResponse { error = true, code = 500, message = __ex.Message });

            }
        }
        /// <summary>
        /// Get List Channel for DN Creation HO
        /// </summary>
        /// <returns></returns>
        [HttpGet("api/dn/creation/ho/channel", Name = "dn_creation_ho_channel")]
        public async Task<IActionResult> GetChannelListDNCreationHO()
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);
                string tokenHeader = Request.Headers["Authorization"]!;
                tokenHeader = tokenHeader.Replace("Bearer ", "");
                var __res = TokenManager.GetClaim(__TokenSecret, tokenHeader);
                List<int> arrayParent = new();
                var __val = await __repoDNCreationHO.GetChannelList(__res.ProfileID!, arrayParent.ToArray());
                if (__val != null)
                {
                    return Ok(new BaseResponse
                    {
                        error = false,
                        code = 200,
                        message = MessageService.GetDataSuccess,
                        values = __val
                    });
                }
                else
                {
                    return NotFound(new BaseResponse { error = true, code = 404, message = MessageService.EmailTokenFailed });
                }
            }
            catch (Exception __ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new BaseResponse { error = true, code = 500, message = __ex.Message });
            }
        }

        /// <summary>
        /// Get WHTType for DN Creation HO"
        /// </summary>
        /// <param name="promoId"></param>
        /// <returns></returns>
        [HttpGet("api/dn/creation/ho/whttype", Name = "debetnote_ho_whttype")]
        public async Task<IActionResult> GetDNCreationHOGetWHTType([FromQuery] int promoId)
        {
            try
            {
                if (!ModelState.IsValid) return Conflict(ModelState);
                var __val = await __repoDNCreationHO.GetDNCreationHOGetWHTType(promoId);
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
                    return Conflict(new BaseResponse { code = 404, error = true, message = MessageService.EmailTokenFailed });
                }
            }
            catch (Exception __ex)
            {
                return Conflict(new BaseResponse { code = 500, error = true, message = __ex.Message });
            }
        }
    }
}
