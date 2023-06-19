using AutoMapper;
using BBMS.Domain.Models;
using BBMS.Repository;
using BBMS.Repository.Interfaces;
using BBMS.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BBMS.Services
{
    public class BreweryBeerLinkService : IBreweryBeerLinkService
    {
        private readonly IBreweryBeerLinkRepository _breweryBeerLinkRepository;
        private readonly IMapper _mapper;

        public BreweryBeerLinkService(IBreweryBeerLinkRepository breweryBeerLinkRepository, IMapper mapper)
        {
            _breweryBeerLinkRepository = breweryBeerLinkRepository;
            _mapper = mapper;
        }

        public async Task InsertBreweryBeerLinkAsync(int breweryId, int beerId)
        {
            var breweryBeerLink = new BreweryBeerLinkRequest
            {
                BreweryId = breweryId,
                BeerId = beerId
            };

            await _breweryBeerLinkRepository.AddBreweryBeerLinkAsync(breweryBeerLink);
        }

        public async Task<List<int>> GetBeerIdsFromDatabase(int breweryId)
        {
            var beerIds = await _breweryBeerLinkRepository.GetBeerIdsFromDatabase(breweryId);
            return beerIds;
        }

       
    }
}
