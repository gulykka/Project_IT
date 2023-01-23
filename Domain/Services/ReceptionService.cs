using Domain.Models;

namespace Domain.Services; 

public class ReceptionService {
    private IReceptionRepository _repository;
    private IDoctorRepository _doctorRepository;
    public ReceptionService(IReceptionRepository repo, IDoctorRepository doctorRepo) {
        _repository = repo;
        _doctorRepository = doctorRepo;
    }

    public Result<Reception> AddToConcreteDate(Reception reception) {
        var doctor = _doctorRepository.Get(reception.DoctorId);
        if (!_doctorRepository.Exists(doctor.Id))
            return Result.Fail<Reception>("Doctor doesn't exists");
		
        if (!_repository.CheckFreeByDoctor(reception.StartTime, doctor))
            return Result.Fail<Reception>("Date with this doctor already taken");
		
        _repository.Create(reception);
        return Result.Ok(reception);
    }

    public Result<Reception> AddToConcreteDate(DateTime dateTime, Profile profile) {
        if (!_repository.CheckFreeBySpec(dateTime, profile))
            return Result.Fail<Reception>("No free doctors for this spec/time");

        var reception = _repository.CreateBySpec(dateTime, profile);
        return Result.Ok(reception);
    }

    public Result<IEnumerable<DateTime>> GetFreeBySpec(Profile profile) {
        var list = _repository.GetFreeBySpec(profile);
        return Result.Ok(list);
    }
	
    public Result<IEnumerable<DateTime>> GetFreeByDoctor(Doctor doctor) {
        if (!_doctorRepository.Exists(doctor.Id))
            return Result.Fail<IEnumerable<DateTime>>("Doctor doesn't exists");
        var list = _repository.GetFreeByDoctor(doctor);
        return Result.Ok(list);
    }
}