using Domain;

namespace DataBase.Models;
using Domain.Models;

public class UserModel
{
    public string Username{ get; set; }
    public string Password{ get; set; }
    public int Id { get; set; }
    public string Number{ get; set; }
    public string FullName{ get; set; }
    public Role Role { get; set; }
}