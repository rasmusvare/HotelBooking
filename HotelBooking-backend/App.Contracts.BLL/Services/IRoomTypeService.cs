using App.BLL.DTO;
using App.Contracts.DAL.Repositories;
using Base.Contracts.BLL;

namespace App.Contracts.BLL.Services;

public interface IRoomTypeService: IEntityService<App.BLL.DTO.RoomType>, IRoomTypeRepositoryCustom<App.BLL.DTO.RoomType>
{
   Task CountRooms(RoomType roomType);
   
   Task<IEnumerable<RoomType>> SearchAvailableRooms(SearchProperties searchProperties,
      bool noTracking = true);
   List<string> ValidateSearch(SearchProperties searchProperties);

}