using System.ComponentModel.DataAnnotations;
using Base.Domain;

namespace App.DAL.DTO.Identity;

public class AppUser : DomainEntityId
{
    [MinLength(1)] [MaxLength(128)] public string FirstName { get; set; } = default!;

    [MinLength(1)] [MaxLength(128)] public string LastName { get; set; } = default!;

    // Fields connected to user
    public ICollection<Booking>? Bookings { get; set; }
}