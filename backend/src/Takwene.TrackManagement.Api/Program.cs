using Microsoft.EntityFrameworkCore;
using Takwene.TrackManagement.Infrastructure.Persistence;
using Takwene.TrackManagement.Application.Interfaces;
using Takwene.TrackManagement.Application.Services;
using Takwene.TrackManagement.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Database connection string 'DefaultConnection' is missing.");
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseNpgsql(connectionString).UseSeeding((context, _) => DatabaseSeeder.Seed(context)));
builder.Services.AddScoped<IArtistRepository, ArtistRepository>();
builder.Services.AddScoped<ArtistService>();

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
