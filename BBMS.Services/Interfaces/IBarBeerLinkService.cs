namespace BBMS.Services.Interfaces
{
    public interface IBarBeerLinkService
    {
          Task InsertBarBeerLinkAsync(int barId, int beerId);

        Task<List<int>> GetBeerIdsFromDatabase(int barId);


    }
}
