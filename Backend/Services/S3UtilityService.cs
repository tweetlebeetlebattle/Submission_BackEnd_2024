using Amazon.S3;
using Amazon.S3.Model;
using System.IO;
using System.Text;
using System.Threading.Tasks;

public class S3BucketAWSService
{
    private readonly IAmazonS3 _s3Client;
    private readonly S3UtilityService _s3UtilityService;

    public S3BucketAWSService(IAmazonS3 s3Client, S3UtilityService s3UtilityService)
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

        // Upload the file to the specified bucket
        await _s3Client.PutObjectAsync(putRequest);

        // Instead of generating a pre-signed URL, just return the public URL
        return _s3UtilityService.GetPublicUrlForObject(keyName);
    }

    public async Task<string> UploadTextAsync(string bucketName, string keyName, string textContent)
    {
        // Convert the string content to a MemoryStream
        using var stream = new MemoryStream(Encoding.UTF8.GetBytes(textContent));

        var putRequest = new PutObjectRequest
        {
            BucketName = bucketName,
            Key = keyName,
            InputStream = stream,
            ContentType = "text/plain", // Set the appropriate MIME type for text
            AutoCloseStream = true,
        };

        // Upload the text to the specified bucket
        await _s3Client.PutObjectAsync(putRequest);

        // Return the public URL for the uploaded text
        return _s3UtilityService.GetPublicUrlForObject(keyName);
    }
}

public class S3UtilityService
{
    private readonly string _bucketName = "bucketheadboris";
    private readonly string _region = "eu-north-1"; // Ensure this matches your bucket's region

    public string GetPublicUrlForObject(string objectKey)
    {
        objectKey = Uri.EscapeDataString(objectKey); // Properly encode the object key
        return $"https://{_bucketName}.s3.{_region}.amazonaws.com/{objectKey}";
    }
}
