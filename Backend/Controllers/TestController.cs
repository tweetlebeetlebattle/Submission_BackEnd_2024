using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    public class TestController : ControllerBase
    {
        private readonly S3BucketAWSService _s3Service;
        private readonly IConfiguration _configuration;

        public TestController(S3BucketAWSService s3Service, IConfiguration configuration)
        {
            _s3Service = s3Service;
            _configuration = configuration;
        }

        [HttpGet("test")]
        public IActionResult Test()
        {
            return Ok(new { message = "ok" });
        }

        [HttpPost("upload")]
        public async Task<IActionResult> UploadFile(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("No file uploaded.");
            }

            var bucketName = _configuration["AWS:Region"]; // Access the configuration here
            var keyName = $"{System.Guid.NewGuid()}_{file.FileName}";

            using (var stream = file.OpenReadStream())
            {
                var url = await _s3Service.UploadFileAsync(bucketName, keyName, stream);
                return Ok(new { message = "File uploaded successfully.", url = url });
            }
        }

        [HttpPost("text")]
        public async Task<IActionResult> UploadText([FromBody] TextContent content)
        {
            if (string.IsNullOrEmpty(content.Text))
            {
                return BadRequest("No text content provided.");
            }

            var bucketName = "bucketheadboris"; // Specify your bucket name
            var keyName = $"{System.Guid.NewGuid()}_text.txt"; // Create a unique file name for the text

            var url = await _s3Service.UploadTextAsync(bucketName, keyName, content.Text);
            return Ok(new { message = "Text uploaded successfully.", url = url });
        }
    }

    public class TextContent
    {
        public string Text { get; set; }
    }
}
