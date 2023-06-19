using BBMS.Domain.Models;

namespace BBMS.Repository.Interfaces
{
    public interface IBarBeerLinkRepository
    {
        Task AddBarBeerLinkAsync(BarBeerLinkRequest barBeerLink);

        Task<List<int>> GetBeerIdsFromDatabase(int barId);

        
    }
}
