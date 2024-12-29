namespace ApiFunkosCS.Storage.Exceptions;

public class FileNotFoundException(string fileName) : StorageException($"the file: {fileName} was not found");