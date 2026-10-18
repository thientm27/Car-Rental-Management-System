using CarRenting.BusinessObjects.Models;

namespace CarRenting.Repositories;

public interface IBaseRepo<TEntity> where TEntity : class
{
    public Task<List<TEntity>> GetAsync();
    public Task<TEntity?> AddAsync(TEntity customerDto);
    public Task<TEntity?> GetByIdAsync(int id);
    public Task<TEntity?> UpdateAsync(TEntity customerDto);
    public Task<bool> DeleteAsync(int customerId);
}