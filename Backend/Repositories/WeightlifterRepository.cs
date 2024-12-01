using Backend.Data.Models;
using Backend.Data;
using Backend.DTO;
using Microsoft.EntityFrameworkCore;
using Backend.DTO.RequestResponseDTOs.Shared;
using System.Diagnostics.Metrics;
using Backend.DTO.ModifiedDataDTO.Weightlifter;
using Backend.DTO.RequestResponseDTOs.Weightlifter;

namespace Backend.Repositories
{
    public class WeightlifterRepository
    {
        private readonly ApplicationDbContext context;

        public WeightlifterRepository(ApplicationDbContext _context)
        {
            context = _context;
        }

        public async Task<List<BlogWithComments>> FetchAllApprovedBlogDataAsync()
        {
            var blogsWithComments = await context.TrainingBlog
                .Where(blog => blog.ApprovedStatus)
                .Include(blog => blog.ApplicationUser)
                .Include(blog => blog.Media)
                .Include(blog => blog.TrainingComments)
                    .ThenInclude(comment => comment.ApplicationUser)
                .Include(blog => blog.TrainingComments)
                    .ThenInclude(comment => comment.Media)
                .Select(blog => new BlogWithComments
                {
                    BlogId = blog.BlogId,
                    ApplicationUserName = blog.ApplicationUser.UserName,
                    MediaTextUrl = blog.Media.TextUrl,
                    MediaPictureUrl = blog.Media.PictureUrl,
                    Time = blog.Time,
                    Comments = blog.TrainingComments
                        .Where(comment => comment.ApprovedStatus)
                        .Select(comment => new CommentDto
                        {
                            CommentId = comment.CommentId,
                            ApplicationUserName = comment.ApplicationUser.UserName,
                            MediaTextUrl = comment.Media.TextUrl,
                            MediaPictureUrl = comment.Media.PictureUrl,
                            Time = comment.Time,
                        }).ToList()
                })
                .ToListAsync();

            return blogsWithComments;
        }

        public async Task CreateNewBlog(string userId, string textUrl, string pictureUrl)
        {
            var newMedia = new Media
            {
                MediaId = Guid.NewGuid().ToString(),
                TextUrl = textUrl,
                PictureUrl = pictureUrl,
                ApplicationUserId = userId
            };

            await context.Media.AddAsync(newMedia);
            await context.SaveChangesAsync();

            var newBlog = new TrainingBlog
            {
                ApplicationUserId = userId,
                MediaId = newMedia.MediaId,
                Time = DateTime.UtcNow,
                ApprovedStatus = false
            };

            await context.TrainingBlog.AddAsync(newBlog);
            await context.SaveChangesAsync();
        }

        public async Task CreateNewCommentAsync(string parentBlogId, string userId, string textUrl, string pictureUrl)
        {
            var newMedia = new Media
            {
                MediaId = Guid.NewGuid().ToString(),
                TextUrl = textUrl,
                PictureUrl = pictureUrl,
                ApplicationUserId = userId
            };

            await context.Media.AddAsync(newMedia);
            await context.SaveChangesAsync();

            var newComment = new TrainingComment
            {
                ParentBlogId = parentBlogId,
                ApplicationUserId = userId,
                MediaId = newMedia.MediaId,
                Time = DateTime.UtcNow,
                ApprovedStatus = false
            };

            await context.TrainingComment.AddAsync(newComment);
            await context.SaveChangesAsync();
        }


        public async Task<int> GetTrainingUnitIdByUnitNameAsync(string unitName)
        {
            if (string.IsNullOrEmpty(unitName))
                throw new ArgumentException("Unit name cannot be null or empty.", nameof(unitName));

            var unit = await context.Set<TrainingUnits>()
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.UnitName == unitName);

            if (unit == null)
            {
                var newUnit = new TrainingUnits
                {
                    UnitName = unitName
                };

                await context.Set<TrainingUnits>().AddAsync(newUnit);
                await context.SaveChangesAsync();

                return newUnit.Id;
            }

