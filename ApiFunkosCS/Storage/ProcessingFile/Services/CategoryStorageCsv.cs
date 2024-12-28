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
            yield return category;
        }
    }


    public Task<int> ExportAsync(FileInfo file, List<Category> data)
    {
        throw new NotImplementedException();
    }
}