using ApiFunkosCS.CategoryNamespace.Model;
using ApiFunkosCS.CategoryNamespace.Service;
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
        return result.IsSuccess ? Ok(result.Value) : NotFound(result.Error);
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
}