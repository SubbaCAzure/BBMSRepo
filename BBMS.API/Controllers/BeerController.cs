using AutoMapper;
using BBMS.Domain.Models;
using BBMS.Services.DTO;
using BBMS.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BBMS.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class BeerController : ControllerBase
    {
        private readonly IBeerService _beerService;
        private readonly ILogger<BeerController> _logger;
        private readonly IMapper _mapper;

        public BeerController(IBeerService beerService, ILogger<BeerController> logger, IMapper mapper)
        {
            _beerService = beerService;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpPost("/beer")]
        public async Task<IActionResult> PostBeer([FromBody] BeerDTO beerDto)
        {
            try
            {
                var beer = _mapper.Map<Beer>(beerDto);
                var addedBeer = await _beerService.AddBeerAsync(beer);
                var addedBeerDto = _mapper.Map<BeerDTO>(addedBeer);
                return CreatedAtAction(nameof(GetBeer), new { id = addedBeerDto.Id }, addedBeerDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching the beers");
                var response = new { error = "An error occurred" };
                return StatusCode(500, response);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBeer(int id, [FromBody] BeerUpdateDTO beerDto)
        {
            try
            {
                var existingBeer = await _beerService.GetBeerAsync(id);
                if (existingBeer == null)
                {
                    return NotFound();
                }

                existingBeer.Name = beerDto.Name;
                existingBeer.PercentageAlcoholByVolume = beerDto.PercentageAlcoholByVolume;

                var beer = _mapper.Map<Beer>(existingBeer);
                await _beerService.UpdateBeer(beer);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating the beer.");
                return StatusCode(500, "An error occurred while updating the beer. Please try again later.");
            }
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<BeerDTO>>> GetBeers()
        {
            try
            {
                var beers = await _beerService.GetAllBeersAsync();
                if (beers == null || !beers.Any())
                {
                    return NotFound();
                }

                var beerDtos = _mapper.Map<IEnumerable<BeerDTO>>(beers);

                return Ok(beerDtos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching the beers");
                var response = new { error = "An error occurred" };
                return StatusCode(500, response);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetBeer(int id)
        {
            _logger.LogInformation("GetBeerAsync method called with id: {Id}", id);

            try
            {
                BeerDTO beerDTO = await _beerService.GetBeerAsync(id);
                if (beerDTO == null)
                {
                    _logger.LogInformation("GetBeerAsync method called with id: {Id}", id);
                    return NotFound();
                }
                _logger.LogInformation("Beer found with id: {Id}", id);
                return Ok(beerDTO);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching the beer");
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }


        [HttpGet("/beer")]
        public async Task<ActionResult<IEnumerable<Beer>>> GetBeers([FromQuery] decimal? gtAlcoholByVolume, [FromQuery] decimal? ltAlcoholByVolume)
        {
            try
            {
                IEnumerable<Beer> beers = await _beerService.GetAllBeersAsync();

                if (gtAlcoholByVolume.HasValue)
                {
                    beers = beers.Where(b => b.PercentageAlcoholByVolume > gtAlcoholByVolume.Value);
                }

                if (ltAlcoholByVolume.HasValue)
                {
                    beers = beers.Where(b => b.PercentageAlcoholByVolume < ltAlcoholByVolume.Value);
                }

                return Ok(beers);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching the beers");
                var response = new { error = "An error occurred" };
                return StatusCode(500, response);
            }
        }

    }
}
