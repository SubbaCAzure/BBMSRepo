using BBMS.Domain.Models;
using BBMS.Services.DTO;

namespace BBMS.Services.Interfaces
{
    public interface IBeerService
    {

        Task<Beer> AddBeerAsync(Beer beer);
        Task UpdateBeer(Beer beer);
        Task<IEnumerable<Beer>> GetAllBeersAsync();
        Task<BeerDTO> GetBeerAsync(int id);

        Task<List<Beer>> GetBeersFromDatabase(List<int> beerIds);
    }
}
