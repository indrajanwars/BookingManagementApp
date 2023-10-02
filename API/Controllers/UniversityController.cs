using API.Contracts;
using API.Models;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UniversityController : ControllerBase
{
    private readonly IUniversityRepository _universityRepository;

    public UniversityController(IUniversityRepository universityRepository)
    {
        _universityRepository = universityRepository;
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        var result = _universityRepository.GetAll();
        if (!result.Any())
        {
            return NotFound("Data Not Found");
        }

        return Ok(result);
    }

    [HttpGet("{guid}")]
    public IActionResult GetByGuid(Guid guid)
    {
        var result = _universityRepository.GetByGuid(guid);
        if (result is null)
        {
            return NotFound("Id Not Found");
        }
        return Ok(result);
    }

    [HttpPost]
    public IActionResult Create(University university)
    {
        var result = _universityRepository.Create(university);
        if (result is null)
        {
            return BadRequest("Failed to create data");
        }

        return Ok(result);
    }

    [HttpPut("{guid}")]
    public IActionResult Update(Guid guid, University university)
    {
        var existingUniversity = _universityRepository.GetByGuid(guid);
        if (existingUniversity == null)
        {
            return NotFound("University not found");
        }

        existingUniversity.Code = university.Code;
        existingUniversity.Name = university.Name;

        if (_universityRepository.Update(existingUniversity))
        {
            return Ok(existingUniversity);
        }

        return BadRequest("Failed to update University");
    }

    [HttpDelete("{guid}")]
    public IActionResult Delete(Guid guid)
    {
        var existingUniversity = _universityRepository.GetByGuid(guid);
        if (existingUniversity == null)
        {
            return NotFound("University not found");
        }

        if (_universityRepository.Delete(existingUniversity))
        {
            return Ok("University deleted successfully");
        }

        return BadRequest("Failed to delete University");
    }
}