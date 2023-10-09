using API.Contracts;
using API.Data;
using API.Models;

namespace API.Repositories;

public class AccountRepository : GeneralRepository<Account>, IAccountRepository
{
    private readonly BookingManagementDbContext _context;
    public AccountRepository(BookingManagementDbContext context) : base(context) { }

    public string? GetPasswordByGuid(Guid guid)
    {
        return _context.Account.Where(e => e.Guid == guid)
            .Select(e => e.Password).FirstOrDefault();

    }

    public Account GetByEmail(string email)
    {
        var account = _context.Account
                      .Join(
                          _context.Employee,
                          account => account.Guid,
                          employee => employee.Guid,
                          (account, employee) => new
                          {
                              Account = account,
                              Employee = employee
                          }
                      )
                      .Where(joinResult => joinResult.Employee.Email == email)
                      .Select(joinResult => joinResult.Account)
                      .FirstOrDefault();

        return account;
    }
}