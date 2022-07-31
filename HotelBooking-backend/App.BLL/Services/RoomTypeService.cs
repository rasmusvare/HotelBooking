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

    public async Task<IEnumerable<RoomType>> SearchAvailableRooms(SearchProperties searchProperties,
        bool noTracking = true)
    {
        var startDateComponents = searchProperties.StartDate.Split("-").Select(d => int.Parse(d)).ToList();
        var start = new DateOnly(startDateComponents[0], startDateComponents[1], startDateComponents[2]);

        var endDateComponents = searchProperties.EndDate.Split("-").Select(d => int.Parse(d)).ToList();
        var end = new DateOnly(endDateComponents[0], endDateComponents[1], endDateComponents[2]);

        var numberOfNights = end.DayNumber - start.DayNumber;

        var rooms = (await Repository.SearchAvailableRooms(searchProperties.HotelId, start, end,
            searchProperties.NoOfGuests,
            noTracking)).Select(x =>
            Mapper.Map(x)!).ToList();

        foreach (var room in rooms)
        {
            room.TotalPrice = numberOfNights * room.PricePerNight;
        }

        return rooms;
    }

    public List<string> ValidateSearch(SearchProperties searchProperties)
    {
        var validationErrors = new List<string>();

        var startDateComponents = searchProperties.StartDate.Split("-").Select(d => int.Parse(d)).ToList();
        var start = new DateOnly(startDateComponents[0], startDateComponents[1], startDateComponents[2]);

        var endDateComponents = searchProperties.EndDate.Split("-").Select(d => int.Parse(d)).ToList();
        var end = new DateOnly(endDateComponents[0], endDateComponents[1], endDateComponents[2]);

        if (start == end)
        {
            validationErrors.Add("Start and end date cannot be on the same day");
        }

        if (start < DateOnly.FromDateTime(DateTime.Now))
        {
            validationErrors.Add("Start date cannot be smaller than today");
        }

        if (start > end)
        {
            validationErrors.Add("End date cannot be smaller than start date");
        }

        return validationErrors;
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