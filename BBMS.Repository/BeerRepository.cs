using BBMS.Domain.Models;
using BBMS.Repository.Data;
using BBMS.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BBMS.Repository
{
    public class BeerRepository : IBeerRepository
    {
        private readonly DataContext _dbContext;
        public BeerRepository(DataContext context)
        {
            _dbContext = context;
        }

        public async Task<Beer> AddBeerAsync(Beer beer)
        {
            await _dbContext.Beers.AddAsync(beer);
            await _dbContext.SaveChangesAsync();
            return beer;
        }

        public async Task<IEnumerable<Beer>> GetAllBeersAsync()
        {
            try
            {
                var beers = await _dbContext.Beers.ToListAsync();
                return beers;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving the Beer.", ex);
            }
        }

        public  async Task<Beer> GetBeerAsync(int id)
        {
            try
            {
                var beer = await _dbContext.Beers.FindAsync(id);
                return beer;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving the Beer.", ex);
            }
        }

        public async Task UpdateBeer(Beer beer)
        {

            try
            {
                var existingBeer = await _dbContext.Beers.FindAsync(beer.Id) ?? throw new ArgumentException("Beer not found");
                existingBeer.Name = beer.Name;    
                existingBeer.PercentageAlcoholByVolume = beer.PercentageAlcoholByVolume;
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while updating the Beer.", ex);
            }
        }


        public async Task<List<Beer>> GetBeersFromDatabase(List<int> beerIds)
        {

            var beers = await _dbContext.Beers
                .Where(b => beerIds.Contains(b.Id))
                .ToListAsync();

            return beers;

        }


    }
}
