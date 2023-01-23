﻿using Domain.Models;

namespace Domain.Services;

public class DoctorService {
    private readonly IDoctorRepository _repository;

    public DoctorService(IDoctorRepository repository) {
        _repository = repository;
    }


    public Result<Doctor> CreateDoctor(Doctor doctor) {
        if (string.IsNullOrEmpty(doctor.FullName))
            return Result.Fail<Doctor>("Invalid doctor FullName");

        if (_repository.Exists(doctor.Id))
            return Result.Fail<Doctor>("Doctor already exists");
		
        _repository.Create(doctor);
        return Result.Ok(doctor);
    }

    public Result DeleteDoctor(int id) {
        if (!_repository.Exists(id))
            return Result.Fail<Doctor>("Doctor doesn't exists");
		
        _repository.Delete(id);
        return Result.Ok();
    }

    public Result<IEnumerable<Doctor>> GetAll() {
        return Result.Ok(_repository.List());
    }

    public Result<Doctor> GetById(int id) {
        if (!_repository.Exists(id))
            return Result.Fail<Doctor>("Doctor doesn't exists");

        return Result.Ok<Doctor>(_repository.Get(id));
    }

    public Result<IEnumerable<Doctor>> GetBySpec(Profile profile) {
        return Result.Ok(_repository.GetBySpec(profile));
    }
}