using Backend.Data;
using Backend.Data.Models;
using Backend.DTO.RequestResponseDTOs.Diver;
using Backend.DTO.RequestResponseDTOs.Shared;
using Microsoft.EntityFrameworkCore;
using System;

namespace Backend.Repositories
{
    public class DiverRepository
    {
        private readonly ApplicationDbContext context;

        public DiverRepository(ApplicationDbContext _context)
        {
            context = _context;
        }

        public async Task<List<BlogWithComments>> FetchAllApprovedBlogDataAsync(int skip, int blogsPerPage)
        {
            var blogsWithComments = await context.SeaBlog
                .Where(blog => blog.ApprovedStatus)
                .Include(blog => blog.ApplicationUser)
                .Include(blog => blog.Media)
                .Include(blog => blog.SeaComments)
                    .ThenInclude(comment => comment.ApplicationUser)
                .Include(blog => blog.SeaComments)
                    .ThenInclude(comment => comment.Media)
                .OrderByDescending(blog => blog.Time) 
                .Skip(skip) 
                .Take(blogsPerPage) 
                .Select(blog => new BlogWithComments
                {
                    BlogId = blog.BlogId,
                    ApplicationUserName = blog.ApplicationUser.UserName,
                    MediaTextUrl = blog.Media.TextUrl,
                    MediaPictureUrl = blog.Media.PictureUrl,
                    Time = blog.Time,
                    Comments = blog.SeaComments
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

            var newBlog = new SeaBlog
            {
                ApplicationUserId = userId,
                MediaId = newMedia.MediaId,
                Time = DateTime.UtcNow,
                ApprovedStatus = false
            };

            await context.SeaBlog.AddAsync(newBlog);
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

            var newComment = new SeaComment
            {
                ParentBlogId = parentBlogId,
                ApplicationUserId = userId,
                MediaId = newMedia.MediaId, 
                Time = DateTime.UtcNow,
                ApprovedStatus = false 
            };

            await context.SeaComment.AddAsync(newComment);
            await context.SaveChangesAsync();
        }

        public async Task<BroadSeaDataHTML> FetchSeaDataForSpecifiedTimeFrameHTML(int timeFrameInDays)
        {
            var startDate = DateTime.UtcNow.AddDays(-timeFrameInDays);
            var endDate = DateTime.UtcNow;

            var readings = await context.DailyHTMLReading
                .Where(reading => reading.Date >= startDate && reading.Date <= endDate)
                .Include(reading => reading.Location) 
                .ToListAsync();

            var groupedReadings = readings
                .GroupBy(reading => reading.Location)
                .Select(group => new SpecificSeaDataHTML
                {
                    Location = group.Key.Name,
                    DailyWaveMax = group.Max(r => r.DailyWaveMax),
                    DailyWaveMin = group.Min(r => r.DailyWaveMin),
                    DailyWaveAvg = group.Average(r => r.DailyWaveAvg),
                    DailyTempMax = group.Max(r => r.DailyTempMax),
                    DailyTempMin = group.Min(r => r.DailyTempMin),
                    DailyTempAvg = group.Average(r => r.DailyTempAvg),
                    DateTime = group.Max(r => r.Date)
                })
                .ToList();

            var broadSeaData = new BroadSeaDataHTML
            {
                SeaDataList = groupedReadings
            };

            return broadSeaData;
        }
        public async Task<BroadSeaDataGif> FetchSeaDataForSpecifiedTimeFrameGif(int timeFrameInDays)
        {
            var startDate = DateTime.UtcNow.AddDays(-timeFrameInDays);
            var endDate = DateTime.UtcNow;

            var readings = await context.DailyGifReading
                .Where(reading => reading.Date >= startDate && reading.Date <= endDate)
                .Include(reading => reading.Location) 
                .ToListAsync();

            var groupedReadings = readings
                .GroupBy(reading => reading.Location)
                .Select(group => new SpecificSeaDataGif
                {
                    Location = group.Key.Name, 
                    DailyWaveMax = group.Max(r => r.DailyWaveMax),
                    DailyWaveMin = group.Min(r => r.DailyWaveMin),
                    DailyWaveAvg = group.Average(r => r.DailyWaveAvg),
                    DateTime = group.Max(r => r.Date)
                })
                .ToList();

            var broadSeaData = new BroadSeaDataGif
            {
                SeaDataList = groupedReadings
            };

            return broadSeaData;
        }
        public async Task<BroadSeaDataGSio> FetchSeaDataForSpecifiedTimeFrameGSio(int timeFrameInDays)
        {
            var startDate = DateTime.UtcNow.AddDays(-timeFrameInDays);
            var endDate = DateTime.UtcNow;

            var readings = await context.DailyGlassStormReading
                .Where(reading => reading.Date >= startDate && reading.Date <= endDate)
                .Include(reading => reading.Location) 
                .ToListAsync();

            var groupedReadings = readings
                .GroupBy(reading => reading.Location)
                .Select(group => new SpecificSeaDataGSio
                {
                    Location = group.Key.Name, 
                    DailyWaveMax = group.Max(r => r.DailyWaveMax),
                    DailyWaveMin = group.Min(r => r.DailyWaveMin),
                    DailyWaveAvg = group.Average(r => r.DailyWaveAvg),
                    DailyTempMax = group.Max(r => r.DailyTempMax),
                    DailyTempMin = group.Min(r => r.DailyTempMin),
                    DailyTempAvg = group.Average(r => r.DailyTempAvg),
                    DailyWindMax = group.Max(r => r.DailyWindMax),
                    DailyWindMin = group.Min(r => r.DailyWindMin),
                    DailyWindAvg = group.Average(r => r.DailyWindAvg),
                    DateTime = group.Max(r => r.Date) 
                })
                .ToList();

            var broadSeaData = new BroadSeaDataGSio
            {
                SeaDataList = groupedReadings
            };

            return broadSeaData;
        }

        public async Task<HistoricSeaDataByLocation> FetchSeaDataHistorcForSpecifiedLocation(string locationName)
        {
            var location = await context.Locations
                .FirstOrDefaultAsync(loc => loc.Name == locationName);

            var readings = await context.DailyGlassStormReading
                .Where(reading => reading.LocationId == location.Id)
                .OrderBy(reading => reading.Date)
                .ToListAsync();

            var historicData = new HistoricSeaDataByLocation
            {
                Location = location.Name,
                Readings = readings.Select(r => new HistoricSeaDataByLocationReadings
                {
                    WaveMin = r.DailyWaveMin ?? 0,
                    WaveMax = r.DailyWaveMax ?? 0,
                    WaveAvg = r.DailyWaveAvg ?? 0,
                    DateTime = r.Date
                }).ToList()
            };

            return historicData;
        }
        public async Task PostUserFeedback(
            string userId,
            int locationId,
            float? waveRead = null,
            int? waveUnitId = null,
            float? tempRead = null,
            int? tempUnitId = null,
            float? windSpeedIndex = null,
            int? windSpeedUnitId = null,
            string? pictureUrl = null,
            string? textUrl = null)
        {
            Media? media = null;

            if (!string.IsNullOrEmpty(pictureUrl) || !string.IsNullOrEmpty(textUrl))
            {
                media = new Media
                {
                    MediaId = Guid.NewGuid().ToString(),
                    PictureUrl = pictureUrl,
                    TextUrl = textUrl,
                    ApplicationUserId = userId
                };

                await context.Media.AddAsync(media);
                await context.SaveChangesAsync();
            }

            var feedback = new Feedback
            {
                ApplicationUserId = userId,
                LocationId = locationId,
                WaveRead = waveRead,
                WaveUnitId = waveUnitId,
                TempRead = tempRead,
                TempUnitId = tempUnitId,
                WindSpeedIndex = windSpeedIndex,
                WindSpeedUnitId = windSpeedUnitId,
                Time = DateTime.UtcNow,
                MediaId = media?.MediaId
            };

            await context.Feedback.AddAsync(feedback);
            await context.SaveChangesAsync();
        }
        public async Task<int> FetchNumberOfBlogs()
        {
            int result = await context.SeaBlog
                .CountAsync(blog => blog.ApprovedStatus); 
            return result;
        }
        public async Task<int> FetchNumberOfApprovedUserDiverBlogs(string query)
        {
            int result = await context.SeaBlog
                .Where(sb => sb.ApplicationUser.UserName == query)
                .CountAsync(blog => blog.ApprovedStatus);
            return result;
        }
        public async Task<List<BlogWithComments>> FetchApprovedUserDiverBlogs(int skip, int blogsPerPage, string username)
        {
            var blogsWithComments = await context.SeaBlog
                .Where(blog => blog.ApprovedStatus)
                .Where(blog => blog.ApplicationUser.UserName == username)
                .Include(blog => blog.ApplicationUser)
                .Include(blog => blog.Media)
                .Include(blog => blog.SeaComments)
                    .ThenInclude(comment => comment.ApplicationUser)
                .Include(blog => blog.SeaComments)
                    .ThenInclude(comment => comment.Media)
                .OrderByDescending(blog => blog.Time)
                .Skip(skip)
                .Take(blogsPerPage)
                .Select(blog => new BlogWithComments
                {
                    BlogId = blog.BlogId,
                    ApplicationUserName = blog.ApplicationUser.UserName,
                    MediaTextUrl = blog.Media.TextUrl,
                    MediaPictureUrl = blog.Media.PictureUrl,
                    Time = blog.Time,
                    Comments = blog.SeaComments
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
    }
}
