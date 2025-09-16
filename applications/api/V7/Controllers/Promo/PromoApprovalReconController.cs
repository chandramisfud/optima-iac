using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
//using System.Web.Http;
//using Repositories.Entities.Models.PromoApproval;
using V7.MessagingServices;
using V7.Model;
using V7.Model.Promo;
using V7.Model.PromoApproval;
using V7.Services;

namespace V7.Controllers.Promo
{
    public partial class PromoController : BaseController
    {
        /// <summary>
        /// Get Promo Approval Recon for LP
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpGet("api/promo/approvalrecon", Name = "promo_approvalrecon_LP")]
        public async Task<IActionResult> GetPromoApprovalReconLP([FromQuery] PromoReconApprovalParam param)
        {
            try
            {
                string tokenHeader = Request.Headers["Authorization"]!;
                tokenHeader = tokenHeader.Replace("Bearer ", "");
                var __res = TokenManager.GetClaim(__TokenSecret, tokenHeader);
                BaseLP respon = new();
                if (__res.ProfileID != null)
                {
                    var __acc = await _repoPromoApproval.GetPromoApprovalReconLP("0", param.category, param.entity, param.distributor, 0, 0, __res.ProfileID);
                    if (__acc != null)
                    {
                        var filteredData = __acc;
                        if (!string.IsNullOrEmpty(param.Search))
                        {
                            param.Search = param.Search.ToLower();
                            filteredData = filteredData.Where(x => x.RequestId.ToString().Contains(param.Search)
                            || x.StartPromo.ToString().Contains(param.Search) ||
                            x.Initiator!.Contains(param.Search) || x.Investment.ToString().Contains(param.Search) ||
                            x.ActivityDesc!.ToLower().Contains(param.Search) || x.RefId!.ToLower().Contains(param.Search)
                            ).ToList();
                        }
                        respon.TotalCount = __acc.Count;

                        switch (param.SortColumn)
                        {
                            case sortPromoReconApprovalField.StartPromo:
                                filteredData = filteredData.OrderBy(x => x.StartPromo).ToList();
                                if (param.SortDirection.ToString() == "desc")
                                {
                                    filteredData = filteredData.OrderByDescending(x => x.StartPromo).ToList();
                                }
                                break;
                            case sortPromoReconApprovalField.RefId:
                                filteredData = filteredData.OrderBy(x => x.RefId).ToList();
                                if (param.SortDirection.ToString() == "desc")
                                {
                                    filteredData = filteredData.OrderByDescending(x => x.RefId).ToList();
                                }
                                break;
                            case sortPromoReconApprovalField.RequestId:
                                filteredData = filteredData.OrderBy(x => x.RequestId).ToList();
                                if (param.SortDirection.ToString() == "desc")
                                {
                                    filteredData = filteredData.OrderByDescending(x => x.RequestId).ToList();
                                }

                                break;
                            case sortPromoReconApprovalField.RequestDate:
                                filteredData = filteredData.OrderBy(x => x.RequestDate).ToList();
                                if (param.SortDirection.ToString() == "desc")
                                {
                                    filteredData = filteredData.OrderByDescending(x => x.RequestDate).ToList();
                                }

                                break;
                            case sortPromoReconApprovalField.Investment:
                                filteredData = filteredData.OrderBy(x => x.Investment.ToString()).ToList();
                                if (param.SortDirection.ToString() == "desc")
                                {
                                    filteredData = filteredData.OrderByDescending(x => x.Investment.ToString()).ToList();
                                }

                                break;
                            case sortPromoReconApprovalField.ActivityDesc:
                                filteredData = filteredData.OrderBy(x => x.ActivityDesc).ToList();
                                if (param.SortDirection.ToString() == "desc")
                                {
                                    filteredData = filteredData.OrderByDescending(x => x.ActivityDesc).ToList();
                                }

                                break;
                            case sortPromoReconApprovalField.AgingApproval:
                                filteredData = filteredData.OrderBy(x => x.AgingApproval).ToList();
                                if (param.SortDirection.ToString() == "desc")
                                {
                                    filteredData = filteredData.OrderByDescending(x => x.AgingApproval).ToList();
                                }

                                break;
                            case sortPromoReconApprovalField.Initiator:
                                filteredData = filteredData.OrderBy(x => x.Initiator).ToList();
                                if (param.SortDirection.ToString() == "desc")
                                {
                                    filteredData = filteredData.OrderByDescending(x => x.Initiator).ToList();
                                }

                                break;
                        }
                        // Lakukan paging dan kembalikan data yang sesuai dengan permintaan
                        if (param.PageSize >= 0)
                        {
                            var pagedData = Helper.GetPagedData(filteredData.Cast<object>().ToList(), param.PageNumber, param.PageSize);
                            respon.Data = pagedData;
                        }
                        else
                        {
                            respon.Data = filteredData.Cast<object>().ToList();
                        }

                        respon.FilteredCount = filteredData.Count;
                        return Ok(new BaseResponse { error = false, code = 200, values = respon });
                    }
                    else
                    {
                        return Ok(new Model.BaseResponse { code = 404, error = true, message = MessageService.GetDataFailed });
                    }
                }
                else
                {
                    return NotFound(new Model.BaseResponse { error = true, code = 404, message = MessageService.EmailTokenFailed });
                }
            }
            catch (Exception __ex)
            {
                return Conflict(new Model.BaseResponse { code = 500, error = true, message = __ex.Message });
            }
        }

