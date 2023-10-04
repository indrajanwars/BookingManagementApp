using API.Contracts;
using API.Data;
using API.DTOs.Employees;
using API.Models;
using Microsoft.EntityFrameworkCore;

namespace API.Repositories;

public class EmployeeRepository : GeneralRepository<Employee>, IEmployeeRepository
{
    public EmployeeRepository(BookingManagementDbContext context) : base(context) { }
    public string GetLastNik()
    {
        Employee? employee = _context.Employee.OrderByDescending(e => e.Nik).FirstOrDefault();

        return employee?.Nik ?? "";
    }

    public object Create(CreateEmployeeDto employeeDto)
    {
        throw new NotImplementedException();
    }
}
