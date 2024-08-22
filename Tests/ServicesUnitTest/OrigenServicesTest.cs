using Domain.Entities;
using Domain.Interfaces;
using Domain.Services;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;


namespace Tests.ServicesUnitTest
{
    public class OrigenServicesTest
    {
        private readonly Mock<IRepository<Origin>> _repositoryMock;
        private readonly Mock<ILogger<OriginService>> _loggerMock;
        private readonly OriginService _originService;

        public OrigenServicesTest()
        {
            _repositoryMock = new Mock<IRepository<Origin>>();
            _loggerMock = new Mock<ILogger<OriginService>>();
            _originService = new OriginService(_repositoryMock.Object, _loggerMock.Object);
        }

        #region CheckValidOriginFields

        [Fact]
        public void CheckValidOriginFields_NameIsEmpty_ShouldReturnValidationError()
        {
            var origin = new Origin { Name = string.Empty };
            var result = _originService.CheckValidOriginFields(origin);

            Assert.Contains("origin Name", result);
            Assert.Equal("Name is required", result["origin Name"]);
        }

        [Fact]
        public void CheckValidOriginFields_NameIsTooLong_ShouldReturnValidationError()
        {
            var origin = new Origin { Name = new string('a', 101) }; // Name with 101 characters
            var result = _originService.CheckValidOriginFields(origin);

            Assert.Contains("origin Name", result);
            Assert.Equal("Name must have a maximum of 100 characters", result["origin Name"]);
        }

        [Fact]
        public void CheckValidOriginFields_NameIsValid_ShouldReturnNoErrors()
        {
            var origin = new Origin { Name = "Valid Origin" };
            var result = _originService.CheckValidOriginFields(origin);

            Assert.Empty(result);
        }

        #endregion

        #region CreateOrigin

        [Fact]
        public void CreateOrigin_InvalidOrigin_ShouldReturnValidationErrors()
        {
            var origin = new Origin { Name = string.Empty };
            var expectedErrors = new Dictionary<string, string>
            {
                { "origin Name", "Name is required" }
            };

            var (result, validations) = _originService.CreateOrigin(origin);

            Assert.Null(result);
            Assert.Equal(expectedErrors, validations);
        }

        [Fact]
        public void CreateOrigin_ValidOrigin_ShouldCreateAndReturnOrigin()
        {
            var origin = new Origin { Name = "Valid Origin" };

            _repositoryMock
                .Setup(repo => repo.Create(It.IsAny<Origin>()))
                .Returns((Origin o) => o);

            var (result, validations) = _originService.CreateOrigin(origin);

            Assert.NotNull(result);
            Assert.Equal(origin.Name, result.Name);
            Assert.Empty(validations);
            _repositoryMock.Verify(repo => repo.Create(It.IsAny<Origin>()), Times.Once);
        }

        [Fact]
        public void CreateOrigin_RepositoryThrowsException_ShouldLogErrorAndReturnNull()
        {
            var origin = new Origin { Name = "Valid Origin" };

            _repositoryMock
                .Setup(repo => repo.Create(It.IsAny<Origin>()))
                .Throws(new Exception("Repository exception"));

            var (result, validations) = _originService.CreateOrigin(origin);

            Assert.Null(result);
            Assert.NotEmpty(validations);
            _repositoryMock.Verify(repo => repo.Create(It.IsAny<Origin>()), Times.Once);
        }

        #endregion

        #region Delete

        [Fact]
        public void Delete_ExistingId_ShouldReturnTrue()
        {
            var originId = Guid.NewGuid();
            var origin = new Origin { Id = originId, Name = "Test Origin" };
            _repositoryMock.Setup(r => r.DeleteById(originId)).Verifiable();
            _repositoryMock.Setup(repo => repo.GetById(originId)).Returns(origin);

            var result = _originService.Delete(originId);

            Assert.True(result);
            _repositoryMock.Verify(r => r.DeleteById(originId), Times.Once);
        }

        #endregion

        #region GetAll

        [Fact]
        public void GetAll_ShouldReturnAllOrigins()
        {
            var origins = new List<Origin>
        {
            new Origin { Id = Guid.NewGuid(), Name = "Origin 1", Ativo = true },
            new Origin { Id = Guid.NewGuid(), Name = "Origin 2", Ativo = true },
        };
            _repositoryMock.Setup(r => r.GetAll()).Returns(origins.AsQueryable());

            var result = _originService.GetAll();

            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            Assert.Contains(result, o => o.Name == "Origin 1");
            Assert.Contains(result, o => o.Name == "Origin 2");
            _repositoryMock.Verify(r => r.GetAll(), Times.Once);
        }

        [Fact]
        public void GetAll_WhenNoOrigins_ShouldReturnEmptyList()
        {
            _repositoryMock.Setup(r => r.GetAll()).Returns(new List<Origin>().AsQueryable());
            var result = _originService.GetAll();

            Assert.NotNull(result);
            Assert.Empty(result);
            _repositoryMock.Verify(r => r.GetAll(), Times.Once);
        }

        #endregion

        #region GetOriginById

        [Fact]
        public void GetOriginById_ExistingId_ShouldReturnOrigin()
        {
            var id = Guid.NewGuid();
            var origin = new Origin { Id = id, Name = "Test Origin" };
            _repositoryMock.Setup(repo => repo.GetById(id)).Returns(origin);

            var result = _originService.GetOriginById(id);

            Assert.NotNull(result);
            Assert.Equal(origin.Id, result.Id);
            Assert.Equal(origin.Name, result.Name);
            _repositoryMock.Verify(repo => repo.GetById(id), Times.Once);
        }

        [Fact]
        public void GetOriginById_NonExistingId_ShouldReturnNull()
        {
            var id = Guid.NewGuid();
            _repositoryMock.Setup(repo => repo.GetById(id)).Returns((Origin)null);

            var result = _originService.GetOriginById(id);

            Assert.Null(result);
            _repositoryMock.Verify(repo => repo.GetById(id), Times.Once);
        }

        #endregion

    }
}
