using API.Contracts;
using API.Models;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AccountController : ControllerBase
{
    private readonly IAccountRepository _accountRepository;

    public AccountController(IAccountRepository accountRepository)
    {
        _accountRepository = accountRepository;
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        var result = _accountRepository.GetAll();
        if (!result.Any())
        {
            return NotFound("Data Not Found");
        }

        return Ok(result);
    }

    [HttpGet("{guid}")]
    public IActionResult GetByGuid(Guid guid)
    {
        var result = _accountRepository.GetByGuid(guid);
        if (result is null)
        {
            return NotFound("Id Not Found");
        }
        return Ok(result);
    }

    [HttpPost]
    public IActionResult Create(Account account)
    {
        var result = _accountRepository.Create(account);
        if (result is null)
        {
            return BadRequest("Failed to create data");
        }

        return Ok(result);
    }

    [HttpPut("{guid}")]
    public IActionResult Update(Guid guid, Account account)
    {
        var existingAccount = _accountRepository.GetByGuid(guid);
        if (existingAccount == null)
        {
            return NotFound("Account not found");
        }

        existingAccount.Password = account.Password;
        existingAccount.OTP = account.OTP;
        existingAccount.IsUsed = account.IsUsed;
        existingAccount.ExpiredTime = account.ExpiredTime;

        if (_accountRepository.Update(existingAccount))
        {
            return Ok(existingAccount);
        }

        return BadRequest("Failed to update Account");
    }

    [HttpDelete("{guid}")]
    public IActionResult Delete(Guid guid)
    {
        var existingAccount = _accountRepository.GetByGuid(guid);
        if (existingAccount == null)
        {
            return NotFound("Account not found");
        }

        if (_accountRepository.Delete(existingAccount))
        {
            return Ok("Account deleted successfully");
        }

        return BadRequest("Failed to delete Account");
    }
}
