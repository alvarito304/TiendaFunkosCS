using System.Globalization;
using ApiFunkosCS.FunkoNamespace.Dto;
using CsvHelper;

namespace ApiFunkosCS.Storage.ProcessingFile.Services.Funkos;

public class FunkoStorageCsv : IFunkoStorageCsv
{
    public async IAsyncEnumerable<FunkoDtoSaveRequest> ImportAsync(Stream file)
    {
        using var reader = new StreamReader(file);
        using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);

        await foreach (var funko in csv.GetRecordsAsync<FunkoDtoSaveRequest>())
        {
            if (string.IsNullOrWhiteSpace(funko.Name) || funko.Price <= 0 || funko.Stock < 0 || string.IsNullOrWhiteSpace(funko.CategoryID) || funko.Description.Length < 10 || funko.Description.Length > 255)
            {
                continue; // Skip funkos 
            }
            yield return funko;
        }
    }

    public Task ExportAsync(IEnumerable<FunkoDtoSaveRequest> data, Stream outputStream)
    {
        throw new NotImplementedException();
    }
    
}