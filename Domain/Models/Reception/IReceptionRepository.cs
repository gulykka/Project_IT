namespace Domain.Models; 

public interface IReceptionRepository : IRepository<Reception> {
    Task<IEnumerable<Reception>> GetAllBySpec(Profile spec);
    Task<IEnumerable<Reception>> GetAllByDoctor(Doctor doctor);
    Task<bool> CheckFreeBySpec(DateTime time, Profile specialization);
    Task<bool> CheckFreeByDoctor(DateTime time, Doctor doctor);
    Reception CreateBySpec(DateTime dateTime, Profile spec);

}
