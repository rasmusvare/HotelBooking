using AutoMapper;
using Base.DAL;

namespace App.Public.DTO.v1.Mappers;

public class RoomTypeMapper : BaseMapper<RoomType, App.BLL.DTO.RoomType>
{
    public RoomTypeMapper(IMapper mapper) : base(mapper)
    {
    }
}