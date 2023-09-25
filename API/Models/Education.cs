using System.ComponentModel.DataAnnotations.Schema;

namespace API.Models;

[Table("tb_m_educations")]
public class Education : BaseEntity
{
    [Column("major", TypeName = "nvarchar(100)")]
    public string Major { get; set; }

    [Column("degree", TypeName = "nvarchar(100)")]
    public string Degree { get; set; }

    public float Gpa { get; set; }
    public Guid UniversityGuid { get; set; }
}