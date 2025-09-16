using Dapper;
using Newtonsoft.Json;
using OfficeOpenXml;
using Renci.SshNet;
using Repositories.Contracts;
using Repositories.Entities.Dtos;
using Repositories.Repos;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Net.WebSockets;


namespace V7.Services
{
    public class TimerTask
    {
        private readonly LoggerService.LoggerManager _logger;
        private readonly IConfiguration _configuration;
        private readonly IServiceScopeFactory _scopeFactory;

        private readonly IToolsEmailRepository __emailRepo;

        public TimerTask(IConfiguration configuration, IServiceScopeFactory scopeFactory)
        {
            _logger = new LoggerService.LoggerManager();
            _configuration = configuration;
            _scopeFactory = scopeFactory;
            using var scope = _scopeFactory.CreateScope();
            __emailRepo = scope.ServiceProvider.GetRequiredService<IToolsEmailRepository>();
        }


        private string DownloadLatestXlsFile(string host, int port, string username, string password,
            string remoteDirectory, string localDir)
        {
            string localFilePath = string.Empty;
            try
            {
                using (var sftp = new SftpClient(host, port, username, password))
                {
                    sftp.Connect();
                    var allowedExtensions = new[] { ".csv", ".xlsx" };
                    // List files in the specified remote directory and filter to get only .xls files
                    var latestFile = sftp.ListDirectory(remoteDirectory)
                        .Where(file => file.IsRegularFile && allowedExtensions.Any(file.Name.ToLower().EndsWith))
                        .OrderByDescending(file => file.LastWriteTime)
                        .FirstOrDefault();

                    var remoteFilePath =  latestFile?.FullName;
                    string originalFileName = Path.GetFileNameWithoutExtension(remoteFilePath!);
                    string extension = Path.GetExtension(remoteFilePath!);
                    string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");

                    // Create a new file name with the timestamp
                    string newFileName = $"{originalFileName}_{timestamp}{extension}";

                    if (!Directory.Exists(localDir))
                    {
                        Directory.CreateDirectory(localDir); // This requires write permission on the parent directory
                    }
                    localFilePath = Path.Combine(localDir, newFileName);
                    using (var fileStream = new FileStream(localFilePath, FileMode.Create))
                    {
                        sftp.DownloadFile(remoteFilePath!, fileStream);
                    }
                    sftp.Disconnect();
                    _logger.LogInfo("DOWNLOAD " + latestFile?.Name + " succesfully");
                }
            } catch (Exception ex)
            {
                _logger.LogInfo(ex.Message);
            }
            return localFilePath;
        }

