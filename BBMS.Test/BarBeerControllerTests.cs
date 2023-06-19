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
    public class BarBeerControllerTests
    {
        private readonly Mock<IBarBeerLinkService> _mockBarBeersLinkService;
        private readonly Mock<IBarService> _mockBarService;
        private readonly Mock<IBeerService> _mockBeerService;
        private readonly Mock<ILogger<BarController>> _mockLogger;
        private readonly Mock<IMapper> _mockMapper;
        private readonly BarBeerController _controller;

        public BarBeerControllerTests()
        {
            _mockBarBeersLinkService = new Mock<IBarBeerLinkService>();
            _mockBarService = new Mock<IBarService>();
            _mockBeerService = new Mock<IBeerService>();
            _mockLogger = new Mock<ILogger<BarController>>();
            _mockMapper = new Mock<IMapper>();
            _controller = new BarBeerController(
                _mockBarBeersLinkService.Object,
                _mockLogger.Object,
                _mockMapper.Object,
                _mockBarService.Object,
                _mockBeerService.Object
            );
        }

        [Fact]
        public async Task InsertBarBeerLink_ReturnsOkResult_WhenBarBeerLinkIsInserted()
        {
            var request = new BarBeerLinkRequest { BarId = 1, BeerId = 1 };
            var result = await _controller.InsertBarBeerLink(request);
            var okResult = Assert.IsType<OkResult>(result);
        }

        [Fact]
        public async Task InsertBarBeerLink_ReturnsStatusCode500_WhenErrorOccurs()
        {
            // Arrange
            var request = new BarBeerLinkRequest { BarId = 1, BeerId = 1 };
            var errorMessage = "An error occurred";
            _mockBarBeersLinkService.Setup(s => s.InsertBarBeerLinkAsync(request.BarId, request.BeerId)).Throws(new Exception(errorMessage));

            // Act
            var result = await _controller.InsertBarBeerLink(request);

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(StatusCodes.Status500InternalServerError, statusCodeResult.StatusCode);
            Assert.Equal("An error occurred while inserting the Bar-Beer link. Please try again later.", statusCodeResult.Value);
        }


        [Fact]
        public async Task GetBarWithAssociatedBeers_ReturnsNotFound_WhenBarDoesNotExist()
        {
          
            var barId = 1;
            _mockBarService.Setup(s => s.GetBarFromDatabase(barId)).ReturnsAsync((BarDTO)null); 
            var result = await _controller.GetBarWithAssociatedBeers(barId); 
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("Bar not found", notFoundResult.Value);
        }

        [Fact]
        public async Task GetBarWithAssociatedBeers_ReturnsNotFound_WhenNoBeersFoundInBar()
        {
            
            var barId = 1;
            var bar = new Bar { Id = barId, Name = "Bar 1", Address = "123 Street" };

            _mockBarBeersLinkService.Setup(s => s.GetBeerIdsFromDatabase(barId)).ReturnsAsync(new List<int>()); 
            var result = await _controller.GetBarWithAssociatedBeers(barId);        
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);

        }

        [Fact]
        public async Task GetBarWithAssociatedBeers_ReturnsStatusCode500_WhenErrorOccurs()
        {
            // Arrange
            var barId = 1;
            var errorMessage = "An error occurred";
            _mockBarService.Setup(s => s.GetBarFromDatabase(barId)).Throws(new Exception(errorMessage));

            // Act
            var result = await _controller.GetBarWithAssociatedBeers(barId);

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(StatusCodes.Status500InternalServerError, statusCodeResult.StatusCode);
            Assert.Equal($"An error occurred: {errorMessage}", statusCodeResult.Value);
        }

        [Fact]
        public async Task GetAllBarsWithAssociatedBeers_ReturnsOkResult_WithAllBarsAndAssociatedBeers()
        {
            // Arrange
            var bars = new List<Bar>
        {
            new Bar { Id = 1, Name = "Bar 1", Address = "123 Street" },
            new Bar { Id = 2, Name = "Bar 2", Address = "456 Street" }
        };
            var bar1BeerIds = new List<int> { 1, 2 };
            var bar2BeerIds = new List<int> { 3, 4 };
            var bar1Beers = new List<Beer>
        {
            new Beer { Id = 1, Name = "Beer 1" },
            new Beer { Id = 2, Name = "Beer 2" }
        };
            var bar2Beers = new List<Beer>
        {
            new Beer { Id = 3, Name = "Beer 3" },
            new Beer { Id = 4, Name = "Beer 4" }
        };
            var expectedResponse = new List<BarWithBeersResponse>
        {
            new BarWithBeersResponse
            {
                BarId = bars[0].Id,
                Name = bars[0].Name,
                Address = bars[0].Address,
                Beers = bar1Beers
            },
            new BarWithBeersResponse
            {
                BarId = bars[1].Id,
                Name = bars[1].Name,
                Address = bars[1].Address,
                Beers = bar2Beers
            }
        };
            _mockBarService.Setup(s => s.GetAllBarsAsync()).ReturnsAsync(bars);
            _mockBarBeersLinkService.Setup(s => s.GetBeerIdsFromDatabase(bars[0].Id)).ReturnsAsync(bar1BeerIds);
            _mockBarBeersLinkService.Setup(s => s.GetBeerIdsFromDatabase(bars[1].Id)).ReturnsAsync(bar2BeerIds);
            _mockBeerService.Setup(s => s.GetBeersFromDatabase(bar1BeerIds)).ReturnsAsync(bar1Beers);
            _mockBeerService.Setup(s => s.GetBeersFromDatabase(bar2BeerIds)).ReturnsAsync(bar2Beers);

            // Act
            var result = await _controller.GetAllBarsWithAssociatedBeers();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<List<BarWithBeersResponse>>(okResult.Value);
            Assert.Equal(expectedResponse.Count, response.Count);
            for (int i = 0; i < expectedResponse.Count; i++)
            {
                Assert.Equal(expectedResponse[i].BarId, response[i].BarId);
                Assert.Equal(expectedResponse[i].Name, response[i].Name);
                Assert.Equal(expectedResponse[i].Address, response[i].Address);
                Assert.Equal(expectedResponse[i].Beers.Count, response[i].Beers.Count);
                for (int j = 0; j < expectedResponse[i].Beers.Count; j++)
                {
                    Assert.Equal(expectedResponse[i].Beers[j].Id, response[i].Beers[j].Id);
                    Assert.Equal(expectedResponse[i].Beers[j].Name, response[i].Beers[j].Name);
                }
            }
        }

        [Fact]
        public async Task GetAllBarsWithAssociatedBeers_ReturnsStatusCode500_WhenErrorOccurs()
        {
            // Arrange
            var errorMessage = "An error occurred";
            _mockBarService.Setup(s => s.GetAllBarsAsync()).Throws(new Exception(errorMessage));

            // Act
            var result = await _controller.GetAllBarsWithAssociatedBeers();

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(StatusCodes.Status500InternalServerError, statusCodeResult.StatusCode);
            Assert.Equal("An error occurred while fetching bars with associated beers. Please try again later.", statusCodeResult.Value);
        }
    }
}
