using BBMS.Domain.Models;

namespace BBMS.Repository.Interfaces
{
    public interface IBeerRepository
    {
        Task<Beer> AddBeerAsync(Beer beer);
        Task UpdateBeer(Beer beer);
        Task<IEnumerable<Beer>> GetAllBeersAsync();
        Task<Beer> GetBeerAsync(int id);
        Task<List<Beer>> GetBeersFromDatabase(List<int> beerIds);
    }
}
