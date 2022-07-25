using App.DAL.DTO;
using Base.Contracts.DAL;

namespace App.Contracts.DAL.Repositories;

public interface IHotelRepository: IEntityRepository<Hotel>, IHotelRepositoryCustom<Hotel>
{
    
}

public interface IHotelRepositoryCustom<TEntity>
{
    
}