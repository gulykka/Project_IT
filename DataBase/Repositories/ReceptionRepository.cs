namespace DataBase.Repositories;

using DataBase.Converters;
using Domain;
using Domain.Models;

public class ReceptionRepository: IReceptionRepository
{
    private readonly ApplicationContext _context;
    private IReceptionRepository _receptionRepositoryImplementation;

    public ReceptionRepository(ApplicationContext context) {
        _context = context;
    }

    public Reception Create(Reception item) {
        _context.Receptions.Add(item.ToModel());
        return item;
    }

    public Reception? Get(int id) {
        return _context.Receptions.FirstOrDefault(a => a.Id == id).ToDomain();
    }

    public bool Exists(int id) {
        return _context.Receptions.Any(a => a.Id == id);
    }
    
    public IEnumerable<Reception> List() {
        return _context.Receptions.Select(receptiontModel => receptiontModel.ToDomain()).ToList();
    }

    public bool Delete(int id) {
        var reception = _context.Receptions.FirstOrDefault(a => a.Id == id);
        if (reception == default)
            return false; // not deleted
        _context.Receptions.Remove(reception);
        return true;
    }

    public bool IsValid(Reception entity) {
        if (entity.Id < 0)
            return false;

        if (entity.StartTime >= entity.EndTime)
            return false;

        return true;
    }

    public Reception Update(Reception entity) {
        _context.Receptions.Update(entity.ToModel());
        return entity;
    }

    public IEnumerable<Reception> GetAllBySpec(Profile spec) {
        var doctors = _context.Doctors.Where(d => d.Profile.Id == spec.Id);
        return _context.Receptions.Where(a => doctors.Any(d => a.DoctorId == d.Id))
            .Select(a => a.ToDomain())
            .ToList();
        
        //return ExcludeAppointments(appointments);
    }

    public IEnumerable<Reception> GetAllByDoctor(Doctor doctor) {
        return _context.Receptions.Where(a => a.DoctorId == doctor.Id)
            .Select(a => a.ToDomain())
            .ToList();
    }

    public IEnumerable<DateTime> GetFreeBySpec(Profile spec)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<DateTime> GetFreeByDoctor(Doctor doctor)
    {
        throw new NotImplementedException();
    }

    public bool CheckFreeBySpec(DateTime time, Profile profile) {
        var doctors = _context.Doctors
            .Where(d => d.Profile.Id == profile.Id);

        var receptions = _context.Receptions.Where(a => doctors.Any(d => d.Id == a.DoctorId));
        return receptions.Any(a => time >= a.StartTime && time <= a.EndTime);
    }

    public bool CheckFreeByDoctor(DateTime time, Doctor doctor) {
        return _context.Receptions.Any(a => time >= a.StartTime && time <= a.EndTime && a.DoctorId == doctor.Id);
    }

    public Reception CreateBySpec(DateTime dateTime, Profile spec) {
        throw new NotImplementedException();
    }
}