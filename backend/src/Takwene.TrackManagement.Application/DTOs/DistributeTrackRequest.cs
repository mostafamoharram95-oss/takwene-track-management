namespace Takwene.TrackManagement.Application.DTOs;
using System.ComponentModel.DataAnnotations;

public class DistributeTrackRequest
{
    public List<int> DspIds { get; set; } = new();
}