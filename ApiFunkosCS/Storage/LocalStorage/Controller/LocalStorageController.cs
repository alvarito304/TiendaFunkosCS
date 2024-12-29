using ApiFunkosCS.Storage.Common;
using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Mvc;

namespace ApiFunkosCS.Storage.LocalStorage.Controller;


[ApiController]
[Route("api/[controller]")]
public class LocalStorageController(IStorageService localStorageService, ILogger<LocalStorageController> logger)
    : ControllerBase
{
    private const string Route = "api/localstorage";

    [HttpPost]
    public async Task<IActionResult> UploadFile(IFormFile file)
    {
            var filename = await localStorageService.SaveFileAsync(file);
            var baseUrl = $"{Request.Scheme}://{Request.Host}{Request.PathBase}";
            var fileUrl = $"{baseUrl}/{Route}/{filename}";
            return Ok(new { fileUrl });
    }

    [HttpGet("{filename}")]
    public async Task<IActionResult> DownloadFile(string filename)
    {
            var fileStream = await localStorageService.GetFileAsync(filename);
            var fileExtension = Path.GetExtension(filename);
            var mimeType = MimeTypes.GetMimeType(fileExtension);
            return File(fileStream, mimeType, filename);
    }

    [HttpDelete("{filename}")]
    public async Task<IActionResult> DeleteFile(string filename)
    {
            await localStorageService.DeleteFileAsync(filename);
            return NoContent();
    }

}