using AutoMapper;
using BBMS.API.Controllers;
using BBMS.Domain.Models;
using BBMS.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace BBMS.Test
{
    public class BreweryBeerControllerTests
    {
        private readonly Mock<IBreweryBeerLinkService> _mockBreweryBeerLinkService;
        private readonly Mock<IBreweryService> _mockBreweryService;
        private readonly Mock<IBeerService> _mockBeerService;
        private readonly Mock<ILogger<BarController>> _mockLogger;
        private readonly Mock<IMapper> _mockMapper;
        private readonly BreweryBeerController _controller;

        public BreweryBeerControllerTests()
        {
            _mockBreweryBeerLinkService = new Mock<IBreweryBeerLinkService>();
            _mockBreweryService = new Mock<IBreweryService>();
            _mockBeerService = new Mock<IBeerService>();
            _mockLogger = new Mock<ILogger<BarController>>();
            _mockMapper = new Mock<IMapper>();

            _controller = new BreweryBeerController(
                _mockBreweryBeerLinkService.Object,
                _mockLogger.Object,
                _mockMapper.Object,
                _mockBreweryService.Object,
                _mockBeerService.Object
            );
        }
             

        [Fact]
        public async Task InsertBreweryBeerLink_ReturnsStatusCode500_WhenErrorOccurs()
        {
            // Arrange
            var request = new BreweryBeerLinkRequest { BreweryId = 1, BeerId = 1 };
            var errorMessage = "An error occurred";

            _mockBreweryBeerLinkService.Setup(s => s.InsertBreweryBeerLinkAsync(request.BreweryId, request.BeerId))
                .Throws(new Exception(errorMessage));

            // Act
            var result = await _controller.InsertBreweryBeerLink(request);

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(StatusCodes.Status500InternalServerError, statusCodeResult.StatusCode);
            Assert.Equal("An error occurred while inserting the brewery beer link. Please try again later.", statusCodeResult.Value);
        }        

        [Fact]
        public async Task GetBreweryWithAssociatedBeers_ReturnsNotFound_WhenBreweryNotFound()
        {
            // Arrange
            var breweryId = 1;
            Brewery nullBrewery = null;            

            // Act
            var result = await _controller.GetBreweryWithAssociatedBeers(breweryId);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("Brewery not found", notFoundResult.Value);
        }

        [Fact]
        public async Task GetBreweryWithAssociatedBeers_ReturnsNotFound_WhenNoBeersFound()
        {            
            var breweryId = 1;
            var brewery = new Brewery { BreweryId = breweryId, Name = "Brewery 1" };
            var beerIds = new List<int>();
            var beers = new List<Beer>();
            
            _mockBreweryBeerLinkService.Setup(s => s.GetBeerIdsFromDatabase(breweryId)).ReturnsAsync(beerIds);            
            var result = await _controller.GetBreweryWithAssociatedBeers(breweryId);
                        
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("Brewery not found", notFoundResult.Value);

        }

        [Fact]
        public async Task GetAllBreweriesWithAssociatedBeers_ReturnsOkResult_WithListOfBreweriesAndAssociatedBeers()
        {
          
            var breweries = new List<Brewery>
        {
            new Brewery { BreweryId = 1, Name = "Brewery 1" },
            new Brewery { BreweryId = 2, Name = "Brewery 2" }
        };

            var brewery1BeerIds = new List<int> { 1, 2 };
            var brewery2BeerIds = new List<int> { 3, 4 };
            var brewery1Beers = new List<Beer>
        {
            new Beer { Id = 1, Name = "Beer 1" },
            new Beer { Id = 2, Name = "Beer 2" }
        };
            var brewery2Beers = new List<Beer>
        {
            new Beer { Id = 3, Name = "Beer 3" },
            new Beer { Id = 4, Name = "Beer 4" }
        };

            _mockBreweryService.Setup(s => s.GetAllBreweriesAsync()).ReturnsAsync(breweries);
            _mockBreweryBeerLinkService.Setup(s => s.GetBeerIdsFromDatabase(1)).ReturnsAsync(brewery1BeerIds);
            _mockBreweryBeerLinkService.Setup(s => s.GetBeerIdsFromDatabase(2)).ReturnsAsync(brewery2BeerIds);
            _mockBeerService.Setup(s => s.GetBeersFromDatabase(brewery1BeerIds)).ReturnsAsync(brewery1Beers);
            _mockBeerService.Setup(s => s.GetBeersFromDatabase(brewery2BeerIds)).ReturnsAsync(brewery2Beers);

            var expectedResponse = new List<BreweryWithBeersResponse>
        {
            new BreweryWithBeersResponse
            {
                BreweryId = 1,
                BreweryName = "Brewery 1",
                Beers = brewery1Beers
            },
            new BreweryWithBeersResponse
            {
                BreweryId = 2,
                BreweryName = "Brewery 2",
                Beers = brewery2Beers
            }
        };

            // Act
            var result = await _controller.GetAllBrewariesWithAssociatedBeers();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<List<BreweryWithBeersResponse>>(okResult.Value);
            Assert.Equal(expectedResponse.Count, response.Count);
            for (int i = 0; i < expectedResponse.Count; i++)
            {
                var expectedBrewery = expectedResponse[i];
                var actualBrewery = response[i];
                Assert.Equal(expectedBrewery.BreweryId, actualBrewery.BreweryId);
                Assert.Equal(expectedBrewery.BreweryName, actualBrewery.BreweryName);
                Assert.Equal(expectedBrewery.Beers.Count, actualBrewery.Beers.Count);
                for (int j = 0; j < expectedBrewery.Beers.Count; j++)
                {
                    Assert.Equal(expectedBrewery.Beers[j].Id, actualBrewery.Beers[j].Id);
                    Assert.Equal(expectedBrewery.Beers[j].Name, actualBrewery.Beers[j].Name);
                }
            }
        }

        [Fact]
        public async Task GetAllBreweriesWithAssociatedBeers_ReturnsStatusCode500_WhenErrorOccurs()
        {         
            var errorMessage = "An error occurred";
            _mockBreweryService.Setup(s => s.GetAllBreweriesAsync()).Throws(new Exception(errorMessage));      
            var result = await _controller.GetAllBrewariesWithAssociatedBeers();             
            var statusCodeResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(StatusCodes.Status500InternalServerError, statusCodeResult.StatusCode);
            Assert.Equal("An error occurred while fetching bars with associated beers. Please try again later.", statusCodeResult.Value);
        }
    }
}