            return unit.Id;
        }
        // FetchAllUserTrainingAndUniversalReading
        public async Task<AllUniversalLogsAndTraining> FetchAllUserTrainingAndUniversalReading(string userId)
        {
            var result = new AllUniversalLogsAndTraining
            {
                PackagedReadings = new List<PackagedUniversalReading>()
            };

            var universalReadings = await context.UniversalReading
                .Where(ur => ur.ApplicationUserId == userId)
                .ToListAsync();

            var trainingLogs = await context.TrainingLog
                .Where(tl => tl.ApplicationUserId == userId)
                .Include(tl => tl.Unit)
                .ToListAsync();

            var trainingSetsLogs = await context.TrainingSetsLog
                .Where(tsl => trainingLogs.Select(tl => tl.TrainingLogId).Contains(tsl.TrainingLogId))
                .ToListAsync();

            var groupedUniversalReadings = universalReadings
                .GroupBy(ur => ur.Title)
                .Select(group => new PackagedUniversalReading
                {
                    Name = group.Key,
                    IsPublic = group.First().IsPublic,
                    IsTraining = false,
                    UniversalReadingsTrainings = group.Select(ur => new _UniversalReading
                    {
                        Measurment = ur.Reading.ToString(),
                        UnitName = ur.Unit.UnitName,
                        Date = ur.Time.ToString("yyyy-MM-dd"),
                        IsSucessTraining = null 
                    }).ToList()
                }).ToList();

            var groupedTrainingLogs = trainingLogs
                .GroupBy(tl => tl.ExerciseName)
                .Select(group => new PackagedUniversalReading
                {
                    Name = group.Key,
                    IsPublic = group.First().IsPublic,
                    IsTraining = true,
                    UniversalReadingsTrainings = group.Select(tl =>
                    {
                        var relatedSetsLogs = trainingSetsLogs.Where(tsl => tsl.TrainingLogId == tl.TrainingLogId).ToList();
                        var isSuccess = relatedSetsLogs.Count == tl.TargetSetCount &&
                                        relatedSetsLogs.All(tsl => tsl.DoneRepCount == tl.TargetRepsCount);

                        return new _UniversalReading
                        {
                            Measurment = tl.TargetWorkingWeight.ToString(),
                            UnitName = tl.Unit?.UnitName,
                            Date = tl.Time.ToString("yyyy-MM-dd"),
                            IsSucessTraining = isSuccess
                        };
                    }).ToList()
                }).ToList();

            result.PackagedReadings.AddRange(groupedUniversalReadings);
            result.PackagedReadings.AddRange(groupedTrainingLogs);

            return result;
        }

        // FetchPublicUserData
        public async Task<AllUniversalLogsAndTraining> FetchPublicUserData(string username)
        {
            var result = new AllUniversalLogsAndTraining
            {
                PackagedReadings = new List<PackagedUniversalReading>()
            };

            var user = await context.Users.FirstOrDefaultAsync(u => u.UserName == username);
            if (user == null) return result;

            var userId = user.Id;

            var universalReadings = await context.UniversalReading
                .Where(ur => ur.ApplicationUserId == userId && ur.IsPublic)
                .ToListAsync();

            var trainingLogs = await context.TrainingLog
                .Where(tl => tl.ApplicationUserId == userId && tl.IsPublic)
                .Include(tl => tl.Unit)
                .ToListAsync();

            var trainingSetsLogs = await context.TrainingSetsLog
                .Where(tsl => trainingLogs.Select(tl => tl.TrainingLogId).Contains(tsl.TrainingLogId))
                .ToListAsync();

            var groupedUniversalReadings = universalReadings
                .GroupBy(ur => ur.Title)
                .Select(group => new PackagedUniversalReading
                {
                    Name = group.Key,
                    IsPublic = group.First().IsPublic,
                    IsTraining = false,
                    UniversalReadingsTrainings = group.Select(ur => new _UniversalReading
                    {
                        Measurment = ur.Reading.ToString(),
                        UnitName = ur.Unit.UnitName,
                        Date = ur.Time.ToString("yyyy-MM-dd"),
                        IsSucessTraining = null 
                    }).ToList()
                }).ToList();

            var groupedTrainingLogs = trainingLogs
                .GroupBy(tl => tl.ExerciseName)
                .Select(group => new PackagedUniversalReading
                {
                    Name = group.Key,
                    IsPublic = group.First().IsPublic,
                    IsTraining = true,
                    UniversalReadingsTrainings = group.Select(tl =>
                    {
                        var relatedSetsLogs = trainingSetsLogs.Where(tsl => tsl.TrainingLogId == tl.TrainingLogId).ToList();
                        var isSuccess = relatedSetsLogs.Count == tl.TargetSetCount &&
                                        relatedSetsLogs.All(tsl => tsl.DoneRepCount == tl.TargetRepsCount);

                        return new _UniversalReading
                        {
                            Measurment = tl.TargetWorkingWeight.ToString(),
                            UnitName = tl.Unit?.UnitName,
                            Date = tl.Time.ToString("yyyy-MM-dd"),
                            IsSucessTraining = isSuccess
                        };
                    }).ToList()
                }).ToList();

            result.PackagedReadings.AddRange(groupedUniversalReadings);
            result.PackagedReadings.AddRange(groupedTrainingLogs);

            return result;
        }


