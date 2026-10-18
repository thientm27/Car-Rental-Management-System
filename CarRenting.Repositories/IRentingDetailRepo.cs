

using CarRenting.BusinessObjects.Models;

namespace CarRenting.Repositories;

public interface IRentingDetailRepo : IBaseRepo<RentingDetail>
{
    public Task<List<int>> GetCarAlreadyRented(DateTime startDay, DateTime endDay);
    public Task<List<RentingDetail>?> GetsByIdAsync(int id);
}