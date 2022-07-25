using App.BLL.DTO;
using App.Contracts.BLL.Services;
using App.Contracts.DAL.Repositories;
using Base.BLL;
using Base.Contracts.Base;

namespace App.BLL.Services;

public class AmenityService : BaseEntityService<App.BLL.DTO.Amenity, App.DAL.DTO.Amenity, IAmenityRepository>, IAmenityService
{
    public AmenityService(IAmenityRepository repository, IMapper<Amenity, DAL.DTO.Amenity> mapper) : base(repository, mapper)
    {
    }

    public async Task<IEnumerable<Amenity>> GetAllAsync(Guid hotelId, bool noTracking = true)
    {
        return (await Repository.GetAllAsync(hotelId, noTracking)).Select(x => Mapper.Map(x)!);
    }
}