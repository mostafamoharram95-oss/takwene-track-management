using Takwene.TrackManagement.Application.DTOs;
using Takwene.TrackManagement.Application.Interfaces;
using Takwene.TrackManagement.Domain.Entities;

namespace Takwene.TrackManagement.Application.Services;

public class ArtistService
{
    private readonly IArtistRepository _artistRepository;

    public ArtistService(IArtistRepository artistRepository)
    {
        _artistRepository = artistRepository;
    }
    public async Task<Artist> CreateAsync(CreateArtistRequest request, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(request.Name))
        {
            throw new ArgumentException("Artist name cannot be empty.");
        }
        if (string.IsNullOrWhiteSpace(request.Email))
        {
            throw new ArgumentException("Artist email cannot be empty.");
        }
        if (string.IsNullOrWhiteSpace(request.Country))
        {
            throw new ArgumentException("Artist country cannot be empty.");
        }
        var artist = new Artist
        {
            Name = request.Name.Trim(),
            Email = request.Email.Trim(),
            Country = request.Country.Trim()
        };
        return await _artistRepository.AddAsync(artist, cancellationToken);
    }
    public Task<List<Artist>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return _artistRepository.GetAllAsync(cancellationToken);
    }
}