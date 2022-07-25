using Base.Domain;

namespace App.BLL.DTO;

public class Hotel : DomainEntityId
{
    public string Name { get; set; } = default!;
    public string Description { get; set; } = default!;
    public string Address { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string TelephoneNumber { get; set; } = default!;

    public ICollection<Room>? Rooms { get; set; }
    public ICollection<RoomType>? RoomTypes { get; set; }
    public ICollection<Amenity>? Amenities { get; set; }
}