using App.Contracts.DAL.Repositories;
using Base.Contracts.BLL;

namespace App.Contracts.BLL.Services;

public interface IGuestService: IEntityService<App.BLL.DTO.Guest>, IGuestRepositoryCustom<App.BLL.DTO.Guest>
{
    
}