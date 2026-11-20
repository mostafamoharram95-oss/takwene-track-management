using Microsoft.EntityFrameworkCore;
using Takwene.TrackManagement.Application.Interfaces;
using Takwene.TrackManagement.Domain.Entities;
using Takwene.TrackManagement.Domain.Enums;
using Takwene.TrackManagement.Infrastructure.Persistence;
using Npgsql;
namespace Takwene.TrackManagement.Infrastructure.Repositories;

public class TrackRepository : ITrackRepository
{
    private readonly ApplicationDbContext _context;

    public TrackRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    public async Task<Track> AddAsync(Track track, CancellationToken cancellationToken = default)
    {
        _context.Tracks.Add(track);
        try
        {
            await _context.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException ex) when (
            ex.InnerException is PostgresException pg &&
            pg.SqlState == PostgresErrorCodes.UniqueViolation &&
            pg.ConstraintName == "IX_Tracks_Isrc")
        {
            throw new InvalidOperationException(
                "A track with this ISRC already exists.", ex);
        }
        return track;
    }
    public Task<List<Track>> GetAllAsync(int? artistId, string? genre, TrackStatus? status, CancellationToken cancellationToken = default)
    {
        return _context.Tracks.Where(t => (!artistId.HasValue || t.ArtistId == artistId.Value) && (string.IsNullOrEmpty(genre) || t.Genre == genre) && (!status.HasValue || t.Status == status.Value)).Include(t => t.Artist).ToListAsync(cancellationToken);
    }
    public Task<Track?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return _context.Tracks.Include(t => t.Artist).Include(t => t.TrackDistributions).ThenInclude(td => td.Dsp).FirstOrDefaultAsync(t => t.Id == id, cancellationToken);
    }

    public Task<List<DSP>> GetDspsByIdsAsync(List<int> dspIds, CancellationToken cancellationToken = default)
    {
        return _context.DSPs.Where(dsp => dspIds.Contains(dsp.Id)).ToListAsync(cancellationToken);
    }

    public Task<bool> IsrcExistsAsync(string isrc, CancellationToken cancellationToken = default)
    {
        return _context.Tracks.AnyAsync(t => t.Isrc == isrc, cancellationToken);
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            await _context.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException ex) when (
            ex.InnerException is PostgresException pg &&
            pg.SqlState == PostgresErrorCodes.UniqueViolation &&
            pg.ConstraintName == "IX_Tracks_Isrc")
        {
            throw new InvalidOperationException(
                "A track with this ISRC already exists.", ex);
        }
    }
}
