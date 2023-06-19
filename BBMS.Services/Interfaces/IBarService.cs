using BBMS.Domain.Models;
using BBMS.Services.DTO;

namespace BBMS.Services.Interfaces
{
    public interface IBarService
    {
        Task<Bar> AddBarAsync(Bar bar);
        Task UpdateBar(Bar bar);
        Task<IEnumerable<Bar>> GetAllBarsAsync();
        Task<BarDTO> GetBarAsync(int id);


        Task<BarDTO> GetBarFromDatabase(int barId);
    }
}
