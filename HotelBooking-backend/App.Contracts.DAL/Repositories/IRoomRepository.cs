using App.DAL.DTO;
using Base.Contracts.DAL;

namespace App.Contracts.DAL.Repositories;

public interface IRoomRepository: IEntityRepository<Room>, IRoomRepositoryCustom<Room>
{
    
}

public interface IRoomRepositoryCustom<TEntity>
{
    Task<IEnumerable<TEntity>> GetAllAsync(Guid hotelId, bool noTracking = true);
 
}