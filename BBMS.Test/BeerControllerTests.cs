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
    public class BeerControllerTests
    {
        [Fact]
        public async Task PostBeer_ReturnsCreatedAtAction_WithValidBeerDto()
        {
            // Arrange
            var mockBeerService = new Mock<IBeerService>();
            var mockLogger = new Mock<ILogger<BeerController>>();
            var mockMapper = new Mock<IMapper>();
            var beerDto = new BeerDTO { Name = "Test Beer", PercentageAlcoholByVolume = 5.5M };
            var addedBeer = new Beer { Id = 1, Name = "Test Beer", PercentageAlcoholByVolume = 5.5M };
            var addedBeerDto = new BeerDTO { Id = 1, Name = "Test Beer", PercentageAlcoholByVolume = 5.5M };


            mockMapper.Setup(m => m.Map<Beer>(It.IsAny<BeerDTO>())).Returns(addedBeer);
            mockBeerService.Setup(s => s.AddBeerAsync(It.IsAny<Beer>())).ReturnsAsync(addedBeer);
            mockMapper.Setup(m => m.Map<BeerDTO>(It.IsAny<Beer>())).Returns(addedBeerDto);

            var controller = new BeerController(mockBeerService.Object, mockLogger.Object, mockMapper.Object);

            // Act
            var result = await controller.PostBeer(beerDto);

            // Assert
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
            Assert.Equal(nameof(BeerController.GetBeer), createdAtActionResult.ActionName);
            Assert.Equal(addedBeerDto.Id, createdAtActionResult.RouteValues["id"]);
            Assert.Equal(addedBeerDto, createdAtActionResult.Value);
        }

        [Fact]
        public async Task PostBeer_ReturnsInternalServerError_WhenExceptionOccurs()
        {            
            var mockBeerService = new Mock<IBeerService>();
            var mockLogger = new Mock<ILogger<BeerController>>();
            var mockMapper = new Mock<IMapper>();
            mockBeerService.Setup(s => s.AddBeerAsync(It.IsAny<Beer>())).ThrowsAsync(new Exception("Test exception"));
            var controller = new BeerController(mockBeerService.Object, mockLogger.Object, mockMapper.Object);
         
            var result = await controller.PostBeer(new BeerDTO { Name = "Test Beer", PercentageAlcoholByVolume = 5.5M });           
            var statusCodeResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(StatusCodes.Status500InternalServerError, statusCodeResult.StatusCode);
           
        }
    }
}
