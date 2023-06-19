namespace BBMS.Domain.Models
{
    public class BarWithBeersResponse
    {
        public int BarId { get; set; }
        public string? Name { get; set; }
        public string? Address { get; set; }
        public List<Beer>? Beers { get; set; }
    }
}
