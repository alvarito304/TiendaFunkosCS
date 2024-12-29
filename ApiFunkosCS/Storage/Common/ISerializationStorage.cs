namespace ApiFunkosCS.Storage.Common;

public interface ISerializationStorage<T>
{
    IAsyncEnumerable<T> ImportAsync(Stream file);
    Task ExportAsync(IEnumerable<T> data, Stream outputStream);
}