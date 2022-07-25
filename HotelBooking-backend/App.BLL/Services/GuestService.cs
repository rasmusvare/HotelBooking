using App.BLL.DTO;
using App.Contracts.BLL.Services;
using App.Contracts.DAL.Repositories;
using Base.BLL;
using Base.Contracts.Base;

namespace App.BLL.Services;

public class GuestService: BaseEntityService<App.BLL.DTO.Guest, App.DAL.DTO.Guest, IGuestRepository>, IGuestService
{
    public GuestService(IGuestRepository repository, IMapper<Guest, DAL.DTO.Guest> mapper) : base(repository, mapper)
    {
    }
}