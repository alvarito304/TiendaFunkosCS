using ApiFunkosCS.FunkoNamespace.Dto;
using ApiFunkosCS.FunkoNamespace.Model;

namespace ApiFunkosCS.Storage.ProcessingFile.Services.Funkos;

public class FunkosStorageJson : IFunkoStorageJson
{
    public IAsyncEnumerable<FunkoDtoSaveRequest> ImportAsync(Stream file)
    {
        throw new NotImplementedException();
    }

    public Task ExportAsync(IEnumerable<FunkoDtoSaveRequest> data, Stream outputStream)
    {
        throw new NotImplementedException();
    }
}