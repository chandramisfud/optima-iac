using Microsoft.AspNetCore.Mvc;
using V7.MessagingServices;
using V7.Model.Configuration;
using V7.Services;

namespace V7.Controllers.Configuration
{
    public partial class ConfigController : BaseController
    {
        [HttpPost("api/config/majorchanges", Name = "update_majorchanges")]
        public async Task<IActionResult> UpdateMajorChanges([FromBody] MajorChangesParam param)
        {
            try
            {
                string tokenHeader = Request.Headers["Authorization"]!;
                tokenHeader = tokenHeader.Replace("Bearer ", "");
                var __res = TokenManager.GetClaim(__TokenSecret, tokenHeader);

                if (!String.IsNullOrEmpty(__res.ProfileID))
                {
                    Repositories.Entities.Configuration.MajorChangesReq majorchanges = new()
                    {
                        SubCategory = param.SubCategory,
                        Attachment = param.Attachment,
                        //Entity = param.Entity,
                        Distributor = param.Distributor,
                        GroupBrand = param.GroupBrand,
                        PromoPlan = param.PromoPlan,
                        BudgetSources = param.BudgetSources,
                        Brand = param.Brand,
                        CR = param.CR,
                        SKU = param.SKU,
                        Account = param.Account,
                        SubAccount = param.SubAccount,
                        Activity = param.Activity,
                        ActivityDesc = param.ActivityDesc,
                        SubActivity = param.SubActivity,
                        ROI = param.ROI,
                        EndPromo = param.EndPromo,
                        StartPromo = param.StartPromo,
                        Mechanism = param.Mechanism,
                        Channel = param.Channel,
                        Id = param.Id,
                        Region = param.Region,
                        SubChannel = param.SubChannel,
                        IncrSales = param.IncrSales,
                        InitiatorNotes = param.InitiatorNotes,
                        Investment = param.Investment,
                        userid = __res.ProfileID,
                        useremail = __res.UserEmail
                    };


                    var result = await __MajorChangesRepo.UpdateMajorChanges(majorchanges);
                    if (result)
                    {
                        return Ok(new Model.BaseResponse
                        {
                            error = false,
                            code = 200,
                            message = MessageService.SaveSuccess,
                            values = result
                        });
                    }
                    else
                    {
                        return Ok(new Model.BaseResponse
                        {
                            error = true,
                            code = 404,
                            message = MessageService.SaveFailed,
                            values = result
                        });
                    }
                }
                else
                {
                    return NotFound(new Model.BaseResponse { error = true, code = 404, message = MessageService.EmailTokenFailed });
                }
            }
            catch (Exception __ex)
            {
                return Conflict(new Model.BaseResponse { error = true, code = 500, message = __ex.Message });
            }
        }

        [HttpPost("api/config/majorchangesdc", Name = "update_majorchanges_dc")]
        public async Task<IActionResult> UpdateMajorChangesDC([FromBody] MajorChangesParam param)
        {
            try
            {
                string tokenHeader = Request.Headers["Authorization"]!;
                tokenHeader = tokenHeader.Replace("Bearer ", "");
                var __res = TokenManager.GetClaim(__TokenSecret, tokenHeader);

                if (!String.IsNullOrEmpty(__res.ProfileID))
                {
                    Repositories.Entities.Configuration.MajorChangesReq majorchanges = new()
                    {
                        SubCategory = param.SubCategory,
                        Attachment = param.Attachment,
                        //Entity = param.Entity,
                        Distributor = param.Distributor,
                        GroupBrand = param.GroupBrand,
                        PromoPlan = param.PromoPlan,
                        BudgetSources = param.BudgetSources,
                        Brand = param.Brand,
                        CR = param.CR,
                        SKU = param.SKU,
                        Account = param.Account,
                        SubAccount = param.SubAccount,
                        Activity = param.Activity,
                        ActivityDesc = param.ActivityDesc,
                        SubActivity = param.SubActivity,
                        ROI = param.ROI,
                        EndPromo = param.EndPromo,
                        StartPromo = param.StartPromo,
                        Mechanism = param.Mechanism,
                        Channel = param.Channel,
                        Id = param.Id,
                        Region = param.Region,
                        SubChannel = param.SubChannel,
                        IncrSales = param.IncrSales,
                        InitiatorNotes = param.InitiatorNotes,
                        Investment = param.Investment,
                        userid = __res.ProfileID,
                        useremail = __res.UserEmail
                    };


                    var result = await __MajorChangesRepo.UpdateMajorChangesDC(majorchanges);
                    if (result)
                    {
                        return Ok(new Model.BaseResponse
                        {
                            error = false,
                            code = 200,
                            message = MessageService.SaveSuccess,
                            values = result
                        });
                    }
                    else
                    {
                        return Ok(new Model.BaseResponse
                        {
                            error = true,
                            code = 404,
                            message = MessageService.SaveFailed,
                            values = result
                        });
                    }
                }
                else
                {
                    return NotFound(new Model.BaseResponse { error = true, code = 404, message = MessageService.EmailTokenFailed });
                }
            }
            catch (Exception __ex)
            {
                return Conflict(new Model.BaseResponse { error = true, code = 500, message = __ex.Message });
            }
        }

        [HttpGet("api/config/majorchanges", Name = "get_majorchanges")]
        public async Task<IActionResult> GetMajorChanges()
        {
            try
            {
                var result = await __MajorChangesRepo.Select();

                return Ok(new Model.BaseResponse
                {
                    error = false,
                    code = 200,
                    message = "Get Data Successfully",
                    values = result
                });
            }
            catch (Exception __ex)
            {
                return Ok(new Model.BaseResponse { error = true, code = 500, message = __ex.Message });
            }

        }

        [HttpGet("api/config/majorchangesdc", Name = "get_majorchanges_dc")]
        public async Task<IActionResult> GetMajorChangesDC()
        {
            try
            {
                var result = await __MajorChangesRepo.SelectDC();

                return Ok(new Model.BaseResponse
                {
                    error = false,
                    code = 200,
                    message = "Get Data Successfully",
                    values = result
                });
            }
            catch (Exception __ex)
            {
                return Ok(new Model.BaseResponse { error = true, code = 500, message = __ex.Message });
            }

        }

        [HttpGet("api/config/majorchanges/history", Name = "get_majorchanges_history")]
        public async Task<IActionResult> GetMajorChangesHistory(string year)
        {
            try
            {
                var result = await __MajorChangesRepo.GetHistory(year);

                return Ok(new Model.BaseResponse
                {
                    error = false,
                    code = 200,
                    message = "Get Data Successfully",
                    values = result
                });
            }
            catch (Exception __ex)
            {
                return Ok(new Model.BaseResponse { error = true, code = 500, message = __ex.Message });
            }

        }

        [HttpGet("api/config/majorchanges/historydc", Name = "get_majorchanges_history_dc")]
        public async Task<IActionResult> GetMajorChangesHistoryDC(string year)
        {
            try
            {
                var result = await __MajorChangesRepo.GetHistory(year, "DC");

                return Ok(new Model.BaseResponse
                {
                    error = false,
                    code = 200,
                    message = "Get Data Successfully",
                    values = result
                });
            }
            catch (Exception __ex)
            {
                return Ok(new Model.BaseResponse { error = true, code = 500, message = __ex.Message });
            }

        }
    }
}
