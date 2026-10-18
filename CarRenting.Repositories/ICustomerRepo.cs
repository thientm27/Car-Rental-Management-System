using CarRenting.BusinessObjects.Models;

namespace CarRenting.Repositories;

public interface ICustomerRepo : IBaseRepo<Customer>
{
    public Task<Customer?> LoginAsync(string email, string password);
    public Task<Customer?> GetCustomerByEmail(string email);
}