namespace Takwene.TrackManagement.Application.DTOs;

public record TrackDistributionResponse(
    int DspId,
    string DspName,
    DateTime SubmittedAt,
    string Status
);