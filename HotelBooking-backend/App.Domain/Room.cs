using System.Security.Cryptography;
using App.Domain.Enums;
using Base.Domain;

namespace App.Domain;

public class Room : DomainEntityId
{
    public Guid HotelId { get; set; }
    public Hotel? Hotel { get; set; }

    public Guid RoomTypeId { get; set; }
    public RoomType? RoomType { get; set; }

    public string RoomNumber { get; set; } = default!;
    // public int NumberOfBeds { get; set; }
    // public decimal PricePerNight { get; set; }
    // public string Description { get; set; } = default!;

    public ERoomStatus RoomStatus { get; set; }
    public ICollection<Stay>? Stays { get; set; }
}