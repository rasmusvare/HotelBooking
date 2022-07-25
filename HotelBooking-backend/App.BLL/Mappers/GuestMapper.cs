using AutoMapper;
using Base.DAL;

namespace App.BLL.Mappers;

public class GuestMapper: BaseMapper<App.BLL.DTO.Guest, App.DAL.DTO.Guest>
{
    public GuestMapper(IMapper mapper) : base(mapper)
    {
    }
}