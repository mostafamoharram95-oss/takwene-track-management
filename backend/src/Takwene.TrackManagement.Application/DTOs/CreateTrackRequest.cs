namespace Takwene.TrackManagement.Application.DTOs;
using System.ComponentModel.DataAnnotations;

public class CreateTrackRequest
{
    public string Title { get; set; } = string.Empty;
    public int ArtistId { get; set; }
    [RegularExpression(@"^[A-Z]{2}[A-Z0-9]{3}\d{7}$", ErrorMessage = "Invalid ISRC format.")]
    public string Isrc { get; set; } = string.Empty;
    public string Genre { get; set; } = string.Empty;
    public DateOnly ReleaseDate { get; set; }
    public Domain.Enums.TrackStatus? Status { get; set; }
}