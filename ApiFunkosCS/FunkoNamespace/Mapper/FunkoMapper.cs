using ApiFunkosCS.CategoryNamespace.Model;
using ApiFunkosCS.FunkoNamespace.Dto;
using ApiFunkosCS.FunkoNamespace.Model;

namespace ApiFunkosCS.FunkoNamespace.Mapper;

public static class FunkoMapper
{
    public static FunkoDtoResponse MapToDto(this Funko funko, Category category)
    {
        return new FunkoDtoResponse
        {
  
            Name = funko.Name,
            Description = funko.Description,
            ImageUrl = funko.ImageUrl,
            Stock = funko.Stock,
            Price = funko.Price,
            Category = category,
            CreatedAt = funko.CreatedAt,
            UpdatedAt = funko.UpdatedAt
        };
    }

    public static Funko MapToEntity(this FunkoDtoUpdateRequest request, Funko oldFunko)
    {
        oldFunko.Description = request.Description;
        oldFunko.Stock = request.Stock;
        oldFunko.ImageUrl = request.ImageUrl;
        oldFunko.Price = request.Price;
        return oldFunko;
    }

    public static Funko MapToEntity(this FunkoDtoSaveRequest request)
    {
        return new Funko
        {
            Name = request.Name,
            Description = request.Description,
            Stock = request.Stock,
            Price = request.Price,
            CategoryID = request.CategoryID
        };
    }
}