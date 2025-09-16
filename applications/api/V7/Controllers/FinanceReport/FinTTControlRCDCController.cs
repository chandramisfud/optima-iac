using Microsoft.AspNetCore.Mvc;
using Repositories.Entities.Models;
using System.Data;
using V7.MessagingServices;
using V7.Model.Report;
using V7.Services;

namespace V7.Controllers.FinanceReport
{
    /// <summary>
    /// Report TT Control RCDC
    /// </summary>
    public partial class FinanceReportController : BaseController
    {
        
        /// <summary>
        /// Report Channel Summary
        /// </summary>
        /// <param name="period"></param>
        /// <returns></returns>
        [HttpGet("api/finance-report/ttcontrolrcdc", Name = "rpt_ttcontrolrcdc")]
        public async Task<IActionResult> GetTTcontrolRcdD([FromQuery]TTControlRCDCParam param)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);
                DataTable dtChannel = Helper.ListIntToKeyId(param.channelId!);
                DataTable dtBrand = Helper.ListIntToKeyId(param.groupBrandId!);
                DataTable dtCategory = Helper.ListIntToKeyId(param.categoryId!);
                DataTable dtSubActivity = Helper.ListIntToKeyId(param.subActivityTypeId!);

                var __val = await __repoFinInvestmentReport.GetTTControlRCDC(param.period,
                        dtCategory, dtBrand, dtChannel, dtSubActivity, "",
                        param.pageNumber, param.pageSize, param.filter!, param.search!);
                return Ok(new BaseResponse
                {
                    error = false,
                    code = 200,
                    message = MessageService.GetDataSuccess,
                    values = __val
                });
            }
            catch (Exception __ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new BaseResponse { error = true, code = 500, message = __ex.Message });
            }
        }

        /// <summary>
        /// Report Channel Summary Download
        /// </summary>
        /// <param name="period"></param>
        /// <returns></returns>
        [HttpGet("api/finance-report/ttcontrolrcdc/download", Name = "rpt_ttcontrolrcdc_download")]
        public async Task<IActionResult> GetTTcontrolRcDcDownload([FromQuery] TTControlRCDCParam param)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);
                DataTable dtChannel = Helper.ListIntToKeyId(param.channelId!);
                DataTable dtBrand = Helper.ListIntToKeyId(param.groupBrandId!);
                DataTable dtCategory = Helper.ListIntToKeyId(param.categoryId!);
                DataTable dtSubActivity = Helper.ListIntToKeyId(param.subActivityTypeId!);

                var __val = await __repoFinInvestmentReport.GetTTControlRCDCDownload(param.period,
                        dtCategory, dtBrand, dtChannel, dtSubActivity, "",
                        param.pageNumber, param.pageSize, param.filter!, param.search!);
                return Ok(new BaseResponse
                {
                    error = false,
                    code = 200,
                    message = MessageService.GetDataSuccess,
                    values = __val
                });
            }
            catch (Exception __ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new BaseResponse { error = true, code = 500, message = __ex.Message });
            }
        }

        /// <summary>
        /// Report TT Control RC DC Filter
        /// </summary>
        /// <param name="period"></param>
        /// <returns></returns>
        [HttpGet("api/finance-report/ttcontrolrcdc/filter", Name = "rpt_ttcontrolrcdc_filter")]
        public async Task<IActionResult> GetTTControlFilter()
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);

                var __val = await __repoFinInvestmentReport.GetTTControlRCDCFilter();
                return Ok(new BaseResponse
                {
                    error = false,
                    code = 200,
                    message = MessageService.GetDataSuccess,
                    values = __val
                });

            }
            catch (Exception __ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new BaseResponse { error = true, code = 500, message = __ex.Message });
            }
        }
    }
}
