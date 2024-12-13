using ApiFunkosCS.FunkoNamespace.Exception;
using ApiFunkosCS.FunkoNamespace.Model;
using ApiFunkosCS.FunkoNamespace.Service;
using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;



namespace ApiFunkosCS.FunkoNamespace.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FunkoController : ControllerBase
{
    private readonly IFunkoService _funkoService;
    private ILogger _logger;
    
    public FunkoController(IFunkoService funkoService, ILogger<FunkoController> logger)
    {
        _funkoService = funkoService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Funko>>> GetAll()
    {
        _logger.LogInformation("Obteniendo todos los Funkos");
        return await _funkoService.FindAllAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Funko>> GetFunkoById(int id)
    {
        var result = await _funkoService.FindByIdAsync(id);

        if (result.IsFailure) return NotFound(result);
        
        return Ok(result.Value);
    }

    [HttpPost]
    public async Task<ActionResult<Funko>> CreateFunko([FromBody] Funko funko)
    {
        return await _funkoService.CreateAsync(funko);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<Funko>> UpdateFunko(int id, [FromBody] Funko funko)
    {
        var result = await _funkoService.UpdateAsync(id, funko);

        if (result.IsFailure) return NotFound(result);

        return Ok(result.Value);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<Funko>> DeleteFunko(int id)
    {
        var result = await _funkoService.DeleteAsync(id);

        if (result.IsFailure) return NotFound(result);

        return Ok(result.Value);
    }

    [HttpDelete]
    public void DeleteAllFunkos()
    {
        _funkoService.DeleteAllAsync();
    }
}