using Amazon.S3;
using Amazon.S3.Transfer;
using Microsoft.Extensions.Options;

namespace Infrastructure.Services;

public class S3StorageService : IStorageService
{
    private readonly IAmazonS3 _s3Client;
    private readonly string _bucketName;
    private readonly IOptions<ImageStoreConfiguration> _imageStoreConfig;
    public S3StorageService(IAmazonS3 s3Client, IOptions<ImageStoreConfiguration> awsConfig, IOptions<ImageStoreConfiguration> imageStoreConfig)
    {
        _s3Client = s3Client;
        _imageStoreConfig = imageStoreConfig;
        _bucketName = awsConfig.Value.BucketName;
    }

    public async Task<string> UploadFileAsync(Stream fileStream, string fileName)
    {
        var fileTransferUtility = new TransferUtility(_s3Client);
        
        await fileTransferUtility.UploadAsync(fileStream, _bucketName, fileName);

        return _imageStoreConfig.Value.ImagekitUrl + fileName;
    }

    public async Task DeleteFileAsync(string fileKey)
    {
        var deleteRequest = new Amazon.S3.Model.DeleteObjectRequest
        {
            BucketName = _bucketName,
            Key = fileKey
        };
        
        await _s3Client.DeleteObjectAsync(deleteRequest);
    }
}