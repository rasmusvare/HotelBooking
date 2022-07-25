using AutoMapper;
using Base.DAL;

namespace App.Public.DTO.v1.Mappers;

public class AmenityMapper : BaseMapper<Amenity, App.BLL.DTO.Amenity>
{
    public AmenityMapper(IMapper mapper) : base(mapper)
    {
    }
}