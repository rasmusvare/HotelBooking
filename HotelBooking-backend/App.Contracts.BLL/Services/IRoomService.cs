using App.BLL.DTO;
using App.Contracts.DAL.Repositories;
using Base.Contracts.BLL;

namespace App.Contracts.BLL.Services;

public interface IRoomService: IEntityService<App.BLL.DTO.Room>, IRoomRepositoryCustom<App.BLL.DTO.Room>
{
    Task<bool> CheckRoomNumberAvailability(Room room);
}