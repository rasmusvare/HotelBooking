using AutoMapper;
using Base.DAL;

namespace App.DAL.EF.Mappers;

public class RoomTypeMapper : BaseMapper<App.DAL.DTO.RoomType, App.Domain.RoomType>
{
    public RoomTypeMapper(IMapper mapper) : base(mapper)
    {
    }
}