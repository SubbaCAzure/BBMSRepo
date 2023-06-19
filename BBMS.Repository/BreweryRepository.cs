using BBMS.Domain.Models;
using BBMS.Repository.Data;
using BBMS.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BBMS.Repository
{
    public class BreweryRepository : IBreweryRepository
    {
        private readonly DataContext _dbContext;
        public BreweryRepository(DataContext context)
        {
            _dbContext = context;
        }

        public async Task<Brewery> AddBreweryAsync(Brewery brewery)
        {
            await _dbContext.Breweries.AddAsync(brewery);
            await _dbContext.SaveChangesAsync();
            return brewery;
        }      

        public async Task<IEnumerable<Brewery>> GetAllBreweryAsync()
        {
            try
            {
                var brewery = await _dbContext.Breweries.ToListAsync();
                return brewery;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving the breweries.", ex);
            }
        }

        public async Task<Brewery> GetBreweryAsync(int id)
        {
            try
            {
                var brewery = await _dbContext.Breweries.FindAsync(id);
                return brewery;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving the Brewery.", ex);
            }
        }

        public async Task<Brewery> GetBreweryFromDatabase(int breweryId)
        {
            var brewery = await _dbContext.Breweries.FindAsync(breweryId);
            if (brewery == null)
            {
                throw new Exception($"Brewary with ID {breweryId} not found");
            }


            return brewery;
        }

        public async Task UpdateBrewery(Brewery brewery)
        {
            try
            {
                var existingBrewery = await _dbContext.Breweries.FindAsync(brewery.BreweryId) ?? throw new ArgumentException("Brewery not found");
                existingBrewery.Name = brewery.Name;                
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while updating the brewery.", ex);
            }
        }
    }
}
