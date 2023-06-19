using AutoMapper;
using BBMS.Domain.Models;
using BBMS.Repository.Interfaces;
using BBMS.Services.Interfaces;

namespace BBMS.Services
{
    public class BarBeerLinkService : IBarBeerLinkService
    {

        private readonly IBarBeerLinkRepository _barBeersLinkRepository;
        private readonly IMapper _mapper;
        public BarBeerLinkService(IBarBeerLinkRepository barBeersLinkRepository, IMapper mapper)
        {
            _barBeersLinkRepository = barBeersLinkRepository;
            _mapper = mapper;
        }

        public async Task<List<int>> GetBeerIdsFromDatabase(int barId)
        {
            var beerIds = await _barBeersLinkRepository.GetBeerIdsFromDatabase(barId);
            return beerIds;
        }

        public async Task InsertBarBeerLinkAsync(int barId, int beerId)
        {             
            var barBeerLink = new BarBeerLinkRequest
            {
                BarId = barId,
                BeerId = beerId
            };

            await _barBeersLinkRepository.AddBarBeerLinkAsync(barBeerLink);
        }

        



    }
}
