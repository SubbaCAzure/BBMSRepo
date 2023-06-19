using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BBMS.Domain.Models
{
    public class Beer
    {
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity), Key()]
        public int Id { get; set; }
        public string? Name { get; set; }
        public decimal PercentageAlcoholByVolume { get; set; }
    }
}
