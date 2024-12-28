namespace ApiFunkosCS.Storage.Common;

public interface ISerializationStorage<T>
{
    IAsyncEnumerable<T> ImportAsync(Stream file);
    Task<int> ExportAsync(FileInfo file, List<T> data);
}