using System.ComponentModel.DataAnnotations.Schema;

namespace API.Models;

[Table("tb_m_rooms")]
public class Room : BaseEntity
{
    [Column("name", TypeName = "nvarchar(100)")]
    public string Name { get; set; }

    public int Floor { get; set; }
    public int Capacity { get; set; }
}