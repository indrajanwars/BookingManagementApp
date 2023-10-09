using API.Models;
using API.Repositories;

namespace API.Contracts;

public interface IRoleRepository : IGeneralRepository <Role>
{
    public Guid? GetDefaultRoleGuid();
}
