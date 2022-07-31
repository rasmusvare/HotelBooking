using App.BLL.Mappers;
using App.BLL.Services;
using App.Contracts.BLL;
using App.Contracts.BLL.Services;
using App.Contracts.DAL;
using AutoMapper;
using Base.BLL;

namespace App.BLL;

public class AppBLL : BaseBLL<IAppUnitOfWork>, IAppBLL
{
    private readonly IAppUnitOfWork _unitOfWork;
    private readonly AutoMapper.IMapper _mapper;

    public AppBLL(IAppUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public override async Task<int> SaveChangesAsync()
    {
        return await _unitOfWork.SaveChangesAsync();
    }

    public override int SaveChanges()
    {
        return _unitOfWork.SaveChanges();
    }

    private IAmenityService? _amenities;

    public IAmenityService Amenities =>
        _amenities ??=
            new AmenityService(_unitOfWork.Amenities, new AmenityMapper(_mapper));

    private IBookingService? _bookings;

    public IBookingService Bookings =>
        _bookings ??=
            new BookingService(_unitOfWork.Bookings, new BookingMapper(_mapper));

    private IGuestService? _guests;

    public IGuestService Guests =>
        _guests ??=
            new GuestService(_unitOfWork.Guests, new GuestMapper(_mapper));

    private IHotelService? _hotels;

    public IHotelService Hotels =>
        _hotels ??=
            new HotelService(_unitOfWork.Hotels, new HotelMapper(_mapper));

    private IRoomService? _rooms;

    public IRoomService Rooms =>
        _rooms ??=
            new RoomService(_unitOfWork.Rooms, new RoomMapper(_mapper));

    private IRoomTypeService? _roomTypes;

    public IRoomTypeService RoomTypes =>
        _roomTypes ??=
            new RoomTypeService(_unitOfWork.RoomTypes, new RoomTypeMapper(_mapper));
}