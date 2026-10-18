using CarRenting.BusinessObjects.Models;
using CarRenting.Repositories.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace CarRenting.Repositories.Repo
{
    public class CustomerRepo : ICustomerRepo
    {
        private readonly FUCarRentingManagementContext _context;

        public CustomerRepo()
        {
            _context = new FUCarRentingManagementContext();
          
        }

        public async Task<Customer?> LoginAsync(string email, string password)
        {     
            // User check
            var loginCustomer = await _context.Customers.FirstOrDefaultAsync(od =>
                od.Email.ToLower().Equals(email.ToLower()) && od.Password.Equals(password)
                && od.CustomerStatus != 0);// not deleted account
            if (loginCustomer == null)
            {
                return null;
            }

            return loginCustomer;
        }
        public async Task<Customer?> AddAsync(Customer customerDto)
        {
            var existedEmail = await GetCustomerByEmail(customerDto.Email);
            if (existedEmail != null && existedEmail.CustomerStatus != 0) // not deleted -> dulicated 
            {
                return null;
            }else if (existedEmail != null && existedEmail.CustomerStatus == 0) //deleted
            {
                var reslut = await UpdateAsync(customerDto);
                return reslut;
            }
            var rEntry = await _context.Customers.AddAsync(customerDto);
            await _context.SaveChangesAsync();
            return rEntry.Entity;
        }

        public async Task<Customer?> GetByIdAsync(int id)
        {
            var customer = await _context.Customers.FirstOrDefaultAsync(od => od.CustomerId == id);
            if (customer == null)
            {
                return null;
            }

            return customer;
        }

        public async Task<Customer?> UpdateAsync(Customer customerDto)
        {
            var existedEmail = await GetCustomerByEmail(customerDto.Email);

            if (existedEmail.CustomerId != customerDto.CustomerId && existedEmail != null && existedEmail.CustomerStatus != 0)
            {
                return null; // new email duplicated
            }

            var customer = _context.Customers.FirstOrDefault(od => od.CustomerId == customerDto.CustomerId);
            if (customer != null)
            {
                _context.Entry(customer).CurrentValues.SetValues(customerDto);
                await _context.SaveChangesAsync();
                return customerDto;
            }

            return null;
        }

        public async Task<List<Customer>> GetAsync()
        {
            var customers = await _context.Customers.ToListAsync();
            return customers;
        }

        public async Task<bool> DeleteAsync(int customerId)
        {
            var existingCustomer = await _context.Customers.FirstOrDefaultAsync(od => od.CustomerId == customerId);
            if (existingCustomer != null)
            {
                existingCustomer.CustomerStatus = 0;
                await _context.SaveChangesAsync();
                return true;
            }

            return false;
        }

        public async Task<Customer?> GetCustomerByEmail(string email)
        {
            return await  _context.Customers.FirstOrDefaultAsync(od =>
             od.Email.ToLower().Equals(email.ToLower()));
        }

        public Task<List<RentingTransaction>?> GetByIdCustomerAsync(int id)
        {
            throw new NotImplementedException();
        }
    }

    public class AdminAccountInfo
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}