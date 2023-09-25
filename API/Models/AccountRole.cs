using System.ComponentModel.DataAnnotations.Schema;

namespace API.Models;

[Table("tb_m_account_roles")]
public class AccountRole : BaseEntity
{
    public Guid AccountGuid { get; set; }
    public Guid RoleGuid { get; set; }
}