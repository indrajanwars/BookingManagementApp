using System.ComponentModel.DataAnnotations.Schema;

namespace API.Models;

[Table("tb_tr_bookings")]
public class Booking : BaseEntity
{
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int Status { get; set; }

    [Column("remarks", TypeName = "nvarchar(max)")]
    public string Remarks { get; set; }

    public Guid RoomGuid { get; set; }
    public Guid EmployeeGuid { get; set; }
}