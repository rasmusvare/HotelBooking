using Base.Domain;

namespace App.Public.DTO.v1;

public class BookingGuest : DomainEntityId
{
    public Guid BookingId { get; set; }
    // public Booking? Booking { get; set; }

    public Guid GuestId { get; set; }
    // public Guest? Guest { get; set; }
}