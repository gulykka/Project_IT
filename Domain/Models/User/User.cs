namespace Domain.Models;

public class User
{
    public string Username;
    public string Password;
    public int Id { get; set; }
    public string Number{ get; set; }
    public string FullName{ get; set; }
    public Role Role { get; set; }
    
    
    public User(string username, string password, int id, string number, string fullName, Role role) {
        Username = username;
        Password = password;
        Id = id;
        Number = number;
        FullName = fullName;
        Role = role;
    }
    
}