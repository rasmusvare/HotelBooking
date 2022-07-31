using System.Net.Mail;
using System.Threading.Channels;
using App.BLL.DTO;
using App.Contracts.BLL.Services;
using App.Contracts.DAL.Repositories;
using Base.BLL;
using Base.Contracts.Base;

namespace App.BLL.Services;

public class BookingService : BaseEntityService<App.BLL.DTO.Booking, App.DAL.DTO.Booking, IBookingRepository>,
    IBookingService
{
    public BookingService(IBookingRepository repository, IMapper<Booking, DAL.DTO.Booking> mapper) : base(repository,
        mapper)
    {
    }

    public async Task<IEnumerable<Booking>> GetAllUserAsync(Guid userId, bool noTracking = true)
    {
        return (await Repository.GetAllUserAsync(userId, noTracking)).Select(x => Mapper.Map(x)!);
    }

    public async Task<IEnumerable<Booking>> GetAllHotelAsync(Guid hotelId, bool noTracking = true)
    {
        var bookings = (await Repository.GetAllHotelAsync(hotelId, noTracking)).Select(x => Mapper.Map(x)!).ToList();

        foreach (var booking in bookings)
        {
            booking.BookingHolder = FindBookingHolder(booking.Guests!);
            booking.BookingHolderId = booking.BookingHolder.Id;
        }

        return bookings;
    }

    public async Task<IEnumerable<Booking>> SearchBookingsByEmail(string email, bool noTracking = true)
    {
        var bookings = (await Repository.SearchBookingsByEmail(email, noTracking)).Select(x => Mapper.Map(x)!).ToList();

        foreach (var booking in bookings)
        {
            booking.BookingHolder = FindBookingHolder(booking.Guests!);
            booking.BookingHolderId = booking.BookingHolder.Id;
        }

        return bookings;
    }

    public async Task<IEnumerable<Booking>> GetFutureBookings(Guid roomTypeId, bool noTracking = true)
    {
        return (await Repository.GetFutureBookings(roomTypeId, noTracking)).Select(x => Mapper.Map(x)!);
    }

    public async Task<Booking?> GetBooking(Guid bookingId, bool noTracking = true)
    {
        var booking = Mapper.Map(await Repository.FirstOrDefaultAsync(bookingId, noTracking));

        if (booking == null)
        {
            return null;
        }

        booking.BookingHolder = FindBookingHolder(booking.Guests!);
        booking.BookingHolderId = booking.BookingHolder.Id;

        return booking;
    }

    public async Task<List<string>> ValidateBooking(Booking booking, RoomType roomType)
    {
        var validationErrors = new List<string>();

        var bookingDb = await Repository.FirstOrDefaultAsync(booking.Id);
        var roomTypeBookings = (await Repository.GetBookingsForRoomType(Mapper.Map(booking)!)).ToList();
        var bookingsCount = roomTypeBookings.Count;

        if (bookingDb != null && roomTypeBookings.Any(b => b.Id == bookingDb.Id))
        {
            bookingsCount--;
        }
        
        if (booking.DateFrom < DateOnly.FromDateTime(DateTime.Now))
        {
            validationErrors.Add("Cannot add or change this booking as the booking start time is smaller than the current date");
        }

        if (booking.DateFrom > booking.DateTo)
        {
            validationErrors.Add("Booking start date cannot be bigger than booking end date");
        }
        
        if (booking.DateFrom == booking.DateTo)
        {
            validationErrors.Add("Booking start and end date cannot be on the same day");
        }

        if (booking.Guests?.Count == 0)
        {
            validationErrors.Add("No guests provided");
        }

        if (roomType.NumberOfBeds < booking.Guests?.Count)
        {
            validationErrors.Add("Too many guests for the selected room type");
        }

        if (bookingsCount >= roomType.Count)
        {
            validationErrors.Add("No rooms available for the selected time period");
        }

        foreach (var guest in booking.Guests!)
        {
            if (guest.FirstName == "" || guest.LastName == "")
            {
                validationErrors.Add("Please specify the first and last names for all guests");
            }

            if (guest.IdCode == "")
            {
                validationErrors.Add("Please specify the national identification number for all guests");
            }
        }
        
        return validationErrors;
    }

    public override Booking Add(Booking booking)
    {
        var numberOfNights = booking.DateTo.DayNumber - booking.DateFrom.DayNumber;
        booking.TotalPrice = numberOfNights * Repository.GetPricePerNight(booking.RoomTypeId).Result;
        
        return base.Add(booking);
    }

    public override Booking Update(Booking booking)
    {
        var numberOfNights = booking.DateTo.DayNumber - booking.DateFrom.DayNumber;
        booking.TotalPrice = numberOfNights * Repository.GetPricePerNight(booking.RoomTypeId).Result;
        
        return base.Update(booking);
    }

    private Guest FindBookingHolder(IEnumerable<Guest> guests)
    {
        return guests.First(g => g.IsBookingOwner);
    }
}