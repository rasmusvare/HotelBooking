using AutoMapper;
using Base.DAL;

namespace App.Public.DTO.v1.Mappers;

public class BookingMapper : BaseMapper<Booking, App.BLL.DTO.Booking>
{
    public BookingMapper(IMapper mapper) : base(mapper)
    {
    }
}