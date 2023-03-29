using System.Text;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Warehouse.API.Helpers.Binders.FileModelBinders;

public sealed class ImageFileModelBinder : IModelBinder
{
    private const int MaxFileSizeInBytes = 2 * 1024 * 1024; // 2 MB

    private static readonly HashSet<string> AllowedContentTypes = new()
    {
        "image/jpeg",
        "image/png",
        "image/webp",
        "image/svg+xml"
    };
    
    private const int MaxFilesAmount = 10;

    private ModelBindingContext _bindingContext;
    
    public async Task BindModelAsync(ModelBindingContext bindingContext)
    {
        _bindingContext = bindingContext;

        await CheckRequestAsync();
    }

    private async Task CheckRequestAsync()
    {
        var request = _bindingContext.ActionContext.HttpContext.Request;

        if (!HasFormContentType(request))
            return;

        var form = await request.ReadFormAsync();

        CheckForm(form);
    }

    private bool HasFormContentType(HttpRequest request)
    {
        if (!request.HasFormContentType)
            _bindingContext.ModelState.AddModelError(_bindingContext.ModelName,
                "The request must be a multipart/form-data request.");

        return request.HasFormContentType;
    }
    
    private void CheckForm(IFormCollection form)
    {
        var files = form.Files;

        var validFiles = AreFilesValid(files);

        if (_bindingContext.ModelState.IsValid)
            _bindingContext.Result = ModelBindingResult.Success(validFiles);
    }

    private IEnumerable<IFormFile> AreFilesValid(IFormFileCollection files)
    {
        if (!IsFileAmountValid(files))
            return Enumerable.Empty<IFormFile>();

        var validFiles = GetValidFiles(files);

        return validFiles;
    }
    
    private bool IsFileAmountValid(IFormFileCollection? files)
    {
        if (files == null || files.Count == 0)
        {
            _bindingContext.ModelState.AddModelError(_bindingContext.ModelName, "No files were provided");
            return false;
        }

        if (files.Count > MaxFilesAmount)
        {
            _bindingContext.ModelState.AddModelError(_bindingContext.ModelName,
                $"Too many files were provided. The maximum number is {MaxFilesAmount}");
            return false;
        }

        return true;
    }
    
    private IEnumerable<IFormFile> GetValidFiles(IFormFileCollection files)
    {
        var validFiles = new List<IFormFile>();

        foreach (var file in files)
        {
            var isThereErrors = !IsFileSizeValid(file);
            isThereErrors |= !IsContentTypeAllowed(file);

            if (!isThereErrors)
                validFiles.Add(file);
        }
        
        return validFiles;
    }

    private bool IsFileSizeValid(IFormFile file)
    {
        var isFileSizeValid = file.Length <= MaxFileSizeInBytes;
        
        if (!isFileSizeValid)
            AddFileValidationError(file,
                $"The file size of the file exceeds the {MaxFileSizeInBytes / (1024 * 1024)} MB limit");

        return isFileSizeValid;
    }

    private bool IsContentTypeAllowed(IFormFile file)
    {
        StringBuilder allowedContentTypesStringBuilder = new StringBuilder();
        foreach (var allowedContentType in AllowedContentTypes)
        {
            if (allowedContentType == file.ContentType)
                return true;

            allowedContentTypesStringBuilder.Append(allowedContentType);
            allowedContentTypesStringBuilder.Append(' ');
        }

        AddFileValidationError(file, "The content type of the file is not allowed." +
                                 $" Allowed content types: {allowedContentTypesStringBuilder}");

        return false;
    }

    private void AddFileValidationError(IFormFile file, string errorMessage)
    {
        _bindingContext.ModelState.AddModelError(file.FileName,errorMessage);
    }
}