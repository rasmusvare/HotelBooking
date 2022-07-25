using App.Contracts.DAL.Repositories;
using Base.Contracts.BLL;

namespace App.Contracts.BLL.Services;

public interface IStayService: IEntityService<App.BLL.DTO.Stay>, IStayRepositoryCustom<App.BLL.DTO.Stay>
{
    
}