using Base.Domain;

namespace App.Public.DTO.v1;

public class RoomType : DomainEntityId
{
    public Guid HotelId { get; set; }

    public string Name { get; set; } = default!;
    public string Description { get; set; } = default!;
    public int Count { get; set; }
    public int NumberOfBeds { get; set; }
    public decimal PricePerNight { get; set; }
    public decimal? TotalPrice { get; set; }


    public ICollection<Amenity>? Amenities { get; set; }
}