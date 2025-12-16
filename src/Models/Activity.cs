namespace MergingtonHighSchool.Models;

public class Activity
{
    public string Description { get; set; } = string.Empty;
    public string Schedule { get; set; } = string.Empty;
    public int MaxParticipants { get; set; }
    public List<string> Participants { get; set; } = new();
}
