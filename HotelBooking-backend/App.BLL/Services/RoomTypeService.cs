using App.BLL.DTO;
using App.Contracts.BLL.Services;
using App.Contracts.DAL.Repositories;
using Base.BLL;
using Base.Contracts.Base;

namespace App.BLL.Services;

public class RoomTypeService : BaseEntityService<App.BLL.DTO.RoomType, App.DAL.DTO.RoomType, IRoomTypeRepository>,
    IRoomTypeService
{
    public RoomTypeService(IRoomTypeRepository repository, IMapper<RoomType, DAL.DTO.RoomType> mapper) : base(
        repository, mapper)
    {
    }

    public async Task<IEnumerable<RoomType>> GetAllAsync(Guid hotelId, bool noTracking = true)
    {
        return (await Repository.GetAllAsync(hotelId, noTracking)).Select(x => Mapper.Map(x)!);
    }

    public async Task<IEnumerable<RoomType>> SearchAvailableRooms(Guid hotelId, string startDate, string endDate,
        int noOfGuests, bool noTracking = true)
    {
        var startDateComponents = startDate.Split("-").Select(d => int.Parse(d)).ToList();
        var start = new DateOnly(startDateComponents[0], startDateComponents[1], startDateComponents[2]);

        var endDateComponents = endDate.Split("-").Select(d => int.Parse(d)).ToList();
        var end = new DateOnly(endDateComponents[0], endDateComponents[1], endDateComponents[2]);

        var numberOfNights = end.DayNumber - start.DayNumber;

        var rooms =
            (await Repository.SearchAvailableRooms(hotelId, start, end, noOfGuests, noTracking)).Select(x =>
                Mapper.Map(x)!).ToList();

        List<RoomType> availableRooms = new List<RoomType>();
        List<Booking> periodBookings = new List<Booking>();
        foreach (var room in rooms)
        {
            room.TotalPrice = numberOfNights * room.PricePerNight;

            foreach (var each in room.Bookings!)
            {
                // TODO: Kas siin kõik korras?
                if (each.DateFrom >= start && each.DateTo <= end)
                {
                    periodBookings.Add(each);
                }
            }
            
            if (periodBookings.Count < room.Count)
            {
                availableRooms.Add(room);
            }
            
        }

        return availableRooms;
    }

    public async Task CountRooms(RoomType roomType)
    {
        var roomTypeDb = await Repository.GetWithRoomsAsync(roomType.Id);

        if (roomTypeDb.Rooms == null)
        {
            return;
        }

        roomType.Count = roomTypeDb.Rooms.Count;

        Repository.Update(Mapper.Map(roomType)!);
    }
}