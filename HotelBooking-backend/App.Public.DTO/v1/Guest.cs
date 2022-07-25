using Base.Domain;

namespace App.Public.DTO.v1;

public class Guest : DomainEntityId
{
    // public Guid BookingId { get; set; }
    // public Booking? Booking { get; set; }
    
    public bool IsBookingOwner { get; set; }

    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public string IdCode { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string PhoneNumber { get; set; } = default!;
}