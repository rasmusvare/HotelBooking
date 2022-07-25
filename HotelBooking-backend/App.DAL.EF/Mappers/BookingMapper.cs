using AutoMapper;
using Base.DAL;

namespace App.DAL.EF.Mappers;

public class BookingMapper : BaseMapper<App.DAL.DTO.Booking, App.Domain.Booking>
{
    public BookingMapper(IMapper mapper) : base(mapper)
    {
    }
}