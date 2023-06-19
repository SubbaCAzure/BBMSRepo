using AutoMapper;
using Azure.Core;
using BBMS.Domain.Models;
using BBMS.Services;
using BBMS.Services.DTO;
using BBMS.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BBMS.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class BreweryBeerController : ControllerBase
    {
        private readonly IBreweryBeerLinkService _breweryBeersLinkService;
        private readonly IBreweryService _breweryService;
        private readonly IBeerService _beerService;

        private readonly ILogger<BarController> _logger;
        private readonly IMapper _mapper;

        public BreweryBeerController(IBreweryBeerLinkService breweryBeerLinkService, ILogger<BarController> logger, IMapper mapper, IBreweryService breweryService, IBeerService beerService)
        {
            _breweryBeersLinkService = breweryBeerLinkService;
            _logger = logger;
            _mapper = mapper;
            _breweryService = breweryService;
            _beerService = beerService;
        }


        [HttpPost("brewery/beer")]
        public async Task<IActionResult> InsertBreweryBeerLink([FromBody] BreweryBeerLinkRequest request)
        {
            try
            {               
                var breweryBeerLink = _mapper.Map<BreweryBeerLinkRequest>(request);
                await _breweryBeersLinkService.InsertBreweryBeerLinkAsync(breweryBeerLink.BreweryId, breweryBeerLink.BeerId);

                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while inserting the brewery beer link");
                return StatusCode(500, "An error occurred while inserting the brewery beer link. Please try again later.");
            }
        }


        [HttpGet("brewery/{breweryId}/beer")]
        public async Task<IActionResult> GetBreweryWithAssociatedBeers(int breweryId)
        {
            try
            {
                var brewery = await _breweryService.GetBreweryFromDatabase(breweryId);

                if (brewery == null)
                {
                    return NotFound("Brewery not found");
                }

                var beerIds = await _breweryBeersLinkService.GetBeerIdsFromDatabase(breweryId);

                if (!beerIds.Any())
                {
                    return NotFound("Beer not found in the brewery");
                }

                var beers = await _beerService.GetBeersFromDatabase(beerIds);
                var breweryWithBeers = new BreweryWithBeersResponse
                {
                    BreweryId = brewery.BreweryId,
                    BreweryName = brewery.Name,                   
                    Beers = beers
                };

                return Ok(breweryWithBeers);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }


        [HttpGet("brewery/beer")]
        public async Task<IActionResult> GetAllBrewariesWithAssociatedBeers()
        {
            try
            {
                var breweries = await _breweryService.GetAllBreweriesAsync();
                var response = new List<BreweryWithBeersResponse>();

                foreach (var brewery in breweries)
                {
                    var beerIds = await _breweryBeersLinkService.GetBeerIdsFromDatabase(brewery.BreweryId);
                    var beers = await _beerService.GetBeersFromDatabase(beerIds);

                    var barWithBeers = new BreweryWithBeersResponse
                    {
                        BreweryId = brewery.BreweryId,
                        BreweryName = brewery.Name,
                        
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
