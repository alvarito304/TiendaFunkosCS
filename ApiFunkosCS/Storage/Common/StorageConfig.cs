namespace ApiFunkosCS.Storage.Common;

public class StorageConfig
{
    public string UploadDirectory { get; set; }
    
    public long MaxFileSize { get; set; }
    
    public List<string>AllowedExtensions { get; set; }
    
    public bool RemoveAll { get; set; }
}