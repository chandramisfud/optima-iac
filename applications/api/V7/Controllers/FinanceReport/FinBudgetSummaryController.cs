using Microsoft.AspNetCore.Mvc;
using V7.MessagingServices;
using V7.Model;
using V7.Services;

namespace V7.Controllers.FinanceReport
{
    /// <summary>
    /// Report DN Display handler
    /// </summary>
    public partial class FinanceReportController : BaseController
    {

        /// <summary>
        /// Get budget summary Landing Page
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpGet("api/finance-report/summarybudget/list", Name = "fin_rpt_budget_summary_list")]
        public async Task<IActionResult> GetBudgetSummaryLP([FromQuery] BudgetSummaryLPParam param)
        {
            IActionResult result;
            try
            {
                if (!ModelState.IsValid) return Conflict(ModelState);
                var dtBrand = Helper.ListIntToKeyId(param.grpBrand);
                var dtChannel = Helper.ListIntToKeyId(param.channel);
                var __val = await __repoFinInvestmentReport.GetBudgetSummaryLP(param.period!, param.category, dtBrand,
                    dtChannel, param.PageNumber, param.PageSize, param.txtSearch, param.sort, param.order);
                result = Ok(new Model.BaseResponse
                {
                    error = false,
                    code = 200,
                    values = __val,
                    message = MessageService.GetDataSuccess
                });
            }
            catch (Exception __ex)
            {
                result = StatusCode(StatusCodes.Status500InternalServerError, new Model.BaseResponse { error = true, code = 500, message = __ex.Message });
            }
            return result;
        }

        /// <summary>
        /// Get budget summary
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpGet("api/finance-report/summarybudget", Name = "fin_rpt_budget_summary")]
        public async Task<IActionResult> GetBudgetSummary([FromQuery] BudgetSummaryParam param)
        {
            IActionResult result;
            try
            {
                if (!ModelState.IsValid) return Conflict(ModelState);
                var dtBrand = Helper.ListIntToKeyId(param.grpBrand);
                var dtChannel = Helper.ListIntToKeyId(param.channel);
                var __val = await __repoFinInvestmentReport.GetBudgetSummary(param.period!, param.category, dtBrand, 
                    dtChannel);
                    result = Ok(new Model.BaseResponse
                    {
                        error = false,
                        code = 200,
                        values = __val,
                        message = MessageService.GetDataSuccess
                    });
            }
            catch (Exception __ex)
            {
                result = StatusCode(StatusCodes.Status500InternalServerError, new Model.BaseResponse { error = true, code = 500, message = __ex.Message });
            }
            return result;
        }
        /// <summary>
        /// Get budget summary
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpGet("api/finance-report/summarybudget/filter", Name = "fin_rpt_budget_summary_filter")]
        public async Task<IActionResult> GetBudgetSummaryFilter()
        {
            IActionResult result;
            try
            {
                var __val = await __repoFinInvestmentReport.GetBudgetSummaryFilter();
                result = Ok(new BaseResponse
                {
                    error = false,
                    code = 200,
                    values =  __val,
                    message = MessageService.GetDataSuccess
                });
            }
            catch (Exception __ex)
            {
                result = StatusCode(StatusCodes.Status500InternalServerError, 
                    new Model.BaseResponse { error = true, code = 500, message = __ex.Message });
            }
            return result;
        }

        /// <summary>
        /// Summary Budgeting Signoff
        /// </summary>
        /// <returns></returns>
        //[HttpGet("api/finance-report/summarybudgetsignoff", Name = "fin_rpt_budget_summary_signoff")]
        //public async Task<IActionResult> GetBudgetSummarySignOff(int period)
        //{
        //    IActionResult result;
        //    try
        //    {
        //        var __val = await __repoFinInvestmentReport.GetBudgetSummarySignoff(period);
        //        result = Ok(new Model.BaseResponse
        //        {
        //            error = false,
        //            code = 200,
        //            values = __val,
        //            message = MessageService.GetDataSuccess
        //        });
        //    }
        //    catch (Exception __ex)
        //    {
        //        result = StatusCode(StatusCodes.Status500InternalServerError, new Model.BaseResponse { error = true, code = 500, message = __ex.Message });
        //    }
        //    return result;
        //}

        ///// <summary>
        ///// Summary Budgeting Signoff
        ///// </summary>
        ///// <returns></returns>
        //[HttpGet("api/finance-report/summarybudgetdc", Name = "fin_rpt_budget_summary_DC")]
        //public async Task<IActionResult> GetBudgetSummaryDC([FromQuery] BudgetSummaryParam param)
        //{
        //    IActionResult result;
        //    try
        //    {
        //        if (!ModelState.IsValid) return Conflict(ModelState);
        //        var dtBrand = Helper.ListIntToKeyId(param.grpBrand);
        //        var dtChannel = Helper.ListIntToKeyId(param.channel);
        //        var __val = await __repoFinInvestmentReport.GetBudgetSummaryDC(param.period!, param.category, dtBrand,
        //            dtChannel);
        //        result = Ok(new Model.BaseResponse
        //        {
        //            error = false,
        //            code = 200,
        //            values = __val,
        //            message = MessageService.GetDataSuccess
        //        });
        //    }
        //    catch (Exception __ex)
        //    {
        //        result = StatusCode(StatusCodes.Status500InternalServerError, new Model.BaseResponse { error = true, code = 500, message = __ex.Message });
        //    }
        //    return result;
        //}
    }
}
