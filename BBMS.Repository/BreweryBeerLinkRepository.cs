using BBMS.Domain.Models;
using BBMS.Repository.Data;
using BBMS.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BBMS.Repository
{
    public class BreweryBeerLinkRepository : IBreweryBeerLinkRepository
    {
        private readonly DataContext _dbContext;
        public BreweryBeerLinkRepository(DataContext context)
        {
            _dbContext = context;
        }

        public async Task AddBreweryBeerLinkAsync(BreweryBeerLinkRequest breweryBeerLink)
        {
            try
            {
                _dbContext.Set<BreweryBeerLinkRequest>().Add(breweryBeerLink);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while inserting the Beers in brewery.", ex);
            }
        }

        //public Task<List<int>> GetBeerIdsFromDatabase(int breweryId)
        //{
        //    throw new NotImplementedException();
        //}


        public async Task<List<int>> GetBeerIdsFromDatabase(int breweryId)
        {

            var beerIds = await _dbContext.BreweryBeers
                .Where(b => b.BreweryId == breweryId)
                .Select(b => b.BeerId)
                .ToListAsync();

            return beerIds;

        }

    }
}
