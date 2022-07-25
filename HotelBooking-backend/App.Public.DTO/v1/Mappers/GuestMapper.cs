using AutoMapper;
using Base.DAL;

namespace App.Public.DTO.v1.Mappers;

public class GuestMapper: BaseMapper<Guest, App.BLL.DTO.Guest>
{
    public GuestMapper(IMapper mapper) : base(mapper)
    {
    }
}