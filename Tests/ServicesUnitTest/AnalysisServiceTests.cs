using System;
using System.Collections.Generic;
using Xunit;
using Moq;
using Microsoft.Extensions.Logging;
using Domain.Services;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Tests.ServicesUnitTest
{
    public class AnalysisServiceTests
    {
        private readonly AnalysisServices _analysisServices;
        private readonly Mock<IRepository<Analysis>> _repositoryMock;
        private readonly Mock<ILogger<AnalysisServices>> _loggerMock;

        public AnalysisServiceTests()
        {
            _repositoryMock = new Mock<IRepository<Analysis>>();
            _loggerMock = new Mock<ILogger<AnalysisServices>>();
            _analysisServices = new AnalysisServices(_repositoryMock.Object, _loggerMock.Object);
        }

        #region CheckValidAnalysisFields

        [Fact]
        public void CheckValidAnalysisFields_ValidAnalysis_ShouldReturnEmptyDictionary()
        {
            // Arrange
            var analysis = new Analysis
            {
                Sweet = 3,
                Tannin = 4,
                Acidity = 2,
                Body = 5,
                Alcohol = 1
            };

            // Act
            var result = _analysisServices.CheckValidAnalysisFields(analysis);

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public void CheckValidAnalysisFields_InvalidSweet_ShouldReturnValidationError()
        {
            // Arrange
            var analysis = new Analysis
            {
                Sweet = 6, // Fora do intervalo permitido
                Tannin = 3,
                Acidity = 3,
                Body = 3,
                Alcohol = 3
            };

            // Act
            var result = _analysisServices.CheckValidAnalysisFields(analysis);

            // Assert
            Assert.Contains("Sweet", result.Keys);
            Assert.Equal("Sweetness must be between 1 and 5", result["Sweet"]);
        }

        [Fact]
        public void CheckValidAnalysisFields_InvalidTannin_ShouldReturnValidationError()
        {
            // Arrange
            var analysis = new Analysis
            {
                Sweet = 3,
                Tannin = 0, // Fora do intervalo permitido
                Acidity = 3,
                Body = 3,
                Alcohol = 3
            };

            // Act
            var result = _analysisServices.CheckValidAnalysisFields(analysis);

            // Assert
            Assert.Contains("Tannin", result.Keys);
            Assert.Equal("Tannin must be between 1 and 5", result["Tannin"]);
        }

        [Fact]
        public void CheckValidAnalysisFields_ExceptionThrown_ShouldReturnError()
        {
            // Arrange
            var analysis = new Analysis
            {
                Sweet = 3,
                Tannin = 3,
                Acidity = 3,
                Body = 3,
                Alcohol = 3
            };

            // Forçar uma exceção
            _loggerMock.Setup(l => l.LogError(It.IsAny<Exception>(), It.IsAny<string>(), analysis))
                       .Callback((Exception ex, string message, Analysis a) =>
                       {
                           throw new Exception("Logging error");
                       });

            // Act
            var result = _analysisServices.CheckValidAnalysisFields(analysis);

            // Assert
            Assert.Contains("Error", result.Keys);
            Assert.Equal("Internal error while validating wine", result["Error"]);
        }

        #endregion

        #region CreateAnalysis

        [Fact]
        public void CreateAnalysis_ValidAnalysis_ShouldReturnSavedAnalysis()
        {
            // Arrange
            var analysis = new Analysis
            {
                Sweet = 3,
                Tannin = 4,
                Acidity = 2,
                Body = 5,
                Alcohol = 3
            };
            var savedAnalysis = new Analysis
            {
                Id = Guid.NewGuid(),
                Sweet = analysis.Sweet,
                Tannin = analysis.Tannin,
                Acidity = analysis.Acidity,
                Body = analysis.Body,
                Alcohol = analysis.Alcohol
            };

            _repositoryMock.Setup(repo => repo.Create(It.IsAny<Analysis>())).Returns(savedAnalysis);

            // Act
            var (result, validations) = _analysisServices.CreateAnalysis(analysis);

            // Assert
            Assert.Equal(savedAnalysis, result);
            Assert.Empty(validations);
        }

        [Fact]
        public void CreateAnalysis_InvalidFields_ShouldReturnValidationErrors()
        {
            // Arrange
            var analysis = new Analysis
            {
                Sweet = 6, // Invalid value
                Tannin = 4,
                Acidity = 2,
                Body = 5,
                Alcohol = 3
            };

            // Act
            var (result, validations) = _analysisServices.CreateAnalysis(analysis);

            // Assert
            Assert.Equal(analysis, result);
            Assert.Contains("Sweet", validations);
            Assert.Equal("Sweetness must be between 1 and 5", validations["Sweet"]);
        }

        [Fact]
        public void CreateAnalysis_DbUpdateException_ShouldReturnErrorForDuplicate()
        {
            // Arrange
            var analysis = new Analysis
            {
                Sweet = 3,
                Tannin = 4,
                Acidity = 2,
                Body = 5,
                Alcohol = 3
            };

            _repositoryMock.Setup(repo => repo.Create(It.IsAny<Analysis>()))
                .Throws(new DbUpdateException("Simulated duplicate"));

            // Act
            var (result, validations) = _analysisServices.CreateAnalysis(analysis);

            // Assert
            Assert.Equal(analysis, result);
            Assert.Contains("Error", validations);
            Assert.Equal("Error adding analysis, already exist analysis for this wine", validations["Error"]);
        }

        [Fact]
        public void CreateAnalysis_Exception_ShouldReturnInternalError()
        {
            // Arrange
            var analysis = new Analysis
            {
                Sweet = 3,
                Tannin = 4,
                Acidity = 2,
                Body = 5,
                Alcohol = 3
            };

            _repositoryMock.Setup(repo => repo.Create(It.IsAny<Analysis>()))
                .Throws(new Exception("Simulated exception"));

            // Act
            var (result, validations) = _analysisServices.CreateAnalysis(analysis);

            // Assert
            Assert.Equal(analysis, result);
            Assert.Contains("Error", validations);
            Assert.Equal("Internal error while adding analysis", validations["Error"]);
        }

        #endregion

        #region DeleteAnalysis

        [Fact]
        public void DeleteAnalysis_ValidId_ShouldReturnTrue()
        {
            // Arrange
            var id = Guid.NewGuid();

            // Act
            var result = _analysisServices.DeleteAnalysis(id);

            // Assert
            _repositoryMock.Verify(repo => repo.DeleteById(id), Times.Once);
            Assert.True(result);
        }

        [Fact]
        public void DeleteAnalysis_ExceptionThrown_ShouldReturnFalse()
        {
            // Arrange
            var id = Guid.NewGuid();
            _repositoryMock.Setup(repo => repo.DeleteById(id))
                .Throws(new Exception("Simulated exception"));

            // Act
            var result = _analysisServices.DeleteAnalysis(id);

            // Assert
            _repositoryMock.Verify(repo => repo.DeleteById(id), Times.Once);
            Assert.False(result);
        }

        #endregion

        #region GetAnalysesByCategories

        [Fact]
        public void GetAnalysesByCategories_WithAllParameters_ShouldApplyFiltersCorrectly()
        {
            // Arrange
            var sweet = 3;
            var tannin = 2;
            var acidity = 4;
            var alcohol = 5;
            var body = 1;

            var analyses = new List<Analysis>
        {
            new Analysis { Sweet = 3, Tannin = 2, Acidity = 4, Alcohol = 5, Body = 1 },
            new Analysis { Sweet = 1, Tannin = 3, Acidity = 2, Alcohol = 4, Body = 5 }
        }.AsQueryable();

            _repositoryMock.Setup(repo => repo.GetByQuery(It.IsAny<Expression<Func<Analysis, bool>>>()))
                .Returns((Expression<Func<Analysis, bool>> filter) =>
                {
                    return analyses.Where(filter.Compile()).ToList();
                });

            // Act
            var result = _analysisServices.GetAnalysesByCategories(sweet, tannin, acidity, alcohol, body);

            // Assert
            Assert.Single(result);
            var resultList = result.ToList();
            Assert.Equal(3, resultList[0].Sweet);
            Assert.Equal(2, resultList[0].Tannin);
            Assert.Equal(4, resultList[0].Acidity);
            Assert.Equal(5, resultList[0].Alcohol);
            Assert.Equal(1, resultList[0].Body);
        }

        [Fact]
        public void GetAnalysesByCategories_WithSomeParameters_ShouldApplyRelevantFilters()
        {
            // Arrange
            var sweet = (int?)null;
            var tannin = 2;
            var acidity = (int?)null;
            var alcohol = (int?)null;
            var body = (int?)null;

            var analyses = new List<Analysis>
        {
            new Analysis { Sweet = 3, Tannin = 2, Acidity = 4, Alcohol = 5, Body = 1 },
            new Analysis { Sweet = 1, Tannin = 2, Acidity = 2, Alcohol = 4, Body = 5 }
        }.AsQueryable();

            _repositoryMock.Setup(repo => repo.GetByQuery(It.IsAny<Expression<Func<Analysis, bool>>>()))
                .Returns((Expression<Func<Analysis, bool>> filter) =>
                {
                    return analyses.Where(filter.Compile()).ToList();
                });

            // Act
            var result = _analysisServices.GetAnalysesByCategories(sweet, tannin, acidity, alcohol, body);

            // Assert
            Assert.Equal(2, result.Count());
            Assert.All(result, analysis => Assert.Equal(2, analysis.Tannin));
        }

        [Fact]
        public void GetAnalysesByCategories_WhenExceptionThrown_ShouldReturnNull()
        {
            // Arrange
            var sweet = 3;
            var tannin = 2;
            var acidity = 4;
            var alcohol = 5;
            var body = 1;

            _repositoryMock.Setup(repo => repo.GetByQuery(It.IsAny<Expression<Func<Analysis, bool>>>()))
                .Throws(new Exception("Simulated exception"));

            // Act
            var result = _analysisServices.GetAnalysesByCategories(sweet, tannin, acidity, alcohol, body);

            // Assert
            Assert.Null(result);
            _loggerMock.Verify(logger => logger.LogError(It.IsAny<Exception>(), "Error getting analysis by category"), Times.Once);
        }


        #endregion

        #region GetAnalysisByWine

        [Fact]
        public void GetAnalysisByWine_WhenAnalysisExists_ShouldReturnAnalysis()
        {
            // Arrange
            var wineId = Guid.NewGuid();
            var analysis = new Analysis { Wine = new Wine { Id = wineId } };
            var analyses = new List<Analysis> { analysis }.AsQueryable();

            _repositoryMock.Setup(repo => repo.GetByQuery(It.IsAny<Expression<Func<Analysis, bool>>>()))
                .Returns((Expression<Func<Analysis, bool>> filter) =>
                {
                    return analyses.Where(filter.Compile()).ToList();
                });

            // Act
            var result = _analysisServices.GetAnalysisByWine(wineId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(wineId, result.Wine.Id);
        }

        [Fact]
        public void GetAnalysisByWine_WhenAnalysisDoesNotExist_ShouldReturnNull()
        {
            // Arrange
            var wineId = Guid.NewGuid();
            var analyses = new List<Analysis>().AsQueryable();

            _repositoryMock.Setup(repo => repo.GetByQuery(It.IsAny<Expression<Func<Analysis, bool>>>()))
                .Returns((Expression<Func<Analysis, bool>> filter) =>
                {
                    return analyses.Where(filter.Compile()).ToList();
                });

            // Act
            var result = _analysisServices.GetAnalysisByWine(wineId);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void GetAnalysisByWine_WhenExceptionThrown_ShouldReturnNull()
        {
            // Arrange
            var wineId = Guid.NewGuid();
            _repositoryMock.Setup(repo => repo.GetByQuery(It.IsAny<Expression<Func<Analysis, bool>>>()))
                .Throws(new Exception("Simulated exception"));

            // Act
            var result = _analysisServices.GetAnalysisByWine(wineId);

            // Assert
            Assert.Null(result);
            _loggerMock.Verify(logger => logger.LogError(It.IsAny<Exception>(), "Error getting analysis by wine with id {id}", wineId), Times.Once);
        }

        #endregion
    }
}
