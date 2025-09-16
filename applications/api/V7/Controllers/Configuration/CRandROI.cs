using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Repositories.Contracts;
using Repositories.Entities.Configuration;
using Repositories.Entities.Models;
using System.ComponentModel.DataAnnotations;
using V7.Controllers;
using V7.MessagingServices;
using V7.Model.Configuration;
using V7.Services;

namespace V7.Controllers.Configuration
{
    public partial class ConfigController : BaseController
    {
        /// <summary>
        /// Get List Config ROI CR
        /// </summary>
        /// <returns></returns>
        [HttpGet("api/config/roicr", Name = "config_roicr_list")]
        public async Task<IActionResult> GetConfigRoiList([FromQuery] int CategoryId, int SubCategoryId, int ActivityId)
        {
            try
            {
                if (!ModelState.IsValid) return Conflict(ModelState);
                var __val = await __ROIandCRRepo.GetConfigRoiList(CategoryId, SubCategoryId, ActivityId);
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
        /// Get List Category
        /// </summary>
        /// <returns></returns>
        [HttpGet("api/config/roicr/category", Name = "config_crroi_list_category")]
        public async Task<IActionResult> GetCategory()
        {
            try
            {
                if (!ModelState.IsValid) return Conflict(ModelState);
                var __val = await __ROIandCRRepo.GetCategoryList();
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
        /// Get List Sub Category by Category ID
        /// </summary>
        /// <param name="CategoryId"></param>
        /// <returns></returns>
        [HttpGet("api/config/roicr/subcategory", Name = "config_crroi_list_subcategory")]
        public async Task<IActionResult> GetSubCategoryROI([FromQuery] int CategoryId)
        {
            try
            {
                if (!ModelState.IsValid) return Conflict(ModelState);
                var __val = await __ROIandCRRepo.GetSubCategoryList(CategoryId);
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
        /// Get List Activity by Sub Category ID
        /// </summary>
        /// <param name="SubCategoryId"></param>
        /// <returns></returns>
        [HttpGet("api/config/roicr/activity", Name = "config_crroi_list_activity")]
        public async Task<IActionResult> GetActivityROI([FromQuery] int SubCategoryId)
        {
            try
            {
                if (!ModelState.IsValid) return Conflict(ModelState);
                var __val = await __ROIandCRRepo.GetActivityList(SubCategoryId);
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
        /// Create Config ROI CR
        /// </summary>
        /// <param name="crroi"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost("api/config/roicr", Name = "config_roicr_post")]
        public async Task<IActionResult> CreateConfigRoi([FromBody] CRandROIParam crroi)
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
                    ConfigRoiStore crroiData = new()
                    {
                        Config = new List<SetRoiCrType>(),
                        UserId = __res.ProfileID!,
                        CreatedEmail = __res.UserEmail
                    };

                    foreach (var item in crroi.config!)
                    {
                        crroiData.Config.Add(new SetRoiCrType
                        {
                            Id = item.id,
                            MinimumROI = item.minimumROI,
                            MaksimumROI = item.maksimumROI,
                            MinimumCostRatio = item.minimumCostRatio,
                            MaksimumCostRatio = item.maksimumCostRatio
                        });
                    }

                    await __ROIandCRRepo.CreateConfigRoi(crroiData);
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
        ///  Delete Config ROI CR
        /// </summary>
        /// <param name="body"></param>
        /// <returns></returns>
        [HttpDelete("api/config/roicr", Name = "config_crroi_delete")]
        public async Task<IActionResult> DeleteConfigRoi([FromBody] ConfigRoiParamDelete body)
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
                    ConfigRoiDelete data = new()
                    {
                        id = body.id,
                        UserId = __res.ProfileID!,
                        DeletedEmail = __res.UserEmail
                    };
                    await __ROIandCRRepo.DeleteConfigRoi(data);
                    return Ok(new BaseResponse { code = 200, error = false, message = MessageService.DeleteSucceed });
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

    }
}
