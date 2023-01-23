namespace DataBase.Models;

public class ReceptionModel
{
    public int Id;
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public int UserId { get; set; }
    public int DoctorId { get; set; }
}