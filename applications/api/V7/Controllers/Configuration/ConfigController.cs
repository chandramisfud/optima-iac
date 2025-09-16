using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Repositories.Contracts;
using Repositories.Entities.Models;
using System.ComponentModel.DataAnnotations;
using V7.MessagingServices;

namespace V7.Controllers.Configuration
{
    public partial class ConfigController : BaseController
    {
        private readonly IConfiguration __config;
        private readonly IConfigRepository __ConfigRepo;
        private readonly ILatePromoCreationRepo __LatePromoRepo;
        private readonly IPromoInitiatorReminderRepo __PromoInitiatorReminderRepo;
        private readonly IReminderRepo __ReminderRepo;
        private readonly IROIandCRRepo __ROIandCRRepo;
        private readonly IMajorChangesRepository __MajorChangesRepo;
        private readonly IPromoItemRepo __PromoItemRepo;
        private readonly IPromoCalculatorRepo __PromoCalculatorRepo;
        private readonly IServiceScopeFactory __ScopeFactory;


        private readonly string __TokenSecret;

        public ConfigController(
            IConfigRepository repoConfig
            , IConfiguration config
            , ILatePromoCreationRepo latePromoCreationRepo
            , IPromoInitiatorReminderRepo promoInitiatorReminderRepo
            , IReminderRepo reminderRepo
            , IROIandCRRepo rOIandCRRepo
            , IMajorChangesRepository majorChangesRepo
            , IPromoItemRepo promoItemRepo
            , IPromoCalculatorRepo promoCalculatorRepo
            , IServiceScopeFactory scopeFactory

            )
        {
            __config = config;
            __ConfigRepo = repoConfig;
            __LatePromoRepo = latePromoCreationRepo;
            __PromoInitiatorReminderRepo = promoInitiatorReminderRepo;
            __ReminderRepo = reminderRepo;
            __ROIandCRRepo = rOIandCRRepo;
            __TokenSecret = __config.GetSection("AppSettings").GetSection("Secret").Value!;
            __MajorChangesRepo = majorChangesRepo;
            __PromoItemRepo = promoItemRepo;
            __PromoCalculatorRepo = promoCalculatorRepo;
            __ScopeFactory = scopeFactory;
        }


        /// <summary>
        /// Get Configuration by category
        /// </summary>
        /// <param name="category"></param>
        /// <returns></returns>
        [HttpGet("api/config/dropdown/", Name = "config_dropdown")]
        public async Task<IActionResult> GetConfigDropdown([FromQuery][Required] string category)
        {
            IActionResult result;
            try
            {
                var __res = await __ConfigRepo.GetConfig(category);

                if (__res != null && __res.Count > 0)
                {
                    result = Ok(new BaseResponse { error = false, code = 200, message = MessageService.GetDataSuccess, values = __res });
                }
                else
                {
                    result = NotFound(new BaseResponse { error = true, code = 404, message = MessageService.DataNotFound });
                }
            }
            catch (Exception __ex)
            {
                result = StatusCode(StatusCodes.Status500InternalServerError, new BaseResponse { error = true, code = 500, message = __ex.Message });
            }

            return result;
        }

