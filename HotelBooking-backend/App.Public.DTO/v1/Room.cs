using App.Domain.Enums;
using Base.Domain;

namespace App.Public.DTO.v1;

public class Room : DomainEntityId
{
    public Guid HotelId { get; set; }

    public Guid RoomTypeId { get; set; }

    public string RoomNumber { get; set; } = default!;

    public ERoomStatus RoomStatus { get; set; }
}