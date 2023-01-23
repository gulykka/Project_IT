using Domain.Models;
using Moq;
namespace Tests;

public class ReceptionServiceTests {
    private readonly ReceptionService _receptionService;
    private readonly Mock<IDoctorRepository> _doctorRepository;
    private readonly Mock<IAppointmentRepository> _repository;


    public ReceptionServiceTests() {
        _repository = new Mock<IAppointmentRepository>();
        _doctorRepository = new Mock<IDoctorRepository>();
        _receptionService = new ReceptionService(_repository.Object, _doctorRepository.Object);
    }

    // Test entities ~~
    public Doctor GetDoctor(string name = "Mart Lee") {
        
        return new Doctor(1, name, new Profile(1, "Лор"));
    }

    public Profile GetProfile() {
        return new Profile(1, "Лор");
    }

    public Reception GetReception() {
        return new Reception(1, DateTime.Now, DateTime.Now, 1, 1);
    }

    [Fact]
    public void AddToConcreteDateByDoctorIsNotExists() {
        _doctorRepository.Setup(repo => repo.Exists(It.Is<int>(id => id == 1)))
            .Returns(false);
        _doctorRepository.Setup(repo => repo.Get(It.Is<int>(id => id == 1)))
            .Returns(GetDoctor());
        
        var response = _receptionService.AddToConcreteDate(GetReception());
        
        Assert.False(response.Success);
        Assert.Equal("Doctor doesn't exists", response.Error);
    }
    [Fact]
    public void AddToConcreteDateByDoctorTimeTaken() {
        _doctorRepository.Setup(repo => repo.Exists(It.Is<int>(id => id == 1)))
            .Returns(true);
        _doctorRepository.Setup(repo => repo.Get(It.Is<int>(id => id == 1)))
            .Returns(GetDoctor());
        _repository.Setup(repo => repo.CheckFreeByDoctor(It.IsAny<DateTime>(), It.Is<Doctor>(doctor => doctor.Id == 1)))
            .Returns(false);

        var response = _receptionService.AddToConcreteDate(GetReception());
        Assert.False(response.Success);
        Assert.Equal("Date with this doctor already taken", response.Error);
    }
    
    [Fact]
    public void AddToConcreteDateByDoctorOk() {
        _doctorRepository.Setup(repo => repo.Exists(It.Is<int>(id => id == 1)))
            .Returns(true);
        _doctorRepository.Setup(repo => repo.Get(It.Is<int>(id => id == 1)))
            .Returns(GetDoctor());
        _repository.Setup(repo => repo.CheckFreeByDoctor(It.IsAny<DateTime>(), It.Is<Doctor>(doctor => doctor.Id == 1)))
            .Returns(true);
        var response = _receptionService.AddToConcreteDate(GetReception());
        
        Assert.True(response.Success);
    }

    [Fact]
    public void AddToConcreteDateBySpecNoFreeTime() {
        _repository.Setup(repo => repo.CheckFreeBySpec(It.IsAny<DateTime>(), It.IsAny<Profile>()))
            .Returns(false);

        var response = _receptionService.AddToConcreteDate(DateTime.Now, GetProfile());
        
        Assert.False(response.Success);
        Assert.Equal("No free doctors for this spec/time", response.Error);
    }

    [Fact]
    public void AddToConcreteDateBySpecOk() {
        _repository.Setup(repo => repo.CheckFreeBySpec(It.IsAny<DateTime>(), It.IsAny<Profile>()))
            .Returns(true);
        
        var response = _receptionService.AddToConcreteDate(DateTime.Now, GetProfile());
        
        Assert.True(response.Success);
    }

    [Fact]
    public void GetFreeBySpecOk() {
        var response = _receptionService.GetFreeBySpec(GetProfile());
        Assert.True(response.Success);
    }

    [Fact]
    public void GetFreeByDoctorIsNotExists() {
        _doctorRepository.Setup(repo => repo.Exists(It.Is<int>(id => id == 1)))
            .Returns(false);
        
        var response = _receptionService.GetFreeByDoctor(GetDoctor());
        
        Assert.False(response.Success);
        Assert.Equal("Doctor doesn't exists", response.Error);
    }
    
    [Fact]
    public void GetFreeByDoctorOk() {
        _doctorRepository.Setup(repo => repo.Exists(It.Is<int>(id => id == 1)))
            .Returns(true);
        
        var response = _receptionService.GetFreeByDoctor(GetDoctor());
        
        Assert.True(response.Success);
    }
}