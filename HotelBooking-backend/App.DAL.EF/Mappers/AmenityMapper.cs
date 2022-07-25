using AutoMapper;
using Base.DAL;

namespace App.DAL.EF.Mappers;

public class AmenityMapper : BaseMapper<App.DAL.DTO.Amenity, App.Domain.Amenity>
{
    public AmenityMapper(IMapper mapper) : base(mapper)
    {
    }
}