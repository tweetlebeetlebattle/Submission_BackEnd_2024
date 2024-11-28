using Backend.DTO;
using Backend.DTO.RequestResponseDTOs.Diver;
using Backend.DTO.RequestResponseDTOs.Shared;
using Backend.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration.UserSecrets;
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
        private readonly UtilityService _utilityService;
        public DiverService(DiverRepository diverRepository, S3BucketAWSService s3BucketAWSService, UtilityService utilityService)
        {
            _diverRepository = diverRepository;
            _s3BucketAWSService = s3BucketAWSService;
            _utilityService = utilityService;
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

            await _diverRepository.CreateNewBlog(userId, textUrl, pictureUrl);
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

            await _diverRepository.CreateNewCommentAsync(blogId, userId, textUrl, pictureUrl);
            return "Comment created successfully!";
        }

        public async Task<List<BlogWithComments>> FetchAllApprovedCommentsAsync()
        {
            return await _diverRepository.FetchAllApprovedBlogDataAsync();
        }

        public async Task PostUserFeedback(string userId, FeedbackDTO feedbackDto)
        {
            // Prepare text and coordinates
            string finalText = feedbackDto.Text;
            if (!string.IsNullOrEmpty(feedbackDto.Coordinates))
            {
                if (string.IsNullOrEmpty(finalText))
                {
                    finalText = feedbackDto.Coordinates;
                }
                else
                {
                    finalText += $"\nCoordinates: {feedbackDto.Coordinates}";
                }
            }

            // Upload text to S3 if it exists
            string textUrl = null;
            if (!string.IsNullOrEmpty(finalText))
            {
                textUrl = await _s3BucketAWSService.UploadTextAsync(
                    "bucketheadboris",
                    $"feedback/{Guid.NewGuid()}.txt",
                    finalText
                );
            }

            // Upload image to S3 if it exists
            string pictureUrl = null;
            if (feedbackDto.Image != null)
            {
                using var stream = feedbackDto.Image.OpenReadStream();
                pictureUrl = await _s3BucketAWSService.UploadFileAsync(
                    "bucketheadboris",
                    $"feedback/{Guid.NewGuid()}_{feedbackDto.Image.FileName}",
                    stream
                );
            }

            // Get the Location ID using UtilityService
            int locationId = await _utilityService.GetLocationIdByNameAsync(feedbackDto.LocationName);

            // Pass all data to the repository method
            await _diverRepository.PostUserFeedback(
                userId,
                locationId,
                feedbackDto.WaveRead,
                feedbackDto.WaveUnitId,
                feedbackDto.TempRead,
                feedbackDto.TempUnitId,
                feedbackDto.WindSpeedIndex,
                feedbackDto.WindSpeedUnitId,
                pictureUrl,
                textUrl
            );
        }
    }
}
