using Domain.Entities;
using Domain.Interfaces;
using Domain.Services;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Tests.ServicesUnitTest
{
    public class WineServicesTests
    {
        private readonly WineServices _wineServices;
        private readonly Mock<IRepository<Wine>> _repositoryMock;
        private readonly Mock<IGrapeService> _grapeServiceMock;
        private readonly Mock<ILogger<WineServices>> _loggerMock;

        public WineServicesTests()
        {
            _repositoryMock = new Mock<IRepository<Wine>>();
            _grapeServiceMock = new Mock<IGrapeService>();
            _loggerMock = new Mock<ILogger<WineServices>>();
            _wineServices = new WineServices(_repositoryMock.Object, _grapeServiceMock.Object, _loggerMock.Object);
        }

        #region  CheckUpdateWineFields

        [Fact]
        public void CheckUpdateWineFields_ValidFields_ShouldUpdateWine()
        {
            var wine = new Wine { Name = "Old Name", Harvest = 2021, Region = "Old Region", Winery = "Old Winery" };
            var fields = new Dictionary<string, string>
            {
                { "Name", "New Name" },
                { "Harvest", "2022" },
                { "Region", "New Region" },
                { "Winery", "New Winery" }
            };

            // Act
            var (validations, updatedWine) = _wineServices.CheckUpdateWineFields(fields, wine);


            // Assert
            Assert.Empty(validations);
            Assert.Equal("New Name", updatedWine.Name);
            Assert.Equal(2022, updatedWine.Harvest);
            Assert.Equal("New Region", updatedWine.Region);
            Assert.Equal("New Winery", updatedWine.Winery);
        }

        [Fact]
        public void CheckUpdateWineFields_InvalidField_ShouldReturnValidationError()
        {
            // Arrange
            var wine = new Wine();
            var fields = new Dictionary<string, string>
        {
            { "InvalidField", "Some Value" }
        };

            // Act
            var (validations, updatedWine) = _wineServices.CheckUpdateWineFields(fields, wine);

            // Assert
            Assert.Single(validations);
            Assert.True(validations.ContainsKey("InvalidField"));
            Assert.Equal("Campo não existe", validations["InvalidField"]);
        }

        [Fact]
        public void CheckUpdateWineFields_InvalidConversion_ShouldReturnError()
        {
            // Arrange
            var wine = new Wine();
            var fields = new Dictionary<string, string>
        {
            { "Harvest", "NotAnInteger" } // Invalid conversion for an integer property
        };

            // Act
            var (validations, updatedWine) = _wineServices.CheckUpdateWineFields(fields, wine);

            // Assert
            Assert.Single(validations);
            Assert.True(validations.ContainsKey("Harvest"));
            Assert.StartsWith("Erro ao atribuir o valor", validations["Harvest"]);
        }

        #endregion

        #region CheckValidWineFields

        [Fact]
        public void CheckValidWineFields_ShouldReturnValidationError_WhenNameIsEmpty()
        {
            // Arrange
            var wine = new Wine { Name = "", Winery = "Some Winery", Harvest = 2022, Region = "Some Region", Grape = new Grape() };

            // Act
            var result = _wineServices.CheckValidWineFields(wine);

            // Assert
            Assert.True(result.ContainsKey("Name"));
            Assert.Equal("Name is required", result["Name"]);
        }

        [Fact]
        public void CheckValidWineFields_ShouldReturnValidationError_WhenWineryIsEmpty()
        {
            // Arrange
            var wine = new Wine { Name = "Wine Name", Winery = "", Harvest = 2022, Region = "Some Region", Grape = new Grape() };

            // Act
            var result = _wineServices.CheckValidWineFields(wine);

            // Assert
            Assert.True(result.ContainsKey("Winery"));
            Assert.Equal("Winery is required", result["Winery"]);
        }

        [Fact]
        public void CheckValidWineFields_ShouldReturnValidationError_WhenHarvestIsInvalid()
        {
            // Arrange
            var wine = new Wine { Name = "Wine Name", Winery = "Some Winery", Harvest = 999, Region = "Some Region", Grape = new Grape() };

            // Act
            var result = _wineServices.CheckValidWineFields(wine);

            // Assert
            Assert.True(result.ContainsKey("Harvest"));
            Assert.Equal("Harvest must be greater than 0 and minor then curent year", result["Harvest"]);
        }

        [Fact]
        public void CheckValidWineFields_ShouldReturnValidationError_WhenRegionIsEmpty()
        {
            // Arrange
            var wine = new Wine { Name = "Wine Name", Winery = "Some Winery", Harvest = 2022, Region = "", Grape = new Grape() };

            // Act
            var result = _wineServices.CheckValidWineFields(wine);

            // Assert
            Assert.True(result.ContainsKey("Region"));
            Assert.Equal("Region is required", result["Region"]);
        }

        [Fact]
        public void CheckValidWineFields_ShouldReturnValidationError_WhenGrapeIsNull()
        {
            // Arrange
            var wine = new Wine { Name = "Wine Name", Winery = "Some Winery", Harvest = 2022, Region = "Some Region", Grape = null };

            // Act
            var result = _wineServices.CheckValidWineFields(wine);

            // Assert
            Assert.True(result.ContainsKey("Grape"));
            Assert.Equal("Grape is required", result["Grape"]);
        }

        [Fact]
        public void CheckValidWineFields_ShouldAddGrapeValidations_WhenGrapeIsValidButHasErrors()
        {
            // Arrange
            var wine = new Wine { Name = "Wine Name", Winery = "Some Winery", Harvest = 2022, Region = "Some Region", Grape = new Grape() };

            var grapeValidations = new Dictionary<string, string>
        {
            { "GrapeType", "Invalid grape type" }
        };

            _grapeServiceMock.Setup(gs => gs.CheckValidGrapeFields(wine.Grape)).Returns(grapeValidations);

            // Act
            var result = _wineServices.CheckValidWineFields(wine);

            // Assert
            Assert.True(result.ContainsKey("GrapeType"));
            Assert.Equal("Invalid grape type", result["GrapeType"]);
        }

        #endregion

        #region EnableDisableWine

        [Fact]
        public void EnableDisableWine_WineExists_ShouldToggleAtivoAndReturnTrue()
        {
            // Arrange
            var wineId = Guid.NewGuid();
            var wine = new Wine { Id = wineId, Ativo = true };

            _repositoryMock.Setup(r => r.GetById(wineId)).Returns(wine);

            // Act
            var result = _wineServices.EnableDisableWine(wineId);

            // Assert
            Assert.True(result);
            Assert.False(wine.Ativo); // Verifica se o campo 'Ativo' foi alterado
            _repositoryMock.Verify(r => r.Update(wine), Times.Once); // Verifica se o método Update foi chamado
        }

        [Fact]
        public void EnableDisableWine_WineDoesNotExist_ShouldReturnFalse()
        {
            // Arrange
            var wineId = Guid.NewGuid();

            _repositoryMock.Setup(r => r.GetById(wineId)).Returns((Wine)null);

            // Act
            var result = _wineServices.EnableDisableWine(wineId);

            // Assert
            Assert.False(result);
            _repositoryMock.Verify(r => r.Update(It.IsAny<Wine>()), Times.Never); // Verifica se o método Update não foi chamado
        }

        #endregion

        #region UpdateWine

        [Fact]
        public void UpdateWine_WineExistsAndNoValidationErrors_ShouldUpdateWine()
        {
            // Arrange
            var wineId = Guid.NewGuid();
            var wine = new Wine { Id = wineId, Name = "Old Wine" };
            var fields = new Dictionary<string, string> { { "Name", "New Wine" } };

            _repositoryMock.Setup(r => r.GetById(wineId)).Returns(wine);
            _repositoryMock.Setup(r => r.Update(wine)).Verifiable();
            _wineServices.CheckUpdateWineFields(fields, wine); // Assuming CheckUpdateWineFields method is called internally

            // Act
            var (updatedWine, errors) = _wineServices.UpdateWine(wineId, fields);

            // Assert
            Assert.NotNull(updatedWine);
            Assert.Empty(errors);
            Assert.Equal("New Wine", updatedWine.Name);
            _repositoryMock.Verify(r => r.Update(wine), Times.Once); // Verifica se o método Update foi chamado
        }

        [Fact]
        public void UpdateWine_WineExistsWithValidationErrors_ShouldNotUpdateWine()
        {
            var wineId = Guid.NewGuid();
            var wine = new Wine { Id = wineId, Name = "Old Wine" };
            var fields = new Dictionary<string, string> { { "InvalidField", "InvalidValue" } };
            var validationErrors = new Dictionary<string, string> { { "InvalidField", "Field does not exist" } };

            _repositoryMock.Setup(r => r.GetById(wineId)).Returns(wine);
            _grapeServiceMock.Setup(gs => gs.CheckValidGrapeFields(wine.Grape)).Returns(new Dictionary<string, string>()); 
            _wineServices.CheckUpdateWineFields(fields, wine);

            var (updatedWine, errors) = _wineServices.UpdateWine(wineId, fields);

            Assert.Equal(validationErrors, errors);
            _repositoryMock.Verify(r => r.Update(wine), Times.Never);
        }

        [Fact]
        public void UpdateWine_WineDoesNotExist_ShouldReturnError()
        {
            // Arrange
            var wineId = Guid.NewGuid();
            var fields = new Dictionary<string, string> { { "Name", "New Wine" } };

            _repositoryMock.Setup(r => r.GetById(wineId)).Returns((Wine)null);

            // Act
            var (updatedWine, errors) = _wineServices.UpdateWine(wineId, fields);

            // Assert
            Assert.Null(updatedWine);
            Assert.Single(errors);
            Assert.Equal("Wine not found", errors["Wine"]);
        }

        #endregion

        #region GetWineById

        [Fact]
        public void GetWineById_WineExists_ShouldReturnWine()
        {
            // Arrange
            var wineId = Guid.NewGuid();
            var expectedWine = new Wine { Id = wineId, Name = "Chardonnay" };

            _repositoryMock.Setup(r => r.GetById(wineId)).Returns(expectedWine);

            // Act
            var result = _wineServices.GetWineById(wineId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedWine, result);
        }

        [Fact]
        public void GetWineById_WineDoesNotExist_ShouldReturnNull()
        {
            // Arrange
            var wineId = Guid.NewGuid();

            _repositoryMock.Setup(r => r.GetById(wineId)).Returns((Wine)null);

            // Act
            var result = _wineServices.GetWineById(wineId);

            // Assert
            Assert.Null(result);
        }

        #endregion

        #region DeleteWine

        [Fact]
        public void DeleteWine_WineExists_ShouldReturnTrue()
        {
            // Arrange
            var wineId = Guid.NewGuid();

            // Act
            var result = _wineServices.DeleteWine(wineId);

            // Assert
            Assert.True(result);
            _repositoryMock.Verify(r => r.DeleteById(wineId), Times.Once);
        }

        #endregion

        #region CreateWine

        [Fact]
        public void CreateWine_ValidWine_ShouldReturnSavedWine()
        {
            // Arrange
            var wine = new Wine
            {
                Name = "Test Wine",
                Winery = "Test Winery",
                Harvest = DateTime.Now.Year,
                Region = "Test Region",
                Grape = new Grape { Name = "Test Grape" }
            };

            var grapeData = new Grape { Id = Guid.NewGuid(), Name = "Test Grape", Ativo = true };
            var savedWine = new Wine
            {
                Id = Guid.NewGuid(),
                Name = "Test Wine",
                Winery = "Test Winery",
                Harvest = DateTime.Now.Year,
                Region = "Test Region",
                Grape = grapeData,
                Ativo = true
            };

            _grapeServiceMock.Setup(g => g.GetGrapeByName(wine.Grape.Name)).Returns(grapeData);
            _grapeServiceMock.Setup(g => g.CheckValidGrapeFields(wine.Grape)).Returns(new Dictionary<string, string>());
            _repositoryMock.Setup(r => r.Create(wine)).Returns(savedWine);

            var (resultWine, validations) = _wineServices.CreateWine(wine);

            Assert.NotNull(resultWine);
            Assert.Empty(validations);
            Assert.Equal(savedWine.Id, resultWine.Id);
            _repositoryMock.Verify(r => r.Create(wine), Times.Once);
            _grapeServiceMock.Verify(g => g.GetGrapeByName(wine.Grape.Name), Times.Once);
        }

        [Fact]
        public void CreateWine_ExceptionThrown_ShouldReturnError()
        {
            // Arrange
            var wine = new Wine
            {
                Name = "Test Wine",
                Winery = "Test Winery",
                Harvest = DateTime.Now.Year,
                Region = "Test Region",
                Grape = new Grape { Name = "Test Grape" }
            };

            _grapeServiceMock.Setup(g => g.GetGrapeByName(wine.Grape.Name)).Returns((Grape)null);
            _grapeServiceMock.Setup(g => g.CheckValidGrapeFields(wine.Grape)).Returns(new Dictionary<string, string>());
            _repositoryMock.Setup(r => r.Create(wine)).Throws(new Exception("Test exception"));

            // Act
            var (resultWine, validations) = _wineServices.CreateWine(wine);

            // Assert
            Assert.Null(resultWine);
            Assert.Contains("Error", validations);
            Assert.Contains("Internal error while adding wine", validations["Error"]);
            _repositoryMock.Verify(r => r.Create(wine), Times.Once);
        }

        #endregion
    }
}
