using Microsoft.AspNetCore.Mvc;
using Repositories.Entities.Models;
using V7.MessagingServices;
using V7.Model.DebitNote;
using V7.Services;

namespace V7.Controllers.DebitNote
{
    /// <summary>
    /// DN Creation
    /// </summary>
    public partial class DebitNoteController : BaseController
    {
        // debetnote_p/list/
        /// <summary>
        /// Get DebitNote Landing Page, Old API = "debetnote_p/list/"
        /// </summary>
        /// <returns></returns>
        [HttpGet("api/dn/creation", Name = "debetnote_list_p")]
        public async Task<IActionResult> GetDNListLandingPage([FromQuery] DNLandingPageParam param)
        {
            IActionResult result;
            try
            {
                string tokenHeader = Request.Headers["Authorization"]!;
                tokenHeader = tokenHeader.Replace("Bearer ", "");
                var __res = TokenManager.GetClaim(__TokenSecret, tokenHeader);

                if (__res.ProfileID != null)
                {
                    var __val = await __repoDNCreation.GetDNListLandingPage(
                        param.Period!,
                        param.EntityId,
                        param.DistributorId,
                        param.ChannelId,
                        param.AccountId,
                        __res.ProfileID,
                        param.IsDNManual,
                        param.Search!,
                        param.SortColumn.ToString(),
                        param.PageNumber,
                        param.PageSize,
                        param.SortDirection.ToString()
                    );
                    result = Ok(new BaseResponse
                    {
                        error = false,
                        code = 200,
                        message = MessageService.GetDataSuccess,
                        values = __val
                    });

                }
                else
                {
                    return NotFound(new { error = true, code = 404, message = MessageService.EmailTokenFailed });
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
        // debetnote/getbyId
        /// <summary>
        /// Get DN by Id, Old API = "debetnote/getbyId"
        /// </summary>
        /// <returns></returns>
        [HttpGet("api/dn/creation/id", Name = "debetnote_list_getbyid")]
        public async Task<IActionResult> GetDebetNoteById([FromQuery] int id)
        {
            IActionResult result;
            List<string> EmptyList = new();
            try
            {
                if (!ModelState.IsValid) return Conflict(ModelState);
                var __val = await __repoDNCreation.GetDebetNoteById(id);
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
        /// DN Cancel, Old API = "debetnote/cancel"
        /// </summary>
        /// <returns></returns>
        [HttpPost("api/dn/creation/cancel", Name = "dn_cancel")]
        public async Task<IActionResult> CancelDN([FromBody] DNCancelBody body)
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
                    var __val = await __repoDNCreation.CancelDN(__bodytoken);
                    return Ok(new BaseResponse { code = 200, error = false, message = "DN with Refid = " + __val.RefId + " successfully cancel", values = __val });
                }
                else
                {
                    return NotFound(new BaseResponse { code = 404, error = true, message = MessageService.EmailTokenFailed });
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
        [HttpGet("api/dn/creation/attribute/user", Name = "all_attribute_byuser")]
        public async Task<IActionResult> GetAttributeByUser()
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
                    var __val = await __repoDNCreation.GetAttributeByUser(__bodytoken.userid);
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
                    return Ok(new BaseResponse { error = true, code = 404, message = MessageService.DataNotFound });
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
        [HttpGet("api/dn/creation/distributor-entity/user", Name = "all_distributor-entity_byuser")]
        public async Task<IActionResult> GetDistributorEntityByUserId()
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
                    var __val = await __repoDNCreation.GetDistributorEntityByUserId(__bodytoken.userid);
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
        [HttpGet("api/dn/creation/sellingpoint/user", Name = "all_sellingpoint_byuser")]
        public async Task<IActionResult> GetSellingPointByUser()
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
                    var __val = await __repoDNCreation.GetSellingPointByUser(__bodytoken.userid);
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
        //  mapmaterial/all
        /// <summary>
        /// Get TaxLevel for DN Creation, Old API = "mapmaterial/all"
        /// </summary>
        /// <returns></returns>
        [HttpGet("api/dn/creation/taxlevel", Name = "debetnote_material")]
        public async Task<IActionResult> GetTaxLevel()
        {
            try
            {
                if (!ModelState.IsValid) return Conflict(ModelState);
                var __val = await __repoDNCreation.GetTaxLevel();
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

        /// <summary>
        /// Get TaxLevel for DN by Entity Id, Old API = "dncreation/taxlevel?entityid"
        /// </summary>
        /// <param name="entityid"></param>
        /// <param name="whtType">Non WHT Object, WHT Deduct, WHT No Deduct or Empty</param>
        /// <returns></returns>
        [HttpGet("api/dn/creation/taxlevel/entityid", Name = "dncreation_taxlevel")]
        public async Task<IActionResult> GetDNCreationTaxlevelByEntity([FromQuery] string entityid, string whtType)
        {
            try
            {
                var __val = await __repoDNCreation.DNCreationTaxLevel(entityid, whtType);
                if (__val != null && __val.Count > 0)
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
                    return Conflict(new BaseResponse
                    {
                        code = 404,
                        error = true,
                        message = MessageService.GetDataFailed,
                        values = __val
                    });
                }
            }
            catch (Exception __ex)
            {
                return Conflict(new BaseResponse
                {
                    code = 500,
                    error = true,
                    message = __ex.Message,
                    values = null
                });
            }
        }
        // debetnote/store
        /// <summary>
        /// Create DN, Old API = "debetnote/store"
        /// </summary>
        /// <returns></returns>
        [HttpPost("api/dn/creation", Name = "dn_storevlidate")]
        public async Task<IActionResult> CreateDN([FromBody] DNCreationParam body)
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

                    var __resValidation = await __repoDNCreation.GetDebetnoteStoreValidate(__bodytoken);
                    if (__resValidation.errorcode == 2)
                    {
                        var __resDN = await __repoDNCreation.CreateDN(__bodytoken);
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
        [HttpPut("api/dn/creation", Name = "dn_updatevalidate")]
        public async Task<IActionResult> UpdateDN([FromBody] DNCreationParam body)
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

                    var __resValidation = await __repoDNCreation.GetDebetnoteUpdateValidate(__bodytoken);
                    if (__resValidation.errorcode == 2)
                    {
                        var __resDN = await __repoDNCreation.UpdateDN(__bodytoken);
                        return Ok(new BaseResponse
                        {
                            code = 200,
                            error = false,
                            message = MessageService.SaveSuccess,
                            values = new
                            {
                                id = body.Id,
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
                                Distributor = __resValidation.distributor,
                                Entity = __resValidation.entity,
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
        // dnattachment/store
        /// <summary>
        /// DN post attachment, Old API = "dnattachment/store"
        /// </summary>
        /// <returns></returns>
        [HttpPost("api/dn/creation/dnattachment", Name = "dn_attachstore")]
        public async Task<IActionResult> CreateDNAttachment([FromBody] DNAttachmentBody body)
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
                    var __val = await __repoDNCreation.CreateDNAttachment(__bodytoken);
                    return Ok(new BaseResponse { code = 200, error = false, message = "Store data " + __val.RefId + "and " + __val.Id + " success", values = __val });
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
        // dnattachment/delete
        /// <summary>
        /// DN delete attachment , Old API = "dnattachment/delete"
        /// </summary>
        /// <returns></returns>
        [HttpDelete("api/dn/creation/dnattachment", Name = "dn_attach_delete")]
        public async Task<IActionResult> DeleteDNAttachment([FromBody] DNAttachmentBody body)
        {
            try
            {
                if (!ModelState.IsValid) return Conflict(ModelState);
                // get token from request header
                await __repoDNCreation.DeleteDNAttachment(body);
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
        [HttpGet("api/dn/creation/print", Name = "debetnote_print")]
        public async Task<IActionResult> DNPrint([FromQuery] int id)
        {
            try
            {
                if (!ModelState.IsValid) return Conflict(ModelState);
                var __val = await __repoDNCreation.DNPrint(id);
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
        /// Get List Entity
        /// </summary>
        /// <returns></returns>
        [HttpGet("api/dn/creation/entity", Name = "dn_creation_entity_list")]
        public async Task<IActionResult> GetEntityDNCreation()
        {
            try
            {
                if (!ModelState.IsValid) return Conflict(ModelState);
                var __val = await __repoDNCreation.GetEntityList();
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
        /// Get List Sub Account
        /// </summary>
        /// <returns></returns>
        [HttpGet("api/dn/creation/subaccount", Name = "dn_creation_subaccount_list")]
        public async Task<IActionResult> GetSubAccountDNCreation()
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);

                string tokenHeader = Request.Headers["Authorization"]!;
                tokenHeader = tokenHeader.Replace("Bearer ", "");
                var __res = TokenManager.GetClaim(__TokenSecret, tokenHeader);
                if (__res.ProfileID != null)
                {
                    var __val = await __repoDNCreation.GetSubAccountList(__res.ProfileID);
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
        /// Get List Channel for DN Creation
        /// </summary>
        /// <returns></returns>
        [HttpGet("api/dn/creation/channel", Name = "dn_creation_channel")]
        public async Task<IActionResult> GetChannelListDNCreation()
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);
                string tokenHeader = Request.Headers["Authorization"]!;
                tokenHeader = tokenHeader.Replace("Bearer ", "");
                var __res = TokenManager.GetClaim(__TokenSecret, tokenHeader);
                List<int> arrayParent = new();
                var __val = await __repoDNCreation.GetChannelList(__res.ProfileID!, arrayParent.ToArray());
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
        // promo/getPromoForDn/
        /// <summary>
        /// DN list approved promo for DN Creation, old API = "promo/getPromoForDn"
        /// </summary>
        /// <param name="periode"></param>
        /// <param name="entity"></param>
        /// <param name="channel"></param>
        /// <param name="account"></param>
        /// <returns></returns>
        [HttpGet("api/dn/creation/approvedpromo-for-dn", Name = "dn_creation_get_approved_promo")]
        public async Task<IActionResult> GetApprovedPromoforDNCreation([FromQuery] string periode, int entity, int channel, int account)
        {
            IActionResult result;
            try
            {
                string tokenHeader = Request.Headers["Authorization"]!;
                tokenHeader = tokenHeader.Replace("Bearer ", "");
                var __res = TokenManager.GetClaim(__TokenSecret, tokenHeader);
                if (__res.ProfileID != null)
                {
                    var __val = await __repoDNCreation.GetApprovedPromoforDNCreation(
                        periode,
                        entity,
                        channel,
                        account,
                        __res.ProfileID
                    );
                    if (__val.Count != 0 && __val != null)
                    {
                        result = Ok(new BaseResponse
                        {
                            error = false,
                            code = 200,
                            message = MessageService.GetDataSuccess,
                            values = __val
                        });
                    }
                    else
                    {
                        return Ok(new BaseResponse { error = false, code = 200, message = MessageService.DataNotFound, values = __val });
                    }
                }
                else
                {
                    return result = StatusCode(StatusCodes.Status407ProxyAuthenticationRequired, new BaseResponse
                    {
                        error = true,
                        code = 500,
                        message = MessageService.EmailTokenFailed
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

        /// <summary>
        /// Get WHTType for DN Creation"
        /// </summary>
        /// <param name="promoId"></param>
        /// <returns></returns>
        [HttpGet("api/dn/creation/whttype", Name = "debetnote_whttype")]
        public async Task<IActionResult> GetDNCreationGetWHTType([FromQuery] int promoId)
        {
            try
            {
                if (!ModelState.IsValid) return Conflict(ModelState);
                var __val = await __repoDNCreation.GetDNCreationGetWHTType(promoId);
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