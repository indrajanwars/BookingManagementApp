using API.Models;

namespace API.Contracts;

public interface IEducationRepository
{
    IEnumerable<Education> GetAll();
    Education? GetByGuid(Guid guid);
    Education? Create(Education education);
    bool Update(Education education);
    bool Delete(Education education);
}
