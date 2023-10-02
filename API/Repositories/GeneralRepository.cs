using API.Contracts;
using API.Data;

namespace API.Repositories;

// Deklarasi kelas menggunakan generic type TEntity. Kelas ini mengimplementasikan interface IGeneralRepository<TEntity>.
public class GeneralRepository<TEntity> : IGeneralRepository<TEntity> where TEntity : class
{
    private readonly BookingManagementDbContext _context;

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
        catch
        {
            return null;
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
        catch
        {
            return false;
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
        catch
        {
            return false;
        }
    }
}