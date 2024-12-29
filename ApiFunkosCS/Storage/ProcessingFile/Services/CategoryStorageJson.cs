using System.Text.Json;
using ApiFunkosCS.CategoryNamespace.Model;
using Newtonsoft.Json;
using JsonSerializer = Newtonsoft.Json.JsonSerializer;

namespace ApiFunkosCS.Storage.ProcessingFile.Services;

public class CategoryStorageJson : ICategoryStorageJson 
{
    public async IAsyncEnumerable<Category> ImportAsync(Stream file)
    {
        using var reader = new StreamReader(file);
        await using var jsonReader = new JsonTextReader(reader);
        var categories = await Task.Run(() => JsonSerializer.Create().Deserialize<List<Category>>(jsonReader));

        if (categories != null)
        {
            foreach (var category in categories)
            {
                if (string.IsNullOrWhiteSpace(category.Name))
                {
                    continue;
                }
                yield return category; 
            }
        }
    }

    public async Task ExportAsync(IEnumerable<Category> data, Stream outputStream)
    {
        if (data == null)
            throw new ArgumentNullException(nameof(data));
        if (outputStream == null)
            throw new ArgumentNullException(nameof(outputStream));
        
        await using var writer = new StreamWriter(outputStream);
        await using var jsonWriter = new JsonTextWriter(writer)
        {
            Formatting = Formatting.Indented,
        };

        var serializer = new JsonSerializer();
        serializer.Serialize(jsonWriter, data);
        await writer.FlushAsync();
    }
}