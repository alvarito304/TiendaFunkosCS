using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ApiFunkosCS.FunkoNamespace.Dto;

public class FunkoDtoUpdateRequest
{

    
    [MaxLength(255)]
    [MinLength(10)]
    public string Description { get; set; }
    
    [Required]
    [DefaultValue(0)]
    [Range(1, 100)]
    public int Stock { get; set; } 
    public int Price { get; set; }
    
}