namespace API.DTOs.Bookings;

public class RoomBookingDurationDto
{
    public Guid RoomGuid { get; set; }
    public string RoomName { get; set; }
    public string BookingDuration { get; set; }
}