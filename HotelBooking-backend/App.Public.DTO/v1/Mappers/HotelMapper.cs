using AutoMapper;
using Base.DAL;

namespace App.Public.DTO.v1.Mappers;

public class HotelMapper : BaseMapper<Hotel, App.BLL.DTO.Hotel>
{
    public HotelMapper(IMapper mapper) : base(mapper)
    {
    }
}