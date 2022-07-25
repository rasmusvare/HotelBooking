using App.Contracts.DAL.Repositories;
using Base.Contracts.DAL;

namespace App.Contracts.DAL;

public interface IAppUnitOfWork : IUnitOfWork
{
    IAmenityRepository Amenities { get; }
    IBookingRepository Bookings { get; }
    IGuestRepository Guests { get; }
    IHotelRepository Hotels { get; }
    IRoomRepository Rooms { get; }
    IRoomTypeRepository RoomTypes { get; }
    IStayRepository Stays { get; }
}
