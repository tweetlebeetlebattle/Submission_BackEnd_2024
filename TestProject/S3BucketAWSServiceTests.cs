using NUnit.Framework;
using Moq;
using Amazon.S3;
using Amazon.S3.Model;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace TestProject
{
    public class S3BucketAWSServiceTests
    {
        private Mock<IAmazonS3> _mockS3Client;
        private Mock<IS3UtilityService> _mockS3UtilityService;
        private IS3BucketAWSService _s3BucketAWSService;

        [SetUp]
        public void Setup()
        {
            _mockS3Client = new Mock<IAmazonS3>();
            _mockS3UtilityService = new Mock<IS3UtilityService>();

            _s3BucketAWSService = new S3BucketAWSService(
                _mockS3Client.Object,
                _mockS3UtilityService.Object
            );
        }

        [Test]
        public async Task UploadFileAsync_UploadsFileSuccessfully_ReturnsPublicUrl()
        {
            // Arrange
            var bucketName = "test-bucket";
            var keyName = "test-key/file.txt";
            var fileStream = new MemoryStream(Encoding.UTF8.GetBytes("file content"));
            var publicUrl = "https://test-bucket.s3.eu-north-1.amazonaws.com/test-key/file.txt";

            _mockS3Client
                .Setup(client => client.PutObjectAsync(It.IsAny<PutObjectRequest>(), default))
                .ReturnsAsync(new PutObjectResponse());

            _mockS3UtilityService
                .Setup(service => service.GetPublicUrlForObject(keyName))
                .Returns(publicUrl);

            // Act
            var result = await _s3BucketAWSService.UploadFileAsync(bucketName, keyName, fileStream);

            // Assert
            Assert.That(result, Is.EqualTo(publicUrl));
            _mockS3Client.Verify(client => client.PutObjectAsync(
                It.Is<PutObjectRequest>(req => req.BucketName == bucketName && req.Key == keyName && req.InputStream == fileStream),
                default),
                Times.Once);
            _mockS3UtilityService.Verify(service => service.GetPublicUrlForObject(keyName), Times.Once);
        }

        [Test]
        public async Task UploadTextAsync_UploadsTextSuccessfully_ReturnsPublicUrl()
        {
            // Arrange
            var bucketName = "test-bucket";
            var keyName = "test-key/text.txt";
            var textContent = "sample text content";
            var publicUrl = "https://test-bucket.s3.eu-north-1.amazonaws.com/test-key/text.txt";

            _mockS3Client
                .Setup(client => client.PutObjectAsync(It.IsAny<PutObjectRequest>(), default))
                .ReturnsAsync(new PutObjectResponse());

            _mockS3UtilityService
                .Setup(service => service.GetPublicUrlForObject(keyName))
                .Returns(publicUrl);

            // Act
            var result = await _s3BucketAWSService.UploadTextAsync(bucketName, keyName, textContent);

            // Assert
            Assert.That(result, Is.EqualTo(publicUrl));
            _mockS3Client.Verify(client => client.PutObjectAsync(
                It.Is<PutObjectRequest>(req => req.BucketName == bucketName && req.Key == keyName && req.ContentType == "text/plain"),
                default),
                Times.Once);
            _mockS3UtilityService.Verify(service => service.GetPublicUrlForObject(keyName), Times.Once);
        }
    }

    public class S3UtilityServiceTests
    {
        private IS3UtilityService _s3UtilityService;

        [SetUp]
        public void Setup()
        {
            _s3UtilityService = new S3UtilityService();
        }

        [Test]
        public void GetPublicUrlForObject_EncodesKey_ReturnsCorrectUrl()
        {
            // Arrange
            var objectKey = "folder/test file.txt";
            var expectedUrl = "https://bucketheadboris.s3.eu-north-1.amazonaws.com/folder%2Ftest%20file.txt"; // Adjusted expected URL

            // Act
            var result = _s3UtilityService.GetPublicUrlForObject(objectKey);

            // Assert
            Assert.That(result, Is.EqualTo(expectedUrl));
        }

    }
}
