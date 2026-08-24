namespace OnlineStore.Application.Contracts
{
    public interface IFileService
    {
        Task<string> UploadAsync(Stream fileData, string fileName, string folderName, CancellationToken cancellationToken = default);

        void Delete(string filePath);


    }
}
