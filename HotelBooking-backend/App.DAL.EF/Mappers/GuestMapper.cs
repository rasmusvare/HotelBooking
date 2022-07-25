using AutoMapper;
using Base.DAL;

namespace App.DAL.EF.Mappers;

public class GuestMapper : BaseMapper<App.DAL.DTO.Guest, App.Domain.Guest>
{
    public GuestMapper(IMapper mapper) : base(mapper)
    {
    }
}