using CarRenting.BusinessObjects.Models;

namespace CarRenting.Repositories;

public interface ICarInformationRepo : IBaseRepo<CarInformation>
{
    public Task<List<CarInformation>> GetCarAvailable();
}