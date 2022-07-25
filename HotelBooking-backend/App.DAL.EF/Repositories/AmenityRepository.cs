using App.Contracts.DAL.Repositories;
using App.DAL.DTO;
using Base.Contracts.Base;
using Base.DAL.EF;
using Microsoft.EntityFrameworkCore;

namespace App.DAL.EF.Repositories;

public class AmenityRepository : BaseEntityRepository<App.DAL.DTO.Amenity, App.Domain.Amenity, AppDbContext>,
    IAmenityRepository
{
    public AmenityRepository(AppDbContext dbContext, IMapper<Amenity, Domain.Amenity> mapper) : base(dbContext, mapper)
    {
    }

    public async Task<IEnumerable<Amenity>> GetAllAsync(Guid hotelId, bool noTracking = true)
    {
        var query = CreateQuery(noTracking);

        query = query.Where(a => a.HotelId == hotelId);

        return (await query.ToListAsync())
            .Select(x => Mapper.Map(x)!);
    }
}