using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ApiFunkosCS.FunkoNamespace.Dto;

public class FunkoDtoSaveRequest
{
    [Required]
    [MaxLength(100)]
    [MinLength(3)]
    public string Name { get; set; }
    
    [MaxLength(255)]
    [MinLength(10)]
    public string Description { get; set; }
    
    [Required]
    [DefaultValue(0)]
    [Range(1, 100)]
    public int Stock { get; set; } 
    
    public int Price { get; set; }
    
    public string CategoryID { get; set; }

}