namespace Takwene.TrackManagement.Application.DTOs;
using System.ComponentModel.DataAnnotations;

public class CreateArtistRequest
{
    public string Name { get; set; } = string.Empty;
    [EmailAddress]
    public string Email { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
}