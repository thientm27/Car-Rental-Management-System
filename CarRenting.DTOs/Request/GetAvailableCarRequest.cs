namespace CarRenting.DTOs.Request;

public class GetAvailableCarRequest
{
    public DateTime StartDateTime { get; set; }
    public DateTime EndDateTime { get; set; }
}