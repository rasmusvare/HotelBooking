using AutoMapper;
using Base.DAL;

namespace App.BLL.Mappers;

public class BookingMapper: BaseMapper<App.BLL.DTO.Booking, App.DAL.DTO.Booking>
{
    public BookingMapper(IMapper mapper) : base(mapper)
    {
    }
}