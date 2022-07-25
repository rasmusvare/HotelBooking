using App.DAL.DTO;
using Base.Contracts.DAL;

namespace App.Contracts.DAL.Repositories;

public interface IStayRepository: IEntityRepository<Stay>, IStayRepositoryCustom<Stay>
{
    
}

public interface IStayRepositoryCustom<TEntity>
{
    
}