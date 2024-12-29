namespace ApiFunkosCS.Storage.Exceptions;

public class FileExtensionNotAllowedException(string fileName) : StorageException($"the file: {fileName} has an unsupported extension");