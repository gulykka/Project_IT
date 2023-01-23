using Domain.Models;
using DataBase.Models;
using Domain;

namespace DataBase.Converters; 

public static class DoctorConverter {
    public static DoctorModel ToModel(this Doctor domainDoctor) {
        return new DoctorModel {
            Id = domainDoctor.Id,
            FullName = domainDoctor.FullName,
            Profile = domainDoctor.Profile.ToModel()
        };
    }
    
    public static Doctor ToDomain(this DoctorModel doctor) {
        return new Doctor(
            doctor.Id,
            doctor.FullName,
            doctor.Profile.ToDomain()
        );
    }
}