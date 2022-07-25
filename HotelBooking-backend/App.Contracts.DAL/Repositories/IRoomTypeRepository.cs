using App.DAL.DTO;
using Base.Contracts.DAL;

namespace App.Contracts.DAL.Repositories;

public interface IRoomTypeRepository : IEntityRepository<RoomType>, IRoomTypeRepositoryCustom<RoomType>
{
    Task<RoomType> GetWithRoomsAsync(Guid roomTypeId, bool noTracking = true);
    Task<IEnumerable<RoomType>> SearchAvailableRooms(Guid hotelId, DateOnly startDate, DateOnly endDate, int noOfGuests,
        bool noTracking = true);
}

public interface IRoomTypeRepositoryCustom<TEntity>
{
    Task<IEnumerable<TEntity>> GetAllAsync(Guid hotelId, bool noTracking = true);

   
}