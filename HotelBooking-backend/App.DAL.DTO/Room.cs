using App.Domain.Enums;
using Base.Domain;

namespace App.DAL.DTO;

public class Room : DomainEntityId
{
    public Guid HotelId { get; set; }
    public Hotel? Hotel { get; set; }

    public Guid RoomTypeId { get; set; }
    public RoomType? RoomType { get; set; }

    public string RoomNumber { get; set; } = default!;

    public ERoomStatus RoomStatus { get; set; }
}