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

        // Fetch all unapproved blog comments for both Training and Sea blogs
        public async Task<UnapprovedBlogCommentData> FetchAllUnapprovedBlogComments()
        {
            // Fetch unapproved training comments
            var unapprovedTrainingComments = await context.TrainingComment
                .Where(c => c.ApprovedStatus == false)
                .Select(c => new UnapprovedBlogComment
                {
                    Id = c.CommentId,
                    TextUrl = c.Media.TextUrl, // Assuming Media.TextUrl exists
                    PictureUrl = c.Media.PictureUrl // Assuming Media.PictureUrl exists
                })
                .ToListAsync();

            // Fetch unapproved sea comments
            var unapprovedSeaComments = await context.SeaComment
                .Where(c => c.ApprovedStatus == false)
                .Select(c => new UnapprovedBlogComment
                {
                    Id = c.CommentId,
                    TextUrl = c.Media.TextUrl, // Assuming Media.TextUrl exists
                    PictureUrl = c.Media.PictureUrl // Assuming Media.PictureUrl exists
                })
                .ToListAsync();

            // Fetch unapproved training blogs
            var unapprovedTrainingBlogs = await context.TrainingBlog
                .Where(b => b.ApprovedStatus == false)
                .Select(b => new UnapprovedBlogComment
                {
                    Id = b.BlogId,
                    TextUrl = b.Media.TextUrl, // Assuming Media.TextUrl exists
                    PictureUrl = b.Media.PictureUrl // Assuming Media.PictureUrl exists
                })
                .ToListAsync();

            // Fetch unapproved sea blogs
            var unapprovedSeaBlogs = await context.SeaBlog
                .Where(b => b.ApprovedStatus == false)
                .Select(b => new UnapprovedBlogComment
                {
                    Id = b.BlogId,
                    TextUrl = b.Media.TextUrl, // Assuming Media.TextUrl exists
                    PictureUrl = b.Media.PictureUrl // Assuming Media.PictureUrl exists
                })
                .ToListAsync();

            // Combine all unapproved data
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

            // Check Training Comments
            var trainingComment = await context.TrainingComment.FirstOrDefaultAsync(c => c.CommentId.ToString() == id);
            if (trainingComment != null)
            {
                if (isApproved)
                    await ApproveDiverComment(id);
                else
                    await DeleteDiverComment(id);
                return;
            }

            // Check Training Blogs
            var trainingBlog = await context.TrainingBlog.FirstOrDefaultAsync(b => b.BlogId.ToString() == id);
            if (trainingBlog != null)
            {
                if (isApproved)
                    await ApproveDiverBlog(id);
                else
                    await DeleteDiverBlog(id);
                return;
            }

            // Check Sea Comments
            var seaComment = await context.SeaComment.FirstOrDefaultAsync(c => c.CommentId.ToString() == id);
            if (seaComment != null)
            {
                if (isApproved)
                    await ApproveWeightlifterComment(id);
                else
                    await DeleteWeightlifterComment(id);
                return;
            }

            // Check Sea Blogs
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
    }
}