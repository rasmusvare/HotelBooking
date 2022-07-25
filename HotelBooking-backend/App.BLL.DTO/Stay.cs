using Base.Domain;

namespace App.BLL.DTO;

public class Stay : DomainEntityId
{
    public Guid GuestId { get; set; }
    public Guest? Guest { get; set; }

    public Guid RoomId { get; set; }
    public Room? Room { get; set; }

    public DateOnly FromDate { get; set; }
    public DateOnly ToDate { get; set; }
}