        /// <summary>
        /// Get Entity List
        /// </summary>
        /// <returns></returns>
        [HttpGet("api/promo/approvalrecon/entity", Name = "promo_approvalrecon_entity")]
        public async Task<IActionResult> GetApprovalReconEntity()
        {
            try
            {
                var __val = await _repoPromoApproval.GetAllEntity();
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
        /// Get distributor list
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpGet("api/promo/approvalrecon/distributor", Name = "promo_approvalrecon_distributor")]
        public async Task<IActionResult> GetApprovalReconDistributor([FromQuery] DistributorListParam param)
        {
            try
            {
                var __val = await _repoPromoApproval.GetDistributorList(param.budgetId, param.entityId!);
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
        /// Set Promo APproval recon approve
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost("api/promo/approvalrecon/approve", Name = "get_promo_approvalrecon_approve")]
        public async Task<IActionResult> PromoApprovalReconApprove([FromBody] PromoApprovalReconParam param)
        {
            try
            {
                string tokenHeader = Request.Headers["Authorization"]!;
                tokenHeader = tokenHeader.Replace("Bearer ", "");
                var __res = TokenManager.GetClaim(__TokenSecret, tokenHeader);
                if (__res.ProfileID != null)
                {
                    string approveCode = "TP2" + __res.ProfileID;
                    string email = __res.UserEmail;

                    var __val = await _repoPromoApproval.ApprovalPromoRecon(param.promoId, approveCode, param.notes!, email);
                    if (__val != null)
                    {
                        return Ok(new Model.BaseResponse
                        {
                            code = 200,
                            error = false,
                            message = MessageService.GetDataSuccess,
                            values = __val
                        });
                    }
                    else
                    {
                        return Ok(new Model.BaseResponse { code = 404, error = true, message = MessageService.GetDataFailed });
                    }
                }
                else
                {
                    return NotFound(new Model.BaseResponse { error = true, code = 404, message = MessageService.EmailTokenFailed });
                }
            }
            catch (Exception __ex)
            {
                return Conflict(new Model.BaseResponse { code = 500, error = true, message = __ex.Message });

            }
        }

        /// <summary>
        /// Set Promo APproval recon sendback
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost("api/promo/approvalrecon/sendback", Name = "get_promo_approvalrecon_sendback")]
        public async Task<IActionResult> PromoApprovalReconSendback([FromBody] PromoApprovalReconParam param)
        {
            try
            {
                string tokenHeader = Request.Headers["Authorization"]!;
                tokenHeader = tokenHeader.Replace("Bearer ", "");
                var __res = TokenManager.GetClaim(__TokenSecret, tokenHeader);
                if (__res.ProfileID != null)
                {
                    string approveCode = "TP3" + __res.ProfileID;
                    string email = __res.UserEmail;
                    var __val = await _repoPromoApproval.ApprovalPromoRecon(param.promoId, approveCode, param.notes!, email);
                    if (__val != null)
                    {
                        return Ok(new Model.BaseResponse
                        {
                            code = 200,
                            error = false,
                            message = MessageService.GetDataSuccess,
                            values = __val
                        });
                    }
                    else
                    {
                        return Ok(new Model.BaseResponse { code = 404, error = true, message = MessageService.GetDataFailed });
                    }
                }
                else
                {
                    return NotFound(new Model.BaseResponse { error = true, code = 404, message = MessageService.EmailTokenFailed });
                }
            }
            catch (Exception __ex)
            {
                return Conflict(new Model.BaseResponse { code = 500, error = true, message = __ex.Message });

            }
        }

        /// <summary>
        /// Get Promo APproval Recon by ID
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpGet("api/promo/approvalrecon/id", Name = "promo_getrecon_byId")]
        public async Task<IActionResult> GetPromoReconByID([FromQuery] int Id)
        {
            try
            {
                var __val = await _repoPromoApproval.GetPromoReconV3(Id);
                if (__val != null)
                {
                    return Ok(new Model.BaseResponse
                    {
                        code = 200,
                        error = false,
                        message = MessageService.GetDataSuccess,
                        values = __val
                    });
                }
                else
                {
                    return Ok(new Model.BaseResponse { code = 404, error = true, message = MessageService.GetDataFailed });
                }
            }
            catch (Exception __ex)
            {
                return Conflict(new Model.BaseResponse { code = 500, error = true, message = __ex.Message });
            }
        }

