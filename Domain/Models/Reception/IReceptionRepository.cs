namespace Domain.Models; 

public interface IReceptionRepository : IRepository<Reception> {
    IEnumerable<DateTime> GetFreeBySpec(Profile spec);
    IEnumerable<DateTime> GetFreeByDoctor(Doctor doctor);
    bool CheckFreeBySpec(DateTime time, Profile profile);
    bool CheckFreeByDoctor(DateTime time, Doctor doctor);
    Reception CreateBySpec(DateTime dateTime, Profile profile);

}