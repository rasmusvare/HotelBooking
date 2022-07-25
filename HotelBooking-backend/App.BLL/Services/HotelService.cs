using App.BLL.DTO;
using App.Contracts.BLL.Services;
using App.Contracts.DAL.Repositories;
using Base.BLL;
using Base.Contracts.Base;

namespace App.BLL.Services;

public class HotelService: BaseEntityService<App.BLL.DTO.Hotel, App.DAL.DTO.Hotel, IHotelRepository>, IHotelService
{
    public HotelService(IHotelRepository repository, IMapper<Hotel, DAL.DTO.Hotel> mapper) : base(repository, mapper)
    {
    }
}