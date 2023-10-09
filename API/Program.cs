using System.Text;
using System.Reflection;
using API.Contracts;
using API.Data;
using API.Repositories;
using API.Utilities.Handlers;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using API.Utilities.Handler;

// Konfigurasi awal aplikasi web menggunakan builder.
var builder = WebApplication.CreateBuilder(args);

// Mengambil koneksi database dari konfigurasi.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<BookingManagementDbContext>(option => option.UseSqlServer(connectionString));

// Menambahkan layanan email ke dalam kontainer IoC (Inversion of Control).
builder.Services.AddTransient<IEmailHandler, EmailHandler>(_ => new EmailHandler(
                                                            server: builder.Configuration["SmtpService:Server"],
                                                            port: int.Parse(builder.Configuration["SmtpService: Port"]),
                                                            fromEmailAddress: builder.Configuration["SmtpService: FromEmailAddress"]
                                                            ));

// Menambahkan layanan TokenHandler ke dalam kontainer IoC.
builder.Services.AddScoped<ITokenHandler, TokensHandler>();

// Menambahkan repositori ke dalam kontainer IoC.
builder.Services.AddScoped<IAccountRepository, AccountRepository>();
builder.Services.AddScoped<IAccountRoleRepository, AccountRoleRepository>();
builder.Services.AddScoped<IBookingRepository, BookingRepository>();
builder.Services.AddScoped<IEducationRepository, EducationRepository>();
builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
builder.Services.AddScoped<IRoleRepository, RoleRepository>();
builder.Services.AddScoped<IRoomRepository, RoomRepository>();
builder.Services.AddScoped<IUniversityRepository, UniversityRepository>();

// Konfigurasi kontroler dan validasi model.
builder.Services.AddControllers()
       .ConfigureApiBehaviorOptions(options =>
        {
            // Konfigurasi respons kustom untuk validasi model yang tidak valid.
            options.InvalidModelStateResponseFactory = context =>
            {
                var errors = context.ModelState.Values
                                     .SelectMany(v => v.Errors)
                                     .Select(v => v.ErrorMessage);

                return new BadRequestObjectResult(new ResponseValidatorHandler(errors));
            };
        });

// Menambahkan dukungan untuk dokumentasi Swagger/OpenAPI.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Menambahkan layanan validasi model dengan FluentValidation.
builder.Services.AddFluentValidationAutoValidation()
       .AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

// Konfigurasi otentikasi JWT.
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
       .AddJwtBearer(options =>
       {
           options.RequireHttpsMetadata = false;    // Hanya untuk pengembangan.
           options.SaveToken = true;
           options.TokenValidationParameters = new TokenValidationParameters
           {
               ValidateIssuer = true,
               ValidIssuer = builder.Configuration["JWTService:Issuer"],
               ValidateAudience = true,
               ValidAudience = builder.Configuration["JWTService:Audience"],
               IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWTService:SecretKey"])),
               ValidateLifetime = true,
               ClockSkew = TimeSpan.Zero
           };
       });

// Konfigurasi Swagger.
builder.Services.AddSwaggerGen(x =>
{
    x.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "Metrodata Coding Camp",
        Description = "ASP.NET Core API 6.0"
    });
    x.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme."
    });
    x.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] { }
        }
    });
});

// Mendaftarkan layanan Controller
builder.Services.AddControllers();

// Mendaftarkan layanan untuk API Explorer (Swagger/OpenAPI)
builder.Services.AddEndpointsApiExplorer();

// Mendaftarkan layanan untuk Swagger
builder.Services.AddSwaggerGen();

// Menambahkan dukungan CORS (Cross-Origin Resource Sharing).
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin();    // Mengizinkan permintaan dari semua asal (origin).
        policy.AllowAnyHeader();    // Mengizinkan penggunaan semua jenis header dalam permintaan.

        // Mengizinkan penggunaan metode HTTP GET, POST, DELETE, dan PUT dalam permintaan.
        policy.WithMethods("GET", "POST", "DELETE", "PUT");
    });

}
    );

var app = builder.Build();

// Konfigurasi pipeline untuk HTTP request.
if (app.Environment.IsDevelopment())
{
    // Menggunakan Swagger UI untuk pengembangan.
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();      // Mengaktifkan HTTPS redirection (pengalihan ke HTTPS).

app.UseAuthentication();        // Menggunakan otentikasi.

app.UseAuthorization();         // Menggunakan otorisasi.

app.MapControllers();           // Menggunakan kontroler API.

app.Run();                      // Menjalankan aplikasi.