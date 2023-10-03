using API.DTOs.Employees;
using API.Models;

namespace API.Contracts;

public interface IEmployeeRepository : IGeneralRepository<Employee>
{
    object Create(CreateEmployeeDto employeeDto);
    bool Update(Employee toUpdate);
}