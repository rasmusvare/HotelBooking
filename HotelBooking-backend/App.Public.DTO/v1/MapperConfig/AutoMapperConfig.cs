using AutoMapper;

namespace App.Public.DTO.v1.MapperConfig;

public class AutoMapperConfig : Profile
{
    public AutoMapperConfig()
    {
        CreateMap<App.Public.DTO.v1.Amenity, App.BLL.DTO.Amenity>().ReverseMap();
        CreateMap<App.Public.DTO.v1.Booking, App.BLL.DTO.Booking>().ReverseMap();
        CreateMap<App.Public.DTO.v1.Guest, App.BLL.DTO.Guest>().ReverseMap();
        CreateMap<App.Public.DTO.v1.Hotel, App.BLL.DTO.Hotel>().ReverseMap();
        CreateMap<App.Public.DTO.v1.Room, App.BLL.DTO.Room>().ReverseMap();
        CreateMap<App.Public.DTO.v1.RoomType, App.BLL.DTO.RoomType>().ReverseMap();
        CreateMap<App.Public.DTO.v1.SearchProperties, App.BLL.DTO.SearchProperties>().ReverseMap();
    }
    
}