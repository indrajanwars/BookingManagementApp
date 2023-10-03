namespace API.Contracts;

/* TEntity adalah generic type yang digunakan untuk menentukan jenis entitas yang akan
 * dioperasikan oleh repositori yang mengimplementasikan interface ini.*/
public interface IGeneralRepository<TEntity> where TEntity : class
{
    // Mengambil semua entitas TEntity dan mengembalikannya sebagai IEnumerable.
    IEnumerable<TEntity> GetAll();

    // Mengambil entitas TEntity berdasarkan GUID yang diberikan.
    TEntity? GetByGuid(Guid guid);

    // Membuat entitas TEntity baru dengan menggunakan objek TEntity yang diberikan.
    TEntity? Create(TEntity entity);

    // Memperbarui entitas TEntity yang ada dengan menggunakan objek TEntity yang diberikan.
    bool Update(TEntity entity);

    // Menghapus entitas TEntity yang ada berdasarkan objek TEntity yang diberikan.
    bool Delete(TEntity entity);
}