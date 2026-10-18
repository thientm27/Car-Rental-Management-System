

using CarRenting.BusinessObjects.Models;

namespace CarRenting.Repositories;

public interface IRentingRepo : IBaseRepo<RentingTransaction>
{
    public Task<RentingTransaction?> AddWithDetailsAsync(RentingTransaction data);
    public Task<List<RentingTransaction>> GetByIdCustomerAsync(int id);
}