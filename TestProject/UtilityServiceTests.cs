using NUnit.Framework;
using Moq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Backend.Services;
using Backend.Repositories;

namespace TestProject
{
    public class UtilityServiceTests
    {
        private Mock<IUtilsRepository> _mockUtilsRepository;
        private Mock<IS3BucketAWSService> _mockBucketAWSService;
        private IUtilityService _utilityService;

        [SetUp]
        public void Setup()
        {
            _mockUtilsRepository = new Mock<IUtilsRepository>();
            _mockBucketAWSService = new Mock<IS3BucketAWSService>();

            _utilityService = new UtilityService(
                _mockUtilsRepository.Object,
                _mockBucketAWSService.Object
            );
        }

        [Test]
        public async Task FetchSearchSuggestions_ReturnsSuggestions()
        {
            // Arrange
            var searchQuery = "test";
            var expectedSuggestions = new List<string> { "test1", "test2" };

            _mockUtilsRepository
                .Setup(repo => repo.FetchSearchSuggestions(searchQuery))
                .ReturnsAsync(expectedSuggestions);

            // Act
            var result = await _utilityService.FetchSearchSuggestions(searchQuery);

            // Assert
            Assert.That(result, Is.EqualTo(expectedSuggestions));
            _mockUtilsRepository.Verify(repo => repo.FetchSearchSuggestions(searchQuery), Times.Once);
        }

        [Test]
        public async Task GetLocationIdByNameAsync_ReturnsLocationId()
        {
            // Arrange
            var locationName = "test-location";
            var expectedLocationId = 1;

            _mockUtilsRepository
                .Setup(repo => repo.GetLocationIdByNameAsync(locationName))
                .ReturnsAsync(expectedLocationId);

            // Act
            var result = await _utilityService.GetLocationIdByNameAsync(locationName);

            // Assert
            Assert.That(result, Is.EqualTo(expectedLocationId));
            _mockUtilsRepository.Verify(repo => repo.GetLocationIdByNameAsync(locationName), Times.Once);
        }

        [Test]
        public async Task GetUserIdByEmailAsync_ReturnsUserId()
        {
            // Arrange
            var email = "test@example.com";
            var expectedUserId = "user123";

            _mockUtilsRepository
                .Setup(repo => repo.GetUserIdByEmailAsync(email))
                .ReturnsAsync(expectedUserId);

            // Act
            var result = await _utilityService.GetUserIdByEmailAsync(email);

            // Assert
            Assert.That(result, Is.EqualTo(expectedUserId));
            _mockUtilsRepository.Verify(repo => repo.GetUserIdByEmailAsync(email), Times.Once);
        }

        [Test]
        public void ConvertStringToDateTime_ValidString_ReturnsDateTime()
        {
            // Arrange
            var dateTimeString = "2024-12-11T10:00:00";
            var expectedDateTime = DateTime.Parse(dateTimeString);

            // Act
            var result = _utilityService.ConvertStringToDateTime(dateTimeString);

            // Assert
            Assert.That(result, Is.EqualTo(expectedDateTime));
        }

        [Test]
        public void ConvertStringToDateTime_InvalidString_ThrowsFormatException()
        {
            // Arrange
            var invalidDateTimeString = "invalid-date";

            // Act & Assert
            Assert.Throws<FormatException>(() => _utilityService.ConvertStringToDateTime(invalidDateTimeString));
        }

        [Test]
        public async Task CreateNewMediaReturnId_CreatesMediaSuccessfully_ReturnsId()
        {
            // Arrange
            var userId = "user123";
            var text = "sample text";
            var mockImage = new Mock<IFormFile>();

            mockImage.Setup(image => image.OpenReadStream()).Returns(new MemoryStream());
            mockImage.Setup(image => image.FileName).Returns("image.jpg");

            var imageUrl = "https://s3.amazonaws.com/images/image.jpg";
            var textUrl = "https://s3.amazonaws.com/texts/text.txt";
            var expectedId = "media123";

            _mockBucketAWSService
                .Setup(service => service.UploadFileAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Stream>()))
                .ReturnsAsync(imageUrl);

            _mockBucketAWSService
                .Setup(service => service.UploadTextAsync(It.IsAny<string>(), It.IsAny<string>(), text))
                .ReturnsAsync(textUrl);

            _mockUtilsRepository
                .Setup(repo => repo.CreateNewMediaReturnId(userId, imageUrl, textUrl))
                .ReturnsAsync(expectedId);

            // Act
            var result = await _utilityService.CreateNewMediaReturnId(userId, mockImage.Object, text);

            // Assert
            Assert.That(result, Is.EqualTo(expectedId));
            _mockBucketAWSService.Verify(service => service.UploadFileAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Stream>()), Times.Once);
            _mockBucketAWSService.Verify(service => service.UploadTextAsync(It.IsAny<string>(), It.IsAny<string>(), text), Times.Once);
            _mockUtilsRepository.Verify(repo => repo.CreateNewMediaReturnId(userId, imageUrl, textUrl), Times.Once);
        }
    }
}
