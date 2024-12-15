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
    
    public DateOnly CreatedAt { get; set; }
    public DateOnly UpdatedAt { get; set; }

        public void Update(Funko updatedFunko)
        {
            Name = !string.IsNullOrEmpty(updatedFunko.Name)? updatedFunko.Name : Name;
            Description = !string.IsNullOrEmpty(updatedFunko.Description)? updatedFunko.Description : Description;
            Stock = updatedFunko.Stock > 0? updatedFunko.Stock : Stock;
            ImageUrl =!string.IsNullOrEmpty(updatedFunko.ImageUrl)? updatedFunko.ImageUrl : ImageUrl;
            Price = updatedFunko.Price >= 0? updatedFunko.Price : Price;
          //  Category = updatedFunko.Category;
            UpdatedAt = DateOnly.FromDateTime(DateTime.Now);
        }
        
        public Funko()
        {
            CreatedAt = DateOnly.FromDateTime(DateTime.Now);
            UpdatedAt = CreatedAt;
        }

        
        
    
}