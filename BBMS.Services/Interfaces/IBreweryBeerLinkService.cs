using BBMS.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BBMS.Services.Interfaces
{
    public interface IBreweryBeerLinkService
    {
        Task InsertBreweryBeerLinkAsync(int barId, int beerId);

        Task<List<int>> GetBeerIdsFromDatabase(int breweryId);
    }
}
