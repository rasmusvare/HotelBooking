using App.Contracts.DAL.Repositories;
using App.DAL.DTO;
using Base.Contracts.Base;
using Base.DAL.EF;
using Microsoft.EntityFrameworkCore;

namespace App.DAL.EF.Repositories;

public class RoomTypeRepository : BaseEntityRepository<App.DAL.DTO.RoomType, App.Domain.RoomType, AppDbContext>,
    IRoomTypeRepository
{
    public RoomTypeRepository(AppDbContext dbContext, IMapper<RoomType, Domain.RoomType> mapper) : base(dbContext,
        mapper)
    {
    }

    public async Task<IEnumerable<RoomType>> GetAllAsync(Guid hotelId, bool noTracking = true)
    {
        var query = CreateQuery(noTracking);

        query = query
            .Include(r => r.Amenities)
            .Where(r => r.HotelId == hotelId);

        return (await query.ToListAsync())
            .Select(x => Mapper.Map(x)!);
    }

    public async Task<IEnumerable<RoomType>> SearchAvailableRooms(Guid hotelId, DateOnly startDate, DateOnly endDate,
        int noOfGuests, bool noTracking = true)
    {
        // var startDateComponents = startDate.Split("-").Select(d=>int.Parse(d)).ToList();
        // var start = new DateOnly(startDateComponents[0], startDateComponents[1], startDateComponents[2]);
        //
        // var endDateComponents = endDate.Split("-").Select(d=>int.Parse(d)).ToList();
        // var end = new DateOnly(endDateComponents[0], endDateComponents[1], endDateComponents[2]);

        var query = CreateQuery(noTracking);

        var availableRoomTypes = await query
            .Where(r => r.HotelId == hotelId)
            .Include(r => r.Amenities)
            .Include(r => r.Bookings!.Where(b =>
                b.DateFrom > startDate &&
                b.DateFrom < endDate
                || b.DateTo > startDate &&
                b.DateTo < endDate
                || b.DateFrom <= startDate
                && b.DateTo >= endDate
            ))
            .ToListAsync();


        return availableRoomTypes.Where(r =>
                r.HotelId == hotelId
                && r.Bookings?.Count < r.Count && r.NumberOfBeds >= noOfGuests)
            .Select(x => Mapper.Map(x)!);
    }

    public async Task<RoomType> GetWithRoomsAsync(Guid roomTypeId, bool noTracking = true)
    {
        var query = CreateQuery(noTracking);

        var roomType = await query
            .Include(r => r.Rooms)
            .Include(r => r.Amenities)
            .FirstOrDefaultAsync(r => r.Id == roomTypeId);

        return Mapper.Map(roomType)!;
    }

    public override RoomType Add(RoomType roomType)
    {
        Domain.RoomType domainRoomType = Mapper.Map(roomType)!;

        domainRoomType.Amenities = domainRoomType.Amenities!.Select(e =>
            {
                var amenity = RepoDbContext.Amenities.FirstOrDefault(f => f == e);
                return amenity;
            })
            .Where(a => a != null && a.HotelId == domainRoomType.HotelId)
            .ToList()!;

        return Mapper.Map(RepoDbSet.Add(domainRoomType).Entity)!;
    }

    public override RoomType Update(RoomType roomType)
    {
        var domainRoomType = RepoDbSet
            .Include(r => r.Amenities)
            .First(x => x.Id == roomType.Id);

        var updatedRoomType = Mapper.Map(roomType)!;

        foreach (var each in domainRoomType.Amenities!)
        {
            domainRoomType.Amenities.Remove(each);
            RepoDbContext.SaveChanges();
        }

        updatedRoomType.Amenities = updatedRoomType.Amenities!.Select(e =>
            {
                var amenity = RepoDbContext.Amenities.FirstOrDefault(a => a == e);
                return amenity;
            })
            .Where(a => a != null && a.HotelId == updatedRoomType.HotelId)
            .ToList()!;

        RepoDbContext.Entry(domainRoomType).State = EntityState.Detached;

        var updatedEntity = RepoDbSet.Update(updatedRoomType).Entity;
        return Mapper.Map(updatedEntity)!;
    }
}