using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Repositories.Contracts;
using Repositories.Entities.Configuration;
using Repositories.Entities.Dtos;
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
        /// Get Promo Calculator Config LP
        /// </summary>
        /// <param name="mainActivityId"></param>
        /// <param name="channelId"></param>
        /// <returns></returns>
        [HttpGet("api/config/promocalculator", Name = "get_config_promo_calculator")]
        public async Task<IActionResult> GetPromoCalculator([FromQuery]int mainActivityId, int channelId)
        {
            try
            {
                if (!ModelState.IsValid) return Conflict(ModelState);
                var __val = await __PromoCalculatorRepo.GetPromoCalculatorLP(mainActivityId, channelId);
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
        /// for filtering LP
        /// </summary>
        /// <returns></returns>
        [HttpGet("api/config/promocalculator/filter", Name = "get_config_promo_calculator_filter")]
        public async Task<IActionResult> GetPromoCalculatorFilter()
        {
            try
            {
                var __val = await __PromoCalculatorRepo.GetPromoCalculatorFilter();
                return Ok(new BaseResponse
                {
                    code = 200,
                    error = false,
                    message = MessageService.GetDataSuccess,
                    values = __val
                });
            }
            catch (Exception __ex)
            {
                return Conflict(new BaseResponse { code = 500, error = true, message = __ex.Message });

            }
        } 
        
        /// <summary>
        /// Get channel list
        /// </summary>
        /// <returns></returns>
        [HttpGet("api/config/promocalculator/channel", Name = "get_config_promo_calculator_channel")]
        public async Task<IActionResult> GetPromoCalculatorChannel()
        {
            try
            {
                var __val = await __PromoCalculatorRepo.GetPromoCalculatorChannel();
                return Ok(new BaseResponse
                {
                    code = 200,
                    error = false,
                    message = MessageService.GetDataSuccess,
                    values = __val
                });
            }
            catch (Exception __ex)
            {
                return Conflict(new BaseResponse { code = 500, error = true, message = __ex.Message });

            }
        }

        /// <summary>
        /// Get Promo Calculator Config by mainActivityId
        /// </summary>
        /// <returns></returns>
        [HttpGet("api/config/promocalculator/id", Name = "get_config_promo_calculator_id")]
        public async Task<IActionResult> GetPromoCalculatorById([FromQuery]int mainActivityId, int channelId)
        {
            try
            {
                if (!ModelState.IsValid) return Conflict(ModelState);
                var __val = await __PromoCalculatorRepo.GetPromoCalculatorById(mainActivityId, channelId);
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
        /// Get Promo Calculator Config Filter and Coverage
        /// </summary>
        /// <returns></returns>
        [HttpGet("api/config/promocalculator/filterandcoverage", Name = "get_config_promo_calculator_filter_coverage")]
        public async Task<IActionResult> GetPromoCalculatorFilterCoverage()
        {
            try
            {
                if (!ModelState.IsValid) return Conflict(ModelState);
//                var __val = await __PromoCalculatorRepo.GetPromoCalculatorSubActivityCoverage(categoryId, subCategoryId, activityId);
                var __val = await __PromoCalculatorRepo.GetPromoCalculatorFilterAndSubActivityCoverage();
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
        /// Create Promo Calculator Config 
        /// </summary>
        /// <returns></returns>
        [HttpPost("api/config/promocalculator", Name = "set_config_promo_calculator_create")]
        public async Task<IActionResult> SetPromoCalculatorCreate([FromBody] ConfigPromoCalculatorCreateParam config)
        {
            try
            {
                if (!ModelState.IsValid) return Conflict(ModelState);
                string tokenHeader = Request.Headers["Authorization"]!;
                tokenHeader = tokenHeader.Replace("Bearer ", "");
                var __res = TokenManager.GetClaim(__TokenSecret, tokenHeader);

                if (!String.IsNullOrEmpty(__res.ProfileID))
                {
                    int i = 0;
                    foreach (var body in config.configCalculator)
                    {
                        var __val = await __PromoCalculatorRepo.SetPromoCalculatorSave(body.mainActivity, body.channelId,
                        body.baseline, body.totalSales, body.uplift, body.salesContribution, body.storesCoverage,
                        body.redemptionRate, body.cr, body.cost,
                        body.baselineRecon, body.totalSalesRecon, body.upliftRecon, body.salesContributionRecon,
                        body.storesCoverageRecon, body.redemptionRateRecon, body.crRecon, body.costRecon,
                        config.subActivity, __res.ProfileID, __res.UserEmail);
                        i++;
                        // add default channel 99999 on last loop
                        if (i== config.configCalculator.Length)
                        {
                            __val = await __PromoCalculatorRepo.SetPromoCalculatorSave(body.mainActivity, 99999,
                            body.baseline, body.totalSales, body.uplift, body.salesContribution, body.storesCoverage,
                            body.redemptionRate, body.cr, body.cost,
                            body.baselineRecon, body.totalSalesRecon, body.upliftRecon, body.salesContributionRecon,
                            body.storesCoverageRecon, body.redemptionRateRecon, body.crRecon, body.costRecon,
                            config.subActivity, __res.ProfileID, __res.UserEmail);

                        }
                        {

                        } 
                    }
                    return Ok(new BaseResponse
                    {
                        code = 200,
                        error = false,
                        message = MessageService.SaveSuccess,
                    });
                }
                else
                {
                    return NotFound(new Model.BaseResponse { error = true, code = 404, message = MessageService.EmailTokenFailed });
                }
            }
            catch (Exception __ex)
            {
                return Conflict(new BaseResponse { code = 500, error = true, message = __ex.Message });

            }
        }

        /// <summary>
        /// Set Promo Calculator Config Update
        /// </summary>
        /// <returns></returns>
        [HttpPut("api/config/promocalculator", Name = "set_config_promo_calculator_update")]
        public async Task<IActionResult> SetPromoCalculatorUpdate([FromBody] ConfigPromoCalculatorUpdateParam config)
        {
            try
            {
                if (!ModelState.IsValid) return Conflict(ModelState);
                string tokenHeader = Request.Headers["Authorization"]!;
                tokenHeader = tokenHeader.Replace("Bearer ", "");
                var __res = TokenManager.GetClaim(__TokenSecret, tokenHeader);

                if (!String.IsNullOrEmpty(__res.ProfileID))
                {
                    foreach (var body in config.configCalculator)
                    {
                        var __val = await __PromoCalculatorRepo.SetPromoCalculatorUpdate(body.mainActivityId,
                        body.mainActivity, body.channelId,
                        body.baseline, body.totalSales, body.uplift, body.salesContribution,
                        body.storesCoverage, body.redemptionRate, body.cr, body.cost,
                        body.baselineRecon, body.totalSalesRecon, body.upliftRecon, body.salesContributionRecon,
                        body.storesCoverageRecon, body.redemptionRateRecon, body.crRecon, body.costRecon,
                        config.subActivity, __res.ProfileID, __res.UserEmail);
                    }
                    return Ok(new BaseResponse
                    {
                        code = 200,
                        error = false,
                        message = MessageService.SaveSuccess,
                    });
                }
                else
                {
                    return NotFound(new Model.BaseResponse { error = true, code = 404, message = MessageService.EmailTokenFailed });
                }
            }
            catch (Exception __ex)
            {
                return Conflict(new BaseResponse { code = 500, error = true, message = __ex.Message });

            }
        }
    }
}