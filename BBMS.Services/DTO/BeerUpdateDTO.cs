using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BBMS.Services.DTO
{
    public class BeerUpdateDTO
    {
        public string? Name { get; set; }
        public decimal PercentageAlcoholByVolume { get; set; }
    }
}
