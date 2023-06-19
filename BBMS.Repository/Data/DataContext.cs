using BBMS.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace BBMS.Repository.Data
{
    public class DataContext : DbContext
    {
        public DbSet<Beer> Beers { get; set; }
        public DbSet<Bar> Bars { get; set; }

        public DbSet<Brewery> Breweries { get; set; }
        public DbSet<BarBeerLinkRequest> BarBeers { get; set; }

        public DbSet<BreweryBeerLinkRequest> BreweryBeers { get; set; }

        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
           

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BarBeerLinkRequest>().HasKey(bbl => new { bbl.BarId, bbl.BeerId });
            modelBuilder.Entity<BreweryBeerLinkRequest>().HasKey(bbl => new { bbl.BreweryId, bbl.BeerId });          

            base.OnModelCreating(modelBuilder);

        }
    }
}
