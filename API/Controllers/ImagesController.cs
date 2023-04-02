using Amazon.Runtime;
using Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;
using Warehouse.API.DTO;
using ILogger = Serilog.ILogger;

namespace Warehouse.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ImagesController : ControllerBase
{
    private readonly IStorageService _storageService;
    private readonly ILogger _logger;
    
    public ImagesController(IStorageService storageService, ILogger logger)
    {
        _storageService = storageService;
        _logger = logger;
    }

    [HttpPost]
    [ProducesResponseType(typeof(IEnumerable<ImageFileDto>),StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<IEnumerable<ImageFileDto>>> Upload(List<IFormFile>? files)
    {
        List<ImageFileDto> uploadedImages;
        List<string> uploadedFileKeys = new();

        try
        {
            uploadedImages = await UploadFilesAsync(files!, uploadedFileKeys);
        }
        catch (AmazonServiceException ex)
        {
            return await HandleAmazonException(uploadedFileKeys, ex);
        }

        return Ok(uploadedImages);
    }

    private async Task<List<ImageFileDto>> UploadFilesAsync(IEnumerable<IFormFile> files, ICollection<string> uploadedFileKeys)
    {
        var uploadTasks = files.Select(async file =>
        {
            await using var fileStream = file.OpenReadStream();
            var fileExtension = Path.GetExtension(file.FileName);
            var fileName = Guid.NewGuid() + fileExtension;

            var imageUrl = await _storageService.UploadFileAsync(fileStream, fileName);
            uploadedFileKeys.Add(fileName);

            return new ImageFileDto { FileName = file.FileName, FileUrl = imageUrl };
        });

        return (await Task.WhenAll(uploadTasks)).ToList();
    }
    
    private async Task<ActionResult> HandleAmazonException(List<string> uploadedFileKeys, 
        AmazonServiceException ex)
    {
        await RollbackUploadedFiles(uploadedFileKeys);

        _logger.Error("Error occurred while uploading images to S3 {@ExceptionMessage}", 
            ex.Message);
        
        var statusCode = (int)ex.StatusCode;
        return StatusCode(statusCode,
        "Couldn't upload all the files. " +
            $"All successfully uploaded files({uploadedFileKeys}) have been rolled back.");
    }

    private async Task RollbackUploadedFiles(List<string> uploadedFileKeys)
    {
        var deleteTasks = uploadedFileKeys.Select(key => _storageService.DeleteFileAsync(key));
        await Task.WhenAll(deleteTasks);
    }
}