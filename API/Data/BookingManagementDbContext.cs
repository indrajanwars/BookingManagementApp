using API.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace API.Data;

public class BookingManagementDbContext : DbContext
{
    public BookingManagementDbContext(DbContextOptions<BookingManagementDbContext> options) : base(options) { }

    // Add Models to migrate
    public DbSet<Account> Account { get; set; }
    public DbSet<AccountRole> AccountRole { get; set; }
    public DbSet<Role> Role { get; set; }
    public DbSet<Room> Room { get; set; }
    public DbSet<Booking> Booking { get; set; }
    public DbSet<Education> Education { get; set; }
    public DbSet<Employee> Employee { get; set; }
    public DbSet<University> University { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Employee>().HasIndex(e => new
        {
            e.Nik,
            e.Email,
            e.PhoneNumber
        }).IsUnique();

        // One University has many Education
        modelBuilder.Entity<University>()
                    .HasMany(e => e.Education)
                    .WithOne(u => u.University)
                    .HasForeignKey(e => e.UniversityGuid)
                    .OnDelete(DeleteBehavior.Restrict);

        // Or :
        /*modelBuilder.Entity<Education>()
            .HasOne(u => u.University)
            .WithMany(e => e.Educations)
            .HasForeignKey(u => u.UniversityGuid);*/

        // One Education has one Employees
        modelBuilder.Entity<Education>()
                    .HasOne(e => e.Employee)
                    .WithOne(e => e.Education)
                    .HasForeignKey<Education>(e => e.Guid);

        // One Employee has many Booking
        modelBuilder.Entity<Employee>()
                    .HasMany(e => e.Booking)
                    .WithOne(u => u.Employee)
                    .HasForeignKey(e => e.EmployeeGuid);

        // One Room has many Booking
        modelBuilder.Entity<Room>()
                    .HasMany(e => e.Booking)
                    .WithOne(u => u.Room)
                    .HasForeignKey(e => e.RoomGuid);

        // One Employee has one Account
        modelBuilder.Entity<Employee>()
                    .HasOne(e => e.Account)
                    .WithOne(a => a.Employee)
                    .HasForeignKey<Account>(a => a.Guid)
                    .OnDelete(DeleteBehavior.Cascade);

        // One Account has many Account Role
        modelBuilder.Entity<Account>()
                    .HasMany(a => a.AccountRole)
                    .WithOne(ar => ar.Account)
                    .HasForeignKey(ar => ar.AccountGuid);

        // One Role has many Account Role
        modelBuilder.Entity<Role>()
                    .HasMany(r => r.AccountRole)
                    .WithOne(ar => ar.Role)
                    .HasForeignKey(ar => ar.RoleGuid);
    }
}