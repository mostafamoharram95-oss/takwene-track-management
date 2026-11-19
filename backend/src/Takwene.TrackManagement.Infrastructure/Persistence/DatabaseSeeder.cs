using Microsoft.EntityFrameworkCore;
using Takwene.TrackManagement.Domain.Entities;
using Takwene.TrackManagement.Domain.Enums;
namespace Takwene.TrackManagement.Infrastructure.Persistence;

public static class DatabaseSeeder
{
    public static void Seed(DbContext context)
    {
        var layla = context.Set<Artist>().Single(a => a.Email == "layla.mansour@example.com");
        var omar = context.Set<Artist>().Single(a => a.Email == "omar.haddad@example.com");
        var nour = context.Set<Artist>().Single(a => a.Email == "nour.elsayed@example.com");
        if (!context.Set<Track>().Any())
        {
            context.Set<Track>().AddRange(
                new Track { Title = "Desert Lights", Isrc = "USRC17607839", Status = TrackStatus.Draft, ArtistId = layla.Id, Genre = "Pop", ReleaseDate = new DateOnly(2023, 5, 15) },
                new Track { Title = "Midnight Cairo", Isrc = "USRC17607840", Status = TrackStatus.Submitted, ArtistId = layla.Id, Genre = "Electronic", ReleaseDate = new DateOnly(2023, 6, 20) },
                new Track { Title = "Amman Nights", Isrc = "USRC17607841", Status = TrackStatus.Distributed, ArtistId = omar.Id, Genre = "Hip-Hop", ReleaseDate = new DateOnly(2023, 7, 1) },
                new Track { Title = "Eastern Winds", Isrc = "USRC17607842", Status = TrackStatus.Draft, ArtistId = omar.Id, Genre = "Jazz", ReleaseDate = new DateOnly(2023, 8, 1) },
                new Track { Title = "City of Stars", Isrc = "USRC17607843", Status = TrackStatus.Submitted, ArtistId = nour.Id, Genre = "R&B", ReleaseDate = new DateOnly(2023, 9, 1) },
                new Track { Title = "Atlas Dreams", Isrc = "USRC17607844", Status = TrackStatus.Distributed, ArtistId = nour.Id, Genre = "Indie", ReleaseDate = new DateOnly(2023, 10, 1) },
                new Track { Title = "Golden Horizon", Isrc = "USRC17607845", Status = TrackStatus.Distributed, ArtistId = layla.Id, Genre = "Pop", ReleaseDate = new DateOnly(2023, 11, 1) },
                new Track { Title = "Tomorrow's Echo", Isrc = "USRC17607846", Status = TrackStatus.Submitted, ArtistId = omar.Id, Genre = "Electronic", ReleaseDate = new DateOnly(2023, 12, 1) }
            );
            context.SaveChanges();
        }

        if (!context.Set<DSP>().Any())
        {
            context.Set<DSP>().AddRange(
                new DSP { Name = "Spotify" },
                new DSP { Name = "Apple Music" },
                new DSP { Name = "YouTube Music" }
            );
            context.Set<Artist>().AddRange(
                new Artist { Name = "Layla Mansour", Email = "layla.mansour@example.com", Country = "Egypt" },
                new Artist { Name = "Omar Haddad", Email = "omar.haddad@example.com", Country = "Jordan" },
                new Artist { Name = "Nour Elsayed", Email = "nour.elsayed@example.com", Country = "Morocco" }
            );
            context.SaveChanges();
        }
    }
}