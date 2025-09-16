using Microsoft.AspNetCore.Mvc;
using Repositories.Entities.Models;
using V7.MessagingServices;
using V7.Model.Tools;
using V7.Services;

namespace V7.Controllers.Tools
{
    public partial class ToolsController : BaseController
    {
        /// <summary>
        /// Generate XML data
        /// </summary>
        /// <param name="body"></param>
        /// <returns></returns>
        [HttpGet("api/tools/xmlgenerate", Name = "xmlgenerate")]
        public async Task<IActionResult> GetXMLGenerateDtos([FromQuery] EntityParam body)
        {
            try
            {
                if (!ModelState.IsValid) return Conflict(ModelState);
                var __val = await __xmlPaymentRepo.GetXMLGenerate(body);
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
        /// Generate XML flagging data, Old API = [POST]"xmlgenerate/flagging"
        /// </summary>
        /// <param name="body"></param>
        /// <returns></returns>
        [HttpPost("api/tools/xmlgenerate/flagging", Name = "xmlgenerate_flagging")]
        public async Task<IActionResult> GenerateXMLFlagging([FromBody] XmlFlaggingBody body)
        {
            try
            {
                if (!ModelState.IsValid) return Conflict(ModelState);
                string tokenHeader = Request.Headers["Authorization"]!;
                tokenHeader = tokenHeader.Replace("Bearer ", "");
                var __res = TokenManager.GetClaim(__TokenSecret, tokenHeader);
                if (__res.UserEmail != null)
                {
                    XmlFlaggingBody __xmlGenerateParam = new()
                    {
                        userid = __res.ProfileID!,
                        useremail = __res.UserEmail,
                        PoNumber = new List<PoNumber>()
                    };
                    foreach (var v in body.PoNumber!)
                    {
                        __xmlGenerateParam.PoNumber.Add(new PoNumber
                        {
                            PONumber = v.PONumber,
                            OriginalId = v.OriginalId,
                            entityId = v.entityId
                        });
                    }
                    var __val = await __xmlPaymentRepo.GenerateXMLFlagging(__xmlGenerateParam);
                    return Ok(new BaseResponse
                    {
                        code = 200,
                        error = false,
                        message = MessageService.SaveSuccess,
                        values = __val
                    });
                }
                else
                {
                    return Conflict(new BaseResponse { code = 404, error = true, message = MessageService.SaveFailed });
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
        /// Generate XML accrual data
        /// </summary>
        /// <param name="body"></param>
        /// <returns></returns>
        [HttpGet("api/tools/xmlgenerate/accrual", Name = "xmlgenerateaccrual")]
        public async Task<IActionResult> GetXmlGenerateAccrual([FromQuery] XmlGenerateAccrualBody body)
        {
            try
            {
                if (!ModelState.IsValid) return Conflict(ModelState);
                var __val = await __xmlPaymentRepo.GetXmlGenerateAccrual(body);
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
        /// Generate XML non batch data
        /// </summary>
        /// <param name="body"></param>
        /// <returns></returns>
        [HttpGet("api/tools/xmlgenerate/nonbatch", Name = "xmlgeneratenonbatch")]
        public async Task<IActionResult> GetXMLGenerateNonBatch([FromQuery] EntityParam body)
        {
            try
            {
                if (!ModelState.IsValid) return Conflict(ModelState);
                var __val = await __xmlPaymentRepo.GetXMLGenerateNonBatch(body);
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
        /// Generate XML non batch distributor payment data
        /// </summary>
        /// <param name="body"></param>
        /// <returns></returns>
        [HttpGet("api/tools/xmlgenerate/nonbatch-distributorpayment", Name = "xmlgeneratenonbatchnonbatchdispayment")]
        public async Task<IActionResult> GetXMLGenerateNonBatchDistributorPayment([FromQuery] EntityParam body)
        {
            try
            {
                if (!ModelState.IsValid) return Conflict(ModelState);
                var __val = await __xmlPaymentRepo.GetXMLGenerateNonBatchDistributorPayment(body);
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
        /// Generate XML batch payment data
        /// </summary>
        /// <param name="body"></param>
        /// <returns></returns>
        [HttpGet("api/tools/xmlgenerate/batch-payment", Name = "xmlgeneratenonbatchnonbatchdispaymentpaymentbatch")]
        public async Task<IActionResult> GenerateXMLPaymentBatch([FromQuery] XMLGeneratePaymentBatchBody body)
        {
            try
            {
                if (!ModelState.IsValid) return Conflict(ModelState);
                var __val = await __xmlPaymentRepo.GenerateXMLPaymentBatch(body);
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
        /// Generate XML accrual by Id, Old EP = "api/xmlgenerate/accrualbyId""
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("api/tools/xmlgenerate/accrual/id", Name = "sap_payment_promoaccrual_byId")]
        public async Task<IActionResult> GenerateXMLAccrualbyId([FromQuery] int id)
        {
            try
            {
                if (!ModelState.IsValid) return Conflict(ModelState);
                var __val = await __xmlPaymentRepo.GenerateXMLAccrualbyId(id);
                if (__val.Count == 0)
                {
                    return NotFound(new BaseResponse
                    {
                        code = 400,
                        error = false,
                        message = MessageService.GetDataFailed,
                        values = (string?)null
                    });
                }
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
        /// Generate XML accrual by Id
        /// </summary>
        /// <param name="entityId"></param>
        /// <param name="userProfileId"></param>
        /// <param name="generateOn"></param>

        /// <returns></returns>
        [HttpGet("api/tools/xmlgenerate/flagging/list", Name = "xmlgeneratenonbatchnonbatchdispaymentpaymentbatchtestbyIdflagginglist")]
        public async Task<IActionResult> GetXMLFlaggingList([FromQuery] int entityId, string? userProfileId, string? generateOn)
        {
            try
            {
                if (!ModelState.IsValid) return Conflict(ModelState);
                userProfileId ??= "0";
                generateOn ??= "";
                var __val = await __xmlPaymentRepo.GetXMLFlaggingList(entityId, userProfileId, generateOn);
                if (__val != null)
                {
                    return Ok(new BaseResponse
                    {
                        code = 200,
                        error = false,
                        message = MessageService.SaveFailed,
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
        /// Update xml generate flagging
        /// </summary>
        /// <param name="body"></param>
        /// <returns></returns>
        [HttpPut("api/tools/xmlgenerate/flagging", Name = "flagging_update")]
        public async Task<IActionResult> XMLFlaggingUpdate([FromBody] XMLFlaggingUpdateParam body)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);
                string tokenHeader = Request.Headers["Authorization"]!;
                tokenHeader = tokenHeader.Replace("Bearer ", "");
                // get user data from token
                var __res = TokenManager.GetClaim(__TokenSecret, tokenHeader);
                if (__res.UserEmail != null)
                {
                    XMLFlaggingUpdateBody __xmlflaggingUpdate = new()
                    {
                        userid = __res.ProfileID!,
                        useremail = __res.UserEmail,
                        UpdateId = new List<XMLGenerateNMNbody>()
                    };
                    foreach (var v in body.updateId!)
                    {
                        __xmlflaggingUpdate.UpdateId.Add(new XMLGenerateNMNbody
                        {
                            id = v.id
                        });
                    }

                    await __xmlPaymentRepo.XMLFlaggingUpdate(__xmlflaggingUpdate);
                    return Ok(new BaseResponse { code = 200, error = false, message = MessageService.DeleteSucceed });
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
        /// Get list data xml upload
        /// </summary>
        /// <param name="body"></param>
        /// <returns></returns>
        [HttpGet("api/tools/xmlgenerate/xmlupload", Name = "xmluploadlist")]
        public async Task<IActionResult> GetXMLUploadList([FromQuery] XMLUploadListBody body)
        {
            try
            {
                if (!ModelState.IsValid) return Conflict(ModelState);
                var __val = await __xmlPaymentRepo.GetXMLUploadList(body);
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
        /// Create xml upload
        /// </summary>
        /// <param name="body"></param>
        /// <returns></returns>
        [HttpPost("api/tools/xmlgenerate/xmlupload", Name = "xmlupload")]
        public async Task<IActionResult> XMLUpload([FromBody] XMLUploadBody body)
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
                    XMLUploadBody __bodytoken = new()
                    {
                        filename = body.filename,
                        uploadtype = body.uploadtype,
                        userid = __res.ProfileID!,
                        useremail = __res.UserEmail
                    };
                    await __xmlPaymentRepo.XMLUpload(__bodytoken);
                    return Ok(new BaseResponse { code = 200, error = false, message = MessageService.SaveSuccess, values = null });
                }
                else
                {
                    return Conflict(new BaseResponse { error = true, code = 404, message = MessageService.EmailTokenFailed });
                }
            }
            catch (Exception __ex)
            {
                return Conflict(new BaseResponse { code = 400, error = true, message = __ex.Message });
            }
        }
        /// <summary>
        /// Get history data xml flagging 
        /// </summary>
        /// <param name="body"></param>
        /// <returns></returns>
        [HttpGet("api/tools/xmlgenerate/flagging/history", Name = "flagginghistory")]
        public async Task<IActionResult> GetXMLFlaggingListHistory([FromQuery] XMLFlaggingHistoryBody body)
        {
            try
            {
                if (!ModelState.IsValid) return Conflict(ModelState);
                var __val = await __xmlPaymentRepo.GetXMLFlaggingListHistory(body);
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
        /// XML generate nmn by Id
        /// </summary>
        /// <param name="body"></param>
        /// <returns></returns>
        [HttpGet("api/tools/xmlgenerate/nmn/id", Name = "XMLGenerateNMNbody")]
        public async Task<IActionResult> GetXMLGenerateNMN([FromQuery] XMLGenerateNMNbody body)
        {
            try
            {
                if (!ModelState.IsValid) return Conflict(ModelState);
                var __val = await __xmlPaymentRepo.GetXMLGenerateNMN(body);
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
        /// XML generate batch name
        /// </summary>
        /// <param name="body"></param>
        /// <returns></returns>
        [HttpGet("api/tools/xmlgenerate/batchname", Name = "GetXMLBatchName")]
        public async Task<IActionResult> GetXMLBatchName([FromQuery] XMLGenerateBatchNameBodyReq body)
        {
            try
            {
                if (!ModelState.IsValid) return Conflict(ModelState);
                var __val = await __xmlPaymentRepo.GetXMLBatchName(body);
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
        /// Get distributor dropdown data base on entity id
        /// </summary>
        /// <param name="PrincipalId"></param>
        /// <returns></returns>
        [HttpGet("api/tools/xmlgenerate/distributor", Name = "get_xml_generate_entity-distributor")]
        public async Task<IActionResult> GetDistributorbyEntity([FromQuery] int PrincipalId)
        {
            try
            {
                if (!ModelState.IsValid) return Conflict(ModelState);
                var __val = await __xmlPaymentRepo.GetDistributorbyEntity(PrincipalId);
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
        /// Get entity dropdown data base on entity id
        /// </summary>
        /// <returns></returns>
        [HttpGet("api/tools/xmlgenerate/entity", Name = "get_xml_generate_entity")]
        public async Task<IActionResult> GetEntityforXMLGenerate()
        {
            try
            {
                if (!ModelState.IsValid) return Conflict(ModelState);
                var __val = await __xmlPaymentRepo.GetEntityforXMLGenerate();
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
        /// Get user profile data for xml generate
        /// </summary>
        /// <param name="usergroupid"></param>
        /// <param name="userlevel"></param>
        /// <param name="isdeleted"></param>
        /// <returns></returns>
        [HttpGet("api/tools/xmlgenerate/flagging/userprofile", Name = "xmlgenerate_userprofile")]
        public async Task<IActionResult> GetUserList([FromQuery]
        string usergroupid = "all",
        int userlevel = 0,
        int isdeleted = 2
        )
        {
            try
            {
                if (!ModelState.IsValid) return Conflict(ModelState);
                var __val = await __xmlPaymentRepo.GetUserList(usergroupid, userlevel, isdeleted);
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
        /// Get Promo Accrual Report Header, Old EP = "promo/accrualreportheader"
        /// </summary>
        /// <param name="periode"></param>
        /// <param name="entityId"></param>
        /// <param name="closingDate"></param>
        /// <returns></returns>
        [HttpGet("api/tools/sap-accrual/promo-accrual", Name = "xml_generate_promoaccrual_report_header")]
        public async Task<IActionResult> GetPromoAccrualHeader([FromQuery] string periode, int entityId, string closingDate)
        {
            try
            {
                if (!ModelState.IsValid) return Conflict(ModelState);
                var __val = await __xmlPaymentRepo.GetPromoAccrualHeader(periode, entityId, closingDate);
                if (__val.Count == 0)
                {
                    return NotFound(new BaseResponse
                    {
                        code = 400,
                        error = false,
                        message = MessageService.GetDataFailed,
                        values = (string?)null
                    });
                }
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