using Backend.Data.Models;
using Microsoft.AspNetCore.Identity;
using Backend.Data.Models;
using System.Collections.Generic;

namespace Backend.Data.ExtendedModel
{
    public class ApplicationUser : IdentityUser
    {
        public ApplicationUser()
        {
            TrainingBlogs = new List<TrainingBlog>();
            SeaBlogs = new List<SeaBlog>();
            TrainingComments = new List<TrainingComment>();
            SeaComments = new List<SeaComment>();
            Feedbacks = new List<Feedback>();
            UniversalReadings = new List<UniversalReading>();
            Medias = new List<Media>();
            TrainingLogs = new List<TrainingLog>();
        }

        public virtual List<TrainingBlog> TrainingBlogs { get; set; }
        public virtual List<SeaBlog> SeaBlogs { get; set; }
        public virtual List<TrainingComment> TrainingComments { get; set; }
        public virtual List<SeaComment> SeaComments { get; set; }
        public virtual List<Feedback> Feedbacks { get; set; }
        public virtual List<TrainingLog> TrainingLogs { get; set; }
        public virtual List<UniversalReading> UniversalReadings { get; set; }
        public virtual List<Media> Medias { get; set; }
    }
}
