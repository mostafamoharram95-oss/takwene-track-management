namespace Takwene.TrackManagement.Application.DTOs;

public record TrackListResponse(
    int Id,
    string Title,
    int ArtistId,
    string ArtistName,
    string Genre,
    string Status
);