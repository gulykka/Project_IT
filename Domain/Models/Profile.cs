namespace Domain;

public class Profile
{
    public int Id { get; set; }
    public string Name { get; set; }

    public Profile(int id, String name)
    {
        Id = id;
        Name = name;
    }
    
}