using System.Net;
using System.Transactions;
using System.Security.Claims;
using API.Contracts;
using API.DTOs.Accounts;
using API.Models;
using API.Utilities.Handlers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace API.Controllers;

// Kontroler API yang berfungsi untuk mengelola data akun (account).
[ApiController]
[Route("api/[controller]")]
[Authorize] // Menandakan endpoint-endpoint dalam kontroler ini memerlukan otentikasi.
public class AccountController : ControllerBase
{
    private readonly IAccountRepository _accountRepository;
    private readonly IEmployeeRepository _employeeRepository;
    private readonly IEducationRepository _educationRepository;
    private readonly IUniversityRepository _universityRepository;
    private readonly IAccountRoleRepository _accountRoleRepository;
    private readonly IRoleRepository _roleRepository;
    private readonly IEmailHandler _emailHandler;
    private readonly ITokenHandler _tokenHandler;

    public AccountController(IAccountRepository accountRepository, IEmployeeRepository employeeRepository,
                             IEducationRepository educationRepository, IUniversityRepository universityRepository,
                             IEmailHandler emailHandler, ITokenHandler tokenHandler, IAccountRoleRepository accountRoleRepository, IRoleRepository roleRepository)
    {
        // Menginisialisasi instance-instance yang dibutuhkan melalui dependency injection.
        _accountRepository = accountRepository;
        _employeeRepository = employeeRepository;
        _educationRepository = educationRepository;
        _universityRepository = universityRepository;
        _emailHandler = emailHandler;
        _tokenHandler = tokenHandler;
        _accountRoleRepository = accountRoleRepository;
        _roleRepository = roleRepository;
    }

    // Endpoint untuk login akun.
    [HttpPost("Login")]
    public IActionResult Login(LoginDto loginDto)
    {
        try
        {
            // Memeriksa apakah email yang dimasukkan valid.
            var employees = _employeeRepository.GetEmail(loginDto.Email);

            if (employees is null)
            {
                // Mengembalikan respons NotFound jika email tidak valid.
                return NotFound(new ResponseErrorHandler
                {
                    Code = StatusCodes.Status404NotFound,
                    Status = HttpStatusCode.NotFound.ToString(),
                    Message = "Acount or Password is invalid!"
                });
            }

            // Mengambil data akun berdasarkan GUID dari employee.
            var account = _accountRepository.GetByGuid(employees.Guid);

            if (!HashingHandler.VerifyPassword(loginDto.Password, account!.Password))
            {
                // Mengembalikan respons BadRequest jika password tidak valid.
                return BadRequest(new ResponseErrorHandler
                {
                    Code = StatusCodes.Status400BadRequest,
                    Status = HttpStatusCode.BadRequest.ToString(),
                    Message = "Account or Password is invalid!"
                });
            }

            // Membuat daftar claims untuk otentikasi.
            var claims = new List<Claim>();
            claims.Add(new Claim("Email", employees.Email));
            claims.Add(new Claim("FullName", string.Concat(employees.FirstName + " " + employees.LastName)));

            // Mengambil role dari akun.
            var getRoleName = from ar in _accountRoleRepository.GetAll()
                              join r in _roleRepository.GetAll() on ar.RoleGuid equals r.Guid
                              where ar.AccountGuid == account.Guid
                              select r.Name;

            // Menambahkan roles ke dalam daftar claims yang akan digunakan dalam proses otentikasi dan otorisasi.
            foreach (var roleName in getRoleName)
            {
                claims.Add(new Claim(ClaimTypes.Role, roleName));
            }

            // Menghasilkan token otentikasi.
            var generateToken = _tokenHandler.Generate(claims);

            // Mengembalikan respons sukses dengan token otentikasi.
            return Ok(new ResponseOKHandler<object>("Login Success", new { Token = generateToken }));
        }
        catch (Exception ex)
        {
            // Mengembalikan respons dengan kode status 500 dan pesan error jika terjadi kesalahan.
            return StatusCode(StatusCodes.Status500InternalServerError, new ResponseErrorHandler
            {
                Code = StatusCodes.Status500InternalServerError,
                Status = HttpStatusCode.InternalServerError.ToString(),
                Message = "Failed to Delete data",
                Error = ex.Message
            });
        }
    }