        public async Task<bool> BlitzUpdate(string ftpHost, string ftpUserName, string ftpPassword,
            string localDirectory, string remoteDirectory)
        {
            try
            {
                string tableBakName = "tbtrx_blitz_offtake_one_baseline_bak";
                string tableName = "tbtrx_blitz_offtake_one_baseline";
                string[] ftp = ftpHost.Split(':');
                if (ftp.Length < 2)
                {
                    _logger.LogInfo("Plz check FTP host format");
                    return false;
                }
                string filePath = DownloadLatestXlsFile(ftp[0], Convert.ToInt16(ftp[1]), ftpUserName, ftpPassword, remoteDirectory, localDirectory);


                var dataTable = LoadCSVFiles(filePath);
                if (dataTable.Rows.Count < 1)
                {
                    _logger.LogInfo("NO data found");
                    return false;
                }
                else
                {
                    _logger.LogInfo(dataTable.Rows.Count + " data(s) available");
                }

                using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    await connection.OpenAsync();

                    using (var transaction = connection.BeginTransaction())
                    {
                        try
                        {
                            _logger.LogInfo("DELETE exisiting ");
                            // Delete existing data
                            string deleteQuery = $"DELETE FROM {tableName}";
                            await connection.ExecuteAsync(deleteQuery, transaction: transaction);

                            _logger.LogInfo($"COPYING DATA to {tableName} ");
                            // Insert new data using SqlBulkCopy without explicit column mappings
                            using (var bulkCopy = new SqlBulkCopy(connection, SqlBulkCopyOptions.Default, transaction))
                            {
                                bulkCopy.DestinationTableName = tableName;
                                await bulkCopy.WriteToServerAsync(dataTable);
                            }

                            // Commit transaction
                            transaction.Commit();
                            return true;
                        }
                        catch (Exception ex)
                        {
                            // Rollback transaction if there's an error
                            transaction.Rollback();
                            _logger.LogInfo("ERROR: " + ex.Message);
                            _logger.LogInfo("ROLLBACK");
                            return false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);    
            }
        }

        private DataTable LoadCSVFiles(string filePath)
        {
            // @"C:\path\to\your\file.xlsx";
            _logger.LogInfo("Read CSV " + filePath);
            var dataTable = new DataTable();
            try
            {
               
                // Add columns to DataTable
                string[] headers = {
                    "distributorId", "distributorName", "channelId", "channelName", "subChannelId", "subChannelDesc",
                    "accountId", "accountName", "subAccountId", "subAccountDesc", "brandGroupName", "productStageName",
                    "productSkuId", "subBrandName", "productSizeName",  "oneBaselineValue"};
                foreach (string header in headers)
                {
                    dataTable.Columns.Add(header);
                }

                using (StreamReader reader = new StreamReader(filePath))
                {
                    bool isFirstLine = true;
                    string line;

                    while ((line = reader.ReadLine()) != null)
                    {
                        // Split each line by commas to get individual columns
                        string[] values = line.Split(',');

                        if (isFirstLine)
                        {
                            isFirstLine = false;
                        }
                        else
                        {
                            // Add subsequent rows as data rows
                            DataRow dataRow = dataTable.NewRow();
                            for (int i = 0; i < dataTable.Columns.Count; i++)
                            {
                                if (i == dataTable.Columns.Count - 1)
                                {
                                    if (float.TryParse(values[i], NumberStyles.Any, CultureInfo.InvariantCulture, out float floatValue))
                                    {
                                        dataRow[i] = floatValue;
                                    }
                                    else
                                    {
                                        dataRow[i] = 0; 
                                    }
                                }
                                else
                                {
                                    dataRow[i] = values[i];
                                }
                            }
                            dataTable.Rows.Add(dataRow);
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return dataTable;
        }
        private DataTable LoadExcelData(string filePath)
        {
            // @"C:\path\to\your\file.xlsx";
            _logger.LogInfo("Read Excel " + filePath);
            var dataTable = new DataTable();
            try
            {
                //using FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
                using var package = new ExcelPackage(new FileInfo(filePath));
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
             
                ExcelWorksheet worksheet = package.Workbook.Worksheets[0]; // Load the first worksheet

                // Add columns to DataTable
                string[] headers = {
                    "distributorId", "distributorName", "channelId", "channelName", "subChannelId", "subChannelDesc",
                    "accountId", "accountName", "subAccountId", "subAccountDesc", "brandGroupName", "productStageName",
                    "productSkuId", "subBrandName", "productSizeName",  "oneBaselineValue"};
                foreach (string header in headers)
                {
                    dataTable.Columns.Add(header);
                }  
                
                // Add rows to DataTable, starting from the second row (to skip headers)
                for (int row = 2; row <= worksheet.Dimension.End.Row; row++)
                {
                    var dataRow = dataTable.NewRow();
                    for (int col = 1; col <= headers.Length; col++)
                    {
                        var cellValue = worksheet.Cells[row, col].Text;
                        dataRow[col - 1] = cellValue;
                    }
                    dataTable.Rows.Add(dataRow);
                }

            }
            catch (Exception ex) {
                throw new Exception(ex.Message);
            }
            
            return dataTable;
        }
        public async Task ProcesMechanismTaskAsync(string sourceFolderPath, string destinationFolderPath,
            string userId, string userEmail)
        {
            string activity = "mechanism upload";
            // Ensure destination folder exists
            if (!Directory.Exists(destinationFolderPath))
            {
                Directory.CreateDirectory(destinationFolderPath);
            }

            // Get the list of .xls files in the source folder
            string[] files = Directory.GetFiles(sourceFolderPath, "*.xlsx");

            if (files.Length == 0)
            {
                _logger.LogInfo("No .xlsx files found in the source folder.");
                return;
            }
            
            UploadRepo repo = new UploadRepo(_configuration);

            // Process the first .xls file in the folder
            string sourceFilePath = files[0];
            string fileName = Path.GetFileName(sourceFilePath);


            try
            {
                int logId = 0;

                // Read the file into a stream
                using FileStream fileStream = new FileStream(sourceFilePath, FileMode.Open, FileAccess.Read);

               
                repo.InsertUploadLog(activity, fileName, userId, userEmail, "on progress");

                DataTable header = new("MstMechanismType");
                try
                {
                    using var package = new ExcelPackage(fileStream);
                    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                    var rowCount = 0;
                    ExcelWorksheet mechanism = package.Workbook.Worksheets[0];
                    rowCount = mechanism.Dimension.Rows;
                    int colCount = 11;
                    header.Columns.Add("Entity", typeof(string));
                    header.Columns.Add("SubCategory", typeof(string));
                    header.Columns.Add("Activity", typeof(string));
                    header.Columns.Add("SubActivity", typeof(string));
                    header.Columns.Add("SKU", typeof(string));
                    header.Columns.Add("Requirement", typeof(string));
                    header.Columns.Add("Discount", typeof(string));
                    header.Columns.Add("Mechanism", typeof(string));
                    header.Columns.Add("Channel", typeof(string));
                    header.Columns.Add("StartPromo", typeof(string));
                    header.Columns.Add("EndPromo", typeof(string));

                    for (int row = 2; row <= rowCount; row++)
                    {
                        List<object> cells = new List<object>();
                        for (int col = 1; col <= colCount; col++)
                        {
                            if (mechanism.Cells[row, 1].Value != null)
                            {
                                cells.Add(Convert.ToString(mechanism.Cells[row, col].Value));
                            }
                        }
                        if (cells.Count > 0)
                            header.Rows.Add(cells.ToArray());
                    }
                }
                catch (Exception)
                {
                    repo.InsertUploadLog(activity, fileName, userId, userEmail, "failed");
                    throw new Exception("Please check template entry");
                }
                fileStream.Close();
                if (userId != null)
                {
                    try
                    {
                        MechanismRepository mecha = new MechanismRepository(_configuration);
                        var result = await mecha.ImportMechanismWithStatInfo(header, userId!, userEmail);
                        _logger.LogInfo($"Import result: " + JsonConvert.SerializeObject(result));
                        string importStatus = "success";
                        if (result.failedRec>0 && result.successRec>0)
                        {
                            importStatus = "success (partial)";
                        } else 
                        if (result.successRec == 0)
                        {
                            importStatus = "failed";    
                        }
                        logId = await repo.InsertUploadLog(activity, fileName, userId, userEmail, importStatus);

                        // write log
                        string destinationFilePath = Path.Combine(destinationFolderPath,
                            Path.GetFileNameWithoutExtension(fileName) + "_" + logId.ToString() + ".xlsx");

                        using (FileStream oriFileStream = new FileStream(sourceFilePath, FileMode.Open, FileAccess.Read))
                        {
                            // Copy the stream to the destination file
                            using (FileStream destinationStream = new FileStream(destinationFilePath, FileMode.Create, FileAccess.Write))
                            {
                                oriFileStream.CopyTo(destinationStream);
                                _logger.LogInfo($"{fileName} copied to {destinationFilePath}");
                            }
                        }


                        // Delete the original file
                        File.Delete(sourceFilePath);
                        _logger.LogInfo($"{fileName} deleted");
                    }
                    catch (Exception ex)
                    {
                        var id = repo.InsertUploadLog(activity, fileName, userId, userEmail, "failed");
                        _logger.LogInfo("ERR: " + ex.Message);
                    }

                }

            }
            catch (Exception ex)
            {
                //repo.InsertUploadLog(activity, fileName, userId, userEmail, "failed");
                _logger.LogInfo("ERR: " + ex.Message);
            }
           

     


        }

        public async Task<bool> SendEmailApprovalReminder()
        {
            bool __res = false;
            try
            {
                SchedulerRepo scheduler = new SchedulerRepo(_configuration, __emailRepo);
        
               await scheduler.SendEmailApprovalReminder();
                __res = true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return __res;
        }
    }
}
