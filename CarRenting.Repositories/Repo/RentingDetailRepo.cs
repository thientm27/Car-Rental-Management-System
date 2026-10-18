using CarRenting.BusinessObjects.Models;
using CarRenting.Repositories.Context;
using Microsoft.EntityFrameworkCore;

namespace CarRenting.Repositories.Repo;

public class RentingDetailRepo : IRentingDetailRepo
{
    private readonly FUCarRentingManagementContext _context;

    public RentingDetailRepo()
    {
        _context = new FUCarRentingManagementContext();
      
    }

    public async Task<List<RentingDetail>> GetAsync()
    {
        var entities = await _context.RentingDetails
            .Include(o => o.Car)
            .ToListAsync();
        return entities;
     //   return entities.Select(dto => _mapper.Map<RentingDetailDto>(dto)).ToList();
    }

    public async Task<RentingDetail?> AddAsync(RentingDetail data)
    {

        var rEntry = await _context.RentingDetails.AddAsync(data);
        await _context.SaveChangesAsync();

        return data;
    }


    public async Task<RentingDetail?> UpdateAsync(RentingDetail data)
    {
        var entity = _context.RentingDetails.FirstOrDefault(od => od.CarId == data.CarId);
        if (entity != null)
        {
            _context.Entry(entity).CurrentValues.SetValues(data);
            await _context.SaveChangesAsync();
            return data;
        }

        return null;
    }

    public async Task<List<RentingDetail>?> GetsByIdAsync(int id)
    {
        var entities = await _context.RentingDetails
            .Include(o => o.Car)
            .Where(od => od.RentingTransactionId == id).ToListAsync();
        if (entities == null || entities.Count == 0)
        {
            return null;
        }

        return entities;
       // return entities.Select(dto => _mapper.Map<RentingDetailDto>(dto)).ToList();
    }


    [Obsolete("This method cannot use")]
    public async Task<RentingDetail?> GetByIdAsync(int id)
    {
        var entity = await _context.RentingDetails
            .Include(o => o.Car)
            .FirstOrDefaultAsync(od => od.RentingTransactionId == id);
        if (entity == null)
        {
            return null;
        }

        return entity;
    }

    [Obsolete("This method cannot use")]
    public async Task<bool> DeleteAsync(int id)
    {
        var carInformation = await _context.RentingDetails.FirstOrDefaultAsync(od => od.RentingTransactionId == id);
        if (carInformation != null)
        {
            _context.RentingDetails.Remove(carInformation);
            await _context.SaveChangesAsync();
            return true;
        }

        return false;
    }

    public async Task<List<int>> GetCarAlreadyRented(DateTime startDay, DateTime endDay)
    {
        return await _context.RentingDetails
            .Where(o => o.RentingTransaction.RentingStatus != 0
                        && ((startDay >= o.StartDate && startDay <= o.EndDate)
                            || (o.StartDate >= startDay && o.EndDate <= endDay)
                            || (o.StartDate <= endDay && o.EndDate >= endDay))
            )
            .Select(o => o.CarId)
            .ToListAsync();
    }
}