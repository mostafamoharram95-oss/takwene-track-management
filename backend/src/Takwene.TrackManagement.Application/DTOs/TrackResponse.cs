namespace Takwene.TrackManagement.Application.DTOs;

public record TrackResponse(
    int Id,
    string Title,
    int ArtistId,
    string Isrc,
    string Genre,
    DateOnly ReleaseDate,
    Domain.Enums.TrackStatus Status
);