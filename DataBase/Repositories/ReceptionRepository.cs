using Domain.Models;
using DataBase.Converters;
using DataBase.Models;
using Domain;
using Microsoft.EntityFrameworkCore;

namespace DataBase.Repositories; 

public class ReceptionRepository: IReceptionRepository {

    private readonly ApplicationContext _context;

    public ReceptionRepository(ApplicationContext context) {
        _context = context;
    }

    public async Task<Reception> Create(Reception item) {
        await _context.Receptions.AddAsync(item.ToModel());
        await _context.SaveChangesAsync();
        return item;
    }

    public async Task<Reception> Get(int id) {
        var appointments = await _context.Receptions.FirstOrDefaultAsync(a => a.Id == id);
        return appointments.ToDomain();
    }

    public async Task<bool> Exists(int id) {
        return await _context.Receptions.AnyAsync(a => a.Id == id);
    }
    
    public async Task<IEnumerable<Reception>> List() {
        return await _context.Receptions.Select(appointmentModel => appointmentModel.ToDomain()).ToListAsync();
    }

    public async Task<bool> Delete(int id) {
        var appointment = await _context.Receptions.FirstOrDefaultAsync(a => a.Id == id);
        if (appointment == default)
            return false; // not deleted
        _context.Receptions.Remove(appointment);
        await _context.SaveChangesAsync();
        return true;
    }

    public bool IsValid(Reception entity) {
        if (entity.Id < 0)
            return false;

        if (entity.StartTime >= entity.EndTime)
            return false;

        return true;
    }

    public async Task<Reception> Update(Reception entity) {
        _context.Receptions.Update(entity.ToModel());
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task<IEnumerable<Reception>> GetAllBySpec(Profile spec) {
        return await _context.Receptions.
            Where(a => _context.Doctors.
                Where(d => d.Profile.Id == spec.Id).Any(d => a.DoctorId == d.Id))
            .Select(a => a.ToDomain())
            .ToListAsync();
        
    }

    public async Task<IEnumerable<Reception>> GetAllByDoctor(Doctor doctor) {
        return await _context.Receptions.Where(a => a.DoctorId == doctor.Id)
            .Select(a => a.ToDomain())
            .ToListAsync();
    }
    

    public async Task<bool> CheckFreeBySpec(DateTime time, Profile specialization) {
        var doctors = _context.Doctors
            .Where(d => d.Profile.Id == specialization.Id);

        var appointments = _context.Receptions.Where(a => doctors.Any(d => d.Id == a.DoctorId));
        return await appointments.AnyAsync(a => time >= a.StartTime && time <= a.EndTime);
    }

    public Task<bool> CheckFreeByDoctor(DateTime time, Doctor doctor) {
        return _context.Receptions.AnyAsync(a => time >= a.StartTime && time <= a.EndTime && a.DoctorId == doctor.Id);
    }

    public Reception CreateBySpec(DateTime dateTime, Profile spec) {
        throw new NotImplementedException();
    }
}
