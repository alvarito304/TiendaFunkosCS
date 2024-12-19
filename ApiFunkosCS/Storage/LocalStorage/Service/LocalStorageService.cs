using ApiFunkosCS.Storage.Common;
using ApiFunkosCS.Storage.Exceptions;

namespace ApiFunkosCS.Storage.LocalStorage.Service;

public class LocalStorageService(StorageConfig config, ILogger<LocalStorageService> logger) : IStorageService
{

    public async Task<string> SaveFileAsync(IFormFile file)
    {
        logger.LogInformation($"Saving file {file.FileName} to local storage");
        if (file.Length > config.MaxFileSize)
        {
            throw new MaxFileSizeStorageException(file.FileName);
        }

        if (file.Length <= 0 || file == null)
        {
            throw new MinFileSizeStorageException();
        }

        var fileExtension = Path.GetExtension(file.FileName);
        if (!config.AllowedExtensions.Contains(fileExtension))
        {
            throw new FileExtensionNotAllowedException(file.FileName);
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
        logger.LogInformation($"Getting file {fileName} from local storage");
            var filePath = Path.Combine(config.UploadDirectory, fileName);
            if (!File.Exists(filePath))
            {
                throw new Exceptions.FileNotFoundException(fileName);
            }
            return new FileStream(filePath, FileMode.Open, FileAccess.Read);
    }

    public async Task<bool> DeleteFileAsync(string fileName)
    {
        logger.LogInformation($"Deleting file {fileName} from local storage");
       
            var filePath = Path.Combine(config.UploadDirectory, fileName);
            if (!File.Exists(filePath))
            {
                throw new Exceptions.FileNotFoundException(fileName);
            }
            File.Delete(filePath);
            return true;
    }
}