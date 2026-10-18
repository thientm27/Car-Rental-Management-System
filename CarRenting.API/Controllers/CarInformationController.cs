using CarRenting.API.Models;
using CarRenting.BusinessObjects.Models;
using CarRenting.Repositories;
using CarRenting.Repositories.Repo;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;

namespace CarRenting.API.Controllers;
[ApiController]
[Route("api/[controller]")]
public class CarInformationController : ControllerBase
{
    private readonly ICarInformationRepo _repository = new CarInformationRepo();

    [HttpGet]
    [EnableQuery]
    [Authorize(Roles = UserRoles.Admin)]
    public async Task<IActionResult> Get()
    {
        var reslut = await _repository.GetAsync();
        foreach (var item in reslut)
        {
            item.Manufacturer.CarInformations = null;
            item.Supplier.CarInformations = null;

        }
        return Ok(reslut);
    }

    [HttpGet("{id}")]
    [Authorize(Roles = UserRoles.Admin)]
    public async Task<IActionResult> GetById(int id)
    {
         var reslut = await _repository.GetByIdAsync(id);
        return Ok(reslut);
    }

    [HttpPost]
    [Authorize(Roles = UserRoles.Admin)]
    public async Task<IActionResult> Create(CarInformation newCar)
    {
        var reslut = await _repository.AddAsync(newCar);
        if (reslut == null)
        {
            return BadRequest();
        }
        return Ok(reslut);
    }
    
    [HttpPut("{id}")]
    [Authorize(Roles = UserRoles.Admin)]
    public async Task<IActionResult> Update(CarInformation newCar)
    {
        var reslut = await _repository.UpdateAsync(newCar);
        if (reslut == null)
        {
            return BadRequest();
        }
        return Ok(reslut);
    }
    
    [HttpDelete("{id}")]
    [Authorize(Roles = UserRoles.Admin)]
    public async Task<IActionResult> Delete(int id)
    {
        await _repository.DeleteAsync(id);
        return Ok();
    }

}