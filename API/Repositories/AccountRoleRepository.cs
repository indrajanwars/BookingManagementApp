using API.Contracts;
using API.Data;
using API.Models;

namespace API.Repositories;

public class AccountRoleRepository : IAccountRoleRepository
{
    private readonly BookingManagementDbContext _context;

    public AccountRoleRepository(BookingManagementDbContext context)
    {
        _context = context;
    }

    public IEnumerable<AccountRole> GetAll()
    {
        return _context.Set<AccountRole>().ToList();
    }

    public AccountRole? GetByGuid(Guid guid)
    {
        return _context.Set<AccountRole>().Find(guid);
    }

    public AccountRole? Create(AccountRole accountRole)
    {
        try
        {
            _context.Set<AccountRole>().Add(accountRole);
            _context.SaveChanges();
            return accountRole;
        }
        catch
        {
            return null;
        }
    }

    public bool Update(AccountRole accountRole)
    {
        try
        {
            _context.Set<AccountRole>().Update(accountRole);
            _context.SaveChanges();
            return true;
        }
        catch
        {
            return false;
        }
    }

    public bool Delete(AccountRole accountRole)
    {
        try
        {
            _context.Set<AccountRole>().Remove(accountRole);
            _context.SaveChanges();
            return true;
        }
        catch
        {
            return false;
        }
    }
}
