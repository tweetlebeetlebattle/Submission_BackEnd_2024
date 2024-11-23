using Backend.Data;
using Backend.Data.Models;
using Backend.DTO;
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

        // FetchAllApprovedBlogData
        public async Task<List<SeaBlogWithComments>> FetchAllApprovedBlogDataAsync()
        {
            var blogsWithComments = await context.SeaBlog
                .Where(blog => blog.ApprovedStatus) // Only approved blogs
                .Include(blog => blog.Media) // Include Media details
                .Select(blog => new SeaBlogWithComments
                {
                    ApplicationUserName = blog.ApplicationUser.UserName,
                    MediaTextUrl = blog.Media.TextUrl,
                    MediaPictureUrl = blog.Media.PictureUrl,
                    Time = blog.Time,
                    Comments = context.SeaComment
                        .Where(comment => comment.ParentBlogId == blog.BlogId)
                        .Where(comment => comment.ApprovedStatus) // Only approved comments
                        .Select(comment => new SeaCommentDto
                        {
                            ApplicationUserName = comment.ApplicationUser.UserName,
                            MediaTextUrl = comment.Media.TextUrl,
                            MediaPictureUrl = comment.Media.PictureUrl,
                            Time = comment.Time,
                        }).ToList()
                })
                .ToListAsync();

            return blogsWithComments;
        }

        // InsertNewBlog
        public async Task CreateNewBlog(string userId, string textUrl, string pictureUrl)
        {
            var newMedia = new Media
            {
                MediaId = Guid.NewGuid().ToString(),
                TextUrl = textUrl,
                PictureUrl = pictureUrl,
                ApplicationUserId = userId // Associate with the user
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

        // InsertNewComment
        public async Task CreateNewCommentAsync(int parentBlogId, string userId, string textUrl, string pictureUrl)
        {
            // Create a new Media entry
            var newMedia = new Media
            {
                MediaId = Guid.NewGuid().ToString(), // Generate unique UUID
                TextUrl = textUrl,
                PictureUrl = pictureUrl,
                ApplicationUserId = userId // Associate with the user
            };

            // Add the media to the context
            await context.Media.AddAsync(newMedia);
            await context.SaveChangesAsync(); // Save to get MediaId

            // Create a new SeaComment entry
            var newComment = new SeaComment
            {
                ParentBlogId = parentBlogId,
                ApplicationUserId = userId,
                MediaId = newMedia.MediaId, // Link the MediaId
                Time = DateTime.UtcNow,
                ApprovedStatus = false // Default to not approved
            };

            // Add the comment to the context
            await context.SeaComment.AddAsync(newComment);
            await context.SaveChangesAsync();
        }

        public async Task<BroadSeaDataHTML> FetchSeaDataForSpecifiedTimeFrameHTML(int timeFrameInDays)
        {
            var startDate = DateTime.UtcNow.AddDays(-timeFrameInDays);
            var endDate = DateTime.UtcNow;

            var readings = await context.DailyHTMLReading
                .Where(reading => reading.Date >= startDate && reading.Date <= endDate)
                .Include(reading => reading.Location) // Include Location details
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
                .Include(reading => reading.Location) // Include Location details
                .ToListAsync();

            var groupedReadings = readings
                .GroupBy(reading => reading.Location)
                .Select(group => new SpecificSeaDataGif
                {
                    Location = group.Key.Name, // Assuming Location has a Name property
                    DailyWaveMax = group.Max(r => r.DailyWaveMax),
                    DailyWaveMin = group.Min(r => r.DailyWaveMin),
                    DailyWaveAvg = group.Average(r => r.DailyWaveAvg),
                    DateTime = group.Max(r => r.Date) // Latest date in the group
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
                .Include(reading => reading.Location) // Include Location details
                .ToListAsync();

            var groupedReadings = readings
                .GroupBy(reading => reading.Location)
                .Select(group => new SpecificSeaDataGSio
                {
                    Location = group.Key.Name, // Assuming Location has a Name property
                    DailyWaveMax = group.Max(r => r.DailyWaveMax),
                    DailyWaveMin = group.Min(r => r.DailyWaveMin),
                    DailyWaveAvg = group.Average(r => r.DailyWaveAvg),
                    DailyTempMax = group.Max(r => r.DailyTempMax),
                    DailyTempMin = group.Min(r => r.DailyTempMin),
                    DailyTempAvg = group.Average(r => r.DailyTempAvg),
                    DailyWindMax = group.Max(r => r.DailyWindMax),
                    DailyWindMin = group.Min(r => r.DailyWindMin),
                    DailyWindAvg = group.Average(r => r.DailyWindAvg),
                    DateTime = group.Max(r => r.Date) // Latest date in the group
                })
                .ToList();

            var broadSeaData = new BroadSeaDataGSio
            {
                SeaDataList = groupedReadings
            };

            return broadSeaData;
        }

        // FetchSeaDataHistorcForSpecifiedLocation
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

    }
}
