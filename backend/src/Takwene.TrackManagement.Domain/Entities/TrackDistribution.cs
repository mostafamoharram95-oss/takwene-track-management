namespace Takwene.TrackManagement.Domain.Entities;
using Takwene.TrackManagement.Domain.Enums;
public class TrackDistribution
{
    public int Id { get; set; }
    public int TrackId { get; set; }
    public Track Track { get; set; } = null!;
    public int DspId { get; set; }
    public DSP Dsp { get; set; } = null!;
    public DateTime SubmittedAt { get; set; }
    public DistributionStatus Status { get; set; } = DistributionStatus.Pending;
}