using API.Contracts;
using API.Data;
using API.Models;

namespace API.Repositories
{
    /* Kelas EducationRepository adalah implementasi dari repositori generik GeneralRepository<T> di mana T adalah Education,
     * kelas ini berfungsi sebagai repositori khusus untuk entitas Education. Kelas ini mengimplementasikan antarmuka IEducationRepository.*/
    public class EducationRepository : GeneralRepository<Education>, IEducationRepository
    {
        /* Konstruktor menerima instance dari BookingManagementDbContext yang akan digunakan oleh repositori
         * untuk berinteraksi dengan database. Konstruktor ini memanggil konstruktor base (dari GeneralRepository).*/
        public EducationRepository(BookingManagementDbContext context) : base(context) { }
    }
}