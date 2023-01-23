namespace DataBase.Converters;
using Domain.Models;
using DataBase.Models;


public static class UserConverter
{
    public static UserModel ToModel(this User domainUser)
    {
        return new UserModel
        {
            Id = domainUser.Id,
            Password = domainUser.Password,
            Number = domainUser.Number,
            FullName = domainUser.FullName,
            Role = domainUser.Role
        };
    }

    public static User ToDomain(this UserModel model)
    {
        return new User(
            model.Username,
            model.Password,
            model.Id,
            model.Number,
            model.FullName,
            model.Role
            
        );
    }
}