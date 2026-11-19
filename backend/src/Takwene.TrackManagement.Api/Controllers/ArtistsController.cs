using Microsoft.AspNetCore.Mvc;
using Takwene.TrackManagement.Application.DTOs;
using Takwene.TrackManagement.Application.Services;

namespace Takwene.TrackManagement.Api.Controllers;

[ApiController]
[Route("api/artists")]
public class ArtistsController : ControllerBase
{
    private readonly ArtistService _artistService;

    public ArtistsController(ArtistService artistService)
    {
        _artistService = artistService;
    }

    [HttpGet]
    public async Task<ActionResult<List<ArtistResponse>>> GetAll(CancellationToken cancellationToken)
    {
        var artists = await _artistService.GetAllAsync(cancellationToken);
        var response = artists.Select(a => new ArtistResponse(a.Id, a.Name, a.Email, a.Country)).ToList();
        return Ok(response);
    }
    [HttpPost]
    public async Task<ActionResult<ArtistResponse>> Create([FromBody] CreateArtistRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var artist = await _artistService.CreateAsync(request, cancellationToken);
            var response = new ArtistResponse(artist.Id, artist.Name, artist.Email, artist.Country);
            return StatusCode(StatusCodes.Status201Created, response);
        }
        catch (ArgumentException ex)
        {   
            return BadRequest(new { error = ex.Message });
        }
    }
}