using AutoMapper;
using Base.DAL;

namespace App.Public.DTO.v1.Mappers;

public class StayMapper : BaseMapper<BLL.DTO.Stay, App.BLL.DTO.Stay>
{
    public StayMapper(IMapper mapper) : base(mapper)
    {
    }
}