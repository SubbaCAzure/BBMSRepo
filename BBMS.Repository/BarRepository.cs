using BBMS.Domain.Models;
using BBMS.Repository.Data;
using BBMS.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BBMS.Repository
{
    public class BarRepository : IBarRepository
    {
        private readonly DataContext _dbContext;
        public BarRepository(DataContext context)
        {
            _dbContext = context;
        }


        public async Task<Bar> AddBarAsync(Bar bar)
        {
            await _dbContext.Bars.AddAsync(bar);
            await _dbContext.SaveChangesAsync();
            return bar;
        }
        public async Task UpdateBar(Bar bar)
        {
            try
            {
                var existingBar = await _dbContext.Bars.FindAsync(bar.Id) ?? throw new ArgumentException("Bar not found");
                existingBar.Name = bar.Name;
                existingBar.Address = bar.Address;
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while updating the Bar.", ex);
            }
        }
        public async Task<IEnumerable<Bar>> GetAllBarsAsync()
        {
            try
            {
                var bars = await _dbContext.Bars.ToListAsync();
                return bars;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving the Bar.", ex);
            }
        }
        public async Task<Bar> GetBarAsync(int id)
        {
            try
            {
                var bar = await _dbContext.Bars.FindAsync(id);
                return bar;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving the Bar.", ex);
            }
        }


        public async Task<Bar> GetBarFromDatabase(int barId)
        {
            var bar = await _dbContext.Bars.FindAsync(barId);
            if (bar == null)
            {
                throw new Exception($"Bar with ID {barId} not found");
            }


            return bar;

        }


    }
}
