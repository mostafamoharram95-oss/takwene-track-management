using Microsoft.EntityFrameworkCore;
using Takwene.TrackManagement.Infrastructure.Persistence;
using Takwene.TrackManagement.Application.Interfaces;
using Takwene.TrackManagement.Application.Services;
using Takwene.TrackManagement.Infrastructure.Repositories;
using Takwene.TrackManagement.Domain.Entities;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Database connection string 'DefaultConnection' is missing.");
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseNpgsql(connectionString).UseSeeding((context, _) => DatabaseSeeder.Seed(context)));
builder.Services.AddScoped<IArtistRepository, ArtistRepository>();
builder.Services.AddScoped<ITrackRepository, TrackRepository>();
builder.Services.AddScoped<ArtistService>();
builder.Services.AddScoped<TrackService>();

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddAuthentication("Bearer").AddJwtBearer();
builder.Services.AddAuthorization();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AngularDev", policy =>
        policy.WithOrigins("http://localhost:4200")
              .AllowAnyHeader()
              .AllowAnyMethod());
});
var app = builder.Build();
app.UseCors("AngularDev");
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
