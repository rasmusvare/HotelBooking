using Base.Domain;

namespace App.Domain;

public class BookingGuest : DomainEntityId
{
    public Guid BookingId { get; set; }
    public Booking? Booking { get; set; }

    public Guid GuestId { get; set; }
    public Guest? Guest { get; set; }
    
}