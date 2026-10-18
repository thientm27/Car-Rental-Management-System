
using CarRenting.API.Models;
using CarRenting.BusinessObjects.Models;
using CarRenting.Repositories;
using CarRenting.Repositories.Repo;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace CarRenting.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CustomerController : ControllerBase
{
    private readonly ICustomerRepo _repository = new CustomerRepo();
    

    [HttpGet]
    [Authorize(Roles = UserRoles.Admin)]
    public async Task<IActionResult> Get()
    {
        var reslut = await _repository.GetAsync();
        if (reslut == null)
        {
            return BadRequest();
        }
        return Ok(reslut);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var reslut = await _repository.GetByIdAsync(id);
        return Ok(reslut);
    }

    [HttpPost]
    [Authorize(Roles = UserRoles.Admin)]
    public async Task<IActionResult> Create(Customer customerDto)
    {
        var createdCustomer = await _repository.AddAsync(customerDto);
        return Ok(createdCustomer);
    }
    
    [HttpPut("{id}")]
    [Authorize(Roles = UserRoles.Admin)]
    public async Task<IActionResult> Update(Customer customerDto)
    {
        var result = await _repository.UpdateAsync(customerDto);
        return Ok(result);
    }
    
    [HttpDelete("{id}")]
    [Authorize(Roles = UserRoles.Admin)]
    public async Task<IActionResult> Delete(int id)
    {
        await _repository.DeleteAsync(id);
        return Ok();
    }
  
    }


