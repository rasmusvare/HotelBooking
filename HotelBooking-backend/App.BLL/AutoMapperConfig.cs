using AutoMapper;

namespace App.BLL;

public class AutoMapperConfig : Profile
{
    public AutoMapperConfig()
    {
        CreateMap<BLL.DTO.Amenity, DAL.DTO.Amenity>().ReverseMap();
        CreateMap<BLL.DTO.Booking, DAL.DTO.Booking>().ReverseMap();
        CreateMap<BLL.DTO.Guest, DAL.DTO.Guest>().ReverseMap();
        CreateMap<BLL.DTO.Hotel, DAL.DTO.Hotel>().ReverseMap();
        CreateMap<BLL.DTO.Room, DAL.DTO.Room>().ReverseMap();
        CreateMap<BLL.DTO.RoomType, DAL.DTO.RoomType>().ReverseMap();
        CreateMap<BLL.DTO.Stay, DAL.DTO.Stay>().ReverseMap();
        CreateMap<BLL.DTO.Identity.AppUser, DAL.DTO.Identity.AppUser>().ReverseMap();
    }
}