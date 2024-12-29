using ApiFunkosCS.CategoryNamespace.Model;

namespace ApiFunkosCS.FunkoNamespace.Dto;

public class FunkoDtoResponse
{
  
    public string Name { get; set; }
    
   
    public string Description { get; set; }
    
  
    public int Stock { get; set; } 
    public string ImageUrl { get; set; }
    
    public int Price { get; set; }
    
    public Category Category { get; set; }
    
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}