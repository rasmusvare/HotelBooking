using App.Contracts.DAL.Repositories;
using App.DAL.EF.Mappers;
using App.Domain;
using AutoMapper;
using Base.Contracts.Base;
using Base.DAL.EF;
using Microsoft.EntityFrameworkCore;
using Booking = App.DAL.DTO.Booking;

namespace App.DAL.EF.Repositories;

public class BookingRepository : BaseEntityRepository<App.DAL.DTO.Booking, App.Domain.Booking, AppDbContext>,
    IBookingRepository
{
    public BookingRepository(AppDbContext dbContext, IMapper<Booking, Domain.Booking> mapper) : base(dbContext, mapper)
    {
    }

    public async Task<IEnumerable<Booking>> GetAllUserAsync(Guid userId, bool noTracking = true)
    {
        var query = CreateQuery(noTracking);

        // query = query.Where(a => a.AppUserId == userId);

        return (await query.ToListAsync())
            .Select(x => Mapper.Map(x)!);
    }

    public async Task<IEnumerable<Booking>> GetAllHotelAsync(Guid hotelId, bool noTracking = true)
    {
        var query = CreateQuery(noTracking);

        query = query
            .Include(b => b.Guests)
            .Where(b => b.HotelId == hotelId);

        return (await query.ToListAsync())
            .Select(x => Mapper.Map(x)!);
    }

    public async Task<IEnumerable<Booking>> GetBookingsForRoomType(Booking booking, bool noTracking = true)
    {
        var query = CreateQuery(noTracking);
        
        var bookings = await query
            .Where(b => b.RoomTypeId == booking.RoomTypeId && b.DateFrom >= booking.DateFrom &&
                        b.DateTo <= booking.DateTo)
            .ToListAsync();

        return bookings.Select(x=>Mapper.Map(x)!);
    }

    public async Task<IEnumerable<Booking>> SearchBookingsByEmail(string email, bool noTracking = true)
    {
        var query = CreateQuery(noTracking);

        // var bookings = await query
        //     .Include(b => b.Guests)
        //     .SelectMany(b=>b.Guests!
        //         .Where(g=>g.Email==email && g.IsBookingOwner))
        //     .Select(g=>g.Booking!)
        //     .ToListAsync();

        var bookings = await query
            .Include(b => b.Guests)
            .Where(b => b.Guests!.Any(g => g.IsBookingOwner && g.Email == email))
            .ToListAsync();

        return bookings.Select(x => Mapper.Map(x)!);
    }

    public override async Task<Booking?> FirstOrDefaultAsync(Guid id, bool noTracking = true)
    {
        var query = CreateQuery(noTracking);

        var booking = await query
            .Include(b => b.Guests)
            .FirstOrDefaultAsync(b => b.Id == id);

        return Mapper.Map(booking);
        // return base.FirstOrDefaultAsync(id, noTracking);
    }


    public async Task<decimal> GetPricePerNight(Guid roomTypeId)
    {
        var roomType = await RepoDbContext.RoomTypes.FirstOrDefaultAsync(r => r.Id == roomTypeId);

        if (roomType == null)
        {
            return 0;
        }

        return roomType.PricePerNight;
    }

    // public override Booking Add(Booking booking)
    // {.
    //     RepoDbContext.RoomTypes.FirstOrDefault(booking.RoomTypeId);
    // }
    //     
    //     foreach (var guest in booking.Guests)
    //     {
    //         RepoDbContext.Guests.Add(_guestMapper.Map(guest)!);
    //         RepoDbContext.SaveChangesAsync();
    //     }
    //     
    //     Domain.Booking domainBooking = Mapper.Map(booking)!;
    //     
    //
    //     domainRoomType.Amenities = domainRoomType.Amenities!.Select(e =>
    //         {
    //             var amentity = RepoDbContext.Amenities.FirstOrDefault(f => f == e);
    //             return amentity;
    //         })
    //         .Where(e => e != null && e.HotelId == domainRoomType.HotelId)
    //         .ToList()!;
    //     
    //     return Mapper.Map(RepoDbSet.Add(domainRoomType).Entity)!;
    // }
}