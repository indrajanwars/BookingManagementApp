using API.Models;

namespace API.Contracts
{
    /* Interface IEducationRepository adalah turunan dari interface IGeneralRepository<Education>.
     * Interface ini mewarisi semua operasi yang didefinisikan dalam IGeneralRepository, dan entitas yang telah ditentukan sebagai Education. */
    public interface IEducationRepository : IGeneralRepository<Education>
    {
        /* Interface ini secara khusus berkaitan dengan entitas Education, dan tidak menambahkan
         * operasi tambahan selain yang telah didefinisikan dalam IGeneralRepository. */
    }
}