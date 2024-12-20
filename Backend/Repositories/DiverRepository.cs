﻿using Backend.Data;
using Backend.Data.Models;
using Backend.DTO.RequestResponseDTOs.Diver;
using Backend.DTO.RequestResponseDTOs.Shared;
using Microsoft.EntityFrameworkCore;
using System;

namespace Backend.Repositories
{
    public class DiverRepository : IDiverRepository
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
                .Where(blog =>
                    blog.ApprovedStatus &&
                    (blog.ApplicationUser.UserName == username || 
                     blog.SeaComments.Any(comment => comment.ApplicationUser.UserName == username)) 
                )
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
        public async Task<HistoricSeaDataByLocation> FetchHistoricSeaDataByLocationHTML(string location)
        {
            if (string.IsNullOrWhiteSpace(location))
                throw new ArgumentException("Location cannot be null or empty.", nameof(location));

            var locationEntity = await context.Locations
                .FirstOrDefaultAsync(loc => loc.Name == location);

            if (locationEntity == null)
                throw new KeyNotFoundException($"No location found with the name '{location}'.");

            var readings = await context.DailyHTMLReading
                .Where(reading => reading.LocationId == locationEntity.Id)
                .OrderBy(reading => reading.Date)
                .Select(reading => new HistoricSeaDataByLocationReadings
                {
                    WaveData = reading.WaveUnit != null ? new WaveData
                    {
                        WaveMin = reading.DailyWaveMin,
                        WaveMax = reading.DailyWaveMax,
                        WaveAvg = reading.DailyWaveAvg,
                        WaveUnit = reading.WaveUnit.UnitName
                    } : null,
                    TempData = reading.TempUnit != null ? new TempData
                    {
                        TempMin = reading.DailyTempMin,
                        TempMax = reading.DailyTempMax,
                        TempAvg = reading.DailyTempAvg,
                        TempUnit = reading.TempUnit.UnitName
                    } : null,
                    DateTime = reading.Date
                })
                .ToListAsync();

            return new HistoricSeaDataByLocation
            {
                Location = locationEntity.Name,
                Readings = readings
            };
        }

        public async Task<HistoricSeaDataByLocation> FetchHistoricSeaDataByLocationGif(string location)
        {
            if (string.IsNullOrWhiteSpace(location))
                throw new ArgumentException("Location cannot be null or empty.", nameof(location));

            var locationEntity = await context.Locations
                .FirstOrDefaultAsync(loc => loc.Name == location);

            if (locationEntity == null)
                throw new KeyNotFoundException($"No location found with the name '{location}'.");

            var readings = await context.DailyGifReading
                .Where(reading => reading.LocationId == locationEntity.Id)
                .OrderBy(reading => reading.Date)
                .Select(reading => new HistoricSeaDataByLocationReadings
                {
                    WaveData = reading.WaveUnit != null ? new WaveData
                    {
                        WaveMin = reading.DailyWaveMin,
                        WaveMax = reading.DailyWaveMax,
                        WaveAvg = reading.DailyWaveAvg,
                        WaveUnit = reading.WaveUnit.UnitName
                    } : null,
                    DateTime = reading.Date
                })
                .ToListAsync();

            return new HistoricSeaDataByLocation
            {
                Location = locationEntity.Name,
                Readings = readings
            };
        }

        public async Task<HistoricSeaDataByLocation> FetchHistoricSeaDataByLocationStorm(string location)
        {
            if (string.IsNullOrWhiteSpace(location))
                throw new ArgumentException("Location cannot be null or empty.", nameof(location));

            var locationEntity = await context.Locations
                .FirstOrDefaultAsync(loc => loc.Name == location);

            if (locationEntity == null)
                throw new KeyNotFoundException($"No location found with the name '{location}'.");

            var readings = await context.DailyGlassStormReading
                .Where(reading => reading.LocationId == locationEntity.Id)
                .OrderBy(reading => reading.Date)
                .Select(reading => new HistoricSeaDataByLocationReadings
                {
                    WaveData = reading.WaveUnit != null ? new WaveData
                    {
                        WaveMin = reading.DailyWaveMin,
                        WaveMax = reading.DailyWaveMax,
                        WaveAvg = reading.DailyWaveAvg,
                        WaveUnit = reading.WaveUnit.UnitName
                    } : null,
                    TempData = reading.TempUnit != null ? new TempData
                    {
                        TempMin = reading.DailyTempMin,
                        TempMax = reading.DailyTempMax,
                        TempAvg = reading.DailyTempAvg,
                        TempUnit = reading.TempUnit.UnitName
                    } : null,
                    WindData = reading.WindUnit != null ? new WindData
                    {
                        WindMin = reading.DailyWindMin,
                        WindMax = reading.DailyWindMax,
                        WindAvg = reading.DailyWindAvg,
                        WindUnit = reading.WindUnit.UnitName
                    } : null,
                    DateTime = reading.Date
                })
                .ToListAsync();

            return new HistoricSeaDataByLocation
            {
                Location = locationEntity.Name,
                Readings = readings
            };
        }
        public async Task<HistoricSeaDataByLocationDate> FetchIndexSeaDataByPeriod(int period)
        {
            var fromDate = DateTime.UtcNow.AddDays(-period);

            var data = await context.DailyGlassStormReading
                .Where(reading => reading.Date >= fromDate)
                .GroupBy(reading => reading.LocationId)
                .Select(group => new HistoricSeaDataByLocation
                {
                    Location = group.First().Location.Name,
                    Readings = group.Select(reading => new HistoricSeaDataByLocationReadings
                    {
                        WaveData = new WaveData
                        {
                            WaveMin = reading.DailyWaveMin,
                            WaveMax = reading.DailyWaveMax,
                            WaveAvg = reading.DailyWaveAvg,
                            WaveUnit = reading.WaveUnit != null ? reading.WaveUnit.UnitName : null
                        },
                        TempData = new TempData
                        {
                            TempMin = reading.DailyTempMin,
                            TempMax = reading.DailyTempMax,
                            TempAvg = reading.DailyTempAvg,
                            TempUnit = reading.TempUnit != null ? reading.TempUnit.UnitName : null
                        },
                        WindData = new WindData
                        {
                            WindMin = reading.DailyWindMin,
                            WindMax = reading.DailyWindMax,
                            WindAvg = reading.DailyWindAvg,
                            WindUnit = reading.WindUnit != null ? reading.WindUnit.UnitName : null
                        },
                        DateTime = reading.Date
                    }).ToList()
                }).ToListAsync();

            return new HistoricSeaDataByLocationDate
            {
                HistoricSeaDataByLocations = data
            };
        }
    }
}
