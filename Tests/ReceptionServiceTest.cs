﻿using Domain.Models;
using Moq;
namespace UnitTests;

public class ReceptionServiceTests {
    private readonly ReceptionService _appointmentService;
    private readonly Mock<IDoctorRepository> _doctorRepository;
    private readonly Mock<IReceptionRepository> _repository;


    public ReceptionServiceTests() {
        _repository = new Mock<IReceptionRepository>();
        _doctorRepository = new Mock<IDoctorRepository>();
        _appointmentService = new ReceptionService(_repository.Object, _doctorRepository.Object);
    }

    // Test entities ~~
    public Doctor GetDoctor(string name = "John Doe") {
        
        return new Doctor(1, name, new Profile(1, "ЛОР"));
    }

    public Profile GetSpecialization() {
        return new Profile(1, "ЛОР");
    }

    public Reception GetAppointment() {
        return new Reception(1, DateTime.Now, DateTime.Now, 1, 1);
    }

    [Fact]
    public void AddToConcreteDateByDoctorIsNotExists() {
        _doctorRepository.Setup(repo => repo.Exists(It.Is<int>(id => id == 1)))
            .ReturnsAsync(false);
        _doctorRepository.Setup(repo => repo.Get(It.Is<int>(id => id == 1)))
            .ReturnsAsync(GetDoctor());
        
        var response = _appointmentService.AddToConcreteDate(GetAppointment());
        
        Assert.False(response.Result.Success);
        Assert.Equal("Doctor doesn't exists", response.Result.Error);
    }
    [Fact]
    public void AddToConcreteDateByDoctorTimeTaken() {
        _doctorRepository.Setup(repo => repo.Exists(It.Is<int>(id => id == 1)))
            .ReturnsAsync(true);
        _doctorRepository.Setup(repo => repo.Get(It.Is<int>(id => id == 1)))
            .ReturnsAsync(GetDoctor());
        _repository.Setup(repo => repo.CheckFreeByDoctor(It.IsAny<DateTime>(), It.Is<Doctor>(doctor => doctor.Id == 1)))
            .ReturnsAsync(false);

        var response = _appointmentService.AddToConcreteDate(GetAppointment());
        Assert.False(response.Result.Success);
        Assert.Equal("Date with this doctor already taken", response.Result.Error);
    }
    
    [Fact]
    public void AddToConcreteDateByDoctorOk() {
        _doctorRepository.Setup(repo => repo.Exists(It.Is<int>(id => id == 1)))
            .ReturnsAsync(true);
        _doctorRepository.Setup(repo => repo.Get(It.Is<int>(id => id == 1)))
            .ReturnsAsync(GetDoctor());
        _repository.Setup(repo => repo.CheckFreeByDoctor(It.IsAny<DateTime>(), It.Is<Doctor>(doctor => doctor.Id == 1)))
            .ReturnsAsync(true);
        var response = _appointmentService.AddToConcreteDate(GetAppointment());
        
        Assert.True(response.Result.Success);
    }

    [Fact]
    public void AddToConcreteDateBySpecNoFreeTime() {
        _repository.Setup(repo => repo.CheckFreeBySpec(It.IsAny<DateTime>(), It.IsAny<Profile>()))
            .ReturnsAsync(false);

        var response = _appointmentService.AddToConcreteDate(DateTime.Now, GetSpecialization());
        
        Assert.False(response.Result.Success);
        Assert.Equal("No free doctors for this spec/time", response.Result.Error);
    }

    [Fact]
    public void AddToConcreteDateBySpecOk() {
        _repository.Setup(repo => repo.CheckFreeBySpec(It.IsAny<DateTime>(), It.IsAny<Profile>()))
            .ReturnsAsync(true);
        
        var response = _appointmentService.AddToConcreteDate(DateTime.Now, GetSpecialization());
        
        Assert.True(response.Result.Success);
    }

    [Fact]
    public void GetFreeBySpecOk() {
        var response = _appointmentService.GetFreeBySpec(GetSpecialization());
        Assert.True(response.Result.Success);
    }

    [Fact]
    public void GetFreeByDoctorIsNotExists() {
        _doctorRepository.Setup(repo => repo.Exists(It.Is<int>(id => id == 1)))
            .ReturnsAsync(false);
        
        var response = _appointmentService.GetFreeByDoctor(GetDoctor());
        
        Assert.False(response.Result.Success);
        Assert.Equal("Doctor doesn't exists", response.Result.Error);
    }
    
    [Fact]
    public void GetFreeByDoctorOk() {
        _doctorRepository.Setup(repo => repo.Exists(It.Is<int>(id => id == 1)))
            .ReturnsAsync(true);
        
        var response = _appointmentService.GetFreeByDoctor(GetDoctor());
        
        Assert.True(response.Result.Success);
    }
}