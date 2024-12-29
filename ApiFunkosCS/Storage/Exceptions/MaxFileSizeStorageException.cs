namespace ApiFunkosCS.Storage.Exceptions;

public class MaxFileSizeStorageException(string fileName)
    : StorageException($"the file: {fileName} exceeds the maximum allowed size");