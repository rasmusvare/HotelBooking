using App.Contracts.DAL;
using App.Contracts.DAL.Repositories;
using App.DAL.DTO;
using App.DAL.EF.Mappers;
using App.DAL.EF.Repositories;
using AutoMapper;
using Base.DAL.EF;

namespace App.DAL.EF;

public class AppUOW : BaseUOW<AppDbContext>, IAppUnitOfWork
{
    private readonly AutoMapper.IMapper _mapper;

    public AppUOW(AppDbContext dbContext, IMapper mapper) : base(dbContext)
    {
        _mapper = mapper;
    }

    private IAmenityRepository? _amenities;

    public virtual IAmenityRepository Amenities =>
        _amenities ??= new AmenityRepository(UOWDbContext, new AmenityMapper(_mapper));

    private IBookingRepository? _bookings;

    public virtual IBookingRepository Bookings =>
        _bookings ??= new BookingRepository(UOWDbContext, new BookingMapper(_mapper));

    private IGuestRepository? _guests;

    public virtual IGuestRepository Guests =>
        _guests ??= new GuestRepository(UOWDbContext, new GuestMapper(_mapper));

    private IHotelRepository? _hotels;

    public virtual IHotelRepository Hotels =>
        _hotels ??= new HotelRepository(UOWDbContext, new HotelMapper(_mapper));

    private IRoomRepository? _rooms;

    public virtual IRoomRepository Rooms =>
        _rooms ??= new RoomRepository(UOWDbContext, new RoomMapper(_mapper));

    private IRoomTypeRepository? _roomTypes;

    public virtual IRoomTypeRepository RoomTypes =>
        _roomTypes ??= new RoomTypeRepository(UOWDbContext, new RoomTypeMapper(_mapper));
}