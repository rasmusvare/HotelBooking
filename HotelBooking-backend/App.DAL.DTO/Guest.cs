using Base.Domain;

namespace App.DAL.DTO;

public class Guest : DomainEntityId
{
    public Guid BookingId { get; set; }
    public Booking? Booking { get; set; }
    
    public bool IsBookingOwner { get; set; }

    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string IdCode { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
}