    // Endpoint untuk mendaftar akun.
    [HttpPost("Register")]
    [AllowAnonymous] // Endpoint dapat diakses tanpa otentikasi.
    public IActionResult Register(RegisterAccountDto registerAccountDto)
    {
        using var transaction = new TransactionScope();
        try
        {
            // Membuat objek Employee dari data registrasi.
            Employee toCreateEmp = registerAccountDto;
            toCreateEmp.Nik = GenerateHandler.Nik(_employeeRepository.GetLastNik());

            _employeeRepository.Create(toCreateEmp);

            // Mencari atau membuat universitas berdasarkan kode dan nama universitas.
            var univFindResult = _universityRepository.GetCodeName(registerAccountDto.UniversityCode, registerAccountDto.UniversityName);
            if (univFindResult is null)
            {
                univFindResult = _universityRepository.Create(registerAccountDto);
            }

            // Membuat objek Education dari data registrasi.
            Education toCreateEdu = registerAccountDto;
            toCreateEdu.Guid = toCreateEmp.Guid;
            toCreateEdu.UniversityGuid = univFindResult.Guid;
            _educationRepository.Create(toCreateEdu);

            if (registerAccountDto.Password != registerAccountDto.ConfirmPassword)
            {
                // Mengembalikan respons BadRequest jika Password dan ConfirmPassword tidak cocok.
                return BadRequest(new ResponseErrorHandler
                {
                    Code = StatusCodes.Status400BadRequest,
                    Status = HttpStatusCode.BadRequest.ToString(),
                    Message = "NewPassword and ConfirmPassword do not match"
                });
            }

            // Membuat objek Account dari data registrasi dan menyimpan hash Password.
            Account toCreateAcc = registerAccountDto;
            toCreateAcc.Guid = toCreateEmp.Guid;
            toCreateAcc.Password = HashingHandler.HashPassword(registerAccountDto.Password);
            _accountRepository.Create(toCreateAcc);

            // Membuat role default untuk akun yang baru dibuat.
            var accountRole = _accountRoleRepository.Create(new AccountRole
            {
                AccountGuid = toCreateAcc.Guid,
                RoleGuid = _roleRepository.GetDefaultRoleGuid() ?? throw new Exception("Default Role Not Found")
            });
            transaction.Complete();

            // Mengembalikan respons sukses jika registrasi berhasil.
            return Ok(new ResponseOKHandler<string>("Registration successfully"));
        }
        catch (Exception ex)
        {
            // Mengembalikan respons dengan kode status 500 dan pesan error jika terjadi kesalahan.
            return StatusCode(StatusCodes.Status500InternalServerError, new ResponseErrorHandler
            {
                Code = StatusCodes.Status500InternalServerError,
                Status = HttpStatusCode.InternalServerError.ToString(),
                Message = "Failed Registration Account",
                Error = ex.Message
            });
        }
    }

