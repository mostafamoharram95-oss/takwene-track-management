namespace Takwene.TrackManagement.Application.DTOs;

public record TrackDetailResponse(
    int Id,
    string Title,
    int ArtistId,
    string ArtistName,
    string Isrc,
    string Genre,
    DateOnly ReleaseDate,
    string Status,
    List<TrackDistributionResponse> Distributions
);