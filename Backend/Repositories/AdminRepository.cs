using Backend.Data;
using Backend.DTO;
using Backend.Data.Models;
using Microsoft.EntityFrameworkCore;
using Backend.DTO.RequestResponseDTOs.Admin;

namespace Backend.Repositories
{
    public class AdminRepository
    {
        private readonly ApplicationDbContext context;

        public AdminRepository(ApplicationDbContext _context)
        {
            context = _context;
        }

        public async Task<UnapprovedBlogCommentData> FetchAllUnapprovedBlogComments()
        {
            var unapprovedTrainingComments = await context.TrainingComment
                .Where(c => c.ApprovedStatus == false)
                .Select(c => new UnapprovedBlogComment
                {
                    Id = c.CommentId,
                    TextUrl = c.Media.TextUrl, 
                    PictureUrl = c.Media.PictureUrl, 
                    Username = c.ApplicationUser.UserName, 
                    TimeOfPosting = c.Time 
                })
                .ToListAsync();

            var unapprovedSeaComments = await context.SeaComment
                .Where(c => c.ApprovedStatus == false)
                .Select(c => new UnapprovedBlogComment
                {
                    Id = c.CommentId,
                    TextUrl = c.Media.TextUrl, 
                    PictureUrl = c.Media.PictureUrl, 
                    Username = c.ApplicationUser.UserName,
                    TimeOfPosting = c.Time 
                })
                .ToListAsync();

            var unapprovedTrainingBlogs = await context.TrainingBlog
                .Where(b => b.ApprovedStatus == false)
                .Select(b => new UnapprovedBlogComment
                {
                    Id = b.BlogId,
                    TextUrl = b.Media.TextUrl, 
                    PictureUrl = b.Media.PictureUrl, 
                    Username = b.ApplicationUser.UserName, 
                    TimeOfPosting = b.Time 
                })
                .ToListAsync();

            var unapprovedSeaBlogs = await context.SeaBlog
                .Where(b => b.ApprovedStatus == false)
                .Select(b => new UnapprovedBlogComment
                {
                    Id = b.BlogId,
                    TextUrl = b.Media.TextUrl, 
                    PictureUrl = b.Media.PictureUrl, 
                    Username = b.ApplicationUser.UserName, 
                    TimeOfPosting = b.Time 
                })
                .ToListAsync();

            return new UnapprovedBlogCommentData
            {
                UnapprovedData = unapprovedTrainingComments
                    .Concat(unapprovedSeaComments)
                    .Concat(unapprovedTrainingBlogs)
                    .Concat(unapprovedSeaBlogs)
                    .ToList()
            };
        }
        public async Task ApproveOrDeleteBlogComment(string id, string status)
        {
            bool isApproved = status.Equals("approved", StringComparison.OrdinalIgnoreCase);

            var trainingComment = await context.TrainingComment.FirstOrDefaultAsync(c => c.CommentId.ToString() == id);
            if (trainingComment != null)
            {
                if (isApproved)
                    await ApproveDiverComment(id);
                else
                    await DeleteDiverComment(id);
                return;
            }

            var trainingBlog = await context.TrainingBlog.FirstOrDefaultAsync(b => b.BlogId.ToString() == id);
            if (trainingBlog != null)
            {
                if (isApproved)
                    await ApproveDiverBlog(id);
                else
                    await DeleteDiverBlog(id);
                return;
            }

            var seaComment = await context.SeaComment.FirstOrDefaultAsync(c => c.CommentId.ToString() == id);
            if (seaComment != null)
            {
                if (isApproved)
                    await ApproveWeightlifterComment(id);
                else
                    await DeleteWeightlifterComment(id);
                return;
            }

            var seaBlog = await context.SeaBlog.FirstOrDefaultAsync(b => b.BlogId.ToString() == id);
            if (seaBlog != null)
            {
                if (isApproved)
                    await ApproveWeightlifterBlog(id);
                else
                    await DeleteWeightlifterBlog(id);
                return;
            }

            throw new ArgumentException("Invalid ID or entity not found.");
        }

        private async Task ApproveDiverBlog(string id)
        {
            var blog = await context.TrainingBlog.FirstOrDefaultAsync(b => b.BlogId.ToString() == id);
            if (blog != null)
            {
                blog.ApprovedStatus = true;
                await context.SaveChangesAsync();
            }
        }

        private async Task DeleteDiverBlog(string id)
        {
            var blog = await context.TrainingBlog.FirstOrDefaultAsync(b => b.BlogId.ToString() == id);
            if (blog != null)
            {
                context.TrainingBlog.Remove(blog);
                await context.SaveChangesAsync();
            }
        }