    // Endpoint untuk mengatasi lupa password.
    [HttpPut("ForgotPassword")]
    [AllowAnonymous] // Endpoint dapat diakses tanpa otentikasi.
    public IActionResult ForgotPassword(string email)
    {
        var employee = _employeeRepository.GetAll();
        var account = _accountRepository.GetAll();

        if (!(employee.Any() && account.Any()))
        {
            // Mengembalikan respons NotFound jika data employee atau account tidak ditemukan.
            return NotFound(new ResponseErrorHandler
            {
                Code = StatusCodes.Status404NotFound,
                Status = HttpStatusCode.NotFound.ToString(),
                Message = "Data Not Found"
            });
        }

        var data = _employeeRepository.GetEmail(email);
        var toUpdate = _accountRepository.GetByGuid(data.Guid); // Mendapatkan akun yang sesuai dengan data employee.

        Random random = new();
        toUpdate.OTP = random.Next(100000, 999999); // Menghasilkan sebuah nomor acak
        toUpdate.ExpiredTime = DateTime.Now.AddMinutes(5);  // Mengatur waktu kedaluwarsa untuk OTP.
        toUpdate.IsUsed = false;    // Mengatur status IsUsed menjadi false, menunjukkan OTP belum digunakan.

        _accountRepository.Update(toUpdate);    // Memperbarui entitas account dengan perubahan yang telah dibuat.

        // Mengirimkan OTP melalui email.
        _emailHandler.Send("Forgot Password", $"Your OTP is {toUpdate.OTP}", email);

        // Menggabungkan data account dan employee untuk respons.
        account = _accountRepository.GetAll();
        var result = from acc in account
                     join emp in employee on acc.Guid equals emp.Guid
                     where emp.Email == email
                     select new ForgotPasswordDto
                     {
                         Email = emp.Email,
                         Otp = acc.OTP,
                         ExpiredDate = acc.ExpiredTime
                     };

        // Mengembalikan respons sukses dengan informasi OTP.
        return Ok(new ResponseOKHandler<IEnumerable<ForgotPasswordDto>>(result));
    }

    // Endpoint untuk mengganti password.
    [HttpPut("ChangePassword")]
    public IActionResult ChangePassword(ChangePasswordDto changePasswordDto)
    {
        try
        {
            // Mengambil data employee berdasarkan alamat email yang diberikan dalam changePasswordDto.
            var data = _employeeRepository.GetEmail(changePasswordDto.Email);

            // Mengambil data account berdasarkan GUID employee yang terkait.
            var accounts = _accountRepository.GetByGuid(data.Guid);


            if (accounts == null)
            {
                // Mengembalikan respons NotFound jika akun tidak ditemukan.
                return NotFound(new ResponseErrorHandler
                {
                    Code = StatusCodes.Status404NotFound,
                    Status = HttpStatusCode.NotFound.ToString(),
                    Message = "Account Not Found"
                });
            }

            if (changePasswordDto.NewPassword != changePasswordDto.ConfirmPassword)
            {
                // Jika kata sandi baru tidak cocok dengan konfirmasi kata sandi, kembalikan respons dengan pesan kesalahan.
                return Ok(new ResponseOKHandler<string>("Password Not Match"));
            }

            if (!accounts.OTP.Equals(changePasswordDto.Otp))
            {
                // Jika OTP tidak cocok, kembalikan respons NotFound dengan pesan kesalahan.
                return NotFound(new ResponseErrorHandler
                {
                    Code = StatusCodes.Status404NotFound,
                    Status = HttpStatusCode.NotFound.ToString(),
                    Message = "OTP Is Incorrect"
                });
            }

            if (accounts.IsUsed)
            {
                // Jika OTP telah digunakan sebelumnya, kembalikan respons BadRequest dengan pesan kesalahan.
                return BadRequest(new ResponseErrorHandler
                {
                    Code = StatusCodes.Status400BadRequest,
                    Status = HttpStatusCode.BadRequest.ToString(),
                    Message = "OTP Has Been Used"
                });
            }

            if (DateTime.Now > accounts.ExpiredTime)
            {
                // Jika OTP telah kadaluarsa, kembalikan respons BadRequest dengan pesan kesalahan.
                return BadRequest(new ResponseErrorHandler
                {
                    Code = StatusCodes.Status400BadRequest,
                    Status = HttpStatusCode.BadRequest.ToString(),
                    Message = "OTP Was Expired"
                });
            }

            // Mengatur flag IsUsed ke true, menandakan bahwa OTP telah digunakan.
            accounts.IsUsed = true;

            // Melakukan hash terhadap kata sandi baru yang telah dikonfirmasi (changePasswordDto.ConfirmPassword).
            accounts.Password = HashingHandler.HashPassword(changePasswordDto.ConfirmPassword);

            // Memperbarui data akun dalam repositori dengan data yang telah diubah.
            _accountRepository.Update(accounts);

            // Kembalikan respons OK dengan pesan sukses.
            return Ok(new ResponseOKHandler<string>("Success Change Password"));

        }
        catch (Exception ex)
        {
            // Mengembalikan respons dengan kode status 500 dan pesan error jika terjadi kesalahan.
            return StatusCode(StatusCodes.Status500InternalServerError, new ResponseErrorHandler
            {
                Code = StatusCodes.Status500InternalServerError,
                Status = HttpStatusCode.InternalServerError.ToString(),
                Message = "Failed to Update Password",
                Error = ex.Message
            });
        }
    }

