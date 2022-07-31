using App.Contracts.DAL.Repositories;
using App.DAL.DTO;
using Base.Contracts.Base;
using Base.DAL.EF;
using Microsoft.EntityFrameworkCore;

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

        return (await query.ToListAsync())
            .Select(x => Mapper.Map(x)!);
    }

    public async Task<IEnumerable<Booking>> GetAllHotelAsync(Guid hotelId, bool noTracking = true)
    {
        var query = CreateQuery(noTracking);

        query = query
            .Include(b => b.Guests)
            .Where(b => b.HotelId == hotelId)
            .OrderByDescending(b => b.DateFrom);

        return (await query.ToListAsync())
            .Select(x => Mapper.Map(x)!);
    }

    public async Task<IEnumerable<Booking>> GetBookingsForRoomType(Booking booking, bool noTracking = true)
    {
        var query = CreateQuery(noTracking);

        var bookings = await query
            .Where(b => b.RoomTypeId == booking.RoomTypeId
                        && (b.DateFrom > booking.DateFrom &&
                            b.DateFrom < booking.DateTo
                            || b.DateTo > booking.DateFrom &&
                            b.DateTo < booking.DateTo
                            || b.DateFrom <= booking.DateFrom
                            && b.DateTo >= booking.DateTo
                        )
            )
            .ToListAsync();

        return bookings.Select(x => Mapper.Map(x)!);
    }

    public async Task<IEnumerable<Booking>> SearchBookingsByEmail(string email, bool noTracking = true)
    {
        var query = CreateQuery(noTracking);

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

    public override Booking Update(Booking booking)
    {
        var query = CreateQuery();

        var bookingDb = FirstOrDefaultAsync(booking.Id).Result!;

        var guestsToDelete = bookingDb.Guests!.Where(g => !booking.Guests!.Select(x => x.Id).ToList().Contains(g.Id))
            .ToList();

        foreach (var guest in guestsToDelete)
        {
            var guestDb = RepoDbContext.Guests.First(g => g.Id == guest.Id);
            RepoDbContext.Guests.Remove(guestDb);
        }

        RepoDbContext.SaveChanges();

        return base.Update(booking);
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