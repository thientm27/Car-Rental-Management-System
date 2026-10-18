using CarRenting.BusinessObjects.Models;

using CarRenting.Repositories.Context;
using Microsoft.EntityFrameworkCore;

namespace CarRenting.Repositories.Repo;

public class RentingRepo : IRentingRepo
{
    private readonly FUCarRentingManagementContext _context;

    public RentingRepo()
    {
        _context = new FUCarRentingManagementContext();
    
    }

    public async Task<List<RentingTransaction>> GetAsync()
    {
        var entities = await _context.RentingTransactions
            .Include(o => o.Customer)
            .ToListAsync();
        return entities;
      //  return entities.Select(dto => _mapper.Map<RentingDto>(dto)).ToList();
    }

    public async Task<RentingTransaction?> AddAsync(RentingTransaction newData)
    {
        var maxId = await _context.RentingTransactions.MaxAsync(o => o.RentingTransationId);
        newData.RentingTransationId = maxId + 1;
        var rEntry = await _context.RentingTransactions.AddAsync(newData);
        await _context.SaveChangesAsync();
        return rEntry.Entity;
    }

    public async Task<RentingTransaction?> GetByIdAsync(int id)
    {
        var entity = await _context.RentingTransactions
            .Include(o => o.Customer)
            .FirstOrDefaultAsync(od => od.RentingTransationId == id);
        if (entity == null)
        {
            return null;
        }

        return entity;
    }

    public async Task<RentingTransaction?> UpdateAsync(RentingTransaction dataDto)
    {
        var entity =
            _context.RentingTransactions.FirstOrDefault(od => od.RentingTransationId == dataDto.RentingTransationId);
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
        var carInformation = await _context.RentingTransactions.FirstOrDefaultAsync(od => od.RentingTransationId == id);
        if (carInformation != null)
        {
            carInformation.RentingStatus = 0;
            await _context.SaveChangesAsync();
            return true;
        }
        return false;
    }

    public async Task<RentingTransaction?> AddWithDetailsAsync(RentingTransaction data)
    {
        //RentingDetailRepo rentingDetailRepo = new RentingDetailRepo();
        //data.rentingDto.RentingStatus = 1;
        //var renting = await AddAsync(data.rentingDto);
        //if (renting == null)
        //{
        //    return null;
        //}

        //foreach (var detail in data.rentingDetails)
        //{
        //    detail.RentingTransactionId = renting.RentingTransationId;
        //    await rentingDetailRepo.AddAsync(detail);
        //}

        return null;
    }

    public async Task<List<RentingTransaction>?> GetByIdCustomerAsync(int id)
    {
        var entities = await _context.RentingTransactions
            .Include(o => o.Customer)
            .Where(od => od.CustomerId == id && od.RentingStatus != 0).ToListAsync();
        if (entities == null || entities.Count == 0)
        {
            return null;
        }

        return entities;
       // return entities.Select(dto => _mapper.Map<RentingDto>(dto)).ToList();
    }
}