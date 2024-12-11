using System.IO;
using System.Threading.Tasks;

public interface IS3BucketAWSService
{
    Task<string> UploadFileAsync(string bucketName, string keyName, Stream fileStream);
    Task<string> UploadTextAsync(string bucketName, string keyName, string textContent);
}
