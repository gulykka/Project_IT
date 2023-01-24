﻿namespace UnitTests; 
using Domain.Models;
using Moq;
public class ScheduleServiceTests {
    private readonly ScheduleService _scheduleService;
    private readonly Mock<IDoctorRepository> _doctorRepository;
    private readonly Mock<IScheduleRepository> _scheduleRepository;

    public ScheduleServiceTests() {
        _doctorRepository = new Mock<IDoctorRepository>();
        _scheduleRepository = new Mock<IScheduleRepository>();
        _scheduleService = new ScheduleService(_scheduleRepository.Object, _doctorRepository.Object);
    }

    Doctor GetDoctor() {
        return new Doctor(1, "aboba", new Profile(1, "ЛОР"));
    }

    Schedule GetSchedule() {
        return new Schedule(1, 1, new DateTime(2002, 04, 26, 16, 40, 0), new DateTime(2002, 04, 26, 16, 40, 0));
    }

    [Fact]
    public void GetByDoctorNotExist() {
        _doctorRepository.Setup(repo => repo.Exists(It.Is<int>(id => id == 1)))
            .ReturnsAsync(false);
        
        _doctorRepository.Setup(repo => repo.IsValid(It.IsAny<Doctor>()))
            .Returns(true);
        
        var res = _scheduleService.GetByDoctor(GetDoctor(), new DateOnly());

        Assert.False(res.Result.Success);
        Assert.Equal("Doctor doesn't exists", res.Result.Error);
    }
    
    [Fact]
    public void GetByDoctorNotValid() {
        _doctorRepository.Setup(repo => repo.Exists(It.Is<int>(id => id == 1)))
            .ReturnsAsync(true);
        
        _doctorRepository.Setup(repo => repo.IsValid(It.IsAny<Doctor>()))
            .Returns(false);
        
        var res = _scheduleService.GetByDoctor(GetDoctor(), new DateOnly());

        Assert.False(res.Result.Success);
        Assert.Equal("Doctor is not invalid", res.Result.Error);
    }
    
    [Fact]
    public void GetByDoctorOk() {
        _doctorRepository.Setup(repo => repo.Exists(It.Is<int>(id => id == 1)))
            .ReturnsAsync(true);
        
        _doctorRepository.Setup(repo => repo.IsValid(It.IsAny<Doctor>()))
            .Returns(true);

        var res = _scheduleService.GetByDoctor(GetDoctor(), new DateOnly());
        
        Assert.True(res.Result.Success);
    }

    [Fact]
    public void AddScheduleDoctorIsNotExists() {
        _doctorRepository.Setup(repo => repo.Exists(It.Is<int>(id => id == 1)))
            .ReturnsAsync(false);
        _doctorRepository.Setup(repo => repo.IsValid(It.IsAny<Doctor>()))
            .Returns(true);

        var res = _scheduleService.Add(GetSchedule());
        
        Assert.False(res.Result.Success);
        Assert.Equal("Doctor doesn't exists", res.Result.Error);
        
    }

    [Fact]
    public void AddScheduleAlreadyExists() {
        _doctorRepository.Setup(repo => repo.Exists(It.Is<int>(id => id == 1)))
            .ReturnsAsync(true);
        
        _scheduleRepository.Setup(repo => repo.Exists(It.Is<int>(id => id == 1)))
            .ReturnsAsync(true);
        
        var res = _scheduleService.Add(GetSchedule());
        
        Assert.False(res.Result.Success);
        Assert.Equal("Schedule already exists", res.Result.Error);
    }

    [Fact]
    public void AddScheduleOk() {
        _doctorRepository.Setup(repo => repo.Exists(It.Is<int>(id => id == 1)))
            .ReturnsAsync(true);
        
        _scheduleRepository.Setup(repo => repo.Exists(It.Is<int>(id => id == 1)))
            .ReturnsAsync(false);
        
        var res = _scheduleService.Add(GetSchedule());
        Assert.True(res.Result.Success);
    }

    [Fact]
    public void UpdateScheduleIsNotExists() {
        _scheduleRepository.Setup(repo => repo.Exists(It.Is<int>(id => id == 1)))
            .ReturnsAsync(false);
        
        var res = _scheduleService.Update(GetSchedule());
        Assert.False(res.Result.Success);
        Assert.Equal("Schedule Doesn't exists", res.Result.Error);
    }

    [Fact]
    public void UpdateScheduleOk() {
        _scheduleRepository.Setup(repo => repo.Exists(It.Is<int>(id => id == 1)))
            .ReturnsAsync(true);
        
        var res = _scheduleService.Update(GetSchedule());
        Assert.True(res.Result.Success);
    }

    [Fact]

    public void DeleteNotExists() {
        _scheduleRepository.Setup(repo => repo.Exists(It.Is<int>(id => id == 1)))
            .ReturnsAsync(false);
        
        var res = _scheduleService.Delete(GetSchedule());
        Assert.False(res.Result.Success);
        Assert.Equal("Schedule Doesn't exists", res.Result.Error);
    }
    
    [Fact]
    public void DeleteOk() {
        _scheduleRepository.Setup(repo => repo.Exists(It.Is<int>(id => id == 1)))
            .ReturnsAsync(true);
        
        var res = _scheduleService.Delete(GetSchedule());
        Assert.True(res.Result.Success);
    }
    
}