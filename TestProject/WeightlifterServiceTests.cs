using NUnit.Framework;
using Moq;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Backend.Services;
using Backend.Repositories;
using Backend.DTO.RequestResponseDTOs.Weightlifter;
using Backend.DTO.ModifiedDataDTO.Weightlifter;
using Backend.DTO.RequestResponseDTOs.Shared;

namespace TestProject
{
    public class WeightlifterServiceTests
    {
        private Mock<IWeightlifterRepository> _mockWeightlifterRepository;
        private Mock<IS3BucketAWSService> _mockS3BucketAWSService;
        private Mock<IUtilityService> _mockUtilityService;
        private IWeightlifterService _weightlifterService;

        [SetUp]
        public void Setup()
        {
            _mockWeightlifterRepository = new Mock<IWeightlifterRepository>();
            _mockS3BucketAWSService = new Mock<IS3BucketAWSService>();
            _mockUtilityService = new Mock<IUtilityService>();

            _weightlifterService = new WeightlifterService(
                _mockWeightlifterRepository.Object,
                _mockS3BucketAWSService.Object,
                _mockUtilityService.Object
            );
        }

        [Test]
        public async Task CreateNewBlogAsync_ValidInputs_ReturnsSuccessMessage()
        {
            // Arrange
            var userId = "user123";
            var text = "This is a blog text.";
            var mockImage = new Mock<IFormFile>();
            mockImage.Setup(image => image.OpenReadStream()).Returns(new MemoryStream());
            mockImage.Setup(image => image.FileName).Returns("image.jpg");

            _mockS3BucketAWSService
                .Setup(service => service.UploadTextAsync(It.IsAny<string>(), It.IsAny<string>(), text))
                .ReturnsAsync("https://s3.amazonaws.com/blogs/blog123.txt");

            _mockS3BucketAWSService
                .Setup(service => service.UploadFileAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Stream>()))
                .ReturnsAsync("https://s3.amazonaws.com/blogs/image123.jpg");

            _mockWeightlifterRepository
                .Setup(repo => repo.CreateNewBlog(userId, It.IsAny<string>(), It.IsAny<string>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _weightlifterService.CreateNewBlogAsync(text, mockImage.Object, userId);

            // Assert
            Assert.That(result, Is.EqualTo("Blog created successfully!"));
            _mockS3BucketAWSService.Verify(service => service.UploadTextAsync(It.IsAny<string>(), It.IsAny<string>(), text), Times.Once);
            _mockS3BucketAWSService.Verify(service => service.UploadFileAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Stream>()), Times.Once);
            _mockWeightlifterRepository.Verify(repo => repo.CreateNewBlog(userId, It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }

        [Test]
        public async Task FetchAllApprovedCommentsAsync_ReturnsApprovedComments()
        {
            // Arrange
            var skip = 0;
            var blogsPerPage = 10;
            var expectedComments = new List<BlogWithComments>
            {
                new BlogWithComments { BlogId = "1", ApplicationUserName = "User1" },
                new BlogWithComments { BlogId = "2", ApplicationUserName = "User2" }
            };

            _mockWeightlifterRepository
                .Setup(repo => repo.FetchAllApprovedBlogDataAsync(skip, blogsPerPage))
                .ReturnsAsync(expectedComments);

            // Act
            var result = await _weightlifterService.FetchAllApprovedCommentsAsync(skip, blogsPerPage);

            // Assert
            Assert.That(result, Is.EqualTo(expectedComments));
            _mockWeightlifterRepository.Verify(repo => repo.FetchAllApprovedBlogDataAsync(skip, blogsPerPage), Times.Once);
        }

        [Test]
        public async Task CreateNewTraining_ValidInputs_CreatesTraining()
        {
            // Arrange
            var userId = "user123";
            var createNewTraining = new CreateNewTraining
            {
                Name = "Training 1",
                Date = "2024-12-11",
                TargetWeight = 100,
                UnitName = "kg",
                TargetSets = 4,
                TargetReps = 10,
                Sets = new List<Set>
                {
                    new Set { Reps = 10, Text = "Good effort!", Image = null }
                }
            };

            _mockUtilityService
                .Setup(service => service.FetchTrainingUnitIdByName(createNewTraining.UnitName))
                .ReturnsAsync(1);

            _mockUtilityService
                .Setup(service => service.ConvertStringToDateTime(createNewTraining.Date))
                .Returns(DateTime.Parse(createNewTraining.Date));

            _mockUtilityService
                .Setup(service => service.CreateNewMediaReturnId(userId, null, "Good effort!"))
                .ReturnsAsync("media123");

            _mockWeightlifterRepository
                .Setup(repo => repo.CreateNewTraining(It.IsAny<string>(), It.IsAny<CreateNewTrainingModified>()))
                .Returns(Task.CompletedTask);

            // Act
            await _weightlifterService.CreateNewTraining(userId, createNewTraining);

            // Assert
            _mockUtilityService.Verify(service => service.FetchTrainingUnitIdByName(createNewTraining.UnitName), Times.Once);
            _mockUtilityService.Verify(service => service.ConvertStringToDateTime(createNewTraining.Date), Times.Once);
            _mockUtilityService.Verify(service => service.CreateNewMediaReturnId(userId, null, "Good effort!"), Times.Once);
            _mockWeightlifterRepository.Verify(repo => repo.CreateNewTraining(It.IsAny<string>(), It.IsAny<CreateNewTrainingModified>()), Times.Once);
        }
    }
}
