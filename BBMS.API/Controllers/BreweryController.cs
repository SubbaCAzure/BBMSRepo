using AutoMapper;
using BBMS.Domain.Models;
using BBMS.Services.DTO;
using BBMS.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BBMS.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class BreweryController : ControllerBase
    {
        private readonly IBreweryService _breweryService;
        private readonly ILogger<BreweryController> _logger;
        private readonly IMapper _mapper;

        public BreweryController(IBreweryService breweryService, ILogger<BreweryController> logger, IMapper mapper)
        {
            _breweryService = breweryService;
            _logger = logger;
            _mapper = mapper;
        }



        [HttpPost("/brewery")]
        public async Task<IActionResult> PostBrewery([FromBody] BreweryDTO breweryDto)
        {
            try
            {
                var brewery = _mapper.Map<Brewery>(breweryDto);
                var addedBrewery = await _breweryService.AddBreweryAsync(brewery);
                var addedBreweryDto = _mapper.Map<BreweryDTO>(addedBrewery);
                return CreatedAtAction(nameof(GetBrewery), new { id = addedBreweryDto.Name }, addedBreweryDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching the breweries");
                var response = new { error = "An error occurred" };
                return StatusCode(500, response);
            }
        }



        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBrewery(int id, [FromBody] BreweryUpdateDTO breweryDto)
        {
            try
            {
                var existingBrewery = await _breweryService.GetBreweryAsync(id);
                if (existingBrewery == null)
                {
                    return NotFound();
                }

                existingBrewery.Name = breweryDto.Name;               

                var brewery = _mapper.Map<Brewery>(existingBrewery);
                await _breweryService.UpdateBrewery(brewery);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating the Brewery.");
                return StatusCode(500, "An error occurred while updating the brewery. Please try again later.");
            }
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<BreweryDTO>>> GetBreweries()
        {
            try
            {
                var brewery = await _breweryService.GetAllBreweriesAsync();
                if (brewery == null || !brewery.Any())
                {
                    return NotFound();
                }

                var breweryDtos = _mapper.Map<IEnumerable<BreweryDTO>>(brewery);

                return Ok(breweryDtos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching the breweries");
                var response = new { error = "An error occurred" };
                return StatusCode(500, response);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetBrewery(int id)
        {
            _logger.LogInformation("GetBreweryAsync method called with id: {Id}", id);

            try
            {
                BreweryDTO breweryDTO = await _breweryService.GetBreweryAsync(id);
                if (breweryDTO == null)
                {
                    _logger.LogInformation("GetBreweryAsync method called with id: {Id}", id);
                    return NotFound();
                }
                _logger.LogInformation("Brewery found with id: {Id}", id);
                return Ok(breweryDTO);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching the brewery");
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }



    }
}
