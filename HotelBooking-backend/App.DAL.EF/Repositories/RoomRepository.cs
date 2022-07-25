using App.Contracts.DAL.Repositories;
using App.DAL.DTO;
using Base.Contracts.Base;
using Base.DAL.EF;
using Microsoft.EntityFrameworkCore;

namespace App.DAL.EF.Repositories;

public class RoomRepository: BaseEntityRepository<App.DAL.DTO.Room, App.Domain.Room, AppDbContext>, IRoomRepository
{
    public RoomRepository(AppDbContext dbContext, IMapper<Room, Domain.Room> mapper) : base(dbContext, mapper)
    {
    }

    public async Task<IEnumerable<Room>> GetAllAsync(Guid hotelId, bool noTracking = true)
    {
        var query = CreateQuery(noTracking);

        query = query.Where(r => r.HotelId == hotelId);
        
        return (await query.ToListAsync())
            .Select(x => Mapper.Map(x)!);
    }
}