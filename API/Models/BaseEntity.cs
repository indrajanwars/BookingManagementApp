namespace API.Models;

public class BaseEntity
{
    public Guid Guid { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime ModifiedDate { get; set; }
}