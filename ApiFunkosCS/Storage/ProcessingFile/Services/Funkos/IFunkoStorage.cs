using ApiFunkosCS.FunkoNamespace.Dto;
using ApiFunkosCS.FunkoNamespace.Model;
using ApiFunkosCS.Storage.Common;

namespace ApiFunkosCS.Storage.ProcessingFile.Services.Funkos;

public interface IFunkoStorage : ISerializationStorage<FunkoDtoSaveRequest>
{
    
}