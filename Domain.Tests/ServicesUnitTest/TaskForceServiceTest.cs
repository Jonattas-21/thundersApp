using Xunit;
using Moq;
using Microsoft.Extensions.Logging;
using Domain.Interfaces;
using Domain.Entities;
using Domain.Services;
using Domain.Enums;

namespace Tests.ServicesUnitTest
{
    public class TaskForceServiceTest
    {
        private readonly Mock<IRepository<TaskForce>> _repositoryMock;
        private readonly Mock<IOriginService> _originServiceMock;
        private readonly Mock<ILogger<TaskForceService>> _loggerMock;
        private readonly TaskForceService _taskForceService;

        public TaskForceServiceTest()
        {
            _repositoryMock = new Mock<IRepository<TaskForce>>();
            _originServiceMock = new Mock<IOriginService>();
            _loggerMock = new Mock<ILogger<TaskForceService>>();
            _taskForceService = new TaskForceService(_repositoryMock.Object, _originServiceMock.Object, _loggerMock.Object);
        }

        #region CheckValidTaskForceFields

        [Fact]
        public void CheckValidTaskForceFields_ValidTaskForce_ShouldReturnEmptyValidationDictionary()
        {
            var taskForce = new TaskForce
            {
                Name = "Test Task",
                Description = "Test Description",
                Priority = 3,
                Assignee = "John Doe"
            };

            var result = _taskForceService.CheckValidTaskForceFields(taskForce);

            Assert.Empty(result);
        }

        [Fact]
        public void CheckValidTaskForceFields_InvalidTaskForce_ShouldReturnValidationErrors()
        {
            var taskForce = new TaskForce
            {
                Name = "",
                Description = "",
                Priority = 6,
                Assignee = ""
            };

            var result = _taskForceService.CheckValidTaskForceFields(taskForce);

            Assert.Contains("Name", result.Keys);
            Assert.Contains("Description", result.Keys);
            Assert.Contains("Priority", result.Keys);
            Assert.Contains("Assignee", result.Keys);
            Assert.Equal("Name is required", result["Name"]);
            Assert.Equal("Description is required", result["Description"]);
            Assert.Equal("HarvPriorityest must be greater than 0 and minor then 5", result["Priority"]);
            Assert.Equal("Assignee is required", result["Assignee"]);
        }

        #endregion

        #region CreateTask

        [Fact]
        public void CreateTask_ValidData_ShouldCreateTask()
        {
            var originId = Guid.NewGuid();
            var origin = new Origin { Id = originId, Name = "Test Origin" };
            var taskForce = new TaskForce { Name = "Test Task", Description = "Test Description", Priority = 3, Assignee = "John Doe" };
            _originServiceMock.Setup(service => service.GetOriginById(originId)).Returns(origin);
            _repositoryMock.Setup(repo => repo.Create(It.IsAny<TaskForce>())).Returns(taskForce);

            var result = _taskForceService.CreateTask(taskForce, originId);

            Assert.NotNull(result.Item1);
            Assert.Empty(result.Item2);
            Assert.Equal(taskForce, result.Item1);
            Assert.Equal(origin, result.Item1.Origin);
            _repositoryMock.Verify(repo => repo.Create(taskForce), Times.Once);
        }

        [Fact]
        public void CreateTask_InvalidOrigin_ShouldReturnValidationErrors()
        {
            // Arrange
            var originId = Guid.NewGuid();
            var taskForce = new TaskForce { Name = "Test Task", Description = "Test Description", Priority = 3, Assignee = "John Doe" };
            _originServiceMock.Setup(service => service.GetOriginById(originId)).Returns((Origin)null);

            // Act
            var result = _taskForceService.CreateTask(taskForce, originId);

            // Assert
            Assert.Null(result.Item1);
            Assert.Contains("Origin", result.Item2.Keys);
            Assert.Equal("Origin not found", result.Item2["Origin"]);
        }

        #endregion

        #region Delete

