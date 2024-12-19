using ApiFunkosCS.Storage.Common;

namespace ApiFunkosCS.Storage.LocalStorage.Service;

public class LocalStorageService(StorageConfig config, ILogger<LocalStorageService> logger) : IStorageService
{

    public async Task<string> SaveFileAsync(IFormFile file)
    {
        logger.LogInformation($"Saving file {file.FileName} to local storage");
        if (file.Length > config.MaxFileSize)
        {
            
        }

        var fileExtension = Path.GetExtension(file.FileName);
        if (!config.AllowedExtensions.Contains(fileExtension))
        {
            
        }
        
        var filename = $"{Guid.NewGuid()}{fileExtension}";
        var filePath = Path.Combine(config.UploadDirectory, filename);
        
        await using (var fileStream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(fileStream);
        } logger.LogInformation($"File {file.FileName} saved successfully to local storage");
        return filename;
    }

    public async Task<FileStream> GetFileAsync(string fileName)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> DeleteFileAsync(string fileName)
    {
        throw new NotImplementedException();
    }
}