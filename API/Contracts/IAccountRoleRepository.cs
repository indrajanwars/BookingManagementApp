using API.Models;
using System;
using System.Collections.Generic;

namespace API.Contracts;

public interface IAccountRoleRepository
{
    IEnumerable<AccountRole> GetAll();
    AccountRole? GetByGuid(Guid guid);
    AccountRole? Create(AccountRole accountRole);
    bool Update(AccountRole accountRole);
    bool Delete(AccountRole accountRole);
}
