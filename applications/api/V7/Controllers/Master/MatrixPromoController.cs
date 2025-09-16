
using Microsoft.AspNetCore.Mvc;
using Repositories.Entities.Models;
using V7.MessagingServices;
using V7.Model.Master;
using V7.Services;

namespace V7.Controllers.Master
{
    public partial class MasterController : BaseController
    {
        /// <summary>
        /// Get matrix promo approval landing page data 
        /// </summary>
        /// <param name="body"></param>
        /// <returns></returns>
        [HttpGet("api/master/matrixpromoapproval", Name = "get_promoapproval_lp")]
        public async Task<IActionResult> GetMatrixPromoAproval([FromQuery] MatrixPromoApprovalBodyReq body)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);
                var __val = await __repoMatrixPromo.GetMatrixPromoAproval(body);
                if (__val == null)
                {
                    return Conflict(new BaseResponse { code = 404, error = true, message = MessageService.GetDataFailed });
                }
                else
                {
                    return Ok(new { error = false, code = 200, message = MessageService.GetDataSuccess, values = __val });
                }

            }
            catch (Exception __ex)
            {
                return Conflict(new BaseResponse { code = 500, error = true, message = __ex.Message });
            }
        }
        /// <summary>
        /// Get Matrix approval history
        /// </summary>
        /// <param name="body"></param>
        /// <returns></returns>
        [HttpGet("api/master/matrixpromoapproval/history", Name = "get_promoapproval_history")]
        public async Task<IActionResult> GetMatrixPromoAprovalHistory([FromQuery] MatrixPromoApprovalHistoryListReq body)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);
                var __val = await __repoMatrixPromo.GetMatrixPromoAprovalHistory(body.category, body.entity, body.distributor, 
                    body.userid!, body.PageNumber, body.PageSize, body.txtSearch!, body.order!, body.sort!);
                if (__val == null)
                {
                    return Conflict(new BaseResponse { code = 404, error = true, message = MessageService.GetDataFailed });
                }
                else
                {
                    return Ok(new { error = false, code = 200, message = MessageService.GetDataSuccess, values = __val });
                }

            }
            catch (Exception __ex)
            {
                return Conflict(new BaseResponse { code = 500, error = true, message = __ex.Message });
            }
        }
        /// <summary>
        /// create matrix promo approval data 
        /// </summary>
        /// <param name="body"></param>
        /// <returns></returns>
        [HttpPost("api/master/matrixpromoapproval", Name = "promoapproval_insert_store")]
        public async Task<IActionResult> CreateMatrixPromoAproval([FromBody] MatrixPromoApprovalInsert body)
        {
            try
            {
                string tokenHeader = Request.Headers["Authorization"]!;
                tokenHeader = tokenHeader.Replace("Bearer ", "");
                var __res = TokenManager.GetClaim(__TokenSecret, tokenHeader);

                if (__res.ProfileID != null)
                {
                    MatrixPromoApprovalInsert __bodyToken = new()
                    {
                        //EP1 2024 #143 
                        //  periode = body.periode,
                        entityid = body.entityid,
                        distributorid = body.distributorid,
                        subactivitytypeid = body.subactivitytypeid,
                        channelid = body.channelid,
                        subchannelid = body.subchannelid,
                        initiator = body.initiator,
                        mininvestment = body.mininvestment,
                        maxinvestment = body.maxinvestment,
                        userid = __res.ProfileID,
                        useremail = __res.UserEmail,
                        categoryId = body.categoryId,
                        matrixApprover = new List<MatrixApproverDetail>()
                    };
                    foreach (var item in body.matrixApprover!)
                    {
                        __bodyToken.matrixApprover.Add(new MatrixApproverDetail
                        {
                            SeqApproval = item.SeqApproval,
                            Approver = item.Approver
                        });
                    }

                    var __resp = await __repoMatrixPromo.CreateMatrixPromoAproval(__bodyToken);
                    return Ok(new BaseResponse
                    {
                        error = false,
                        code = 200,
                        values = __resp,
                        message = MessageService.SaveSuccess
                    });
                }
                else
                {
                    return Conflict(new BaseResponse { error = true, code = 404, message = MessageService.EmailTokenFailed });
                }
            }
            catch (Exception __ex)
            {
                return Conflict(new BaseResponse { error = true, code = 555, message = __ex.Message });
            }
        }
        /// <summary>
        /// Modified matrix promo approval
        /// </summary>
        /// <param name="body"></param>
        /// <returns></returns>
        [HttpPut("api/master/matrixpromoapproval", Name = "promoapproval_insert_update")]
        public async Task<IActionResult> UpdateMatrixPromoAproval([FromBody] MatrixPromoApprovalUpdate body)
        {
            try
            {
                string tokenHeader = Request.Headers["Authorization"]!;
                tokenHeader = tokenHeader.Replace("Bearer ", "");
                var __res = TokenManager.GetClaim(__TokenSecret, tokenHeader);

                if (__res.ProfileID != null)
                {
                    MatrixPromoApprovalUpdate __bodyToken = new()
                    {
                        id = body.id,
                        periode = body.periode,
                        entityid = body.entityid,
                        distributorid = body.distributorid,
                        subactivitytypeid = body.subactivitytypeid,
                        channelid = body.channelid,
                        subchannelid = body.subchannelid,
                        initiator = body.initiator,
                        mininvestment = body.mininvestment,
                        maxinvestment = body.maxinvestment,
                        userid = __res.ProfileID,
                        useremail = __res.UserEmail,
                        categoryId = body.categoryId,
                        matrixApprover = new List<MatrixApproverDetail>()
                    };
                    foreach (var item in body.matrixApprover!)
                    {
                        __bodyToken.matrixApprover.Add(new MatrixApproverDetail
                        {
                            SeqApproval = item.SeqApproval,
                            Approver = item.Approver
                        });
                    }
                    var __resp = await __repoMatrixPromo.UpdateMatrixPromoAproval(__bodyToken);
                    return Ok(new BaseResponse
                    {
                        error = false,
                        code = 200,
                        values = __resp,
                        message = MessageService.UpdateSuccess
                    });
                }
                else
                {
                    return Conflict(new BaseResponse { error = true, code = 404, message = MessageService.EmailTokenFailed });
                }
            }
            catch (Exception __ex)
            {
                return Conflict(new BaseResponse { error = true, code = 555, message = __ex.Message });
            }
        }
        /// <summary>
        /// Get matrix promo approval data base on matrix promo approval Id
        /// </summary>
        /// <param name="body"></param>
        /// <returns></returns>
        [HttpGet("api/master/matrixpromoapproval/id", Name = "get_promoapproval_lp_byid")]
        public async Task<IActionResult> GetMatrixPromoAprovalbyId([FromQuery] GetMatrixPromoAprovalbyIdBody body)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);
                var __val = await __repoMatrixPromo.GetMatrixPromoAprovalbyId(body);
                if (__val == null)
                {
                    return Conflict(new BaseResponse { code = 404, error = true, message = MessageService.GetDataFailed });
                }
                else
                {
                    return Ok(new BaseResponse { error = false, code = 200, message = "Success Get Data Matrix Promo Approval", values = __val });
                }
            }
            catch (Exception __ex)
            {
                return Conflict(new BaseResponse { code = 500, error = true, message = __ex.Message });

            }
        }
        /// <summary>
        /// Get entity dropdown data
        /// </summary>
        /// <returns></returns>
        [HttpGet("api/master/matrixpromoapproval/entity", Name = "master_matrixpromoapproval_entity_dropdown")]
        public async Task<IActionResult> GetEntityForMatrixPromo()
        {
            try
            {
                if (!ModelState.IsValid) return Conflict(ModelState);
                var __val = await __repoMatrixPromo.GetEntityForMatrixPromo();
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
        /// Get channel dropdown data
        /// </summary>
        /// <returns></returns>
        [HttpGet("api/master/matrixpromoapproval/channel", Name = "master_matrixpromoapproval_channel_dropdown")]
        public async Task<IActionResult> GetChannelforMatrixPromo()
        {
            try
            {
                if (!ModelState.IsValid) return Conflict(ModelState);
                var __val = await __repoMatrixPromo.GetChannelforMatrixPromo();
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
        /// Get distributor dropdown data base on entity id
        /// </summary>
        /// <param name="PrincipalId"></param>
        /// <returns></returns>
        [HttpGet("api/master/matrixpromoapproval/distributor", Name = "master_matrixpromoapproval_distributor_dropdown")]
        public async Task<IActionResult> GetDistributorforMatrixPromo([FromQuery] int PrincipalId)
        {
            try
            {
                if (!ModelState.IsValid) return Conflict(ModelState);
                var __val = await __repoMatrixPromo.GetDistributorforMatrixPromo(PrincipalId);
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
        /// Get initiator dropdown data
        /// </summary>
        /// <returns></returns>
        [HttpGet("api/master/matrixpromoapproval/initiator", Name = "master_matrixpromoapproval_initiator_dropdown")]
        public async Task<IActionResult> GetInitiatorforMatrixPromo()
        {
            try
            {
                if (!ModelState.IsValid) return Conflict(ModelState);
                var __val = await __repoMatrixPromo.GetInitiatorforMatrixPromo();
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
        /// Get activity type dropdown data
        /// </summary>
        /// <returns></returns>
        [HttpGet("api/master/matrixpromoapproval/activitytype", Name = "master_matrixpromoapproval_submatrix promotype_dropdown")]
        public async Task<IActionResult> GetSubActivityTypeforMatrixPromo()
        {
            try
            {
                if (!ModelState.IsValid) return Conflict(ModelState);
                var __val = await __repoMatrixPromo.GetSubActivityTypeforMatrixPromo();
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
        /// Get subchannel dropdown data base on channel id
        /// </summary>
        /// <param name="ChannelId"></param>
        /// <returns></returns>
        [HttpGet("api/master/matrixpromoapproval/subchannel", Name = "master_matrixpromoapproval_subchannel_dropdown")]
        public async Task<IActionResult> GetSubChannelforMatrixPromo([FromQuery] int ChannelId)
        {
            try
            {
                if (!ModelState.IsValid) return Conflict(ModelState);
                var __val = await __repoMatrixPromo.GetSubChannelforMatrixPromo(ChannelId);
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
        /// Get category dropdown data
        /// </summary>
        /// <returns></returns>
        [HttpGet("api/master/matrixpromoapproval/category", Name = "master_matrixpromoapproval_category_dropdown")]
        public async Task<IActionResult> GetCategoryforMatrixPromo()
        {
            try
            {
                if (!ModelState.IsValid) return Conflict(ModelState);
                var __val = await __repoMatrixPromo.GetCategoryforMatrixPromo();
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
        /// Get Sub Activity Type base on category Id dropdown data
        /// </summary>
        /// <returns></returns>
        [HttpGet("api/master/matrixpromoapproval/subactivitytype/categoryid", Name = "master_matrixpromoapproval_subactivitytype_baseon_categoryid_dropdown")]
        public async Task<IActionResult> GetSubActivityTypebyCategoryId([FromQuery] int categoryId)
        {
            try
            {
                if (!ModelState.IsValid) return Conflict(ModelState);
                var __val = await __repoMatrixPromo.GetSubActivityTypebyCategoryId(categoryId);
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
    }
}