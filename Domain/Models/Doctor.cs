namespace Domain;

public class Doctor
{
    public int Id { get; set; }
    public string FullName { get; set; }
    public Profile Profile { get; set; }


    public Doctor(int id, string fullName, Profile profile)
    {
        Id = id;
        FullName = fullName;
        Profile = profile;
    }
}