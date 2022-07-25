using App.Contracts.DAL.Repositories;
using Base.Contracts.BLL;

namespace App.Contracts.BLL.Services;

public interface IHotelService: IEntityService<App.BLL.DTO.Hotel>, IHotelRepositoryCustom<App.BLL.DTO.Hotel>
{
    
}