        private async Task ApproveDiverComment(string id)
        {
            var comment = await context.TrainingComment.FirstOrDefaultAsync(c => c.CommentId.ToString() == id);
            if (comment != null)
            {
                comment.ApprovedStatus = true;
                await context.SaveChangesAsync();
            }
        }

        private async Task DeleteDiverComment(string id)
        {
            var comment = await context.TrainingComment.FirstOrDefaultAsync(c => c.CommentId.ToString() == id);
            if (comment != null)
            {
                context.TrainingComment.Remove(comment);
                await context.SaveChangesAsync();
            }
        }

        private async Task ApproveWeightlifterBlog(string id)
        {
            var blog = await context.SeaBlog.FirstOrDefaultAsync(b => b.BlogId.ToString() == id);
            if (blog != null)
            {
                blog.ApprovedStatus = true;
                await context.SaveChangesAsync();
            }
        }

        private async Task DeleteWeightlifterBlog(string id)
        {
            var blog = await context.SeaBlog.FirstOrDefaultAsync(b => b.BlogId.ToString() == id);
            if (blog != null)
            {
                context.SeaBlog.Remove(blog);
                await context.SaveChangesAsync();
            }
        }

        private async Task ApproveWeightlifterComment(string id)
        {
            var comment = await context.SeaComment.FirstOrDefaultAsync(c => c.CommentId.ToString() == id);
            if (comment != null)
            {
                comment.ApprovedStatus = true;
                await context.SaveChangesAsync();
            }
        }

        private async Task DeleteWeightlifterComment(string id)
        {
            var comment = await context.SeaComment.FirstOrDefaultAsync(c => c.CommentId.ToString() == id);
            if (comment != null)
            {
                context.SeaComment.Remove(comment);
                await context.SaveChangesAsync();
            }
        }

        public async Task<FeedbacksToDisplay> FetchAllFeedbacks()
        {
            var feedbacks = await context.Feedback
                .Include(f => f.Media)
                .Include(f => f.ApplicationUser)
                .Include(f => f.Location) 
                .Include(f => f.WaveUnit) 
                .Include(f => f.TempUnit)
                .Include(f => f.WindSpeedUnit) 
                .Select(f => new FeedbackToDisplay
                {
                    Id = f.Id,
                    Username = f.ApplicationUser.UserName,
                    LocationName = f.Location.Name, 
                    Date = f.Time.ToString(),
                    WaveRead = f.WaveRead,
                    WaveUnitName = f.WaveUnit != null ? f.WaveUnit.UnitName : null,
                    TempRead = f.TempRead,
                    TempUnitName = f.TempUnit != null ? f.TempUnit.UnitName : null,
                    windSpeedRead = f.WindSpeedIndex,
                    WindSpeedUnitName = f.WindSpeedUnit != null ? f.WindSpeedUnit.UnitName : null,
                    ImageUrl = f.Media != null ? f.Media.PictureUrl : null,
                    TextUrl = f.Media != null ? f.Media.TextUrl : null
                })
                .ToListAsync();

            return new FeedbacksToDisplay
            {
                Feedbacks = feedbacks
            };
        }

        public async Task DeleteFeedback(string id)
        {
            var feedback = await context.Feedback
                .Include(f => f.Media)
                .FirstOrDefaultAsync(f => f.Id == id);

            if (feedback == null)
            {
                throw new ArgumentException("Feedback with the specified ID does not exist.");
            }

            if (feedback.Media != null)
            {
                context.Media.Remove(feedback.Media);
            }

            context.Feedback.Remove(feedback);

            await context.SaveChangesAsync();
        }

        public async Task<ServerLogs> FetchAllServerLogs()
        {
            var logs = await context.DataFetchingLogs
                .OrderByDescending(log => log.Time) 
                .ToListAsync();

            var serverLogs = logs.Select(log => new ServerLog
            {
                Id = log.Id.ToString(),
                StatusLog = log.StatusLog,
                Time = log.Time.ToString("yyyy-MM-dd HH:mm:ss")
            }).ToList();

            return new ServerLogs
            {
                _ServerLogs = serverLogs
            };
        }
        public async Task DeleteServerLog(string id)
        {
            var log = await context.DataFetchingLogs.FirstOrDefaultAsync(l => l.Id.ToString() == id);

            if (log == null)
            {
                throw new ArgumentException($"No log found with ID: {id}");
            }

            context.DataFetchingLogs.Remove(log);
            await context.SaveChangesAsync();
        }
    }
}