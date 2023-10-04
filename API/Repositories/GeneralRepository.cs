using API.Contracts;
using API.Data;
using API.Utilities.Handlers;
using Microsoft.EntityFrameworkCore;

namespace API.Repositories;

// Deklarasi kelas menggunakan generic type TEntity. Kelas ini mengimplementasikan interface IGeneralRepository<TEntity>.
public class GeneralRepository<TEntity> : IGeneralRepository<TEntity> where TEntity : class
{
    protected readonly BookingManagementDbContext _context;

    // Konstruktor untuk GeneralRepository, menerima instance dari BookingManagementDbContext yang akan digunakan untuk berinteraksi dengan database.
    protected GeneralRepository(BookingManagementDbContext context)
    {
        _context = context;
    }

    // Mengambil semua entitas TEntity dari database dan mengembalikannya sebagai IEnumerable.
    public IEnumerable<TEntity> GetAll()
    {
        return _context.Set<TEntity>().ToList();
    }

    // Mengambil entitas TEntity berdasarkan Guid yang diberikan.
    public TEntity? GetByGuid(Guid guid)
    {
        // Menggunakan metode Find untuk mencari entitas dengan Guid yang cocok.
        var entity = _context.Set<TEntity>().Find(guid);

        // Menghapus entitas untuk menghindari pelacakan perubahan yang tidak diperlukan.
        _context.ChangeTracker.Clear();

        return entity;
    }

    // Membuat entitas TEntity baru di database.
    public TEntity? Create(TEntity entity)
    {
        try
        {
            // Menambahkan entitas baru ke set TEntity dan menyimpan perubahan ke database.
            _context.Set<TEntity>().Add(entity);
            _context.SaveChanges();
            return entity;
        }
        catch (Exception ex)
        {
            if (ex.InnerException is not null && ex.InnerException.Message.Contains("IX_tb_m_employees_nik"))
            {
                throw new ExceptionHandler("NIK already exists");
            }
            if (ex.InnerException is not null && ex.InnerException.Message.Contains("IX_tb_m_employees_email"))
            {
                throw new ExceptionHandler("Email already exists");
            }
            if (ex.InnerException != null && ex.InnerException.Message.Contains("IX_tb_m_employees_phone_number"))
            {
                throw new ExceptionHandler("Phone number already exists");
            }
            throw new ExceptionHandler(ex.InnerException?.Message ?? ex.Message);
        }
    }

    // Memperbarui entitas TEntity yang ada di database.
    public bool Update(TEntity entity)
    {
        try
        {
            // Memperbarui entitas di set TEntity dan menyimpan perubahan ke database.
            _context.Set<TEntity>().Update(entity);
            _context.SaveChanges();
            return true;
        }
        catch (Exception ex)
        {
            throw new ExceptionHandler(ex.InnerException?.Message ?? ex.Message);
        }
    }

    // Menghapus entitas TEntity yang ada di database.
    public bool Delete(TEntity entity)
    {
        try
        {
            // Menghapus entitas dari set TEntity dan menyimpan perubahan ke database.
            _context.Set<TEntity>().Remove(entity);
            _context.SaveChanges();
            return true;
        }
        catch (Exception ex)
        {
            throw new ExceptionHandler(ex.InnerException?.Message ?? ex.Message);
        }
    }
}