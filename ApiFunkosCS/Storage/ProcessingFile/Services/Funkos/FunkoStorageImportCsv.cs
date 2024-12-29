using System.Globalization;
using ApiFunkosCS.FunkoNamespace.Dto;
using ApiFunkosCS.FunkoNamespace.Model;
using CsvHelper;

namespace ApiFunkosCS.Storage.ProcessingFile.Services.Funkos;

public class FunkoStorageImportCsv : IFunkoStorageImportCsv
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
    

    public async Task ExportAsync(IEnumerable<Funko> data, Stream outputStream)
    {
        // Validación de entrada
        if (data == null)
            throw new ArgumentNullException(nameof(data));
        if (outputStream == null)
            throw new ArgumentNullException(nameof(outputStream));
        
        await using var writer = new StreamWriter(outputStream);
        await using var csv = new CsvWriter(writer, CultureInfo.InvariantCulture);
        // Escribir encabezados y datos
        await csv.WriteRecordsAsync(data);

        // Asegurarse de que todo esté escrito al flujo
        await writer.FlushAsync();
    }
    
}