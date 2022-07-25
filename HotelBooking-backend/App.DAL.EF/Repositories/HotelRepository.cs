using App.Contracts.DAL.Repositories;
using App.DAL.DTO;
using Base.Contracts.Base;
using Base.DAL.EF;

namespace App.DAL.EF.Repositories;

public class HotelRepository: BaseEntityRepository<App.DAL.DTO.Hotel, App.Domain.Hotel, AppDbContext>, IHotelRepository
{
    public HotelRepository(AppDbContext dbContext, IMapper<Hotel, Domain.Hotel> mapper) : base(dbContext, mapper)
    {
    }
}