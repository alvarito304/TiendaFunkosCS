namespace ApiFunkosCS.FunkoNamespace.Exception;

public abstract class FunkoError : System.Exception
{
    private string? Message { get; init; }
    
    public override string ToString()
    {
        return Message ?? "ERROR";
    }
    
    public class NotFound : FunkoError
    {
        public NotFound(int id)
        {
            Message = "Esta Funko no se encuentra con el ID: " + id;
        }
    }
        
}