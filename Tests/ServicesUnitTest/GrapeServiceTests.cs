using Moq;
using Xunit;
using Microsoft.Extensions.Logging;
using Domain.Services;
using Domain.Interfaces;
using Domain.Entities;
using System.Linq.Expressions;

namespace Tests.ServicesUnitTest
{
    public class GrapeServiceTests
    {
        private readonly GrapeServices _grapeServices;
        private readonly Mock<IRepository<Grape>> _repositoryMock;
        private readonly Mock<ILogger<GrapeServices>> _loggerMock;

        public GrapeServiceTests()
        {
            _repositoryMock = new Mock<IRepository<Grape>>();
            _loggerMock = new Mock<ILogger<GrapeServices>>();
            _grapeServices = new GrapeServices(_repositoryMock.Object, _loggerMock.Object);
        }

        #region CreateGrape

        [Fact]
        public void CreateGrape_ValidGrape_ShouldReturnGrapeAndTrue()
        {
            // Arrange
            var grape = new Grape { Name = "Chardonnay" };
            var expectedGrape = new Grape { Id = Guid.NewGuid(), Name = "Chardonnay", Ativo = true };

            _repositoryMock.Setup(repo => repo.Create(It.IsAny<Grape>()))
                .Returns(expectedGrape);

            // Act
            var (result, success) = _grapeServices.CreateGrape(grape);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedGrape, result);
            Assert.True(success);
            _loggerMock.Verify(logger => logger.LogInformation("Added wine grape {grape}", grape), Times.Once);
        }

        [Fact]
        public void CreateGrape_WhenExceptionThrown_ShouldReturnNullAndFalse()
        {
            // Arrange
            var grape = new Grape { Name = "Cabernet Sauvignon" };
            _repositoryMock.Setup(repo => repo.Create(It.IsAny<Grape>()))
                .Throws(new Exception("Simulated exception"));

            // Act
            var (result, success) = _grapeServices.CreateGrape(grape);

            // Assert
            Assert.Null(result);
            Assert.False(success);
            _loggerMock.Verify(logger => logger.LogError(It.IsAny<Exception>(), "Error adding wine grape {grape}", grape), Times.Once);
        }

        #endregion

        #region DeleteGrape

        [Fact]
        public void DeleteGrape_ValidId_ShouldReturnTrue()
        {
            // Arrange
            var grapeId = Guid.NewGuid();

            // Act
            var result = _grapeServices.DeleteGrape(grapeId);

            // Assert
            Assert.True(result);
            _repositoryMock.Verify(repo => repo.DeleteById(grapeId), Times.Once);
            _loggerMock.Verify(logger => logger.LogError(It.IsAny<Exception>(), It.IsAny<string>(), It.IsAny<Guid>()), Times.Never);
        }

        [Fact]
        public void DeleteGrape_WhenExceptionThrown_ShouldReturnFalse()
        {
            // Arrange
            var grapeId = Guid.NewGuid();
            _repositoryMock.Setup(repo => repo.DeleteById(It.IsAny<Guid>()))
                .Throws(new Exception("Simulated exception"));

            // Act
            var result = _grapeServices.DeleteGrape(grapeId);

            // Assert
            Assert.False(result);
            _repositoryMock.Verify(repo => repo.DeleteById(grapeId), Times.Once);
            _loggerMock.Verify(logger => logger.LogError(It.IsAny<Exception>(), "Error deleting wine grape id {id}", grapeId), Times.Once);
        }

        #endregion

        #region GetGrapeById

        [Fact]
        public void GetGrapeById_ExistingId_ShouldReturnGrape()
        {
            // Arrange
            var grapeId = Guid.NewGuid();
            var expectedGrape = new Grape { Id = grapeId, Name = "Test Grape" };
            _repositoryMock.Setup(repo => repo.GetById(grapeId)).Returns(expectedGrape);

            // Act
            var result = _grapeServices.GetGrapeById(grapeId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedGrape.Id, result.Id);
            Assert.Equal(expectedGrape.Name, result.Name);
            _repositoryMock.Verify(repo => repo.GetById(grapeId), Times.Once);
        }

