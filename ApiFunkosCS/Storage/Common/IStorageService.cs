namespace ApiFunkosCS.Storage.Common;

public interface IStorageService
{
    Task<string> SaveFileAsync(IFormFile file);
    Task<FileStream> GetFileAsync(string fileName);
    Task<bool> DeleteFileAsync(string fileName);
}