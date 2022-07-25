using App.DAL.DTO;
using Base.Contracts.DAL;

namespace App.Contracts.DAL.Repositories;

public interface IGuestRepository: IEntityRepository<Guest>, IGuestRepositoryCustom<Guest>
{
    
}

public interface IGuestRepositoryCustom<TEntity>
{
    
}
