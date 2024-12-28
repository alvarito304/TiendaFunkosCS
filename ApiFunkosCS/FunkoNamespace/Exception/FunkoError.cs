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
    
    public class NothingToUpdate : FunkoError
    {
        public NothingToUpdate(int id)
        {
            Message = "No se ha pasado ningun dato o imagen para actualizar la Funko con el ID: " + id;
        }
    }
        
}