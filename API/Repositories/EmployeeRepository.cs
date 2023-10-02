using API.Contracts;
using API.Data;
using API.Models;

namespace API.Repositories;

public class EmployeeRepository : IEmployeeRepository
{
    private readonly BookingManagementDbContext _context;

    public EmployeeRepository(BookingManagementDbContext context)
    {
        _context = context;
    }

    public IEnumerable<Employee> GetAll()
    {
        return _context.Set<Employee>().ToList();
    }

    public Employee? GetByGuid(Guid guid)
    {
        return _context.Set<Employee>().Find(guid);
    }

    public Employee? Create(Employee employee)
    {
        try
        {
            _context.Set<Employee>().Add(employee);
            _context.SaveChanges();
            return employee;
        }
        catch
        {
            return null;
        }
    }

    public bool Update(Employee employee)
    {
        try
        {
            _context.Set<Employee>().Update(employee);
            _context.SaveChanges();
            return true;
        }
        catch
        {
            return false;
        }
    }

    public bool Delete(Employee employee)
    {
        try
        {
            _context.Set<Employee>().Remove(employee);
            _context.SaveChanges();
            return true;
        }
        catch
        {
            return false;
        }
    }
}
