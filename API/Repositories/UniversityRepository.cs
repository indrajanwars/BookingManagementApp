using API.Contracts;
using API.Data;
using API.Models;

namespace API.Repositories;

public class UniversityRepository : GeneralRepository<University>, IUniversityRepository
{
    public UniversityRepository(BookingManagementDbContext context) : base(context) { }

    public University GetCodeName(string code, string name)
    {
        var codeName = _context // Akses db context
            .Set<University>() // Set bahwa yang akan diakses adalah university
            .FirstOrDefault(u => u.Code == code && u.Name == name); // LINQ untuk find data beradasarkan code dan name
        _context.ChangeTracker.Clear(); // Menghapus entitas dari Change Tracker untuk mencegah perubahan yang tidak diinginkan. 
        return codeName;
    }
}