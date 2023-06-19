using AutoMapper;
using BBMS.API.Controllers;
using BBMS.Domain.Models;
using BBMS.Services.DTO;
using BBMS.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace BBMS.Test
{
    public class BarControllerTests
    {
       

        [Fact]
        public async Task UpdateBar_ReturnsNotFound_WhenBarDoesNotExist()
        {
            // Arrange
            var mockBarService = new Mock<IBarService>();
            var mockLogger = new Mock<ILogger<BarController>>();
            var mockMapper = new Mock<IMapper>();

            int barId = 1;
            var barDto = new BarUpdateDTO { Name = "Updated Bar", Address = "Updated Address" };

            mockBarService.Setup(s => s.GetBarAsync(barId)).ReturnsAsync((BarDTO)null);
            var controller = new BarController(mockBarService.Object, mockLogger.Object, mockMapper.Object);

            // Act
            var result = await controller.UpdateBar(barId, barDto);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task GetBars_ReturnsNotFound_WhenBarsAreNull()
        {
            // Arrange
            var mockBarService = new Mock<IBarService>();
            var mockLogger = new Mock<ILogger<BarController>>();
            var mockMapper = new Mock<IMapper>();

            mockBarService.Setup(s => s.GetAllBarsAsync()).ReturnsAsync((IEnumerable<Bar>)null);
            var controller = new BarController(mockBarService.Object, mockLogger.Object, mockMapper.Object);

            // Act
            var result = await controller.GetBars();

            // Assert
            var notFoundResult = Assert.IsType<NotFoundResult>(result.Result);
        }
        

    }



}
