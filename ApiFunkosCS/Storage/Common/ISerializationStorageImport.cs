namespace ApiFunkosCS.Storage.Common;

public interface ISerializationStorageImport<T>
{
    IAsyncEnumerable<T> ImportAsync(Stream file);
 
}