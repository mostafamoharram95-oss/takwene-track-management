namespace Takwene.TrackManagement.Domain.Entities;
using Takwene.TrackManagement.Domain.Enums;

public interface ITrackRepository
{
    Task<Track> AddAsync(Track track, CancellationToken cancellationToken = default);
    Task<List<Track>> GetAllAsync(int? artistId, string? genre, TrackStatus? status, CancellationToken cancellationToken = default);
    Task<Track?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<List<DSP>> GetDspsByIdsAsync(List<int> dspIds, CancellationToken cancellationToken = default);
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
    Task<bool> IsrcExistsAsync(string isrc, CancellationToken cancellationToken = default);
}