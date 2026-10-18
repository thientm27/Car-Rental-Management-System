using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace CarRenting.DTOs;

public class CustomerDto : IdentityUser
{
    public int CustomerId { get; set; } 
    public string? CustomerName { get; set; }
    public string? Telephone { get; set; }
    public string Email { get; set; } = null!;
    [DataType(DataType.Date)] public DateTime? CustomerBirthday { get; set; }
    public byte? CustomerStatus { get; set; }
    public string? Password { get; set; }
    public object? Data { get; set; }
}