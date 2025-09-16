using Microsoft.AspNetCore.Mvc;
using Repositories.Entities.Configuration;
using Repositories.Entities.Models;
using V7.MessagingServices;
using V7.Model.Configuration;
using V7.Services;

namespace V7.Controllers.Configuration
{
    public partial class ConfigController : BaseController
    {
        /// <summary>
        /// Get Data Configuration Promo Item
        /// </summary>
        /// <returns></returns>
        [HttpGet("api/config/promoitem", Name = "config_promo_item")]
        public async Task<IActionResult> GetConfigPromoItems([FromQuery] string categoryShortDesc)
        {
            try
            {
                if (!ModelState.IsValid) return Conflict(ModelState);
                var __val = await __PromoItemRepo.GetConfigPromoItems(categoryShortDesc);
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
        /// Get Data Configuration Promo Item History
        /// </summary>
        /// <returns></returns>
        [HttpGet("api/config/promoitemhistory", Name = "config_promo_item_history")]
        public async Task<IActionResult> GetConfigPromoItemsHistory([FromQuery] int year, string categoryShortDesc)
        {
            try
            {
                if (!ModelState.IsValid) return Conflict(ModelState);
                var __val = await __PromoItemRepo.GetConfigPromoItemsHistory(year, categoryShortDesc);
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
        /// Update Config Promo Item 
        /// </summary>
        /// <param name="promoItem"></param>
        /// <returns></returns>
        [HttpPut("api/config/promoItem", Name = "config_promoItem_edit")]
        public async Task<IActionResult> CreateConfigRoi([FromBody] UpdatePromoItemParam promoItem)
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
                    PromoItem __promoItemParams = new();
                    {
                        __promoItemParams.budgetYear = promoItem.ConfigPromoItem!.budgetYear;
                        __promoItemParams.promoPlanning = promoItem.ConfigPromoItem!.promoPlanning;
                        __promoItemParams.budgetSource = promoItem.ConfigPromoItem!.budgetSource;
                        __promoItemParams.entity = promoItem.ConfigPromoItem!.entity;
                        __promoItemParams.distributor = promoItem.ConfigPromoItem!.distributor;
                        __promoItemParams.subCategory = promoItem.ConfigPromoItem!.subCategory;
                        __promoItemParams.activity = promoItem.ConfigPromoItem!.activity;
                        __promoItemParams.subActivity = promoItem.ConfigPromoItem!.subActivity;
                        __promoItemParams.subActivityType = promoItem.ConfigPromoItem!.subActivityType;
                        __promoItemParams.startPromo = promoItem.ConfigPromoItem!.startPromo;
                        __promoItemParams.endPromo = promoItem.ConfigPromoItem!.endPromo;
                        __promoItemParams.activityName = promoItem.ConfigPromoItem!.activityName;
                        __promoItemParams.initiatorNotes = promoItem.ConfigPromoItem!.initiatorNotes;
                        __promoItemParams.incrSales = promoItem.ConfigPromoItem!.incrSales;
                        __promoItemParams.investment = promoItem.ConfigPromoItem!.investment;
                        __promoItemParams.channel = promoItem.ConfigPromoItem!.channel;
                        __promoItemParams.subChannel = promoItem.ConfigPromoItem!.subChannel;
                        __promoItemParams.account = promoItem.ConfigPromoItem!.account;
                        __promoItemParams.subAccount = promoItem.ConfigPromoItem!.subAccount;
                        __promoItemParams.region = promoItem.ConfigPromoItem!.region;
                        __promoItemParams.groupBrand = promoItem.ConfigPromoItem!.groupBrand;
                        __promoItemParams.brand = promoItem.ConfigPromoItem!.brand;
                        __promoItemParams.SKU = promoItem.ConfigPromoItem!.SKU;
                        __promoItemParams.mechanism = promoItem.ConfigPromoItem!.mechanism;
                        __promoItemParams.Attachment = promoItem.ConfigPromoItem!.Attachment;
                        __promoItemParams.ROI = promoItem.ConfigPromoItem!.ROI;
                        __promoItemParams.CR = promoItem.ConfigPromoItem!.CR;

                    }
                    await __PromoItemRepo.UpdateConfigPromoItem(promoItem.categoryId, __res.ProfileID!, __res.UserEmail, __promoItemParams);
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
    }
}