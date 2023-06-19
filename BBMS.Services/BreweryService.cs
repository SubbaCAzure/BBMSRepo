using AutoMapper;
using BBMS.Domain.Models;
using BBMS.Repository;
using BBMS.Repository.Interfaces;
using BBMS.Services.DTO;
using BBMS.Services.Interfaces;

namespace BBMS.Services
{
    public class BreweryService : IBreweryService
    {
        private readonly IBreweryRepository _BreweryRepository;
        private readonly IMapper _mapper;
        public BreweryService(IBreweryRepository breweryRepository, IMapper mapper)
        {
            _BreweryRepository = breweryRepository;
            _mapper = mapper;
        }

        public async Task<Brewery> AddBreweryAsync(Brewery brewery)
        {
            try
            {
                var addedBrewery = await _BreweryRepository.AddBreweryAsync(brewery);
                return addedBrewery;
            }
            catch (Exception ex)
            {
                throw new Exception("Error occurred while adding the breweryry.", ex);
            }
        }


         
        public async Task<IEnumerable<Brewery>> GetAllBreweriesAsync()
        {
            var breweries = await _BreweryRepository.GetAllBreweryAsync();
            return breweries;
        }

        public async Task<BreweryDTO> GetBreweryAsync(int id)
        {
            try
            {
                Brewery brewery = await _BreweryRepository.GetBreweryAsync(id);
                BreweryDTO breweryDTO = _mapper.Map<BreweryDTO>(brewery);
                return breweryDTO;

            }
            catch (Exception ex)
            {
                throw new Exception("Error occurred while adding the brewery.", ex);
            }

        }

        public async Task<BreweryDTO> GetBreweryFromDatabase(int breweryId)
        {
            try
            {
                Brewery brewery = await _BreweryRepository.GetBreweryAsync(breweryId);
                BreweryDTO breweryDTO = _mapper.Map<BreweryDTO>(brewery);
                return breweryDTO;

            }
            catch (Exception ex)
            {
                throw new Exception("Error occurred while adding the beer.", ex);
            }
        }

        public async Task UpdateBrewery(Brewery brewery)
        {
            try
            {
                await _BreweryRepository.UpdateBrewery(brewery);
            }
            catch (Exception ex)
            {
                throw new Exception("Error occurred while adding the bar.", ex);
            }
        }
    }
}
