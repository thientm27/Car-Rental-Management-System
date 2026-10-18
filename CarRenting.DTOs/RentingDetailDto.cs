namespace CarRenting.DTOs;

public class RentingDetailDto
{
    public int RentingTransactionId { get; set; }
    public int CarId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public decimal? Price { get; set; }
    
    public string? CarName { get; set; }
    public virtual CarInformationDto? Car { get; set; }
}