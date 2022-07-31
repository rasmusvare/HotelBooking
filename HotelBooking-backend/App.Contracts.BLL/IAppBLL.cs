using App.Contracts.BLL.Services;
using Base.Contracts.BLL;

namespace App.Contracts.BLL;

public interface IAppBLL : IBLL
{
    IAmenityService Amenities { get; }
    IBookingService Bookings { get; }
    IGuestService Guests { get; }
    IHotelService Hotels { get; }
    IRoomService Rooms { get; }
    IRoomTypeService RoomTypes { get; }
}