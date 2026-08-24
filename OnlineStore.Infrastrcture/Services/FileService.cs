using OnlineStore.Application.Contracts;

namespace OnlineStore.Infrastructure.Services
{
    public class FileService : IFileService
    {
        private readonly string _basePath;
        public FileService(string basePath)
        {
            _basePath = basePath;
        }



        public async Task<string> UploadAsync(Stream fileData, string fileName, string folderName, CancellationToken cancellationToken = default)
        {
            var folderPath = Path.Combine(_basePath, folderName);
            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);
            var uniqueName = $"{Guid.NewGuid()}_{fileName}";
            var filePath = Path.Combine(folderPath, uniqueName);
            await using var fileStream = File.Create(filePath);
            await fileData.CopyToAsync(fileStream, cancellationToken);
            return Path.Combine(folderName, uniqueName).Replace("\\", "/");

        }
        public void Delete(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath))
                return;

            var absolutePath = Path.Combine(_basePath, filePath);
            if (File.Exists(absolutePath))
                File.Delete(absolutePath);
        }
    }
}
