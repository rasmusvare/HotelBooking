using App.DAL.DTO;
using Base.Contracts.DAL;

namespace App.Contracts.DAL.Repositories;

public interface IAmenityRepository : IEntityRepository<Amenity>, IAmenityRepositoryCustom<Amenity>
{
    
}

public interface IAmenityRepositoryCustom<TEntity>
{
    Task<IEnumerable<TEntity>> GetAllAsync(Guid hotelId, bool noTracking = true);

}
