using Backend.Data.Models;
using Backend.Data;
using Backend.DTO;
using Microsoft.EntityFrameworkCore;
using Backend.DTO.RequestResponseDTOs.Shared;

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
    }
}
