using System.ComponentModel.DataAnnotations;
using Base.Domain;

namespace App.Domain;

public class Guest : DomainEntityId
{
    public Guid BookingId { get; set; }
    public Booking? Booking { get; set; }

    public bool IsBookingOwner { get; set; }

    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public string IdCode { get; set; } = default!;
    [EmailAddress(ErrorMessage = "Please enter a correct email address")]
    public string Email { get; set; } = default!;
    public string PhoneNumber { get; set; } = default!;
}