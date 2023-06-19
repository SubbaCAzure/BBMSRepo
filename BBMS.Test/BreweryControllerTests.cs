using AutoMapper;
using BBMS.API.Controllers;
using BBMS.Domain.Models;
using BBMS.Services.DTO;
using BBMS.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace BBMS.Test
{
    public class BreweryControllerTests
    {
        private readonly Mock<IBreweryService> _mockBreweryService;
        private readonly Mock<ILogger<BreweryController>> _mockLogger;
        private readonly Mock<IMapper> _mockMapper;
        private readonly BreweryController _controller;

        public BreweryControllerTests()
        {
            _mockBreweryService = new Mock<IBreweryService>();
            _mockLogger = new Mock<ILogger<BreweryController>>();
            _mockMapper = new Mock<IMapper>();

            _controller = new BreweryController(_mockBreweryService.Object, _mockLogger.Object, _mockMapper.Object);
        }

        [Fact]
        public async Task PostBrewery_ReturnsCreatedAtAction_WithAddedBrewery()
        {
            // Arrange
            var breweryDto = new BreweryDTO { Name = "Test Brewery" };
            var addedBrewery = new Brewery { Name = "Test Brewery" };
            var addedBreweryDto = new BreweryDTO { Name = "Test Brewery" };

            _mockMapper.Setup(m => m.Map<Brewery>(It.IsAny<BreweryDTO>())).Returns(addedBrewery);
            _mockBreweryService.Setup(s => s.AddBreweryAsync(addedBrewery)).ReturnsAsync(addedBrewery);
            _mockMapper.Setup(m => m.Map<BreweryDTO>(addedBrewery)).Returns(addedBreweryDto);
          
            var result = await _controller.PostBrewery(breweryDto);          
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
            Assert.Equal(nameof(BreweryController.GetBrewery), createdAtActionResult.ActionName);
            Assert.Equal(addedBreweryDto, createdAtActionResult.Value);
            Assert.Equal(StatusCodes.Status201Created, createdAtActionResult.StatusCode);
        }

        [Fact]
        public async Task PostBrewery_ReturnsStatusCode500_WhenErrorOccurs()
        {
           
            var breweryDto = new BreweryDTO { Name = "Test Brewery" };
            var errorMessage = "An error occurred";

            _mockMapper.Setup(m => m.Map<Brewery>(It.IsAny<BreweryDTO>())).Throws(new Exception(errorMessage));
            
            var result = await _controller.PostBrewery(breweryDto);
            
            var statusCodeResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(StatusCodes.Status500InternalServerError, statusCodeResult.StatusCode);
            
        }

        [Fact]
        public async Task UpdateBrewery_ReturnsNotFound_WhenBreweryDoesNotExist()
        {
            // Arrange
            var id = 1;
            var breweryDto = new BreweryUpdateDTO { Name = "Updated Brewery" };

            _mockBreweryService.Setup(s => s.GetBreweryAsync(id)).ReturnsAsync((BreweryDTO)null);

            // Act
            var result = await _controller.UpdateBrewery(id, breweryDto);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

      
        [Fact]
        public async Task UpdateBrewery_ReturnsStatusCode500_WhenErrorOccurs()
        {
            // Arrange
            var id = 1;
            var breweryDto = new BreweryUpdateDTO { Name = "Updated Brewery" };
            var errorMessage = "An error occurred";

            _mockBreweryService.Setup(s => s.GetBreweryAsync(id)).Throws(new Exception(errorMessage));

            // Act
            var result = await _controller.UpdateBrewery(id, breweryDto);

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(StatusCodes.Status500InternalServerError, statusCodeResult.StatusCode);
            
        }
       
        [Fact]
        public async Task GetBrewery_ReturnsOkResult_WithBreweryDTO()
        {
            // Arrange
            var id = 1;
            var breweryDto = new BreweryDTO {  Name = "Brewery 1" };

            _mockBreweryService.Setup(s => s.GetBreweryAsync(id)).ReturnsAsync(breweryDto);

            // Act
            var result = await _controller.GetBrewery(id);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedBrewery = Assert.IsAssignableFrom<BreweryDTO>(okResult.Value);
            Assert.Equal(breweryDto, returnedBrewery);
        }

        [Fact]
        public async Task GetBrewery_ReturnsNotFound_WhenBreweryDoesNotExist()
        {
            // Arrange
            var id = 1;
            _mockBreweryService.Setup(s => s.GetBreweryAsync(id)).ReturnsAsync((BreweryDTO)null);

            // Act
            var result = await _controller.GetBrewery(id);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task GetBrewery_ReturnsStatusCode500_WhenErrorOccurs()
        {            
            var id = 1;
            var errorMessage = "An error occurred";

            _mockBreweryService.Setup(s => s.GetBreweryAsync(id)).Throws(new Exception(errorMessage));
                        
            var result = await _controller.GetBrewery(id);         
            var statusCodeResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(StatusCodes.Status500InternalServerError, statusCodeResult.StatusCode);
         
        }
    }
}
