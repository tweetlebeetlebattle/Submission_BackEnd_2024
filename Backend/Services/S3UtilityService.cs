using Amazon.S3;
using Amazon.S3.Model;
using System.IO;
using System.Text;
using System.Threading.Tasks;

public class S3BucketAWSService : IS3BucketAWSService
{
    private readonly IAmazonS3 _s3Client;
    private readonly IS3UtilityService _s3UtilityService;

    public S3BucketAWSService(IAmazonS3 s3Client, IS3UtilityService s3UtilityService)
    {
        _s3Client = s3Client;
        _s3UtilityService = s3UtilityService;
    }

    public async Task<string> UploadFileAsync(string bucketName, string keyName, Stream fileStream)
    {
        var putRequest = new PutObjectRequest
        {
            BucketName = bucketName,
            Key = keyName,
            InputStream = fileStream,
            AutoCloseStream = true,
        };

        await _s3Client.PutObjectAsync(putRequest);

        return _s3UtilityService.GetPublicUrlForObject(keyName);
    }

    public async Task<string> UploadTextAsync(string bucketName, string keyName, string textContent)
    {
        using var stream = new MemoryStream(Encoding.UTF8.GetBytes(textContent));

        var putRequest = new PutObjectRequest
        {
            BucketName = bucketName,
            Key = keyName,
            InputStream = stream,
            ContentType = "text/plain",
            AutoCloseStream = true,
        };

        await _s3Client.PutObjectAsync(putRequest);

        return _s3UtilityService.GetPublicUrlForObject(keyName);
    }
}

public class S3UtilityService : IS3UtilityService
{
    private readonly string _bucketName = "bucketheadboris";
    private readonly string _region = "eu-north-1"; 

    public string GetPublicUrlForObject(string objectKey)
    {
        objectKey = Uri.EscapeDataString(objectKey);
        return $"https://{_bucketName}.s3.{_region}.amazonaws.com/{objectKey}";
    }
}
