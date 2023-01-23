﻿using Domain.Models;
using DataBase.Converters;
namespace DataBase.Repositories;

public class UserRepository
{
    private readonly ApplicationContext _context;

    public UserRepository(ApplicationContext context) {
        _context = context;
    }
    
    public User Create(User item) {
        _context.Users.Add(item.ToModel());
        return item;
    }

    public User? Get(int id) {
        var user = _context.Users.FirstOrDefault(user => user.Id == id);
        return user?.ToDomain();
    }
    
    public IEnumerable<User> List() {
        return _context.Users.Select(userModel => userModel.ToDomain()).ToList();
    }
    public bool Exists(int id) {
        return _context.Users.Any(user => user.Id == id);
    }

    public bool Delete(int id) {
        var user = _context.Users.FirstOrDefault(user => user.Id == id);
        if (user == default)
            return false; // not deleted
        _context.Users.Remove(user);
        return true;
    }

    public bool IsValid(User entity) {

        if (entity.Id < 0)
            return false;
        
        if (string.IsNullOrEmpty(entity.Username) || string.IsNullOrEmpty(entity.Password))
            return false;
        
        if (string.IsNullOrEmpty(entity.Number) || string.IsNullOrEmpty(entity.FullName))
            return false;

        return true;
    }

    public User Update(User entity) {
        _context.Users.Update(entity.ToModel());
        return entity;
    }

    public bool ExistLogin(string login, string password) {
        return _context.Users.Any(user => user.Username == login && user.Password == password);
    }

    public bool ExistLogin(string login) {
        return _context.Users.Any(user => user.Username == login);
    }

    public User? GetByLogin(string login) {
        var user = _context.Users.FirstOrDefault(user => user.Username == login);
        return user?.ToDomain();
    }
}