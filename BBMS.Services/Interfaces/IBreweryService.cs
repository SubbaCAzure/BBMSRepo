using BBMS.Domain.Models;
using BBMS.Services.DTO;

namespace BBMS.Services.Interfaces
{
    public interface IBreweryService
    {
        Task<Brewery> AddBreweryAsync(Brewery brewery);
        Task UpdateBrewery(Brewery brewery);
        Task<IEnumerable<Brewery>> GetAllBreweriesAsync();
        Task<BreweryDTO> GetBreweryAsync(int id);



        Task<BreweryDTO> GetBreweryFromDatabase(int breweryId);

    }
}
