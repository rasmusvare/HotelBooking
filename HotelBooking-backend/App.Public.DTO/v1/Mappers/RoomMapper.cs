using AutoMapper;
using Base.DAL;

namespace App.Public.DTO.v1.Mappers;

public class RoomMapper: BaseMapper<Room, App.BLL.DTO.Room>
{
    public RoomMapper(IMapper mapper) : base(mapper)
    {
    }
}