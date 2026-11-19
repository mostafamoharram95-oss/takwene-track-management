namespace Takwene.TrackManagement.Application.DTOs;

public record ArtistResponse(
    int Id,
    string Name,
    string Email,
    string Country
);