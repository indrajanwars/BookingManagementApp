using API.Contracts;
using API.Models;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EmployeeController : ControllerBase
{
    private readonly IEmployeeRepository _employeeRepository;

    public EmployeeController(IEmployeeRepository employeeRepository)
    {
        _employeeRepository = employeeRepository;
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        var result = _employeeRepository.GetAll();
        if (!result.Any())
        {
            return NotFound("Data Not Found");
        }

        return Ok(result);
    }

    [HttpGet("{guid}")]
    public IActionResult GetByGuid(Guid guid)
    {
        var result = _employeeRepository.GetByGuid(guid);
        if (result is null)
        {
            return NotFound("Id Not Found");
        }
        return Ok(result);
    }

    [HttpPost]
    public IActionResult Create(Employee employee)
    {
        var result = _employeeRepository.Create(employee);
        if (result is null)
        {
            return BadRequest("Failed to create data");
        }

        return Ok(result);
    }

    [HttpPut("{guid}")]
    public IActionResult Update(Guid guid, Employee employee)
    {
        var existingEmployee = _employeeRepository.GetByGuid(guid);
        if (existingEmployee == null)
        {
            return NotFound("Employee not found");
        }

        existingEmployee.Nik = employee.Nik;
        existingEmployee.FirstName = employee.FirstName;
        existingEmployee.LastName = employee.LastName;
        existingEmployee.BirthDate = employee.BirthDate;
        existingEmployee.Gender = employee.Gender;
        existingEmployee.HiringDate = employee.HiringDate;
        existingEmployee.Email = employee.Email;
        existingEmployee.PhoneNumber = employee.PhoneNumber;

        if (_employeeRepository.Update(existingEmployee))
        {
            return Ok(existingEmployee);
        }

        return BadRequest("Failed to update Employee");
    }

    [HttpDelete("{guid}")]
    public IActionResult Delete(Guid guid)
    {
        var existingEmployee = _employeeRepository.GetByGuid(guid);
        if (existingEmployee == null)
        {
            return NotFound("Employee not found");
        }

        if (_employeeRepository.Delete(existingEmployee))
        {
            return Ok("Employee deleted successfully");
        }

        return BadRequest("Failed to delete Employee");
    }
}
