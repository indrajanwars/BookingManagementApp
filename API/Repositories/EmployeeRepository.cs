using API.Contracts;
using API.Data;
using API.DTOs.Employees;
using API.Models;

namespace API.Repositories;

public class EmployeeRepository : GeneralRepository<Employee>, IEmployeeRepository
{
    public EmployeeRepository(BookingManagementDbContext context) : base(context) { }

    public object Create(CreateEmployeeDto employeeDto)
    {
        throw new NotImplementedException();
    }
}
