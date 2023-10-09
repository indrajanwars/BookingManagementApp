using System.Net;
using System.Data;
using API.Contracts;
using API.DTOs.Employees;
using API.Models;
using API.Utilities.Handlers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace API.Controllers;

// EmployeeController: sebuah kontroler API yang berfungsi untuk mengelola data karyawan.
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class EmployeeController : ControllerBase
{
    private readonly IEmployeeRepository _employeeRepository;
    private readonly IEducationRepository _educationRepository;
    private readonly IUniversityRepository _universityRepository;

    // Konstruktor untuk EmployeeController, menerima instance dari IEmployeeRepository yang akan digunakan untuk mengakses data karyawan.
    public EmployeeController(IEmployeeRepository employeeRepository, IEducationRepository educationRepository, IUniversityRepository universityRepository)
    {
        _employeeRepository = employeeRepository;
        _educationRepository = educationRepository;
        _universityRepository = universityRepository;
    }

    [HttpGet("details")]
    public IActionResult GetDetails()
    {
        var employees = _employeeRepository.GetAll();
        var educations = _educationRepository.GetAll();
        var universities = _universityRepository.GetAll();

        if (!(employees.Any() && educations.Any() && universities.Any()))
        {
            return NotFound(new ResponseErrorHandler
            {
                Code = StatusCodes.Status404NotFound,
                Status = HttpStatusCode.NotFound.ToString(),
                Message = "Data Not Found"
            });
        }

        var employeeDetails = from emp in employees
                              join edu in educations on emp.Guid equals edu.Guid
                              join unv in universities on edu.UniversityGuid equals unv.Guid
                              select new EmployeeDetailDto
                              {
                                  Guid = emp.Guid,
                                  Nik = emp.Nik,
                                  FullName = string.Concat(emp.FirstName, " ", emp.LastName),
                                  BirthDate = emp.BirthDate,
                                  Gender = emp.Gender.ToString(),
                                  HiringDate = emp.HiringDate,
                                  Email = emp.Email,
                                  PhoneNumber = emp.PhoneNumber,
                                  Major = edu.Major,
                                  Degree = edu.Degree,
                                  Gpa = edu.Gpa,
                                  University = unv.Name
                              };

        return Ok(new ResponseOKHandler<IEnumerable<EmployeeDetailDto>>(employeeDetails));
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        // Mengambil semua data karyawan dari repositori.
        var result = _employeeRepository.GetAll();

        // Jika tidak ada data karyawan yang ditemukan, mengembalikan respons NotFound.
        if (!result.Any())
        {
            return NotFound(new ResponseErrorHandler
            {
                Code = StatusCodes.Status404NotFound,
                Status = HttpStatusCode.NotFound.ToString(),
                Message = "Data Not Found"
            });
        }

        // Mengkonversi hasil ke dalam bentuk EmployeeDto dan mengembalikannya sebagai respons API.
        var data = result.Select(x => (EmployeeDto)x);

        // Mengembalikan respons sukses dengan objek ResponseOKHandler yang berisi data.
        return Ok(new ResponseOKHandler<IEnumerable<EmployeeDto>>(data));
    }

    [HttpGet("{guid}")]
    public IActionResult GetByGuid(Guid guid)
    {
        // Mengambil data karyawan berdasarkan GUID yang diberikan.
        var result = _employeeRepository.GetByGuid(guid);

        // Jika data karyawan tidak ditemukan, mengembalikan respons NotFound.
        if (result is null)
        {
            return NotFound(new ResponseErrorHandler
            {
                Code = StatusCodes.Status404NotFound,
                Status = HttpStatusCode.NotFound.ToString(),
                Message = "Data Not Found"
            });
        }

        // Mengkonversi hasil ke dalam bentuk EmployeeDto dan mengembalikannya sebagai respons API.
        return Ok(new ResponseOKHandler<EmployeeDto>((EmployeeDto)result));
    }

    [HttpPost]
    public IActionResult Create(CreateEmployeeDto employeeDto)
    {
        try
        {
            // Membuat objek Employee baru dari CreateEmployeeDto.
            Employee toCreate = employeeDto;
            // Generate NIK untuk karyawan berdasarkan data terakhir dari repositori.
            toCreate.Nik = GenerateHandler.Nik(_employeeRepository.GetLastNik());
            // Menyimpan objek Employee baru ke dalam repositori.
            var result = _employeeRepository.Create(toCreate);

            // Mengembalikan respons sukses dengan objek ResponseOKHandler yang berisi data karyawan yang baru dibuat.
            return Ok(new ResponseOKHandler<EmployeeDto>((EmployeeDto)result));
        }
        catch (ExceptionHandler ex)
        {
            // Mengembalikan respons dengan kode status 500 dan pesan error jika terjadi kesalahan.
            return StatusCode(StatusCodes.Status500InternalServerError, new ResponseErrorHandler
            {
                Code = StatusCodes.Status500InternalServerError,
                Status = HttpStatusCode.InternalServerError.ToString(),
                Message = "Failed to create data",
                Error = ex.Message
            });
        }
    }

    [HttpPut]
    public IActionResult Update(EmployeeDto employeeDto)
    {
        try
        {
            var entity = _employeeRepository.GetByGuid(employeeDto.Guid);

            if (entity is null)
            {
                return NotFound(new ResponseErrorHandler
                {
                    Code = StatusCodes.Status404NotFound,
                    Status = HttpStatusCode.NotFound.ToString(),
                    Message = "Data Not Found"
                });
            }

            // Menyalin data dari EmployeeDto ke objek Employee yang akan diperbarui dengan tetap mempertahankan NIK dan tanggal pembuatan.
            Employee toUpdate = employeeDto;
            toUpdate.Nik = entity.Nik;
            toUpdate.CreatedDate = entity.CreatedDate;

            // Memperbarui data karyawan di repositori.
            _employeeRepository.Update(toUpdate);

            // Mengembalikan respons sukses dengan pesan.
            return Ok(new ResponseOKHandler<string>("Data Updated"));
        }
        catch (ExceptionHandler ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new ResponseErrorHandler
            {
                Code = StatusCodes.Status500InternalServerError,
                Status = HttpStatusCode.InternalServerError.ToString(),
                Message = "Failed to create data",
                Error = ex.Message
            });
        }
    }

    [HttpDelete("{guid}")]
    public IActionResult Delete(Guid guid)
    {
        try
        {
            var entity = _employeeRepository.GetByGuid(guid);

            if (entity is null)
            {
                return NotFound(new ResponseErrorHandler
                {
                    Code = StatusCodes.Status404NotFound,
                    Status = HttpStatusCode.NotFound.ToString(),
                    Message = "Data Not Found"
                });
            }

            // Menghapus data karyawan dari repositori.
            _employeeRepository.Delete(entity);

            return Ok(new ResponseOKHandler<string>("Data Deleted"));
        }
        catch (ExceptionHandler ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new ResponseErrorHandler
            {
                Code = StatusCodes.Status500InternalServerError,
                Status = HttpStatusCode.InternalServerError.ToString(),
                Message = "Failed to create data",
                Error = ex.Message
            });
        }
    }
}