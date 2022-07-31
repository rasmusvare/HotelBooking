namespace App.Public.DTO.v1;

public class SearchProperties
{
    public Guid HotelId { get; set; }
    public string StartDate { get; set; } = default!;
    public string EndDate { get; set; } = default!;
    public int NoOfGuests { get; set; }

}