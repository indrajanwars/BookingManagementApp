using API.Contracts;
using API.DTOs.AccountRoles;
using API.Models;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AccountRoleController : ControllerBase
{
    private readonly IAccountRoleRepository _accountRoleRepository;

    public AccountRoleController(IAccountRoleRepository accountRoleRepository)
    {
        _accountRoleRepository = accountRoleRepository;
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        var result = _accountRoleRepository.GetAll();
        if (!result.Any())
        {
            return NotFound("Data Not Found");
        }

        return Ok(result);
    }

    [HttpGet("{guid}")]
    public IActionResult GetByGuid(Guid guid)
    {
        var result = _accountRoleRepository.GetByGuid(guid);
        if (result is null)
        {
            return NotFound("Id Not Found");
        }
        return Ok(result);
    }

    [HttpPost]
    public IActionResult Create(AccountRole accountRole)
    {
        var result = _accountRoleRepository.Create(accountRole);
        if (result is null)
        {
            return BadRequest("Failed to create data");
        }

        return Ok(result);
    }

    [HttpPut("{guid}")]
    public IActionResult Update(Guid guid, AccountRole accountRole)
    {
        var existingAccountRole = _accountRoleRepository.GetByGuid(guid);
        if (existingAccountRole == null)
        {
            return NotFound("AccountRole not found");
        }

        existingAccountRole.AccountGuid = accountRole.AccountGuid;
        existingAccountRole.RoleGuid = accountRole.RoleGuid;

        if (_accountRoleRepository.Update(existingAccountRole))
        {
            return Ok(existingAccountRole);
        }

        return BadRequest("Failed to update AccountRole");
    }

    [HttpDelete("{guid}")]
    public IActionResult Delete(Guid guid)
    {
        var existingAccountRole = _accountRoleRepository.GetByGuid(guid);
        if (existingAccountRole == null)
        {
            return NotFound("AccountRole not found");
        }

        if (_accountRoleRepository.Delete(existingAccountRole))
        {
            return Ok("AccountRole deleted successfully");
        }

        return BadRequest("Failed to delete AccountRole");
    }
}
