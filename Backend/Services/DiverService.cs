using Backend.DTO;
using Backend.DTO.RequestResponseDTOs.Diver;
using Backend.DTO.RequestResponseDTOs.Shared;
using Backend.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.SqlServer.Query.Internal;
using Microsoft.Extensions.Configuration.UserSecrets;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Backend.Services
{
    public class DiverService : IDiverService
    {
        private readonly IDiverRepository _diverRepository;
        private readonly IS3BucketAWSService _s3BucketAWSService;
        private readonly IUtilityService _utilityService;
        public DiverService(IDiverRepository diverRepository, IS3BucketAWSService s3BucketAWSService, IUtilityService utilityService)
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

        public async Task<List<BlogWithComments>> FetchAllApprovedCommentsAsync(int skip, int blogsPerPage)
        {
            return await _diverRepository.FetchAllApprovedBlogDataAsync(skip, blogsPerPage);
        }
        public async Task<int> FetchNumberOfBlogs()
        {
            return await _diverRepository.FetchNumberOfBlogs();
        }
        public async Task<int> FetchNumberOfApprovedUserDiverBlogs(string query)
        {
            return await _diverRepository.FetchNumberOfApprovedUserDiverBlogs(query);
        }
        public async Task<List<BlogWithComments>> FetchApprovedUserDiverBlogs(int skip, int pageNumber, string username)
        {
            return await _diverRepository.FetchApprovedUserDiverBlogs(skip, pageNumber, username);
        }
        public async Task PostUserFeedback(string userId, FeedbackDTO feedbackDto)
        {
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

            string textUrl = null;
            if (!string.IsNullOrEmpty(finalText))
            {
                textUrl = await _s3BucketAWSService.UploadTextAsync(
                    "bucketheadboris",
                    $"feedback/{Guid.NewGuid()}.txt",
                    finalText
                );
            }

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

            int locationId = await _utilityService.GetLocationIdByNameAsync(feedbackDto.LocationName);
            int? waveUnitId = null;
            if (!string.IsNullOrEmpty(feedbackDto.WaveUnitId))
            {
                waveUnitId = await _utilityService.GetUnitIdByUnitNameAsync(feedbackDto.WaveUnitId);
            }

            int? tempUnitId = null;
            if (!string.IsNullOrEmpty(feedbackDto.TempUnitId))
            {
                tempUnitId = await _utilityService.GetUnitIdByUnitNameAsync(feedbackDto.TempUnitId);
            }

            int? windUnitId = null;
            if (!string.IsNullOrEmpty(feedbackDto.WindSpeedUnitId))
            {
                windUnitId = await _utilityService.GetUnitIdByUnitNameAsync(feedbackDto.WindSpeedUnitId);
            }

            await _diverRepository.PostUserFeedback(
                userId,
                locationId,
                feedbackDto.WaveRead,
                waveUnitId,
                feedbackDto.TempRead,
                tempUnitId,
                feedbackDto.windSpeedRead,
                windUnitId,
                pictureUrl,
                textUrl
            );
        }
        public async Task<HistoricSeaDataByLocation> FetchHistoricSeaDataByLocationHTML(string location)
        {
            return await _diverRepository.FetchHistoricSeaDataByLocationHTML(location);
        }

        public async Task<HistoricSeaDataByLocation> FetchHistoricSeaDataByLocationGif(string location)
        {
            return await _diverRepository.FetchHistoricSeaDataByLocationGif(location);
        }

        public async Task<HistoricSeaDataByLocation> FetchHistoricSeaDataByLocationStorm(string location)
        {
            return await _diverRepository.FetchHistoricSeaDataByLocationStorm(location);
        }
        public async Task<SeaDataIndex> FetchIndexSeaData(int period)
        {
            var dataForPeriod = await _diverRepository.FetchIndexSeaDataByPeriod(period);

            var dataIndices = new List<DataIndex>();

            foreach (var locationData in dataForPeriod.HistoricSeaDataByLocations)
            {
                var index = await ReturnDiveIndexOnData(locationData.Readings);
                dataIndices.Add(new DataIndex
                {
                    Location = locationData.Location,
                    Index = index
                });
            }

            return new SeaDataIndex
            {
                DataIndices = dataIndices
            };
        }

        public async Task<double> ReturnDiveIndexOnData(List<HistoricSeaDataByLocationReadings> data)
        {
            double index = 0;

            var averageWaveHeight = data
                .Where(r => r.WaveData != null && r.WaveData.WaveAvg.HasValue)
                .Average(r => r.WaveData.WaveAvg.Value);

            if (averageWaveHeight < 0.2)
            {
                index = 0;
            }
            else if (averageWaveHeight < 0.4)
            {
                index = 1;
            }
            else if (averageWaveHeight < 0.6)
            {
                index = 2;
            }
            else
            {
                index = 3;
            }

            if (index < 3)
            {
                var maxWaveHeight = data
                    .Where(r => r.WaveData != null && r.WaveData.WaveMax.HasValue)
                    .Max(r => r.WaveData.WaveMax.Value);

                if (maxWaveHeight > (index + 1) * 0.2)
                {
                    index += 0.5;
                }
            }

            return index;
        }
    }
}
