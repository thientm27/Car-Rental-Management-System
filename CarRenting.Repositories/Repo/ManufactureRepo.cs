using CarRenting.BusinessObjects.Models;
using CarRenting.Repositories.Context;
using Microsoft.EntityFrameworkCore;

namespace CarRenting.Repositories.Repo;

public class ManufactureRepo : IManufactureRepo
{
    private readonly FUCarRentingManagementContext _context;

    public ManufactureRepo()
    {
        _context = new FUCarRentingManagementContext();
    }

    public async Task<List<Manufacturer>> GetAsync()
    {
        var entities = await _context.Manufacturers.ToListAsync();
        return entities;
   //   return entities.Select(dto => _mapper.Map<ManufacturerDto>(dto)).ToList();
    }

    public Task<Manufacturer?> AddAsync(Manufacturer customerDto)
    {
        throw new NotImplementedException();
    }

    public Task<Manufacturer?> GetByIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<Manufacturer?> UpdateAsync(Manufacturer customerDto)
    {
        throw new NotImplementedException();
    }

    public Task<bool> DeleteAsync(int customerId)
    {
        throw new NotImplementedException();
    }
}