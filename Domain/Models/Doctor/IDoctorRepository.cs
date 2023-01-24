using Domain.Models;

namespace Domain;

public interface IDoctorRepository : IRepository<Doctor>
{
    public IEnumerable<Doctor> GetBySpec(Profile profile);
}