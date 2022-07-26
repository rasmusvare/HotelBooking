using App.DAL.DTO;
using Base.Contracts.DAL;

namespace App.Contracts.DAL.Repositories;

public interface IBookingRepository : IEntityRepository<Booking>, IBookingRepositoryCustom<Booking>
{
    Task<IEnumerable<Booking>> GetBookingsForRoomType(Booking booking, bool noTracking = true);
    Task<IEnumerable<Booking>> SearchBookingsByEmail(string email, bool noTracking = true);

}

public interface IBookingRepositoryCustom<TEntity>
{
    Task<IEnumerable<TEntity>> GetAllUserAsync(Guid userId, bool noTracking = true);
    Task<IEnumerable<TEntity>> GetAllHotelAsync(Guid hotelId, bool noTracking = true);
}