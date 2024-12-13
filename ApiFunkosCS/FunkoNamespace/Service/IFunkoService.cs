using ApiFunkosCS.FunkoNamespace.Exception;
using ApiFunkosCS.FunkoNamespace.Model;
using CSharpFunctionalExtensions;

namespace ApiFunkosCS.FunkoNamespace.Service;

public interface IFunkoService
{
    Task<List<Funko>> FindAllAsync();
    Task<Result<Funko, FunkoError.NotFound>> FindByIdAsync(int id);
    Task<Funko> CreateAsync(Funko funko);
    Task<Result<Funko, FunkoError.NotFound>> UpdateAsync(int id, Funko funko);
    Task<Result<Funko, FunkoError.NotFound>> DeleteAsync(int id);
    void DeleteAllAsync();
}