        /// <summary>
        /// Set Promo APproval recon approve by email (no token)
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost("api/promo/approvalreconbyemail/approve", Name = "get_promo_approvalrecon_byemail_approve")]
        public async Task<IActionResult> PromoApprovalReconByEmailApprove([FromBody] PromoApprovalReconByEmailParam param)
        {
            try
            {
                string approveCode = "TP2" + param.profileId;
                string email = "";
                var __val = await _repoPromoApproval.ApprovalPromoRecon(param.promoId, approveCode, param.notes!, email);
                if (__val != null)
                {
                    return Ok(new Model.BaseResponse
                    {
                        code = 200,
                        error = false,
                        message = MessageService.GetDataSuccess,
                        values = __val
                    });
                }
                else
                {
                    return Ok(new Model.BaseResponse { code = 404, error = true, message = MessageService.GetDataFailed });
                }
            }
            catch (Exception __ex)
            {
                return Conflict(new Model.BaseResponse { code = 500, error = true, message = __ex.Message });
            }
        }

        /// <summary>
        /// Set Promo APproval recon sendback by email (no token)
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost("api/promo/approvalreconbyemail/sendback", Name = "get_promo_approvalrecon_byemail_sendback")]
        public async Task<IActionResult> PromoApprovalReconByEmailSendback([FromBody] PromoApprovalReconByEmailParam param)
        {
            try
            {
                string approveCode = "TP3" + param.profileId;
                string email = "";

                var __val = await _repoPromoApproval.ApprovalPromoRecon(param.promoId, approveCode, param.notes!, email);
                if (__val != null)
                {
                    return Ok(new Model.BaseResponse
                    {
                        code = 200,
                        error = false,
                        message = MessageService.GetDataSuccess,
                        values = __val
                    });
                }
                else
                {
                    return Ok(new Model.BaseResponse { code = 404, error = true, message = MessageService.GetDataFailed });
                }

            }
            catch (Exception __ex)
            {
                return Conflict(new Model.BaseResponse { code = 500, error = true, message = __ex.Message });
            }
        }
        /// <summary>
        /// Get category list
        /// </summary>
        /// <returns></returns>
        [HttpGet("api/promo/approvalrecon/category", Name = "promo_getrecon_getcategory")]
        public async Task<IActionResult> GetCategoryListforReconApproval()
        {
            try
            {
                var __val = await _repoPromoApproval.GetCategoryListforReconApproval();
                if (__val != null)
                {
                    return Ok(new Model.BaseResponse
                    {
                        code = 200,
                        error = false,
                        message = MessageService.GetDataSuccess,
                        values = __val
                    });
                }
                else
                {
                    return Ok(new Model.BaseResponse { code = 404, error = true, message = MessageService.GetDataFailed });
                }
            }
            catch (Exception __ex)
            {
                return Conflict(new Model.BaseResponse { code = 500, error = true, message = __ex.Message });
            }
        }
        /// <summary>
        /// Get category list
        /// </summary>
        /// <returns></returns>
        [HttpGet("api/promo/approval/category", Name = "promo_getcategory")]
        public async Task<IActionResult> GetCategoryListforPromoApproval()
        {
            try
            {
                var __val = await _repoPromoApproval.GetCategoryListforPromoApproval();
                if (__val != null)
                {
                    return Ok(new Model.BaseResponse
                    {
                        code = 200,
                        error = false,
                        message = MessageService.GetDataSuccess,
                        values = __val
                    });
                }
                else
                {
                    return Ok(new Model.BaseResponse { code = 404, error = true, message = MessageService.GetDataFailed });
                }
            }
            catch (Exception __ex)
            {
                return Conflict(new Model.BaseResponse { code = 500, error = true, message = __ex.Message });
            }
        }

        // debetnote/getbyId
        /// <summary>
        /// Get DN by Id, Old API = "debetnote/getbyId"
        /// </summary>
        /// <returns></returns>
        [HttpGet("api/promo/approvalrecon/dn/id", Name = "promo_approvalrecon_getdndetail_byid")]
        public async Task<IActionResult> GetDNDetailbyIdforPromoApproval([FromQuery] int id)
        {
            IActionResult result;
            List<string> EmptyList = new();
            try
            {
                if (!ModelState.IsValid) return Conflict(ModelState);
                var __val = await _repoPromoApproval.GetDNDetailbyIdforPromoApproval(id);
                if (__val == null)
                {
                    return Ok(new BaseResponse { code = 204, error = true, message = MessageService.DataNotFound, values = EmptyList });
                }
                else
                {
                    return Ok(new BaseResponse
                    {
                        code = 200,
                        error = false,
                        message = MessageService.GetDataSuccess,
                        values = __val
                    });
                }
            }
            catch (Exception __ex)
            {
                result = StatusCode(StatusCodes.Status403Forbidden, new BaseResponse { error = true, code = 403, message = __ex.Message });
            }
            return result;
        }
        /// <summary>
        /// Get DN Paid
        /// </summary>
        /// <returns></returns>
        [HttpGet("api/promo/approvalrecon/dn/paid/id", Name = "promo_approvalrecon_getdnpaid_byid")]
        public async Task<IActionResult> GetDNPaidforPromoApprovalRecon([FromQuery] int id)
        {
            IActionResult result;
            List<string> EmptyList = new();
            try
            {
                if (!ModelState.IsValid) return Conflict(ModelState);
                var __val = await _repoPromoApproval.GetDNPaidforPromoApprovalRecon(id);
                if (__val == null)
                {
                    return Ok(new BaseResponse { code = 204, error = true, message = MessageService.DataNotFound, values = EmptyList });
                }
                else
                {
                    return Ok(new BaseResponse
                    {
                        code = 200,
                        error = false,
                        message = MessageService.GetDataSuccess,
                        values = __val
                    });
                }
            }
            catch (Exception __ex)
            {
                result = StatusCode(StatusCodes.Status403Forbidden, new BaseResponse { error = true, code = 403, message = __ex.Message });
            }
            return result;
        }
        /// <summary>
        /// Get DN Claim 
        /// </summary>
        /// <returns></returns>
        [HttpGet("api/promo/approvalrecon/dn/claim/id", Name = "promo_approvalrecon_getdnclaim_byid")]
        public async Task<IActionResult> GetDNClaimforPromoApprovalRecon([FromQuery] int id)
        {
            IActionResult result;
            List<string> EmptyList = new();
            try
            {
                if (!ModelState.IsValid) return Conflict(ModelState);
                var __val = await _repoPromoApproval.GetDNClaimforPromoApprovalRecon(id);
                if (__val == null)
                {
                    return Ok(new BaseResponse { code = 204, error = true, message = MessageService.DataNotFound, values = EmptyList });
                }
                else
                {
                    return Ok(new BaseResponse
                    {
                        code = 200,
                        error = false,
                        message = MessageService.GetDataSuccess,
                        values = __val
                    });
                }
            }
            catch (Exception __ex)
            {
                result = StatusCode(StatusCodes.Status403Forbidden, new BaseResponse { error = true, code = 403, message = __ex.Message });
            }
            return result;
        }

        //  mapmaterial/all
        /// <summary>
        /// Get TaxLevel for Promo Approval Recon DN Id, Old API = "mapmaterial/all"
        /// </summary>
        /// <returns></returns>
        [HttpGet("api/promo/approvalrecon/dn/taxlevel", Name = "promo_approvalrecon_material")]
        public async Task<IActionResult> GetTaxLevelforPromoApprovalRecon()
        {
            try
            {
                if (!ModelState.IsValid) return Conflict(ModelState);
                var __val = await _repoPromoApproval.GetTaxLevelforPromoApprovalRecon();
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
