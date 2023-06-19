using BBMS.Repository;
using BBMS.Repository.Data;
using BBMS.Repository.Interfaces;
using BBMS.Services;
using BBMS.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(); 

builder.Services.AddAutoMapper(typeof(MappingProfile));

builder.Services.AddScoped<IBeerService, BeerService>();
builder.Services.AddScoped<IBeerRepository, BeerRepository>();

builder.Services.AddScoped<IBreweryService, BreweryService>();
builder.Services.AddScoped<IBreweryRepository, BreweryRepository>();

builder.Services.AddScoped<IBreweryBeerLinkService, BreweryBeerLinkService>();
builder.Services.AddScoped<IBreweryBeerLinkRepository, BreweryBeerLinkRepository>();


builder.Services.AddScoped<IBarService, BarService>();
builder.Services.AddScoped<IBarRepository, BarRepository>();
 
builder.Services.AddScoped<IBarBeerLinkService, BarBeerLinkService>();
builder.Services.AddScoped<IBarBeerLinkRepository, BarBeerLinkRepository>();


builder.Services.AddDbContext<DataContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

 