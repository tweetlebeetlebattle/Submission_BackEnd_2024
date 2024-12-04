using Backend.Data.Models;
using Backend.DTO.ModifiedDataDTO.Weightlifter;
using Backend.DTO.RequestResponseDTOs.Shared;
using Backend.DTO.RequestResponseDTOs.Weightlifter;
using Backend.Repositories;

namespace Backend.Services
{
    public class WeightlifterService
    {
        private readonly WeightlifterRepository _weightlifterRepository;
        private readonly S3BucketAWSService _s3BucketAWSService;
        private readonly UtilityService _utilityService;

        public WeightlifterService(WeightlifterRepository weightlifterRepository, S3BucketAWSService s3BucketAWSService, UtilityService utilityService)
        {
            _weightlifterRepository = weightlifterRepository;
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

        public async Task<List<BlogWithComments>> FetchAllApprovedCommentsAsync(int skip, int blogsPerPage)
        {
            return await _weightlifterRepository.FetchAllApprovedBlogDataAsync(skip, blogsPerPage);
        }
        public async Task<int> FetchNumberOfBlogs()
        {
            return await _weightlifterRepository.FetchNumberOfBlogs();
        }
        public async Task<AllUniversalLogsAndTraining> FetchAllUserTrainingAndUniversalLogs(string userId)
        {
            return await _weightlifterRepository.FetchAllUserTrainingAndUniversalReading(userId);
        }
        public async Task<AllUniversalLogsAndTraining> FetchPublicUserData(string username)
        {
            return await _weightlifterRepository.FetchPublicUserData(username);
        }
        public async Task<List<string>> FetchAllTrainingUnits()
        {
            return await _weightlifterRepository.FetchAllTrainingUnits();
        }
        public async Task<List<string>> FetchAllTrainingTitles()
        {
            return await _weightlifterRepository.FetchAllTrainingTitles();
        }
        public async Task CreateNewUniversalReading(string userId, CreateUniversalReading createUniversalReading)
        {
            int trainingUnitId = await this._utilityService.FetchTrainingUnitIdByName(createUniversalReading.UnitName);
            DateTime dateTime =  this._utilityService.ConvertStringToDateTime(createUniversalReading.Date);
            await _weightlifterRepository.CreateNewUniversalReading( userId, createUniversalReading.Name, createUniversalReading.Measurment, trainingUnitId,  dateTime);
        }
        public async Task CreateNewTraining(string userId, CreateNewTraining createNewTraining)
        {
            int trainingUnitId = await _utilityService.FetchTrainingUnitIdByName(createNewTraining.UnitName);
            DateTime dateTime = _utilityService.ConvertStringToDateTime(createNewTraining.Date);
            CreateNewTrainingModified createNewTrainingModified = new CreateNewTrainingModified
            {
                Name = createNewTraining.Name,
                Date = dateTime,
                TargetWeight = createNewTraining.TargetWeight,
                UnitId = await _utilityService.FetchTrainingUnitIdByName(createNewTraining.UnitName),
                TargetSets = createNewTraining.TargetSets,
                TargetReps = createNewTraining.TargetReps,
                Sets = new List<SetModified>()
            };

            foreach (var set in createNewTraining.Sets)
            {
                string? mediaId = null;

                if (set.Image != null || !string.IsNullOrWhiteSpace(set.Text))
                {
                    mediaId = await _utilityService.CreateNewMediaReturnId(userId, set.Image, set.Text);
                }
                createNewTrainingModified.Sets.Add(new SetModified
                {
                    Reps = set.Reps,
                    MediaId = mediaId
                });
            }

            await _weightlifterRepository.CreateNewTraining(userId, createNewTrainingModified);
        }
        public async Task UpdateUniversalReadingPublicity(ChangeUniversalReadingTrainingPublicity changeForm)
        {
            await _weightlifterRepository.UpdateUniversalReadingPublicity(changeForm.Name, changeForm.IsPublic);
        }
        public async Task UpdateTrainingLogPublicity(ChangeUniversalReadingTrainingPublicity changeForm)
        {
            await _weightlifterRepository.UpdateTrainingLogPublicity(changeForm.Name, changeForm.IsPublic);
        }
    }
}
