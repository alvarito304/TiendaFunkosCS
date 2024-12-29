using System.Globalization;
using ApiFunkosCS.CategoryNamespace.Model;
using CsvHelper;

namespace ApiFunkosCS.Storage.ProcessingFile.Services;

public class CategoryStorageCsv : ICategoryStorageCsv
{
    public async IAsyncEnumerable<Category> ImportAsync(Stream fileStream)
    {
        using var reader = new StreamReader(fileStream);
        using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);

        await foreach (var category in csv.GetRecordsAsync<Category>())
        {
            if (string.IsNullOrWhiteSpace(category.Name))
            {
                continue; // Skip categories without a name
            }
            yield return category;
        }
    }


    public async Task ExportAsync(IEnumerable<Category> categories, Stream outputStream)
    {
        // Validación de entrada
        if (categories == null)
            throw new ArgumentNullException(nameof(categories));
        if (outputStream == null)
            throw new ArgumentNullException(nameof(outputStream));

        await using var writer = new StreamWriter(outputStream, leaveOpen: true);
        await using var csv = new CsvWriter(writer, CultureInfo.InvariantCulture);

        // Escribir encabezados y datos
        await csv.WriteRecordsAsync(categories);

        // Asegurarse de que todo esté escrito al flujo
        await writer.FlushAsync();
    }

}