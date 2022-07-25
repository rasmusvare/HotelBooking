using App.Contracts.DAL.Repositories;
using App.DAL.DTO;
using Base.Contracts.Base;
using Base.DAL.EF;

namespace App.DAL.EF.Repositories;

public class StayRepository: BaseEntityRepository<App.DAL.DTO.Stay, App.Domain.Stay, AppDbContext>, IStayRepository
{
    public StayRepository(AppDbContext dbContext, IMapper<Stay, Domain.Stay> mapper) : base(dbContext, mapper)
    {
    }
}