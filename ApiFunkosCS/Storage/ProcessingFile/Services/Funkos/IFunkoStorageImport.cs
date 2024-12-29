using ApiFunkosCS.FunkoNamespace.Dto;
using ApiFunkosCS.FunkoNamespace.Model;
using ApiFunkosCS.Storage.Common;

namespace ApiFunkosCS.Storage.ProcessingFile.Services.Funkos;

public interface IFunkoStorageImport : ISerializationStorageImport<FunkoDtoSaveRequest>, ISerializationStorageExport<Funko>
{
 
}