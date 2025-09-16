using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Text.Json.Serialization;
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
        /// Get investment type data base on Id
        /// </summary>
        /// <param name="body"></param>
        /// <returns></returns>
        [HttpGet("api/master/investmenttype/id", Name = "master_InvestmentType_id")]
        public async Task<IActionResult> GetInvestmentTypeById([FromQuery] InvestmentTypeById body)
        {
            try
            {
                if (!ModelState.IsValid) return Conflict(ModelState);
                var __val = await __repoInvestmentType.GetInvestmentTypeById(body);
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
                    return Conflict(new BaseResponse{ code = 404, error = true, message = MessageService.GetDataFailed });
                }
            }
            catch (Exception __ex)
            {
                return Conflict(new BaseResponse{ code = 500, error = true, message = __ex.Message });

            }
        }
        /// <summary>
        /// Create investment type data
        /// </summary>
        /// <param name="body"></param>
        /// <returns></returns>
        [HttpPost("api/master/investmenttype", Name = "InvestmentType_store")]
        public async Task<IActionResult> CreateInvestmentType([FromBody] InvestmentTypeCreate body)
        {
            try
            {
                if (!ModelState.IsValid) return Conflict(ModelState);
                string tokenHeader = Request.Headers["Authorization"]!;
                tokenHeader = tokenHeader.Replace("Bearer ", "");
                var __res = TokenManager.GetClaim(__TokenSecret, tokenHeader);
                if (__res.UserEmail != null)
                {
                    InvestmentTypeCreate __bodytoken = new()
                    {
                        RefId = body.RefId,
                        LongDesc = body.LongDesc,
                        CreateBy = __res.ProfileID,
                        CreatedEmail = __res.UserEmail
                    };
                    var __val = await __repoInvestmentType.CreateInvestmentType(__bodytoken);
                    return Ok(new BaseResponse { code = 200, error = false, message = "Store data " + __val.RefId + " success", values = __val });
                }
                else
                {
                    return NotFound(new BaseResponse { error = true, code = 404, message = MessageService.EmailTokenFailed });
                }
            }
            catch (Exception __ex)
            {
                return Conflict(new BaseResponse { code = 400, error = true, message = __ex.Message });
            }
        }
        /// <summary>
        /// Modified investment type data
        /// </summary>
        /// <param name="body"></param>
        /// <returns></returns>
        [HttpPut("api/master/investmenttype", Name = "master_InvestmentType_update")]
        public async Task<IActionResult> UpdateInvestmentType([FromBody] InvestmentTypeUpdate body)
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
                    InvestmentTypeUpdate __bodytoken = new()
                    {
                        Id = body.Id,
                        RefId = body.RefId,
                        LongDesc = body.LongDesc,
                        ModifiedBy = __res.ProfileID,
                        ModifiedEmail = __res.UserEmail
                    };
                    var __val = await __repoInvestmentType.UpdateInvestmentType(__bodytoken);
                    return Ok(new BaseResponse { code = 200, error = false, message = "Update data " + __val.RefId + " success", values = __val });
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
        /// Activating deactivated investment type data
        /// </summary>
        /// <param name="body"></param>
        /// <returns></returns>
        [HttpPost("api/master/investmenttype/activate", Name = "master_InvestmentType_update_activate")]
        public async Task<IActionResult> ActivateInvestmentType([FromBody] InvestmentTypeActivate body)
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
                    InvestmentTypeActivate __bodytoken = new()
                    {
                        Id = body.Id,
                        ModifiedBy = __res.ProfileID,
                        ModifiedEmail = __res.UserEmail
                    };
                    var __val = await __repoInvestmentType.ActivateInvestmentType(__bodytoken);
                    return Ok(new BaseResponse { code = 200, error = false, message = "Update data " + __val.Id + " success", values = __val });
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
        /// Delete investment type data
        /// </summary>
        /// <param name="body"></param>
        /// <returns></returns>
        [HttpDelete("api/master/investmenttype", Name = "master_InvestmentType_delete")]
        public async Task<IActionResult> DeleteInvestmentType([FromBody] InvestmentTypeDelete body)
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
                    InvestmentTypeDelete __bodytoken = new()
                    {
                        Id = body.Id,
                        DeleteBy = __res.ProfileID,
                        DeleteEmail = __res.UserEmail
                    };
                    var __val = await __repoInvestmentType.DeleteInvestmentType(__bodytoken);
                    return Ok(new BaseResponse { code = 200, error = false, message = "Update data " + __val.RefId + " success", values = __val });
                }
                else
                {
                    return NotFound(new BaseResponse { error = true, code = 404, message = MessageService.EmailTokenFailed });
                }
            }
            catch (Exception __ex)
            {
                return Conflict(new BaseResponse { code = 400, error = true, message = __ex.Message });
            }
        }
        /// <summary>
        /// Get investment type data for landing page
        /// </summary>
        /// <param name="body"></param>
        /// <returns></returns>
        [HttpGet("api/master/investmenttype", Name = "master_InvestmentType_lp")]
        public async Task<IActionResult> GetInvestmentTypeLandingPage([FromQuery] InvestmentTypeListRequest body)
        {
            try
            {
                if (!ModelState.IsValid) return Conflict(ModelState);
                var __val = await __repoInvestmentType.GetInvestmentTypeLandingPage(body.Search!, body.SortColumn.ToString(), body.SortDirection.ToString(),
                    body.PageSize, body.PageNumber);
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
                    return Conflict(new BaseResponse{ code = 404, error = true, message = MessageService.EmailTokenFailed });
                }
            }
            catch (Exception __ex)
            {
                return Conflict(new BaseResponse{ code = 500, error = true, message = __ex.Message });

            }
        }
        /// <summary>
        /// Create investment type mapping data
        /// </summary>
        /// <param name="body"></param>
        /// <returns></returns>
        [HttpPost("api/master/investmenttype/mapping", Name = "postinvestment")]
        public async Task<IActionResult> CreateInvestmentTypeMapping([FromBody] InvestmentTypeMappingCreateParam body)
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
                    InvestmentTypeMappingCreate __investmentTypeMapping = new()
                    {
                        investment = new List<InvestmentDataType>(),
                        userid = __res.ProfileID
                    };
                    foreach (var item in body.investmentMap!)
                    {
                        __investmentTypeMapping.investment.Add(new InvestmentDataType
                        {
                            id = item.id,
                            SubActivityId = item.SubActivityId,
                            InvestmentTypeId = item.InvestmentTypeId
                        });
                    }

                    await __repoInvestmentType.CreateInvestmentTypeMapping(__investmentTypeMapping);
                    return Ok(new BaseResponse { code = 200, error = false, message = MessageService.SaveSuccess });
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
        /// Deleted investment type mapping data
        /// </summary>
        /// <param name="body"></param>
        /// <returns></returns>
        [HttpDelete("api/master/investmenttype/mapping", Name = "delete__investment")]
        public async Task<IActionResult> DeleteInvestmentTypeMapping([FromBody] InvestmentTypeMappingDeleteParam body)
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
                    InvestmentTypeMappingDelete __investmentTypeMapping = new()
                    {
                        investment = new List<InvestmentDataType>(),
                        userid = __res.ProfileID
                    };

                    foreach (var item in body.investmentMap!)
                    {
                        __investmentTypeMapping.investment.Add(new InvestmentDataType
                        {
                            id = item.id,
                            SubActivityId = item.SubActivityId,
                            InvestmentTypeId = item.InvestmentTypeId
                        });
                    }

                    await __repoInvestmentType.DeleteInvestmentTypeMapping(__investmentTypeMapping);
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
        /// Get investment type mmaping data base on parameter
        /// </summary>
        /// <param name="activityid"></param>
        /// <param name="subactivity"></param>
        /// <param name="categoryid"></param>
        /// <param name="subcategoryid"></param>
        /// <returns></returns>
        [HttpGet("api/master/investmenttype/mapping", Name = "investmentreport_investmenttype_map")]
        public async Task<IActionResult> InvestmentTypeMap([FromQuery] int activityid, int subactivity, int categoryid, int subcategoryid)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);
                var __val = await __repoInvestmentType.InvestmentTypeMap(activityid, subactivity, categoryid, subcategoryid);
                if (__val != null)
                {
                    return Ok(new { error = false, code = 200, Message = MessageService.GetDataSuccess, values = __val });
                }
                else
                {
                    return Conflict(new { error = true, code = 404, Message = MessageService.GetDataFailed });
                }
            }
            catch (System.Exception __ex)
            {
                return Conflict(new { code = 500, error = true, Message = __ex.Message });
            }
        }
        /// <summary>
        /// Get category dropdown data  
        /// </summary>
        /// <returns></returns>
        [HttpGet("api/master/investmenttype/mapping/category", Name = "master_investmenttype_category_dropdown")]
        public async Task<IActionResult> GetCategoryforInvestmentMap()
        {
            try
            {
                if (!ModelState.IsValid) return Conflict(ModelState);
                var __val = await __repoInvestmentType.GetCategoryforInvestmentMap();
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
                    return Conflict(new BaseResponse{ code = 404, error = true, message = MessageService.GetDataFailed });
                }
            }
            catch (Exception __ex)
            {
                return Conflict(new BaseResponse{ code = 500, error = true, message = __ex.Message });

            }
        }
        /// <summary>
        /// Get subcategory for dropdown data base on category id 
        /// </summary>
        /// <param name="CategoryId"></param>
        /// <returns></returns>
        [HttpGet("api/master/investmenttype/mapping/subcategory", Name = "master_investmenttype_subcategory_dropdown")]
        public async Task<IActionResult> GetSubCategoryInvestmentMap([FromQuery] int CategoryId)
        {
            try
            {
                if (!ModelState.IsValid) return Conflict(ModelState);
                var __val = await __repoInvestmentType.GetSubCategoryInvestmentMap(CategoryId);
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
                    return Conflict(new BaseResponse{ code = 404, error = true, message = MessageService.GetDataFailed });
                }
            }
            catch (Exception __ex)
            {
                return Conflict(new BaseResponse{ code = 500, error = true, message = __ex.Message });

            }
        }
        /// <summary>
        /// Get activity dropdown data base on sub category id
        /// </summary>
        /// <param name="SubCategoryId"></param>
        /// <returns></returns>
        [HttpGet("api/master/investmenttype/mapping/activity", Name = "master_investmenttype_activity_dropdown")]
        public async Task<IActionResult> GetActivityInvestmentMap([FromQuery] int SubCategoryId)
        {
            try
            {
                if (!ModelState.IsValid) return Conflict(ModelState);
                var __val = await __repoInvestmentType.GetActivityInvestmentMap(SubCategoryId);
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
                    return Conflict(new BaseResponse{ code = 404, error = true, message = MessageService.GetDataFailed });
                }
            }
            catch (Exception __ex)
            {
                return Conflict(new BaseResponse{ code = 500, error = true, message = __ex.Message });

            }
        }
        /// <summary>
        /// Get subactivity dropdown data
        /// </summary>
        /// <param name="ActivityId"></param>
        /// <returns></returns>
        [HttpGet("api/master/investmenttype/mapping/subactivity", Name = "master_investmenttype_subactivity_dropdown")]
        public async Task<IActionResult> GetSubActivityInvestmentMap([FromQuery] int ActivityId)
        {
            try
            {
                if (!ModelState.IsValid) return Conflict(ModelState);
                var __val = await __repoInvestmentType.GetSubActivityInvestmentMap(ActivityId);
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
                    return Conflict(new BaseResponse{ code = 404, error = true, message = MessageService.GetDataFailed });
                }
            }
            catch (Exception __ex)
            {
                return Conflict(new BaseResponse{ code = 500, error = true, message = __ex.Message });

            }
        }
        /// <summary>
        /// Get investment type dropdown data
        /// </summary>
        /// <returns></returns>
        [HttpGet("api/master/investmenttype/mapping/investmenttype", Name = "master_investmenttype_investmenttype_dropdown")]
        public async Task<IActionResult> InvestmentTypeInvestmentMap()
        {
            try
            {
                if (!ModelState.IsValid) return Conflict(ModelState);
                var __val = await __repoInvestmentType.InvestmentTypeInvestmentMap();
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
                    return Conflict(new BaseResponse{ code = 404, error = true, message = MessageService.GetDataFailed });
                }
            }
            catch (Exception __ex)
            {
                return Conflict(new BaseResponse { code = 500, error = true, message = __ex.Message });

            }
        }
    }
}