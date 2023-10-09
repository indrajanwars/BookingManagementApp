using API.Models;

namespace API.Contracts;

public interface IAccountRepository : IGeneralRepository<Account>
{
    string? GetPasswordByGuid(Guid guid);
    public Account GetByEmail(string email);
}