using ApiFunkosCS.CategoryNamespace.Model;
using ApiFunkosCS.CategoryNamespace.Service;
using ApiFunkosCS.Storage.Common;
using Microsoft.AspNetCore.Mvc;

namespace ApiFunkosCS.CategoryNamespace.Controller;

[ApiController]
[Route("api/[controller]")]
public class CategoryController : ControllerBase
{
    private readonly ICategoryService _categoryService;
    private readonly ILogger<CategoryController> _logger;

    public CategoryController(ICategoryService categoryService, ILogger<CategoryController> logger)
    {
        _categoryService = categoryService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<List<Category>>> GetAll()
    {
        _logger.LogInformation("Getting all categories");
        var result = await _categoryService.FindAllAsync();
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Category>> GetById(string id)
    {
        _logger.LogInformation($"Getting category by id: {id}");
        var result = await _categoryService.FindByIdAsync(id);
        if (result.IsFailure) return NotFound(result);
        return Ok(result.Value);
    }

    [HttpPost]
    public async Task<ActionResult<Category>> Post([FromBody] Category category)
    {
        _logger.LogInformation($"Adding new category: {category.Name}");
        var result = await _categoryService.AddAsync(category);
        return CreatedAtAction(nameof(GetById), new {id = result.Id }, result);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<Category>> Put(string id, [FromBody] Category category)
    {
        _logger.LogInformation($"Updating category: {id}");
        var result = await _categoryService.UpdateAsync(id, category);
        return result.IsSuccess ? Ok(result.Value) : NotFound(result.Error);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<Category>> Delete(string id)
    {
        _logger.LogInformation($"Deleting category: {id}");
        var result = await _categoryService.DeleteAsync(id);
        return Ok(result);
    }

    [HttpPost("importByCsv")]
    public async Task<ActionResult> ImportByCsv(IFormFile file)
    {
        _logger.LogInformation("Importing categories from CSV");
        var categories = await _categoryService.ImportByCsvAsync(file);
        return Ok(categories);
    }
    
    [HttpGet("exportCsvFile")]
    public async Task<IActionResult> ExportCsvFile()
    {
        var fileStream = await _categoryService.ExportCsvAsync();
        var fileName = Path.GetFileName(fileStream.Name);
        var fileExtension = Path.GetExtension(fileName);
        var mimeType = MimeTypes.GetMimeType(fileExtension);
        return File(fileStream, mimeType, fileName);
    }
    
    [HttpPost("importByJson")]
    public async Task<ActionResult> ImportByJson(IFormFile file)
    {
        _logger.LogInformation("Importing categories from CSV");
        var categories = await _categoryService.ImportByJsonAsync(file);
        return Ok(categories);
    }
    
    [HttpGet("exportJsonFile")]
    public async Task<IActionResult> ExportJsonFile()
    {
        var fileStream = await _categoryService.ExportJsonAsync();
        var fileName = Path.GetFileName(fileStream.Name);
        var fileExtension = Path.GetExtension(fileName);
        var mimeType = MimeTypes.GetMimeType(fileExtension);
        return File(fileStream, mimeType, fileName);
    }
}