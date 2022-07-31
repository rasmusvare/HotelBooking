using AutoMapper;
using Base.DAL;

namespace App.Public.DTO.v1.Mappers;

public class SearchPropertiesMapper: BaseMapper<SearchProperties, App.BLL.DTO.SearchProperties>
{
    public SearchPropertiesMapper(IMapper mapper) : base(mapper)
    {
    }
}