using Base.Domain;

namespace App.Public.DTO.v1;

public class Amenity : DomainEntityId
{
    public Guid HotelId { get; set; }
    // public Hotel? Hotel { get; set; }
    
    public string Name { get; set; } = default!;
    public string Description { get; set; } = default!;

    // public ICollection<RoomType>? RoomTypes { get; set; }
}