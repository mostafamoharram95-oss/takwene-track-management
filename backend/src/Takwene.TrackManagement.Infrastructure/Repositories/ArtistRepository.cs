
using Microsoft.EntityFrameworkCore;
using Takwene.TrackManagement.Application.Interfaces;
using Takwene.TrackManagement.Domain.Entities;
using Takwene.TrackManagement.Infrastructure.Persistence;

namespace Takwene.TrackManagement.Infrastructure.Repositories;

public class ArtistRepository : IArtistRepository
{
    private readonly ApplicationDbContext _context;

    public ArtistRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    public async Task<Artist> AddAsync(Artist artist, CancellationToken cancellationToken = default)
    {
        _context.Artists.Add(artist);
        await _context.SaveChangesAsync(cancellationToken);
        return artist;
    }
    public async Task<List<Artist>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Artists.AsNoTracking().ToListAsync(cancellationToken);
    }
}