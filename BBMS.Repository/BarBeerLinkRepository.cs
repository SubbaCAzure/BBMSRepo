using BBMS.Domain.Models;
using BBMS.Repository.Data;
using BBMS.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BBMS.Repository
{
    public class BarBeerLinkRepository : IBarBeerLinkRepository
    {
        private readonly DataContext _dbContext;
        public BarBeerLinkRepository(DataContext context)
        {
            _dbContext = context;
        }
         
        public async Task AddBarBeerLinkAsync(BarBeerLinkRequest barBeerLink)
        {

            try
            {
                _dbContext.Set<BarBeerLinkRequest>().Add(barBeerLink);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while inserting the BarBeers.", ex);
            }
            
        }


        public async Task<List<int>> GetBeerIdsFromDatabase(int barId)
        {

            var beerIds = await _dbContext.BarBeers
                .Where(b => b.BarId == barId)
                .Select(b => b.BeerId)
                .ToListAsync();

            return beerIds;

        }
 
    }
}
