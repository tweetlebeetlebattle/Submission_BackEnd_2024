using Backend.DTO.RequestResponseDTOs.Shared;
using Backend.Repositories;

namespace Backend.Services
{
    public class WeightlifterService
    {
        private readonly WeightlifterRepository _weightlifterRepository;
        private readonly S3BucketAWSService _s3BucketAWSService;

        public WeightlifterService(WeightlifterRepository weightlifterRepository, S3BucketAWSService s3BucketAWSService)
        {
            _weightlifterRepository = weightlifterRepository;
            _s3BucketAWSService = s3BucketAWSService;
        }

        public async Task<string> CreateNewBlogAsync(string text, IFormFile image, string userId)
        {
            string textUrl = null, pictureUrl = null;

            if (!string.IsNullOrEmpty(text))
            {
                textUrl = await _s3BucketAWSService.UploadTextAsync("bucketheadboris", $"blogs/{Guid.NewGuid()}.txt", text);
            }

            if (image != null)
            {
                using var stream = image.OpenReadStream();
                pictureUrl = await _s3BucketAWSService.UploadFileAsync("bucketheadboris", $"blogs/{Guid.NewGuid()}_{image.FileName}", stream);
            }

            await _weightlifterRepository.CreateNewBlog(userId, textUrl, pictureUrl);
            return "Blog created successfully!";
        }

        public async Task<string> CreateNewCommentAsync(string blogId, string text, IFormFile image, string userId)
        {
            string textUrl = null, pictureUrl = null;

            if (!string.IsNullOrEmpty(text))
            {
                textUrl = await _s3BucketAWSService.UploadTextAsync("bucketheadboris", $"comments/{Guid.NewGuid()}.txt", text);
            }

            if (image != null)
            {
                using var stream = image.OpenReadStream();
                pictureUrl = await _s3BucketAWSService.UploadFileAsync("bucketheadboris", $"comments/{Guid.NewGuid()}_{image.FileName}", stream);
            }

            await _weightlifterRepository.CreateNewCommentAsync(blogId, userId, textUrl, pictureUrl);
            return "Comment created successfully!";
        }

        public async Task<List<BlogWithComments>> FetchAllApprovedCommentsAsync()
        {
            return await _weightlifterRepository.FetchAllApprovedBlogDataAsync();
        }
        // FetchAllUserTrainingAndUniversalLogs
        // FetchPublicUserData
        // FetchAllTrainingUnits
        // CreateNewUniversalLog
        // AddToUniversalLog
        // CreateNewTraining
        // UpdateUniversalLogPublicity
        // UpdateTrainingLogPublicity
    }
}