        [Fact]
        public void Delete_ValidId_ShouldReturnTrue()
        {
            // Arrange
            var id = Guid.NewGuid();
            var item = new TaskForce { Id = id, Name = "Test" };
            _repositoryMock.Setup(repo => repo.GetById(id)).Returns(item);

            _repositoryMock.Setup(repo => repo.DeleteById(id)).Verifiable();

            // Act
            var result = _taskForceService.Delete(id);

            // Assert
            Assert.True(result);
            _repositoryMock.Verify(repo => repo.DeleteById(id), Times.Once);
        }

        [Fact]
        public void Delete_ExceptionThrown_ShouldReturnFalse()
        {
            // Arrange
            var id = Guid.NewGuid();
            _repositoryMock.Setup(repo => repo.DeleteById(id)).Throws(new Exception());

            // Act
            var result = _taskForceService.Delete(id);

            // Assert
            Assert.False(result);
            _repositoryMock.Verify(repo => repo.DeleteById(id), Times.Never);
        }

        #endregion

        #region GetByPriority

        [Fact]
        public void GetByPriority_NoPriority_ShouldReturnAllTasks()
        {
            var tasks = new List<TaskForce>
        {
            new TaskForce { Priority = 3 },
            new TaskForce { Priority = 2 }
        };
            _repositoryMock.Setup(repo => repo.GetAll()).Returns(tasks.AsQueryable());

            var result = _taskForceService.GetByPriority(null);

            Assert.Equal(tasks.Count, result.Count());
            Assert.Equal(tasks, result);
        }

        #endregion

        #region GetTaskById

        [Fact]
        public void GetTaskById_ValidId_ShouldReturnTask()
        {
            // Arrange
            var id = Guid.NewGuid();
            var task = new TaskForce { Id = id };
            _repositoryMock.Setup(repo => repo.GetById(id)).Returns(task);

            // Act
            var result = _taskForceService.GetTaskById(id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(id, result.Id);
        }

        [Fact]
        public void GetTaskById_InvalidId_ShouldReturnNull()
        {
            // Arrange
            var id = Guid.NewGuid();
            _repositoryMock.Setup(repo => repo.GetById(id)).Returns((TaskForce)null);

            // Act
            var result = _taskForceService.GetTaskById(id);

            // Assert
            Assert.Null(result);
        }

        #endregion

        #region UpdateStatus

        [Fact]
        public void UpdateStatus_ValidId_ShouldUpdateStatus()
        {
            // Arrange
            var id = Guid.NewGuid();
            var task = new TaskForce { Id = id, Status = (int)StatusEnum.Todo };
            var newStatus = StatusEnum.InProgress;
            _repositoryMock.Setup(repo => repo.GetById(id)).Returns(task);
            _repositoryMock.Setup(repo => repo.Update(It.IsAny<TaskForce>())).Verifiable();

            // Act
            var result = _taskForceService.UpdateStatus(id, newStatus);

            // Assert
            Assert.True(result);
            Assert.Equal((int)newStatus, task.Status);
            _repositoryMock.Verify(repo => repo.Update(task), Times.Once);
        }

        [Fact]
        public void UpdateStatus_TaskNotFound_ShouldReturnFalse()
        {
            // Arrange
            var id = Guid.NewGuid();
            var newStatus = StatusEnum.InProgress;
            _repositoryMock.Setup(repo => repo.GetById(id)).Returns((TaskForce)null);

            // Act
            var result = _taskForceService.UpdateStatus(id, newStatus);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void UpdateStatus_ExceptionThrown_ShouldThrowException()
        {
            // Arrange
            var id = Guid.NewGuid();
            var newStatus = StatusEnum.InProgress;
            _repositoryMock.Setup(repo => repo.GetById(id)).Returns(new TaskForce { Id = id });
            _repositoryMock.Setup(repo => repo.Update(It.IsAny<TaskForce>())).Throws(new Exception());

            // Act & Assert
            Assert.Throws<Exception>(() => _taskForceService.UpdateStatus(id, newStatus));
        }

        #endregion
    }
}
