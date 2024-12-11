using Moq;
using Backend.Services;
using Backend.Repositories;
using Backend.DTO.RequestResponseDTOs.Diver;
using Microsoft.AspNetCore.Http;
using Backend.DTO.RequestResponseDTOs.Shared;

namespace TestProject
{
    public class DiverServiceTests
    {
        private Mock<IDiverRepository> _mockDiverRepository;
        private Mock<IS3BucketAWSService> _mockS3BucketService;
        private Mock<IUtilityService> _mockUtilityService;
        private IDiverService _diverService;

        [SetUp]
        public void Setup()
        {
            _mockDiverRepository = new Mock<IDiverRepository>();
            _mockS3BucketService = new Mock<IS3BucketAWSService>();
            _mockUtilityService = new Mock<IUtilityService>();

            _diverService = new DiverService(
                _mockDiverRepository.Object,
                _mockS3BucketService.Object,
                _mockUtilityService.Object
            );
        }

        [Test]
        public async Task CreateNewBlogAsync_CreatesBlogSuccessfully()
        {
            // Arrange
            var userId = "user123";
            var text = "This is a blog text.";
            var mockImage = new Mock<IFormFile>();

            _mockS3BucketService
                .Setup(service => service.UploadTextAsync(It.IsAny<string>(), It.IsAny<string>(), text))
                .ReturnsAsync("https://s3.amazonaws.com/blogs/blog123.txt");

            _mockS3BucketService
                .Setup(service => service.UploadFileAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Stream>()))
                .ReturnsAsync("https://s3.amazonaws.com/blogs/blogImage123.jpg");

            _mockDiverRepository
                .Setup(repo => repo.CreateNewBlog(userId, It.IsAny<string>(), It.IsAny<string>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _diverService.CreateNewBlogAsync(text, mockImage.Object, userId);

            // Assert
            Assert.That(result, Is.EqualTo("Blog created successfully!"));
            _mockS3BucketService.Verify(service => service.UploadTextAsync(It.IsAny<string>(), It.IsAny<string>(), text), Times.Once);
            _mockDiverRepository.Verify(repo => repo.CreateNewBlog(userId, It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }

        [Test]
        public async Task FetchAllApprovedCommentsAsync_ReturnsApprovedComments()
        {
            // Arrange
            var skip = 0;
            var blogsPerPage = 10;
            var mockBlogs = new List<BlogWithComments>
            {
                new BlogWithComments { BlogId = "1", ApplicationUserName = "User1", Time = DateTime.Now },
                new BlogWithComments { BlogId = "2", ApplicationUserName = "User2", Time = DateTime.Now.AddMinutes(-10) }
            };

            _mockDiverRepository
                .Setup(repo => repo.FetchAllApprovedBlogDataAsync(skip, blogsPerPage))
                .ReturnsAsync(mockBlogs);

            // Act
            var result = await _diverService.FetchAllApprovedCommentsAsync(skip, blogsPerPage);

            // Assert
            Assert.That(result.Count, Is.EqualTo(mockBlogs.Count));
            Assert.That(result[0].BlogId, Is.EqualTo("1"));
            Assert.That(result[1].BlogId, Is.EqualTo("2"));
            _mockDiverRepository.Verify(repo => repo.FetchAllApprovedBlogDataAsync(skip, blogsPerPage), Times.Once);
        }

        [Test]
        public async Task PostUserFeedback_CreatesFeedbackSuccessfully()
        {
            // Arrange
            var userId = "user123";
            var feedback = new FeedbackDTO
            {
                LocationName = "Beach",
                Coordinates = "40.7128,-74.0060",
                WaveRead = 2.5f,
                TempRead = 22.3f,
                windSpeedRead = 5.5f,
                Text = "Good conditions for diving.",
                Image = null
            };

            _mockUtilityService
                .Setup(service => service.GetLocationIdByNameAsync(feedback.LocationName))
                .ReturnsAsync(1);

            _mockDiverRepository
                .Setup(repo => repo.PostUserFeedback(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<float?>(), It.IsAny<int?>(), It.IsAny<float?>(), It.IsAny<int?>(), It.IsAny<float?>(), It.IsAny<int?>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(Task.CompletedTask);

            // Act
            await _diverService.PostUserFeedback(userId, feedback);

            // Assert
            _mockUtilityService.Verify(service => service.GetLocationIdByNameAsync(feedback.LocationName), Times.Once);
            _mockDiverRepository.Verify(repo => repo.PostUserFeedback(
                userId,
                1,
                feedback.WaveRead,
                null,
                feedback.TempRead,
                null,
                feedback.windSpeedRead,
                null,
                null,
                It.IsAny<string>()), Times.Once);
        }

        [Test]
        public async Task ReturnDiveIndexOnData_ReturnsCorrectIndex()
        {
            // Arrange
            var readings = new List<HistoricSeaDataByLocationReadings>
            {
                new HistoricSeaDataByLocationReadings
                {
                    WaveData = new WaveData { WaveAvg = 0.3f, WaveMax = 0.6f }
                },
                new HistoricSeaDataByLocationReadings
                {
                    WaveData = new WaveData { WaveAvg = 0.5f, WaveMax = 0.8f }
                }
            };

            // Act
            var result = await _diverService.ReturnDiveIndexOnData(readings);

            // Assert
            Assert.That(result, Is.EqualTo(2.5).Within(0.1));
        }
    }
}
