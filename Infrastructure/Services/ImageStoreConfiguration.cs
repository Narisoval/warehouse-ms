namespace Infrastructure.Services;

public class ImageStoreConfiguration
{
    public string BucketName { get; set; } = default!;
    
    public string ImagekitUrl { get; set; } = default!;
}