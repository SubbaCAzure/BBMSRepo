using BBMS.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BBMS.Repository.Interfaces
{
    public interface IBreweryBeerLinkRepository
    {
        Task AddBreweryBeerLinkAsync(BreweryBeerLinkRequest breweryBeerLink);

        Task<List<int>> GetBeerIdsFromDatabase(int breweryId);

    }
}
