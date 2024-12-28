using ApiFunkosCS.FunkoNamespace.Dto;
using ApiFunkosCS.FunkoNamespace.Exception;
using ApiFunkosCS.FunkoNamespace.Model;
using CSharpFunctionalExtensions;

namespace ApiFunkosCS.FunkoNamespace.Service;

public interface IFunkoService
{
    Task<List<FunkoDtoResponse>> FindAllAsync();
    Task<Result<FunkoDtoResponse, FunkoError.NotFound>> FindByIdAsync(int id);
    Task<FunkoDtoResponse> CreateAsync(FunkoDtoSaveRequest funko);
    Task<Result<FunkoDtoResponse, FunkoError>> UpdateAsync(int id, FunkoDtoUpdateRequest funko);
    Task<Result<FunkoDtoResponse, FunkoError>> UpdateImageAsync(int id, IFormFile imageFunko);
    Task<Result<FunkoDtoResponse, FunkoError.NotFound>> DeleteAsync(int id);
    void DeleteAllAsync();
}