    // Endpoint untuk mengambil semua akun (khusus admin dan superAdmin).
    [HttpGet]
    [Authorize(Roles = "admin, superAdmin")]
    public IActionResult GetAll()
    {
        var result = _accountRepository.GetAll();

        if (!result.Any())
        {
            // Mengembalikan respons NotFound jika data akun tidak ditemukan.
            return NotFound(new ResponseErrorHandler
            {
                Code = StatusCodes.Status404NotFound,
                Status = HttpStatusCode.NotFound.ToString(),
                Message = "Data Not Found"
            });
        }

        var data = result.Select(x => (AccountDto)x);

        // Mengembalikan respons sukses dengan data akun.
        return Ok(new ResponseOKHandler<IEnumerable<AccountDto>>(data));
    }

    // Endpoint untuk mengambil akun berdasarkan GUID (khusus admin dan superAdmin).
    [HttpGet("{guid}")]
    [Authorize(Roles = "admin, superAdmin")]
    public IActionResult GetByGuid(Guid guid)
    {
        var result = _accountRepository.GetByGuid(guid);

        if (result is null)
        {
            // Mengembalikan respons NotFound jika akun tidak ditemukan.
            return NotFound(new ResponseErrorHandler
            {
                Code = StatusCodes.Status404NotFound,
                Status = HttpStatusCode.NotFound.ToString(),
                Message = "Data Not Found"
            });
        }

        // Mengembalikan respons sukses dengan data akun.
        return Ok(new ResponseOKHandler<AccountDto>((AccountDto)result));
    }

    // Endpoint untuk membuat akun baru.
    [HttpPost]
    public IActionResult Create(CreateAccountDto accountDto)
    {
        try
        {
            // Membuat objek Account dari data yang diterima dan menyimpan hash Password.
            Account toCreate = accountDto;
            toCreate.Password = HashingHandler.HashPassword(toCreate.Password);

            var result = _accountRepository.Create(toCreate);

            // Mengembalikan respons sukses dengan data akun yang baru dibuat.
            return Ok(new ResponseOKHandler<AccountDto>((AccountDto)result));
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

    // Endpoint untuk mengupdate data akun.
    [HttpPut]
    public IActionResult Update(AccountDto accountDto)
    {
        try
        {
            var entity = _accountRepository.GetByGuid(accountDto.EmployeeGuid);

            if (entity is null)
            {
                // Mengembalikan respons NotFound jika akun tidak ditemukan.
                return NotFound(new ResponseErrorHandler
                {
                    Code = StatusCodes.Status404NotFound,
                    Status = HttpStatusCode.NotFound.ToString(),
                    Message = "Data Not Found"
                });
            }
            var update = (Account)entity;
            update.CreatedDate = entity.CreatedDate;
            var results = _accountRepository.Update(update);
            return Ok(new ResponseOKHandler<string>("Success Update Data"));
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

    // Endpoint untuk menghapus akun berdasarkan GUID (khusus admin dan superAdmin).
    [HttpDelete("{guid}")]
    [Authorize(Roles = "admin, superAdmin")]
    public IActionResult Delete(Guid guid)
    {
        try
        {
            var entity = _accountRepository.GetByGuid(guid);

            if (entity is null)
            {
                // Mengembalikan respons NotFound jika akun tidak ditemukan.
                return NotFound(new ResponseErrorHandler
                {
                    Code = StatusCodes.Status404NotFound,
                    Status = HttpStatusCode.NotFound.ToString(),
                    Message = "Data Not Found"
                });
            }

            _accountRepository.Delete(entity);

            // Mengembalikan respons sukses setelah menghapus akun.
            return Ok(new ResponseOKHandler<string>("Data Deleted"));
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
}