using AutoMapper;
using BBMS.Domain.Models;
using BBMS.Services.DTO;
using BBMS.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BBMS.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class BarController : ControllerBase
    {
        private readonly IBarService _barService;
        private readonly ILogger<BarController> _logger;
        private readonly IMapper _mapper;

        public BarController(IBarService barService, ILogger<BarController> logger, IMapper mapper)
        {
            _barService = barService;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpPost("/bar")]
        public async Task<IActionResult> PostBar([FromBody] BarDTO barDto)
        {
            try
            {
                var bar = _mapper.Map<Bar>(barDto);
                var addedBar = await _barService.AddBarAsync(bar);
                var addedBarDto = _mapper.Map<BarDTO>(addedBar);
                return CreatedAtAction(nameof(GetBar), new { id = addedBarDto.Id }, addedBarDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching the bars");
                var response = new { error = "An error occurred" };
                return StatusCode(500, response);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBar(int id, [FromBody] BarUpdateDTO barDto)
        {
            try
            {
                var existingBar = await _barService.GetBarAsync(id);
                if (existingBar == null)
                {
                    return NotFound();
                }

                existingBar.Name = barDto.Name;
                existingBar.Address = barDto.Address;

                var bar = _mapper.Map<Bar>(existingBar);
                await _barService.UpdateBar(bar);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating the bar.");
                return StatusCode(500, "An error occurred while updating the bar. Please try again later.");
            }
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<BarDTO>>> GetBars()
        {
            try
            {
                var bars = await _barService.GetAllBarsAsync();
                if (bars == null || !bars.Any())
                {
                    return NotFound();
                }

                var barDtos = _mapper.Map<IEnumerable<BarDTO>>(bars);

                return Ok(barDtos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching the bars");
                var response = new { error = "An error occurred" };
                return StatusCode(500, response);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetBar(int id)
        {
            _logger.LogInformation("GetBarAsync method called with id: {Id}", id);

            try
            {
                BarDTO barDTO = await _barService.GetBarAsync(id);
                if (barDTO == null)
                {
                    _logger.LogInformation("GetBarAsync method called with id: {Id}", id);
                    return NotFound();
                }
                _logger.LogInformation("Bar found with id: {Id}", id);
                return Ok(barDTO);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching the bar");
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }




    }
}
