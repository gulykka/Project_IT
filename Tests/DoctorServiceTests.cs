﻿using Domain.Models;
using Moq;

namespace UnitTests; 

public class DoctorServiceTests {
	private readonly DoctorService _doctorService;
	private readonly Mock<IDoctorRepository> _repository;

	public DoctorServiceTests() {
		_repository = new Mock<IDoctorRepository>();
		_doctorService = new DoctorService(_repository.Object);
	}

	public Doctor GetDoctor(string name = "Mart Lee") { // Test entity
		return new Doctor(1, name, new Profile(1, "Лор"));
	}
	
	[Fact]
	public void CreateAlreadyExists() {
		_repository.Setup(repo => repo.Exists(It.Is<int>(id => id == 1)))
			.Returns(true);

		_repository.Setup(repo => repo.IsValid(It.IsAny<Doctor>()))
			.Returns(true);

		var response = _doctorService.CreateDoctor(GetDoctor());
        
		Assert.False(response.Success);
		Assert.Equal("Doctor already exists", response.Error);
	}

	[Fact]
	public void CreateInvalidFullName() {
		Doctor doctor = GetDoctor(string.Empty);
		_repository.Setup(repo => repo.Exists(It.Is<int>(id => id == 1)))
			.Returns(false);

		var response = _doctorService.CreateDoctor(doctor);
		Assert.False(response.Success);
	}

	[Fact]
	public void CreateOk() {
		_repository.Setup(repo => repo.Exists(It.Is<int>(id => id == 1)))
			.Returns(false);

		_repository.Setup(repo => repo.IsValid(It.IsAny<Doctor>()))
			.Returns(true);
		
		var response = _doctorService.CreateDoctor(GetDoctor());
		
		Assert.True(response.Success);
	}

	[Fact]
	public void DeleteNotExists() {
		_repository.Setup(repo => repo.Exists(It.Is<int>(id => id == 1)))
			.Returns(false);
		
		_repository.Setup(repo => repo.IsValid(It.IsAny<Doctor>()))
			.Returns(true);
		
		var response = _doctorService.DeleteDoctor(GetDoctor().Id);
		
		Assert.False(response.Success);
		Assert.Equal("Doctor doesn't exists", response.Error);
		
	}

	[Fact]
	public void DeleteOk() {
		_repository.Setup(repo => repo.Exists(It.Is<int>(id => id == 1)))
			.Returns(true);

		_repository.Setup(repo => repo.IsValid(It.IsAny<Doctor>()))
			.Returns(true);
		
		var response = _doctorService.DeleteDoctor(GetDoctor().Id);
		
		Assert.True(response.Success);
	}
	
	[Fact]
	public void GetAllOk() {
		var response = _doctorService.GetAll();
		Assert.True(response.Success);
	}
	
	[Fact]
	public void GetByIdNotExists() {
		_repository.Setup(repo => repo.Exists(It.Is<int>(id => id == 1)))
			.Returns(false);

		var response = _doctorService.GetById(1);
		
		Assert.False(response.Success);
		Assert.Equal("Doctor doesn't exists", response.Error);
	}

	[Fact]
	public void GetByIdOk() {
		_repository.Setup(repo => repo.Exists(It.Is<int>(id => id == 1)))
			.Returns(true);

		var response = _doctorService.GetById(1);

		Assert.True(response.Success);
	}
	
	[Fact]
	public void GetBySpecOk() {
		var response = _doctorService.GetBySpec(new Profile(1, "Лор"));
		Assert.True(response.Success);
	}
	
}