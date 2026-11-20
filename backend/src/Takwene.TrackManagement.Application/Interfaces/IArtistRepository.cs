using Takwene.TrackManagement.Domain.Entities;

namespace Takwene.TrackManagement.Application.Interfaces;

public interface IArtistRepository
{
    Task<List<Artist>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<Artist> AddAsync(Artist artist, CancellationToken cancellationToken = default);
    Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default);
}