using System.ComponentModel.DataAnnotations;

namespace CarRenting.DTOs;

public class RentingDto
{
    public int RentingTransationId { get; set; }
    [DataType(DataType.Date)] public DateTime? RentingDate { get; set; }
    public decimal? TotalPrice { get; set; }
    public int CustomerId { get; set; }
    public byte? RentingStatus { get; set; }

}