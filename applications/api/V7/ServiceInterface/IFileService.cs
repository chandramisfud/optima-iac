using Repositories.Entities.Models;

namespace V7.Services
{
    public interface IFileService
    {
        bool UploadFiles(List<IFormFile> files, string subDirectory);

        bool UploadFile(IFormFile files, string subDirectory, string fileName);
        (string fileType, byte[] archiveData, string archiveName) DownloadFiles(string subDirectory);
        string SizeConverter(long bytes);
        List<FileInformation> GetFileList(string urlRoot, string subDirectory);
        string GetRootPath();
        string GetFileContentType(string filePath);

        // get NLog files
        List<FileInformation> GetLogFileList(int year=2022, int month = 0);
    }
}
