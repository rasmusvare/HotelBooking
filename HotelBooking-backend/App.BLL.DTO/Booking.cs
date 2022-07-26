using App.BLL.DTO.Identity;
using Base.Domain;

namespace App.BLL.DTO;

public class Booking : DomainEntityId
{
    public Guid HotelId { get; set; }
    public Hotel? Hotel { get; set; }
    
    public Guid RoomTypeId { get; set; }
    public RoomType? RoomType { get; set; }
    
    // public Guid AppUserId { get; set; }
    // public AppUser? AppUser { get; set; }

    public Guid BookingHolderId { get; set; }
    public Guest? BookingHolder { get; set; }

    public DateOnly DateFrom { get; set; }
    public DateOnly DateTo { get; set; }
    public decimal TotalPrice { get; set; }

    public ICollection<Guest>? Guests { get; set; }
}