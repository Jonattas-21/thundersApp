using AutoFixture;
using AutoMapper;
using crudApp.Controllers;
using crudApp.Dtos;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace Backend.Tests
{
    public class Tests
    {
        private Mock<IOriginService> _service;
        private Mock<ILogger<OriginController>> _logger;
        private Mock<IMapper> _mapper;
        OriginController _originController;

        //Teste to get by id sucess / not found / get all


        [SetUp]
        public void Setup()
        {
            _service = new Mock<IOriginService>();
            _mapper = new Mock<IMapper>();
            _logger = new Mock<ILogger<OriginController>>();
            _originController = new OriginController(_service.Object, _mapper.Object, _logger.Object);
        }

        [Test]
        public void FindById_Success()
        {
            //Arrange
            bool actionResult200 = false;
            DefaultResponse? defaultResponse = null;
            Fixture fixture = new Fixture();
            Origin origin_entity = fixture.Create<Origin>();
            OriginResponseDto origin_dto = new OriginResponseDto();
            origin_dto.Id = origin_entity.Id;

            _service.Setup(o => o.GetOriginById(origin_entity.Id)).Returns(origin_entity);
            _mapper.Setup(o => o.Map<OriginResponseDto>(It.IsAny<Origin>())).Returns(origin_dto);

            //Act
            var action_result = _originController.FindById(origin_dto.Id);
            if (action_result is OkObjectResult okResult)
            {
                actionResult200 = true;
                defaultResponse = okResult?.Value as DefaultResponse;
            }


            //Assert
            Assert.IsTrue(action_result is OkObjectResult);
            Assert.IsTrue(actionResult200);
            Assert.IsNotNull(defaultResponse);
            Assert.AreEqual("origin was found successfully", defaultResponse.Message);
            Assert.AreEqual(origin_dto.Id, origin_entity.Id);

        }

        [Test]
        public void FindById_Notfound()
        {
            //Arrange
            bool errorStatus = false;
            DefaultResponse defaultResponse = null;
            Guid guid = Guid.NewGuid();

            _service.Setup(o => o.GetOriginById(It.IsAny<Guid>()));

            //Act 
            var action_result = _originController.FindById(guid);

            if (action_result is NotFoundObjectResult objResult)
            {
                errorStatus = true;
                defaultResponse = objResult?.Value as DefaultResponse;
            }

            //Assert
            Assert.True(errorStatus);
            Assert.IsNull(defaultResponse.Data);
            Assert.AreEqual("origin not found for ID: " + guid.ToString(), defaultResponse.Message);

        }

        [Test]
        public void FindAll_Success()
        {
            //Arrange
            bool statusOkResult = false;
            DefaultResponse? defaultResponse = null;
            Fixture builder = new Fixture();
            Origin origin1 = builder.Create<Origin>();
            Origin origin2 = builder.Create<Origin>();
            List<OriginResponseDto>? originList = null;
            List<Origin> originListParam = new List<Origin>() { origin1, origin2 };

            _service.Setup(o => o.GetAll()).Returns(originListParam);

            //act
            var actionResult = _originController.FindById(Guid.Empty);
            if (actionResult is OkObjectResult objResult)
            {
                statusOkResult = true;
                defaultResponse = objResult?.Value as DefaultResponse;
                originList = defaultResponse?.Data as List<OriginResponseDto>;
            }

            //assert
            Assert.True(statusOkResult);
            Assert.True(2 == originList?.Count());
            Assert.AreEqual("origin was found successfully", defaultResponse.Message);

        }

        [TearDown]
        public void TearDown()
        {
            _service = null;
            _mapper = null;
            _logger = null;
            _originController.Dispose();
        }

    }
}