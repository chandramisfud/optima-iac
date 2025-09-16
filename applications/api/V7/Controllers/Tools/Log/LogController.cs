using Microsoft.AspNetCore.Mvc;
using NLog.Targets;
using NLog;
using Repositories.Entities.Models;
using V7.MessagingServices;
using Microsoft.AspNetCore.Authorization;


namespace V7.Controllers.Tools
{
    public partial class ToolsController : BaseController
    {
        /// <summary>
        /// Handle download log file link
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet("api/tools/log/dl/{filename}", Name = "GetLogFileDownload")]
        public IActionResult log_download(string filename)
        {
            try
            {
                //filename = "2023-05-17_logfile.txt";
                var fileTarget = LogManager.Configuration.FindTargetByName<FileTarget>("logfile");
                string filePath = __fileService.GetRootPath() + "\\Log\\" + filename;
                //int i = 0;
                byte[] buffer = null!;
                using (var stream = System.IO.File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    buffer = new byte[stream.Length];
                    stream.Read(buffer, 0, Convert.ToInt32(stream.Length));
                }

                return File(buffer, "text/plain", Path.GetFileName(filePath));
            }
            catch (Exception __ex)
            {
                return BadRequest(new BaseResponse { error = true, code = 400, message = __ex.Message }); ;
            }
        }

        /// <summary>
        /// Get log req/resp list base on year-month
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet("api/tools/log/list/", Name = "GetLogFileList")]
        public IActionResult log_list(int year, int month)
        {
            try
            {
                var fileTarget = LogManager.Configuration.FindTargetByName<FileTarget>("logfile");
                var lsFiles = __fileService.GetLogFileList(year, month);
                List<string> ls = new();
                string hostUrl = Request.Scheme + "://" + Request.Host.Value + "/api/tools/log/dl/";
                foreach (var lsFile in lsFiles)
                {
                    ls.Add(hostUrl + lsFile.FileName); ;
                }
                return Ok(new BaseResponse { error = false, code = 200, values = ls, message = "OK" });
            }
            catch (Exception __ex)
            {
                return BadRequest(new BaseResponse { error = true, code = 400, message = __ex.Message }); ;
            }
        }
        /// <summary>
        /// Get version of deployment
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet("api/tools/version", Name = "tools_get_api_version")]
        public IActionResult GetAVersion()
        {
            return Ok(new BaseResponse
            {
                error = false,
                code = 200,
                values = new
                {
                    version = "3.1.7.10.1.24",
                    changeLog = "fix fin report promoapproval schedule"
                }
            }
             );
        }

    }
}
