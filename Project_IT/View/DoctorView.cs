using System.Text.Json.Serialization;
using Domain;

namespace Project_IT.View;

public class DoctorView
{
    [JsonPropertyName("id")]
    public int DoctorId { get; set; }
    [JsonPropertyName("name")]
    public string Name { get; set; }
    [JsonPropertyName("specialization")]
    public Profile Specialization { get; set; }
}