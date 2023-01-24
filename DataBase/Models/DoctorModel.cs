using Domain;

namespace DataBase.Models;

public class DoctorModel
{
    public int Id { get; set; }
    public string FullName { get; set; }
    public ProfileModel Profile { get; set; }

}