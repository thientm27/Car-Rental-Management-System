using CarRenting.BusinessObjects.Models;
using CarRenting.Repositories.Context;
using Microsoft.EntityFrameworkCore;

namespace CarRenting.Repositories.Repo;

public class CarInformationRepo : ICarInformationRepo
{
    private readonly FUCarRentingManagementContext _context;

    public CarInformationRepo()
    {
        _context = new FUCarRentingManagementContext();
 
    }

    public async Task<List<CarInformation>> GetAsync()
    {
        var entities = await _context.CarInformations
            .Include(o => o.Manufacturer)
            .Include(o => o.Supplier)
            .ToListAsync();
        return entities;
    }
    public async Task<List<CarInformation>> GetCarAvailable()
    {
        var entities = await _context.CarInformations
            .Include(o => o.Manufacturer)
            .Include(o => o.Supplier)
            .Where(o => o.CarStatus != 0)
            .ToListAsync();
        return entities;
    }
    public async Task<CarInformation?> AddAsync(CarInformation data)
    {
     
        // var maxId = await _context.CarInformations.MaxAsync(o => o.CarId);
        // entity.CarId = maxId + 1;
        var rEntry = await _context.CarInformations.AddAsync(data);
        await _context.SaveChangesAsync();
        return rEntry.Entity;
    }

    public async Task<CarInformation?> GetByIdAsync(int id)
    {
        var entity = await _context.CarInformations
            .Include(o => o.Manufacturer)
            .Include(o => o.Supplier)
            .FirstOrDefaultAsync(od => od.CarId == id);
        if (entity == null)
        {
            return null;
        }

        return entity;
    }

    public async Task<CarInformation?> UpdateAsync(CarInformation dataDto)
    {
        var entity = _context.CarInformations.FirstOrDefault(od => od.CarId == dataDto.CarId);
        if (entity != null)
        {
            _context.Entry(entity).CurrentValues.SetValues(dataDto);
            await _context.SaveChangesAsync();
            return dataDto;
        }

        return null;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var carInformation = await _context.CarInformations.FirstOrDefaultAsync(od => od.CarId == id);
        if (carInformation != null)
        {
            // _context.CarInformations.Remove(carInformation);
            // await _context.SaveChangesAsync();
            carInformation.CarStatus = 0;
            await _context.SaveChangesAsync();
            return true;
        }

        return false;
    }
}