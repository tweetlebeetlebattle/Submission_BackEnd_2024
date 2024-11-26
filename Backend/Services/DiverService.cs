using Backend.DTO;
using Backend.Repositories;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Backend.Services
{
    public class DiverService
    {
        private readonly DiverRepository _diverRepository;
        private readonly S3BucketAWSService _s3BucketAWSService;

        public DiverService(DiverRepository diverRepository, S3BucketAWSService s3BucketAWSService)
        {
            _diverRepository = diverRepository;
            _s3BucketAWSService = s3BucketAWSService;
        }

        public async Task<string> CreateNewBlogAsync(string text, IFormFile image, string userId, string dateTimestamp)
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

            await _diverRepository.CreateNewBlog(userId, textUrl, pictureUrl);
            return "Blog created successfully!";
        }

        public async Task<string> CreateNewCommentAsync(string blogId, string text, IFormFile image, string userId, string dateTimestamp)
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

            await _diverRepository.CreateNewCommentAsync(blogId, userId, textUrl, pictureUrl);
            return "Comment created successfully!";
        }

        public async Task<List<SeaBlogWithComments>> FetchAllApprovedCommentsAsync()
        {
            return await _diverRepository.FetchAllApprovedBlogDataAsync();
        }
    }
}
