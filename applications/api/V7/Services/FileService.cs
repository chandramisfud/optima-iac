using Microsoft.AspNetCore.StaticFiles;
using Repositories.Entities.Models;
using System.IO.Compression;

namespace V7.Services
{
    /// <summary>
    /// 
    /// </summary>
    public class FileService : IFileService
    {
        #region Property  
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly string _RootPath;
        #endregion

        #region Constructor  
        /// <summary>
        /// 
        /// </summary>
        /// <param name="hostingEnvironment"></param>
        public FileService(IWebHostEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
            _RootPath = hostingEnvironment.ContentRootPath;
        }
        #endregion

        /// <summary>
        /// Get Root Path from IWebHostEnvironment
        /// </summary>
        /// <returns></returns>
        public string GetRootPath()
        {
            return _RootPath;
        }
        #region Upload File  
        public bool UploadFiles(List<IFormFile> files, string subDirectory)
        {
            bool res = false;
            try
            {
                subDirectory ??= string.Empty;
                var target = Path.Combine(_hostingEnvironment.ContentRootPath, subDirectory);

                if (!Directory.Exists(target))
                {
                    Directory.CreateDirectory(target);
                }

                files.ForEach(async file =>
                {
                    if (file.Length <= 0) return;
                    var filePath = Path.Combine(target, file.FileName);
                    using var stream = new FileStream(filePath, FileMode.Create);
                    await file.CopyToAsync(stream);
                });
                res = true;
            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
            return res;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="files"></param>
        /// <param name="subDirectory"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public bool UploadFile(IFormFile files, string subDirectory, string fileName)
        {
            bool res;
            try
            {
                string uploadsFolder = Path.Combine(_hostingEnvironment.ContentRootPath, subDirectory);
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                string filePath = Path.Combine(uploadsFolder, fileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    files.CopyTo(fileStream);
                }
                res = true;
            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
            return res;
        }
        #endregion

        #region Download File  
        public (string fileType, byte[] archiveData, string archiveName) DownloadFiles(string subDirectory)
        {
            DateTime utcTime = DateTime.UtcNow;
            var zone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
            var zipName = $"archive-{TimeZoneInfo.ConvertTimeFromUtc(utcTime, zone):yyyy_MM_dd-HH_mm_ss}.zip";

            var files = Directory.GetFiles(Path.Combine(_hostingEnvironment.ContentRootPath, subDirectory)).ToList();

            using var memoryStream = new MemoryStream();
            using (var archive = new ZipArchive(memoryStream, ZipArchiveMode.Create, true))
            {
                files.ForEach(file =>
                {
                    var theFile = archive.CreateEntry(file);
                    using var streamWriter = new StreamWriter(theFile.Open());
                    streamWriter.Write(File.ReadAllText(file));

                });
            }

            return ("application/zip", memoryStream.ToArray(), zipName);

        }
        #endregion

        public string GetFileContentType(string filePath)
        {
            _ = new FileExtensionContentTypeProvider().TryGetContentType(filePath, out string? contentType);
            contentType ??= "application/octet-stream";
            return contentType;

        }
        public List<FileInformation> GetFileList(string urlRoot, string subDirectory)
        {
            List<FileInformation> res = new();
            try
            {
                var files = Directory.GetFiles(Path.Combine(_hostingEnvironment.ContentRootPath, subDirectory)).ToList();
                int i = 0;
                files.ForEach(file =>
                {
                    FileInformation info = new();
                    string[] arr = file.Split(subDirectory + "\\");
                    info.FileName = arr[1];
                    info.FileUrl = urlRoot + "/" + subDirectory + "/" + i.ToString();
                    res.Add(info);
                    i++;
                });
            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
            return res;
        }

        public List<FileInformation> GetLogFileList(int year = 2022, int month = 0)
        {
            List<FileInformation> res = new();
            try
            {
                string fileFilter = year + "-" + month.ToString("D2") + "-";
                var files = Directory.GetFiles(Path.Combine(_hostingEnvironment.ContentRootPath, "Log")).ToList();
                int i = 0;
                files.ForEach(file =>
                {
                    string[] arr = file.Split("Log" + "\\");
                    if (arr[1].Contains(fileFilter)) // if match month requested
                    {
                        FileInformation info = new()
                        {
                            FileName = arr[1]
                        };
                        res.Add(info);
                    }
                    i++;
                });
            }
            catch (Exception __ex)
            {
                throw new Exception(__ex.Message);
            }
            return res;
        }
        #region Size Converter  
        public string SizeConverter(long bytes)
        {
            var fileSize = new decimal(bytes);
            var kilobyte = new decimal(1024);
            var megabyte = new decimal(1024 * 1024);
            var gigabyte = new decimal(1024 * 1024 * 1024);

            return fileSize switch
            {
                var _ when fileSize < kilobyte => $"Less then 1KB",
                var _ when fileSize < megabyte => $"{Math.Round(fileSize / kilobyte, 0, MidpointRounding.AwayFromZero):##,###.##}KB",
                var _ when fileSize < gigabyte => $"{Math.Round(fileSize / megabyte, 2, MidpointRounding.AwayFromZero):##,###.##}MB",
                var _ when fileSize >= gigabyte => $"{Math.Round(fileSize / gigabyte, 2, MidpointRounding.AwayFromZero):##,###.##}GB",
                _ => "n/a",
            };
        }
        #endregion
    }
}
