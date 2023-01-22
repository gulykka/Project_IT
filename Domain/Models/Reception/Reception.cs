namespace Domain;

public class Reception
{
    public int Id;
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public int UserId { get; set; }
    public int DoctorId { get; set; }


    public Reception(int id, DateTime startTime, DateTime endTime, int userId, int doctorId)
    {
        Id = id;
        StartTime = startTime;
        EndTime = endTime;
        UserId = userId;
        DoctorId = doctorId;
    }
}