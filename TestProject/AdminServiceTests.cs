using NUnit.Framework;
using Moq;
using Backend.Repositories;
using Backend.Services;
using Backend.DTO.RequestResponseDTOs.Admin;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TestProject
{
    public class AdminServiceTests
    {
        private Mock<IAdminRepository> _mockRepository;
        private IAdminService _adminService;

        [SetUp]
        public void Setup()
        {
            _mockRepository = new Mock<IAdminRepository>();
            _adminService = new AdminService(_mockRepository.Object);
        }

        [Test]
        public async Task FetchAllUnapprovedBlogCommentsAsync_ReturnsCorrectData()
        {
            // Arrange
            var expectedData = new UnapprovedBlogCommentData
            {
                UnapprovedData = new List<UnapprovedBlogComment>
                {
                    new UnapprovedBlogComment
                    {
                        Id = "1",
                        TextUrl = "http://example.com/comment1",
                        Username = "User1",
                        PictureUrl = "http://example.com/user1.jpg",
                        TimeOfPosting = DateTime.Now
                    },
                    new UnapprovedBlogComment
                    {
                        Id = "2",
                        TextUrl = "http://example.com/comment2",
                        Username = "User2",
                        PictureUrl = "http://example.com/user2.jpg",
                        TimeOfPosting = DateTime.Now.AddMinutes(-10)
                    }
                }
            };

            _mockRepository
                .Setup(repo => repo.FetchAllUnapprovedBlogComments())
                .ReturnsAsync(expectedData);

            // Act
            var result = await _adminService.FetchAllUnapprovedBlogCommentsAsync();

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.UnapprovedData.Count, Is.EqualTo(2));
            Assert.That(result.UnapprovedData[0].Id, Is.EqualTo("1"));
            Assert.That(result.UnapprovedData[0].Username, Is.EqualTo("User1"));
        }

        [Test]
        public async Task ApproveOrRejectBlogCommentAsync_ReturnsSuccessMessage()
        {
            // Arrange
            var id = "123";
            var status = "approved";

            _mockRepository
                .Setup(repo => repo.ApproveOrDeleteBlogComment(id, status))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _adminService.ApproveOrRejectBlogCommentAsync(id, status);

            // Assert
            Assert.That(result, Is.EqualTo($"Successfully {status} entity with ID: {id}"));
            _mockRepository.Verify(repo => repo.ApproveOrDeleteBlogComment(id, status), Times.Once);
        }

        [Test]
        public async Task FetchAllFeedbacks_ReturnsFeedbacks()
        {
            // Arrange
            var expectedFeedbacks = new FeedbacksToDisplay
            {
                Feedbacks = new List<FeedbackToDisplay>
                {
                    new FeedbackToDisplay
                    {
                        Id = "1",
                        Username = "User1",
                        LocationName = "Location1",
                        Date = "2024-12-10",
                        WaveRead = 2.5f,
                        WaveUnitName = "m",
                        TempRead = 22.3f,
                        TempUnitName = "C",
                        windSpeedRead = 5.0f,
                        WindSpeedUnitName = "km/h",
                        ImageUrl = "http://example.com/image1.jpg",
                        TextUrl = "http://example.com/text1"
                    },
                    new FeedbackToDisplay
                    {
                        Id = "2",
                        Username = "User2",
                        LocationName = "Location2",
                        Date = "2024-12-09",
                        WaveRead = 3.1f,
                        WaveUnitName = "m",
                        TempRead = 19.8f,
                        TempUnitName = "C",
                        windSpeedRead = 7.5f,
                        WindSpeedUnitName = "km/h",
                        ImageUrl = "http://example.com/image2.jpg",
                        TextUrl = "http://example.com/text2"
                    }
                }
            };

            _mockRepository
                .Setup(repo => repo.FetchAllFeedbacks())
                .ReturnsAsync(expectedFeedbacks);

            // Act
            var result = await _adminService.FetchAllFeedbacks();

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Feedbacks.Count, Is.EqualTo(2));
            Assert.That(result.Feedbacks[0].Id, Is.EqualTo("1"));
            Assert.That(result.Feedbacks[0].Username, Is.EqualTo("User1"));
        }

        [Test]
        public async Task DeleteFeedback_ReturnsSuccessMessage()
        {
            // Arrange
            var id = "feedback123";

            _mockRepository
                .Setup(repo => repo.DeleteFeedback(id))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _adminService.DeleteFeedback(id);

            // Assert
            Assert.That(result, Is.EqualTo($"Successfully deleted entity with ID: {id}"));
            _mockRepository.Verify(repo => repo.DeleteFeedback(id), Times.Once);
        }

        [Test]
        public async Task FetchAllServerLogs_ReturnsLogs()
        {
            // Arrange
            var expectedLogs = new ServerLogs
            {
                _ServerLogs = new List<ServerLog>
                {
                    new ServerLog
                    {
                        Id = "1",
                        StatusLog = "Error: Something went wrong",
                        Time = "2024-12-11 10:00:00"
                    },
                    new ServerLog
                    {
                        Id = "2",
                        StatusLog = "Info: System running normally",
                        Time = "2024-12-11 10:05:00"
                    }
                }
            };

            _mockRepository
                .Setup(repo => repo.FetchAllServerLogs())
                .ReturnsAsync(expectedLogs);

            // Act
            var result = await _adminService.FetchAllServerLogs();

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result._ServerLogs.Count, Is.EqualTo(2));
            Assert.That(result._ServerLogs[0].Id, Is.EqualTo("1"));
            Assert.That(result._ServerLogs[0].StatusLog, Is.EqualTo("Error: Something went wrong"));
        }

        [Test]
        public async Task DeleteServerLog_ReturnsSuccessMessage()
        {
            // Arrange
            var id = "log123";

            _mockRepository
                .Setup(repo => repo.DeleteServerLog(id))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _adminService.DeleteServerLog(id);

            // Assert
            Assert.That(result, Is.EqualTo($"Successfully deleted entity with ID: {id}"));
            _mockRepository.Verify(repo => repo.DeleteServerLog(id), Times.Once);
        }
    }
}