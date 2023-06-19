using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BBMS.Domain.Models
{
    public class Brewery
    {
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity), Key()]
        public int BreweryId { get; set; }
        public string? Name { get; set; }
    }
}
