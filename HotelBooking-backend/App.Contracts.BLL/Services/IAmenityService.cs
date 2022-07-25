using App.Contracts.DAL.Repositories;
using Base.Contracts.BLL;

namespace App.Contracts.BLL.Services;

public interface IAmenityService : IEntityService<App.BLL.DTO.Amenity>, IAmenityRepositoryCustom<App.BLL.DTO.Amenity>
{
    
}