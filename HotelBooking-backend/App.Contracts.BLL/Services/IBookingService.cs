using App.BLL.DTO;
using App.Contracts.DAL.Repositories;
using Base.Contracts.BLL;

namespace App.Contracts.BLL.Services;

public interface IBookingService: IEntityService<App.BLL.DTO.Booking>, IBookingRepositoryCustom<App.BLL.DTO.Booking>
{
    Task<Booking?> GetBooking(Guid bookingId, bool noTracking = true);
    Task<IEnumerable<Booking>> SearchBookingsByEmail(string email, bool noTracking = true);
    Task<IEnumerable<Booking>> GetFutureBookings(Guid roomTypeId, bool noTracking = true);
    Task<List<string>> ValidateBooking(Booking booking, RoomType roomType);


}