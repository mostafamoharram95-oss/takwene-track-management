using Microsoft.AspNetCore.Mvc;
using Takwene.TrackManagement.Application.DTOs;
using Takwene.TrackManagement.Application.Services;
using Takwene.TrackManagement.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
namespace Takwene.TrackManagement.Api.Controllers;

[ApiController]
[Route("api/tracks")]
public class TracksController : ControllerBase
{
    private readonly TrackService _trackService;

    public TracksController(TrackService trackService)
    {
        _trackService = trackService;
    }
    [HttpGet]
    public async Task<ActionResult<List<TrackListResponse>>> GetAll([FromQuery] int? artistId, [FromQuery] string? genre, [FromQuery] TrackStatus? status, CancellationToken cancellationToken)
    {
        var tracks = await _trackService.GetAllAsync(
            artistId,
            genre,
            status,
            cancellationToken
        );

        var response = tracks.Select(t => new TrackListResponse(
            t.Id,
            t.Title,
            t.ArtistId,
            t.Artist.Name,
            t.Genre,
            t.Status.ToString()
        )).ToList();

        return Ok(response);
    }
    [HttpPost]
    public async Task<ActionResult<TrackResponse>> Create([FromBody] CreateTrackRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var track = await _trackService.CreateAsync(request, cancellationToken);
            var response = new TrackResponse(
                track.Id,
                track.Title,
                track.ArtistId,
                track.Isrc,
                track.Genre,
                track.ReleaseDate,
                track.Status
            );
            return StatusCode(StatusCodes.Status201Created, response);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { error = ex.Message });
        }
    }
    [HttpGet("{id}")]
    public async Task<ActionResult<TrackDetailResponse>> GetById(int id, CancellationToken cancellationToken)
    {
        var track = await _trackService.GetByIdAsync(id, cancellationToken);
        if (track == null)
        {
            return NotFound();
        }
        var response = new TrackDetailResponse(
            track.Id,
            track.Title,
            track.ArtistId,
            track.Artist.Name,
            track.Isrc,
            track.Genre,
            track.ReleaseDate,
            track.Status.ToString(),
            track.TrackDistributions.Select(td => new TrackDistributionResponse(
                td.DspId,
                td.Dsp.Name,
                td.SubmittedAt,
                td.Status.ToString()
            )).ToList()
        );
        return Ok(response);
    }
    [Authorize]
    [HttpPost("{id}/distribute")]
    public async Task<IActionResult> Distribute(int id, [FromBody] DistributeTrackRequest request, CancellationToken cancellationToken)
    {
        try
        {
            await _trackService.DistributeAsync(id, request, cancellationToken);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { error = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { error = ex.Message });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }
    [HttpPatch("{id}/status")]
    public async Task<IActionResult> UpdateStatus(int id, [FromBody] UpdateTrackStatusRequest request, CancellationToken cancellationToken)
    {
        try
        {
            // Call TrackService.UpdateStatusAsync().
            await _trackService.UpdateStatusAsync(id, request, cancellationToken);
            // Return 204 on success.        
            return NoContent();
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { error = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { error = ex.Message });
        }
    }
}