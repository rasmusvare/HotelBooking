using App.DAL.DTO;
using Base.Contracts.DAL;

namespace App.Contracts.DAL.Repositories;

public interface IBookingRepository : IEntityRepository<Booking>, IBookingRepositoryCustom<Booking>
{
    Task<IEnumerable<Booking>> GetBookingsForRoomType(Booking booking, bool noTracking = true);
    Task<IEnumerable<Booking>> SearchBookingsByEmail(string email, bool noTracking = true);
    Task<IEnumerable<Booking>> GetFutureBookings(Guid roomTypeId, bool noTracking = true);
    Task<decimal> GetPricePerNight(Guid roomTypeId);


}

public interface IBookingRepositoryCustom<TEntity>
{
    Task<IEnumerable<TEntity>> GetAllUserAsync(Guid userId, bool noTracking = true);
    Task<IEnumerable<TEntity>> GetAllHotelAsync(Guid hotelId, bool noTracking = true);
}