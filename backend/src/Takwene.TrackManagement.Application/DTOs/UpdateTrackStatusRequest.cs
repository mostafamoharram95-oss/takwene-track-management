namespace Takwene.TrackManagement.Application.DTOs;
using System.ComponentModel.DataAnnotations;

public class UpdateTrackStatusRequest
{
    public Domain.Enums.TrackStatus? Status { get; set; }
}