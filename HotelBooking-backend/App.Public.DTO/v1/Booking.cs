using Base.Domain;

namespace App.Public.DTO.v1;

public class Booking : DomainEntityId
{
    public Guid HotelId { get; set; }

    public Guid RoomTypeId { get; set; }

    public Guid BookingHolderId { get; set; }
    public Guest? BookingHolder { get; set; }

    public DateOnly DateFrom { get; set; }
    public DateOnly DateTo { get; set; }
    public decimal TotalPrice { get; set; }

    public ICollection<Guest>? Guests { get; set; }
}