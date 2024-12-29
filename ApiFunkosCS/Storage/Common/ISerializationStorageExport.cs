namespace ApiFunkosCS.Storage.Common;

public interface ISerializationStorageExport<T>
{
    Task ExportAsync(IEnumerable<T> data, Stream outputStream);
}