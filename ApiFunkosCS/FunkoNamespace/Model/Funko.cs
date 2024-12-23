using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ApiFunkosCS.CategoryNamespace.Model;

namespace ApiFunkosCS.FunkoNamespace.Model;

public class Funko
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    
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
    public string ImageUrl { get; set; }
    
    public int Price { get; set; }
    
    public string CategoryID { get; set; }
    
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public DateTime CreatedAt { get; set; }
    [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
    public DateTime UpdatedAt { get; set; }
    
        
        public Funko()
        {
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = CreatedAt;
            ImageUrl = "https://via.placeholder.com/150";
        }

        
        
    
}