using System.Security.Cryptography;
using Base.Domain;

namespace App.Domain;

public class RoomType : DomainEntityId
{
    public Guid HotelId { get; set; }
    public Hotel? Hotel { get; set; }
    
    public string Name { get; set; } = default!;
    public string Description { get; set; } = default!;
    public int Count { get; set; }
    public int NumberOfBeds { get; set; }

    public decimal PricePerNight { get; set; }

    
    public ICollection<Amenity>? Amenities { get; set; }
    public ICollection<Room>? Rooms { get; set; }
    public ICollection<Booking>? Bookings { get; set; }
}