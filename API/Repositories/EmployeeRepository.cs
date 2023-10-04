using API.Contracts;
using API.Data;
using API.DTOs.Employees;
using API.Models;
using Microsoft.EntityFrameworkCore;

namespace API.Repositories;

public class EmployeeRepository : GeneralRepository<Employee>, IEmployeeRepository
{
    public EmployeeRepository(BookingManagementDbContext context) : base(context) { }

    public string? GetLastNik()
    {
        return _context.Set<Employee>().OrderBy(e => e.Nik).LastOrDefault()?.Nik;
    }
}