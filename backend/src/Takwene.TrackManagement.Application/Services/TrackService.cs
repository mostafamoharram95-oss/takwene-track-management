using Takwene.TrackManagement.Application.DTOs;
using Takwene.TrackManagement.Application.Interfaces;
using Takwene.TrackManagement.Domain.Entities;

namespace Takwene.TrackManagement.Application.Services;

public class TrackService
{
    private readonly ITrackRepository _trackRepository;
    private readonly IArtistRepository _artistRepository;

    public TrackService(ITrackRepository trackRepository, IArtistRepository artistRepository)
    {
        _trackRepository = trackRepository;
        _artistRepository = artistRepository;
    }
    public async Task<Track> CreateAsync(CreateTrackRequest request, CancellationToken cancellationToken = default)
    {
        if (request.Status.HasValue && !Enum.IsDefined(request.Status.Value))
        {
            throw new ArgumentException("Invalid track status.");
        }
        if (string.IsNullOrWhiteSpace(request.Title))
        {
            throw new ArgumentException("Track title cannot be empty.");
        }
        if (string.IsNullOrWhiteSpace(request.Isrc))
        {
            throw new ArgumentException("Track ISRC cannot be empty.");
        }
        if (request.ArtistId <= 0)
        {
            throw new ArgumentException("Invalid artist ID.");
        }
        if (!await _artistRepository.ExistsAsync(request.ArtistId, cancellationToken))
        {
            throw new ArgumentException("The specified artist does not exist.");
        }
        if (string.IsNullOrWhiteSpace(request.Genre))
        {
            throw new ArgumentException("Track genre cannot be empty.");
        }
        if (request.ReleaseDate == default)
        {
            throw new ArgumentException("Track release date is required.");
        }
        if (request.ReleaseDate == default)
        {
            throw new ArgumentException("Track release date is required.");
        }
        var track = new Track
        {
            Title = request.Title.Trim(),
            Isrc = request.Isrc.Trim(),
            ArtistId = request.ArtistId,
            Genre = request.Genre.Trim(),
            Status = request.Status ?? Domain.Enums.TrackStatus.Draft,
            ReleaseDate = request.ReleaseDate
        };

        return await _trackRepository.AddAsync(track, cancellationToken);
    }
    public async Task<Track?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        if (id <= 0)
        {
            throw new ArgumentException("Invalid track ID.");
        }

        return await _trackRepository.GetByIdAsync(id, cancellationToken);
    }
    public Task<List<Track>> GetAllAsync(int? artistId, string? genre, Domain.Enums.TrackStatus? status, CancellationToken cancellationToken = default)
    {
        return _trackRepository.GetAllAsync(artistId, genre, status, cancellationToken);
    }
    public async Task DistributeAsync(int trackId, DistributeTrackRequest request, CancellationToken cancellationToken = default)
    {
        // 1. Validate DSP IDs.
        if (request.DspIds == null || request.DspIds.Distinct().Count() == 0)
        {
            throw new ArgumentException("At least one DSP ID must be provided.");
        }
        if (request.DspIds.Any(id => id <= 0))
        {
            throw new ArgumentException("DSP IDs must be positive integers.");
        }
        if (request.DspIds.Count != request.DspIds.Distinct().Count())
        {
            throw new ArgumentException("Duplicate DSP IDs are not allowed.");
        }
        // 2. Retrieve the track.
        var track = await _trackRepository.GetByIdAsync(trackId, cancellationToken);
        if (track == null)
        {
            throw new KeyNotFoundException("Track not found.");
        }
        if (track.Status == Domain.Enums.TrackStatus.Distributed)
        {
            throw new InvalidOperationException("Track has already been distributed.");
        }
        // 3. Retrieve the requested DSPs.
        var dsps = await _trackRepository.GetDspsByIdsAsync(request.DspIds, cancellationToken);
        if (dsps == null || dsps.Count != request.DspIds.Count)
        {
            throw new ArgumentException("One or more requested DSP IDs do not exist.");
        }
        // 4. Check for duplicate distributions.
        if (track.TrackDistributions.Any(td => request.DspIds.Contains(td.DspId)))
        {
            throw new InvalidOperationException("One or more DSPs have already been distributed for this track.");
        }
        // 5. Create distribution records.
        var newDistributions = dsps.Select(dsp => new TrackDistribution{TrackId = track.Id, DspId = dsp.Id, SubmittedAt = DateTime.UtcNow}).ToList();
        foreach (var distribution in newDistributions)
        {
            track.TrackDistributions.Add(distribution);
        }
        // 6. Update Track status.
        track.Status = Domain.Enums.TrackStatus.Submitted;
        // 7. Save changes.
        await _trackRepository.SaveChangesAsync(cancellationToken);
    }
    public async Task UpdateStatusAsync(int trackId, UpdateTrackStatusRequest request, CancellationToken cancellationToken = default)
    {
        // 1. Validate the requested status.
        if (!request.Status.HasValue || !Enum.IsDefined(request.Status.Value))
        {
            throw new ArgumentException("Invalid or missing track status.");
        }
        // 2. Retrieve the track using GetByIdAsync().
        var track = await _trackRepository.GetByIdAsync(trackId, cancellationToken);
        // 3. Throw KeyNotFoundException if it doesn't exist.
        if (track == null)
        {
            throw new KeyNotFoundException("Track not found.");
        }
        // 4. Update track.Status.
        track.Status = request.Status.Value;
        // 5. Call SaveChangesAsync().
        await _trackRepository.SaveChangesAsync(cancellationToken);
    }
}