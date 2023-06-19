using BBMS.Domain.Models;

namespace BBMS.Repository.Interfaces
{
    public interface IBarRepository
    {
        Task<Bar> AddBarAsync(Bar bar);
        Task UpdateBar(Bar bar);
        Task<IEnumerable<Bar>> GetAllBarsAsync();
        Task<Bar> GetBarAsync(int id);
        Task<Bar> GetBarFromDatabase(int barId);



    }
}
