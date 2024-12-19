namespace ApiFunkosCS.Storage.Exceptions;

public class MinFileSizeStorageException() : StorageException($"The file is too small or there is no file provided");