        [Fact]
        public void GetGrapeById_NonExistingId_ShouldReturnNull()
        {
            // Arrange
            var grapeId = Guid.NewGuid();
            _repositoryMock.Setup(repo => repo.GetById(grapeId)).Returns((Grape)null);

            // Act
            var result = _grapeServices.GetGrapeById(grapeId);

            // Assert
            Assert.Null(result);
            _repositoryMock.Verify(repo => repo.GetById(grapeId), Times.Once);
        }

        #endregion

        #region GetGrapeByName

        [Fact]
        public void GetGrapeByName_ExistingName_ShouldReturnGrape()
        {
            // Arrange
            var grapeName = "Test Grape";
            var expectedGrape = new Grape { Id = Guid.NewGuid(), Name = grapeName };
            _repositoryMock.Setup(repo => repo.GetByQuery(It.IsAny<Expression<Func<Grape, bool>>>()))
                .Returns(new List<Grape> { expectedGrape }.AsQueryable());

            // Act
            var result = _grapeServices.GetGrapeByName(grapeName);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedGrape.Id, result.Id);
            Assert.Equal(expectedGrape.Name, result.Name);
            _repositoryMock.Verify(repo => repo.GetByQuery(It.IsAny<Expression<Func<Grape, bool>>>()), Times.Once);
        }

        [Fact]
        public void GetGrapeByName_NonExistingName_ShouldReturnNull()
        {
            // Arrange
            var grapeName = "Nonexistent Grape";
            _repositoryMock.Setup(repo => repo.GetByQuery(It.IsAny<Expression<Func<Grape, bool>>>()))
                .Returns(Enumerable.Empty<Grape>().AsQueryable());

            // Act
            var result = _grapeServices.GetGrapeByName(grapeName);

            // Assert
            Assert.Null(result);
            _repositoryMock.Verify(repo => repo.GetByQuery(It.IsAny<Expression<Func<Grape, bool>>>()), Times.Once);
        }

        #endregion


        #region CheckValidGrapeFields

        [Fact]
        public void CheckValidGrapeFields_ValidGrape_ShouldReturnEmptyDictionary()
        {
            // Arrange
            var grape = new Grape
            {
                Name = "Valid Grape",
                Description = "A nice grape",
                Origin = "France"
            };

            // Act
            var result = _grapeServices.CheckValidGrapeFields(grape);

            // Assert
            Assert.Empty(result);
            _loggerMock.Verify(log => log.Log(It.IsAny<LogLevel>(), It.IsAny<EventId>(), It.IsAny<object>(), It.IsAny<Exception>(), It.IsAny<Func<object, Exception, string>>()), Times.Never);
        }

        [Fact]
        public void CheckValidGrapeFields_InvalidGrape_ShouldReturnValidationErrors()
        {
            // Arrange
            var grape = new Grape
            {
                Name = "",
                Description = "",
                Origin = ""
            };

            // Act
            var result = _grapeServices.CheckValidGrapeFields(grape);

            // Assert
            Assert.Contains("Grape Name", result);
            Assert.Equal("Name is required", result["Grape Name"]);

            Assert.Contains("Grape Description", result);
            Assert.Equal("Description is required", result["Grape Description"]);

            Assert.Contains("Grape Origin", result);
            Assert.Equal("Origin is required", result["Grape Origin"]);

            _loggerMock.Verify(log => log.Log(It.IsAny<LogLevel>(), It.IsAny<EventId>(), It.IsAny<object>(), It.IsAny<Exception>(), It.IsAny<Func<object, Exception, string>>()), Times.Never);
        }

        [Fact]
        public void CheckValidGrapeFields_Exception_ShouldReturnErrorDictionary()
        {
            // Arrange
            var grape = new Grape();
            _loggerMock.Setup(log => log.Log(It.IsAny<LogLevel>(), It.IsAny<EventId>(), It.IsAny<object>(), It.IsAny<Exception>(), It.IsAny<Func<object, Exception, string>>()))
                .Throws(new Exception("Test exception"));

            // Act
            var result = _grapeServices.CheckValidGrapeFields(grape);

            // Assert
            Assert.Contains("Error", result);
            Assert.Equal("Internal error while validating grape", result["Error"]);
        }

        #endregion

    }
}
