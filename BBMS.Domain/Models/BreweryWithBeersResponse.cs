using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BBMS.Domain.Models
{
    public class BreweryWithBeersResponse
    {
        public int BreweryId { get; set; }
        public string? BreweryName { get; set; }
        public List<Beer>? Beers { get; set; }
    }
}
