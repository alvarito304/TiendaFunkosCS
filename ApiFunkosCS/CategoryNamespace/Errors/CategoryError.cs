namespace ApiFunkosCS.CategoryNamespace.Errors;

public class CategoryError : Exception
{
    private string? Message { get; init; }
    
    public override string ToString()
    {
        return Message ?? "ERROR";
    }
    
    public class NotFound : CategoryError
    {
        public string CategoryId { get; }
        public NotFound(string id)
        {
            CategoryId = id;
            Message = "Esta Categoria no se encuentra con el ID: " + id;
        }
    }
}