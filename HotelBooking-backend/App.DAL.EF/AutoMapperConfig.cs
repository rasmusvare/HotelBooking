using AutoMapper;

namespace App.DAL.EF;

public class AutoMapperConfig : Profile
{
    public AutoMapperConfig()
    {
        CreateMap<DAL.DTO.Amenity, Domain.Amenity>().ReverseMap();
        CreateMap<DAL.DTO.Booking, Domain.Booking>().ReverseMap();
        CreateMap<DAL.DTO.Guest, Domain.Guest>().ReverseMap();
        CreateMap<DAL.DTO.Hotel, Domain.Hotel>().ReverseMap();
        CreateMap<DAL.DTO.Room, Domain.Room>().ReverseMap();
        CreateMap<DAL.DTO.RoomType, Domain.RoomType>().ReverseMap();
        CreateMap<DAL.DTO.Identity.AppUser, Domain.Identity.AppUser>().ReverseMap();
    }
    
}