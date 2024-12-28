using ApiFunkosCS.FunkoNamespace.Dto;
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
    public async Task<ActionResult<IEnumerable<FunkoDtoResponse>>> GetAll()
    {
        _logger.LogInformation("Obteniendo todos los Funkos");
        return await _funkoService.FindAllAsync();
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<FunkoDtoResponse>> GetFunkoById(int id)
    {
        var result = await _funkoService.FindByIdAsync(id);

        if (result.IsFailure) return NotFound(result);
        
        return Ok(result.Value);
    }

    [HttpPost]
    public async Task<ActionResult<FunkoDtoResponse>> CreateFunko([FromBody] FunkoDtoSaveRequest funko)
    {
        return await _funkoService.CreateAsync(funko);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<FunkoDtoResponse>> UpdateFunko(int id, [FromBody] FunkoDtoUpdateRequest funko)
    {
        var result = await _funkoService.UpdateAsync(id, funko);

        if (result.IsFailure) return NotFound(result);

        return Ok(result.Value);
    }
    
    [HttpPatch("{id}")]
    public async Task<ActionResult<FunkoDtoResponse>> UpdateImageFunko(int id, IFormFile imageFunko)
    {
        var result = await _funkoService.UpdateImageAsync(id, imageFunko);

        if (result.IsFailure) return NotFound(result);

        return Ok(result.Value);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<FunkoDtoResponse>> DeleteFunko(int id)
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