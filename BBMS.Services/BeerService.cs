using AutoMapper;
using BBMS.Domain.Models;
using BBMS.Repository.Interfaces;
using BBMS.Services.DTO;
using BBMS.Services.Interfaces;

namespace BBMS.Services
{
    public class BeerService : IBeerService
    {
        private readonly IBeerRepository _beerRepository;
        private readonly IMapper _mapper;
        public BeerService(IBeerRepository beerRepository, IMapper mapper)
        {
            _beerRepository = beerRepository;
            _mapper = mapper;
        }

        public async Task<Beer> AddBeerAsync(Beer beer)
        {
            try
            {
                var addedBeer = await _beerRepository.AddBeerAsync(beer);
                return addedBeer;
            }
            catch (Exception ex)
            {
                throw new Exception("Error occurred while adding the beer.", ex);
            }
        }

        public async Task<IEnumerable<Beer>> GetAllBeersAsync()
        {
            var beers = await _beerRepository.GetAllBeersAsync();
            return beers;
        }

        public async Task<BeerDTO> GetBeerAsync(int id)
        {
            try
            {
                Beer beer = await _beerRepository.GetBeerAsync(id);
                BeerDTO beerDTO = _mapper.Map<BeerDTO>(beer);
                return beerDTO;
            }
            catch (Exception ex)
            {
                throw new Exception("Error occurred while adding the beer.", ex);
            }
        }

        public async Task<List<Beer>> GetBeersFromDatabase(List<int> beerIds)
        {
            var beers = await _beerRepository.GetBeersFromDatabase(beerIds);
           // var beerDTOs = _mapper.Map<List<BeerDTO>>(beers);
            return beers;
        }

        public async Task UpdateBeer(Beer beer)
        {
            try
            {
                await _beerRepository.UpdateBeer(beer);
            }
            catch (Exception ex)
            {
                throw new Exception("Error occurred while adding the beer.", ex);
            }
        }


      

    }
}
