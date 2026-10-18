namespace CarRenting.DTOs.Request;

public class NewRenting
{
    public RentingDto rentingDto { get; set; }
    public List<RentingDetailDto> rentingDetails { get; set; }
}