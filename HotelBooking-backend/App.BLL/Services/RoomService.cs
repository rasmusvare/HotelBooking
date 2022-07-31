using App.BLL.DTO;
using App.Contracts.BLL.Services;
using App.Contracts.DAL.Repositories;
using Base.BLL;
using Base.Contracts.Base;

namespace App.BLL.Services;

public class RoomService : BaseEntityService<App.BLL.DTO.Room, App.DAL.DTO.Room, IRoomRepository>, IRoomService
{
    public RoomService(IRoomRepository repository, IMapper<Room, DAL.DTO.Room> mapper) : base(repository, mapper)
    {
    }

    public async Task<IEnumerable<Room>> GetAllAsync(Guid hotelId, bool noTracking = true)
    {
        return (await Repository.GetAllAsync(hotelId, noTracking)).Select(x => Mapper.Map(x)!).OrderBy(r=>r.RoomNumber);
    }

    public async Task<bool> CheckRoomNumberAvailability(Room room)
    {
        var hotelRooms = await Repository.GetAllAsync(room.HotelId);

        return hotelRooms.All(r => r.RoomNumber != room.RoomNumber);
    }
}