        // FetchAllTrainingUnits
        public async Task<List<string>> FetchAllTrainingUnits()
        {
            var trainingUnits = await context.TrainingUnits
                .AsNoTracking() 
                .Select(unit => unit.UnitName)
                .ToListAsync();

            return trainingUnits;
        }


        // CreateNewUniversalReading
        public async Task CreateNewUniversalReading(string userId, string name, double measurement, int trainingUnitId, DateTime dateTime)
        {
            var existingReading = await context.UniversalReading
                .Where(ur => ur.ApplicationUserId == userId && ur.Title == name)
                .FirstOrDefaultAsync();

            bool isPublic = existingReading != null ? existingReading.IsPublic : false;

            var newUniversalReading = new UniversalReading
            {
                Title = name,
                Reading = (float)measurement,
                UnitId = trainingUnitId,
                ApplicationUserId = userId,
                Time = dateTime,
                IsPublic = isPublic
            };

            await context.UniversalReading.AddAsync(newUniversalReading);
            await context.SaveChangesAsync();
        }


        // CreateNewTraining
        public async Task CreateNewTraining(string userId, CreateNewTrainingModified modifiedDTO)
        {
            var existingTraining = await context.TrainingLog
                .Where(tl => tl.ApplicationUserId == userId && tl.ExerciseName == modifiedDTO.Name)
                .FirstOrDefaultAsync();

            bool isPublic = existingTraining != null ? existingTraining.IsPublic : false;
            TrainingUnits unit = await context.TrainingUnits.Where(u => u.Id == modifiedDTO.UnitId).FirstOrDefaultAsync();
            var trainingLog = new TrainingLog
            {
                ExerciseName = modifiedDTO.Name,
                TargetWorkingWeight = (float)modifiedDTO.TargetWeight,
                Unit = unit,
                TargetSetCount = modifiedDTO.TargetSets,
                TargetRepsCount = modifiedDTO.TargetReps,
                ApplicationUserId = userId,
                Time = modifiedDTO.Date,
                IsPublic = isPublic
            };

            await context.TrainingLog.AddAsync(trainingLog);
            await context.SaveChangesAsync();

            List<TrainingSetsLog> trainingSets = modifiedDTO.Sets.Select((set, index) => new TrainingSetsLog
            {
                TrainingLogId = trainingLog.TrainingLogId,
                DoneSetCount = index + 1,
                DoneRepCount = set.Reps,
                MediaId = set.MediaId,
                Time = DateTime.UtcNow
            }).ToList();

            await context.TrainingSetsLog.AddRangeAsync(trainingSets);
            await context.SaveChangesAsync();
        }



        // UpdateUniversalReadingPublicity
        public async Task UpdateUniversalReadingPublicity(string title, bool isPublic)
        {
            var universalReadings = await context.UniversalReading
                .Where(ur => ur.Title == title)
                .ToListAsync();

            if (!universalReadings.Any())
            {
                throw new ArgumentException($"No Universal Readings found with the title: {title}");
            }

            foreach (var reading in universalReadings)
            {
                reading.IsPublic = isPublic;
            }

            await context.SaveChangesAsync();
        }

        // UpdateTrainingLogPublicity
        public async Task UpdateTrainingLogPublicity(string title, bool isPublic)
        {
            if (string.IsNullOrEmpty(title))
            {
                throw new ArgumentException("Title cannot be null or empty.", nameof(title));
            }

            var trainingLogs = await context.TrainingLog
                .Where(tl => tl.ExerciseName == title)
                .ToListAsync();

            if (!trainingLogs.Any())
            {
                throw new ArgumentException($"No Training Logs found with the title: {title}");
            }

            foreach (var log in trainingLogs)
            {
                log.IsPublic = isPublic;
            }

            await context.SaveChangesAsync();
        }
    }
}
