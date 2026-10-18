using CarRenting.BusinessObjects.Models;
using CarRenting.Repositories.Context;
using Microsoft.EntityFrameworkCore;

namespace CarRenting.Repositories.Repo;

public class SupplierRepo : ISupplierRepo
{
    private readonly FUCarRentingManagementContext _context;

    public SupplierRepo()
    {
        _context = new FUCarRentingManagementContext();
    }

    public async Task<List<Supplier>> GetAsync()
    {
        var entities = await _context.Suppliers.ToListAsync();
        return entities;
      //  return entities.Select(dto => _mapper.Map<SupplierDto>(dto)).ToList();
    }

    public Task<Supplier?> AddAsync(Supplier customerDto)
    {
        throw new NotImplementedException();
    }

    public Task<Supplier?> GetByIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<Supplier?> UpdateAsync(Supplier customerDto)
    {
        throw new NotImplementedException();
    }

    public Task<bool> DeleteAsync(int customerId)
    {
        throw new NotImplementedException();
    }
}