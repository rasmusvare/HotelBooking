using App.Contracts.DAL.Repositories;
using App.DAL.DTO;
using Base.Contracts.Base;
using Base.DAL.EF;

namespace App.DAL.EF.Repositories;

public class GuestRepository: BaseEntityRepository<App.DAL.DTO.Guest, App.Domain.Guest, AppDbContext>, IGuestRepository
{
    public GuestRepository(AppDbContext dbContext, IMapper<Guest, Domain.Guest> mapper) : base(dbContext, mapper)
    {
    }
}