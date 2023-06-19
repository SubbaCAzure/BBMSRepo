using BBMS.Domain.Models;

namespace BBMS.Repository.Interfaces
{
    public interface IBreweryRepository
    {
        Task<Brewery> AddBreweryAsync(Brewery brewery);
        Task UpdateBrewery(Brewery brewery);
        Task<IEnumerable<Brewery>> GetAllBreweryAsync();
        Task<Brewery> GetBreweryAsync(int id);


        Task<Brewery> GetBreweryFromDatabase(int breweryId);

    }
}