        /// <summary>
        /// Get API version
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet("api/config/version", Name = "Config_aVersion")]
        public IActionResult GetAVersion()
        {
            return Ok(new
            {
                error = false,
                statuscode = 200,
                values = new
                {
                    version = "3.52",
                    history = " History: " + System.Environment.NewLine +
                     "#262 [EB1 2025 - Item 6] - Promo Approval Reminder" + System.Environment.NewLine + 
                     "#259, #260, #261" + System.Environment.NewLine +
                     "#247 [Changes] - EP1 2025 Item 1 - Finance Report - Channel Summary - UAT Feedback 20250703" + System.Environment.NewLine +
                     "#238 [Changes] - DN - DN Received & Approved (Validate by HO) - Change Param Type" + System.Environment.NewLine +
                     "#237 [Changes] - DN - Received by Danone - Change Param Type" + System.Environment.NewLine +
                     "#236 [Changes] - DN - Send to Danone - Change Param Type" + System.Environment.NewLine +
                     "#235 [Changes] - DN - Send to HO - Change Param Type" + System.Environment.NewLine +
                     "#234 [Fixing] - Master - Matrix Promo Approval - Add Param Email on Mass Upload" + System.Environment.NewLine +
                     "#233 [Changes] - Finance Report - Promo Display - Tambah Set Data DN" + System.Environment.NewLine +
                     "#232 [Changes] - Promo - Promo ID Workflow - Tambah Set Data DN" + System.Environment.NewLine +
                     "#229 [Changed] - EP1 2025 Item 1a - Feedback UAT 20250320 - Finance Report - Channel Summary  " + System.Environment.NewLine +
                     "fixed tt console display on edit " + System.Environment.NewLine +
                     "ReLayout List Of Promo IDs That Failed on email summary " + System.Environment.NewLine +
                     "#219 [Fixing] - MS 2025 Item 95 - Fixing Error Master SKU " + System.Environment.NewLine +
                     "Add List Of Promo IDs That Failed on email summary " + System.Environment.NewLine +
                     "Revisi UAT 20250221 " + System.Environment.NewLine +
                     "Revisi UAT 20250220 " + System.Environment.NewLine +
                     "Add CC, BCC setting " + System.Environment.NewLine +
                     "Fixing error email address with spasi " + System.Environment.NewLine +
                     "#203 [Changes] - EP1 2025 Item 3 - Budget - Mass Approval - Change Process " + System.Environment.NewLine +
                     "#179 180 182 184 191 " + System.Environment.NewLine +
                     "#194 #195 MS 2025 Item 53 " + System.Environment.NewLine +
                     "#177  [Changes] - Promo - Add Parameter Get Mechanism " + System.Environment.NewLine +
                     "#175 Fixing User Management - User Group - Cannot Update Data " + System.Environment.NewLine +
                     "add period on api/dn/multiprint-promo result " + System.Environment.NewLine +
                     "update default password " + System.Environment.NewLine +
                     "add totSales for promo/display/email/id " + System.Environment.NewLine +
                     "add month and is5bio filter " + System.Environment.NewLine +
                     "MS, Handle duplicate TT Console on creation " + System.Environment.NewLine +
                     "Feedback Go Live 2024. add brand on channel summary " + System.Environment.NewLine +
                     "UAT Feedback Des04. Budget mass approval > Filter category " + System.Environment.NewLine +
                     "- param API diubah menjadi multi category" + System.Environment.NewLine +
                     "- Listing Promo Reporting - Download - Tambahkan column batchid" + System.Environment.NewLine +
                     "Fixing on update TT Consol, divide tt with 100 as value on excel" + System.Environment.NewLine +
                     "UAT Feedback, Add respon for email approval when editing budget component" + System.Environment.NewLine +
                     "UAT Feedback, Add promo start-end for geting ps value" + System.Environment.NewLine +
                     "UAT Feedback, Add category param for budget approval" + System.Environment.NewLine +
                     "Add Blitz try-catch-error" + System.Environment.NewLine +
                     "Fixing exception return code on promo" + System.Environment.NewLine +
                     "Add Task for blitz update (fix)" + System.Environment.NewLine +
                     "Add Promo Display + Workflow V2" + System.Environment.NewLine +
                     "Add Promo Calculator setting for DC " + System.Environment.NewLine +
                     "Maintenance Support 24 Okt 2024, add profile user to filter the query result " + System.Environment.NewLine +
                     "UAT Feedback 11 Oktober 2024 " + System.Environment.NewLine +
                     "UAT Feedback 23 Sep 2024 " + System.Environment.NewLine +
                     "EP1 2024 - UAT Feedback - 25jul Task 2 " + System.Environment.NewLine +
                     "Maintenance Support 2024 - Item 183 " + System.Environment.NewLine +
                     "EP1 2024 - Item 38 - mass upload SS GPS volume " + System.Environment.NewLine +
                     "EP1 2024 - Item 37 - mass upload SS GPS conversion rate " + System.Environment.NewLine +
                     "EP1 2024 - Item 35 - 2. Show Popup confirmation | " + System.Environment.NewLine +
                     "EP1 2024 - Item 33  upload mechanism file into shared folder | " + System.Environment.NewLine +
                     "EP1 2024 - Item 7 - 2. Add new function for checking mechanism validity | " + System.Environment.NewLine +
                     "EP1 2024 - Item 4.1, 4.2 | " + System.Environment.NewLine +
                     "Maintenance Support 2024 - Item 156" + System.Environment.NewLine +
                     "Maintenance Support 2024 - Item 155++" + System.Environment.NewLine +
                     "Maintenance Support 2024 - Item 101 | " + System.Environment.NewLine +
                     "IssueEP2 2023 - UAT Feedback 5 April 2024 | " + System.Environment.NewLine +
                     "IssueEP2 2023 - UAT Feedback 15 MAR 2024 point 2 " + System.Environment.NewLine 
                }
            }
             );
        }

    }
}


