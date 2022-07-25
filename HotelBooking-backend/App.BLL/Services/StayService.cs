using App.BLL.DTO;
using App.Contracts.BLL.Services;
using App.Contracts.DAL.Repositories;
using Base.BLL;
using Base.Contracts.Base;

namespace App.BLL.Services;

public class StayService: BaseEntityService<App.BLL.DTO.Stay, App.DAL.DTO.Stay, IStayRepository>, IStayService
{
    public StayService(IStayRepository repository, IMapper<Stay, DAL.DTO.Stay> mapper) : base(repository, mapper)
    {
    }
}