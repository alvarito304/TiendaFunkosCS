using ApiFunkosCS.CategoryNamespace.Model;
using ApiFunkosCS.Storage.Common;

namespace ApiFunkosCS.Storage.ProcessingFile.Services.Categories;

public interface ICategoryStorageImport: ISerializationStorageImport<Category> , ISerializationStorageExport<Category>
{
    
}