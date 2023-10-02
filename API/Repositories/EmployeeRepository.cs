using API.Contracts;
using API.Data;
using API.Models;

namespace API.Repositories;

public class EmployeeRepository : GeneralRepository<Employees>, IEmployeeRepository
{
    public EmployeeRepository(BookingManagementDbContext context) : base(context) { }
}
