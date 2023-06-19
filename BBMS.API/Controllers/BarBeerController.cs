using AutoMapper;
using BBMS.Domain.Models;
using BBMS.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BBMS.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class BarBeerController : ControllerBase
    {
        private readonly IBarBeerLinkService _barBeersLinkService;
        private readonly IBarService _barService;
        private readonly IBeerService _beerService;

        private readonly ILogger<BarController> _logger;
        private readonly IMapper _mapper;

        public BarBeerController(IBarBeerLinkService barBeersLinkService, ILogger<BarController> logger, IMapper mapper, IBarService barService, IBeerService beerService)
        {
            _barBeersLinkService = barBeersLinkService;
            _logger = logger;
            _mapper = mapper;
            _barService = barService;
            _beerService = beerService;
        }

        [HttpPost("bar/beer")]
        public async Task<IActionResult> InsertBarBeerLink([FromBody] BarBeerLinkRequest request)
        {
            try
            {
                await _barBeersLinkService.InsertBarBeerLinkAsync(request.BarId, request.BeerId);

                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while inserting the Bar-Beer link.");
                return StatusCode(500, "An error occurred while inserting the Bar-Beer link. Please try again later.");
            }
        }

        [HttpGet("bar/{barId}/beer")]
        public async Task<IActionResult> GetBarWithAssociatedBeers(int barId)
        {
            try
            {
                var bar = await _barService.GetBarFromDatabase(barId);

                if (bar == null)
                {
                    return NotFound("Bar not found");
                }

                var beerIds = await _barBeersLinkService.GetBeerIdsFromDatabase(barId);

                if (!beerIds.Any())
                {
                    return NotFound("Beer not found in the bar");
                }

                var beers = await _beerService.GetBeersFromDatabase(beerIds);
                var barWithBeers = new BarWithBeersResponse
                {
                    BarId = bar.Id,
                    Name = bar.Name,
                    Address = bar.Address,
                    Beers = beers
                };

                return Ok(barWithBeers);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }


        [HttpGet("bar/beer")]
        public async Task<IActionResult> GetAllBarsWithAssociatedBeers()
        {
            try
            {
                var bars = await _barService.GetAllBarsAsync();
                var response = new List<BarWithBeersResponse>();

                foreach (var bar in bars)
                {
                    var beerIds = await _barBeersLinkService.GetBeerIdsFromDatabase(bar.Id);
                    var beers = await _beerService.GetBeersFromDatabase(beerIds);

                    var barWithBeers = new BarWithBeersResponse
                    {
                        BarId = bar.Id,
                        Name = bar.Name,
                        Address = bar.Address,
                        Beers = beers
                    };

                    response.Add(barWithBeers);
                }

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching bars with associated beers");
                return StatusCode(500, "An error occurred while fetching bars with associated beers. Please try again later.");
            }
        }
    }
}
